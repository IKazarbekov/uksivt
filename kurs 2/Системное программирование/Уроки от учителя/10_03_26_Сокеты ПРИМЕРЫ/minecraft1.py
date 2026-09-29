import socket
import threading
import struct
import time
import json
from datetime import datetime


class MinecraftServer:
    def __init__(self, host='0.0.0.0', port=40477):
        self.host = host
        self.port = port
        self.server_socket = None
        self.running = False
        self.server_name = "PyCraft Server 1.8.0"
        self.server_version = "1.8"
        self.protocol_version = 47
        self.max_players = 20
        self.online_players = {}
        self.player_entities = {}
        self.next_entity_id = 100

    def write_varint(self, value):
        """Кодирование числа в VarInt формат"""
        out = bytearray()
        while True:
            if value & ~0x7F == 0:
                out.append(value)
                return bytes(out)
            out.append((value & 0x7F) | 0x80)
            value >>= 7

    def read_varint(self, data, offset=0):
        """Чтение VarInt из данных"""
        result = 0
        position = 0
        while True:
            if offset >= len(data):
                return None, offset
            byte = data[offset]
            result |= (byte & 0x7F) << position
            offset += 1
            position += 7
            if not (byte & 0x80):
                return result, offset
            if position > 28:
                return None, offset

    def write_string(self, string):
        """Запись строки в формате Minecraft"""
        string_bytes = string.encode('utf-8')
        return self.write_varint(len(string_bytes)) + string_bytes

    def create_packet(self, packet_id, data=b''):
        """Создание пакета Minecraft"""
        packet_id_bytes = self.write_varint(packet_id)
        packet = packet_id_bytes + data
        length_bytes = self.write_varint(len(packet))
        return length_bytes + packet

    def recv_all(self, sock, length):
        """Получение точного количества байт"""
        data = b''
        while len(data) < length:
            try:
                packet = sock.recv(length - len(data))
                if not packet:
                    return None
                data += packet
            except socket.timeout:
                continue
            except:
                return None
        return data

    def read_packet(self, sock):
        """Чтение полного пакета из сокета"""
        try:
            # Сначала читаем длину пакета (VarInt)
            length_data = b''
            while True:
                try:
                    byte = sock.recv(1)
                    if not byte:
                        return None, None
                    length_data += byte
                    # Пробуем прочитать VarInt
                    length, _ = self.read_varint(length_data, 0)
                    if length is not None:
                        break
                except socket.timeout:
                    continue
                except:
                    return None, None

            # Теперь читаем сам пакет
            packet_data = self.recv_all(sock, length)
            if packet_data is None:
                return None, None

            # Читаем ID пакета из данных
            packet_id, offset = self.read_varint(packet_data, 0)
            if packet_id is None:
                return None, None

            return packet_id, packet_data[offset:]

        except Exception as e:
            print(f"Ошибка чтения пакета: {e}")
            return None, None

    def send_keep_alive(self, client_socket):
        """Отправка Keep Alive пакета"""
        keep_alive_id = struct.pack('>i', int(time.time()))
        client_socket.send(self.create_packet(0x00, keep_alive_id))

    def send_join_game(self, client_socket, entity_id):
        """Отправка пакета Join Game"""
        data = struct.pack('>i', entity_id)
        data += struct.pack('>B', 1)  # Gamemode (creative)
        data += struct.pack('>B', 0)  # Dimension (overworld)
        data += struct.pack('>B', 0)  # Difficulty (peaceful)
        data += struct.pack('>B', self.max_players)
        data += self.write_string("default")
        data += struct.pack('>B', 0)  # Reduced Debug Info

        client_socket.send(self.create_packet(0x01, data))

    def send_spawn_position(self, client_socket):
        """Отправка позиции спавна"""
        data = struct.pack('>i', 0) + struct.pack('>i', 64) + struct.pack('>i', 0)
        client_socket.send(self.create_packet(0x05, data))

    def send_player_position(self, client_socket):
        """Отправка позиции игрока"""
        x, y, z = 0.0, 64.0, 0.0
        yaw, pitch = 0.0, 0.0
        flags = 0x00

        data = struct.pack('>d', x) + struct.pack('>d', y) + struct.pack('>d', z)
        data += struct.pack('>f', yaw) + struct.pack('>f', pitch)
        data += bytes([flags])

        client_socket.send(self.create_packet(0x08, data))

    def broadcast_to_others(self, exclude_socket, packet):
        """Отправка пакета всем кроме указанного"""
        for player_socket in list(self.online_players.keys()):
            if player_socket != exclude_socket:
                try:
                    player_socket.send(packet)
                except:
                    pass

    def broadcast_message(self, message, exclude=None):
        """Отправка сообщения всем игрокам"""
        if isinstance(message, str):
            message = json.dumps({"text": message})

        packet = self.create_packet(0x03, self.write_string(message))

        for player_socket in list(self.online_players.keys()):
            if player_socket != exclude:
                try:
                    player_socket.send(packet)
                except:
                    pass

    def handle_client(self, client_socket, address):
        """Обработка клиента"""
        print(f"✓ Новое подключение от {address}")

        try:
            # Устанавливаем таймаут
            client_socket.settimeout(5.0)

            # Читаем первый пакет (handshake)
            packet_id, data = self.read_packet(client_socket)

            if packet_id is None:
                print("  ✗ Не удалось прочитать пакет")
                return

            print(f"  Получен пакет ID: {packet_id}, данные: {data.hex()}")

            if packet_id == 0x00:  # Handshake
                offset = 0
                protocol, offset = self.read_varint(data, offset)
                server_addr_len, offset = self.read_varint(data, offset)
                server_addr = data[offset:offset + server_addr_len].decode('utf-8')
                offset += server_addr_len
                server_port = struct.unpack('>H', data[offset:offset + 2])[0]
                offset += 2
                next_state, offset = self.read_varint(data, offset)

                print(f"  Протокол: {protocol}")
                print(f"  Адрес: {server_addr}:{server_port}")
                print(f"  След. состояние: {next_state}")

                if next_state == 1:  # Status
                    self.handle_status(client_socket)
                elif next_state == 2:  # Login
                    self.handle_login(client_socket)
                else:
                    print(f"  ✗ Неизвестное состояние: {next_state}")

        except socket.timeout:
            print(f"  ✗ Таймаут при чтении данных")
        except Exception as e:
            print(f"  ✗ Ошибка: {e}")
        finally:
            # Закрываем соединение если игрок не залогинился
            if client_socket not in self.online_players:
                client_socket.close()

    def handle_status(self, client_socket):
        """Обработка статус запроса"""
        try:
            # Читаем следующий пакет (обычно запрос статуса)
            packet_id, data = self.read_packet(client_socket)

            if packet_id == 0x00:  # Status Request
                # Отправляем информацию о сервере
                server_info = {
                    "version": {
                        "name": self.server_version,
                        "protocol": self.protocol_version
                    },
                    "players": {
                        "max": self.max_players,
                        "online": len(self.online_players),
                        "sample": []
                    },
                    "description": {
                        "text": f"§a{self.server_name}\n§7Сервер на Python для 1.8.0!"
                    }
                }

                response = json.dumps(server_info)
                client_socket.send(self.create_packet(0x00, self.write_string(response)))

                # Ждем ping
                packet_id, data = self.read_packet(client_socket)
                if packet_id == 0x01:  # Ping
                    # Отправляем pong
                    client_socket.send(self.create_packet(0x01, data))

        except Exception as e:
            print(f"  Ошибка при status: {e}")

    def handle_login(self, client_socket):
        """Обработка логина"""
        try:
            # Читаем Login Start
            packet_id, data = self.read_packet(client_socket)

            if packet_id == 0x00:  # Login Start
                offset = 0
                username_len, offset = self.read_varint(data, offset)
                username = data[offset:offset + username_len].decode('utf-8')

                print(f"  Логин игрока: {username}")

                # Проверяем, не занят ли ник
                for player_data in self.online_players.values():
                    if player_data['username'].lower() == username.lower():
                        disconnect_msg = json.dumps({"text": "Этот ник уже занят!"})
                        client_socket.send(self.create_packet(0x00, self.write_string(disconnect_msg)))
                        return

                # Генерируем entity ID
                entity_id = self.next_entity_id
                self.next_entity_id += 1

                # Отправляем Login Success
                success_data = (
                        self.write_string(username) +
                        self.write_string("00000000-0000-0000-0000-000000000000")
                )
                client_socket.send(self.create_packet(0x02, success_data))

                # Сохраняем игрока
                self.online_players[client_socket] = {
                    'username': username,
                    'entity_id': entity_id,
                    'address': client_socket.getpeername()
                }

                print(f"  ✓ {username} успешно залогинился")

                # Отправляем Join Game
                self.send_join_game(client_socket, entity_id)

                # Отправляем спавн
                self.send_spawn_position(client_socket)

                # Отправляем позицию
                self.send_player_position(client_socket)

                # Отправляем приветствие
                welcome_msg = json.dumps({"text": f"§aДобро пожаловать на сервер, {username}!"})
                client_socket.send(self.create_packet(0x03, self.write_string(welcome_msg)))

                # Запускаем игровой цикл
                self.game_loop(client_socket, username)

        except Exception as e:
            print(f"  Ошибка при логине: {e}")

    def game_loop(self, client_socket, username):
        """Игровой цикл"""
        print(f"  → {username} в игровом режиме")

        last_keep_alive = time.time()

        while True:
            try:
                # Отправляем keep alive каждые 2 секунды
                if time.time() - last_keep_alive > 2:
                    self.send_keep_alive(client_socket)
                    last_keep_alive = time.time()

                # Читаем пакеты с таймаутом
                client_socket.settimeout(1.0)

                packet_id, data = self.read_packet(client_socket)

                if packet_id is None:
                    continue

                if packet_id == 0x00:  # Keep Alive ответ
                    pass

                elif packet_id == 0x01:  # Chat Message
                    offset = 0
                    msg_len, offset = self.read_varint(data, offset)
                    msg_json = data[offset:offset + msg_len].decode('utf-8')

                    try:
                        msg_obj = json.loads(msg_json)
                        message = msg_obj.get('text', '')
                    except:
                        message = msg_json

                    print(f"  Чат от {username}: {message}")

                    # Обработка команд
                    if message.startswith('/'):
                        self.handle_command(client_socket, username, message)
                    else:
                        # Отправляем всем
                        chat_msg = json.dumps({"text": f"<{username}> {message}"})
                        self.broadcast_message(chat_msg, exclude=client_socket)

                elif packet_id == 0x04:  # Client Settings
                    print(f"  Получены настройки от {username}")

            except socket.timeout:
                continue
            except Exception as e:
                print(f"  Ошибка в игровом цикле {username}: {e}")
                break

        # Удаляем игрока при выходе
        if client_socket in self.online_players:
            del self.online_players[client_socket]
            leave_msg = json.dumps({"text": f"§e{username} покинул игру"})
            self.broadcast_message(leave_msg, exclude=client_socket)
            print(f"  ✗ {username} отключился")

    def handle_command(self, client_socket, username, command):
        """Обработка команд"""
        cmd = command.lower().split()[0] if command else ""

        responses = {
            '/help': "§6Доступные команды:\n§7/help, /list, /time",
            '/list': f"§6Игроки: §f{', '.join([p['username'] for p in self.online_players.values()])}",
            '/time': f"§7Время: §f{datetime.now().strftime('%H:%M:%S')}"
        }

        if cmd in responses:
            response_msg = json.dumps({"text": responses[cmd]})
            client_socket.send(self.create_packet(0x03, self.write_string(response_msg)))
        else:
            response_msg = json.dumps({"text": "§cНеизвестная команда"})
            client_socket.send(self.create_packet(0x03, self.write_string(response_msg)))

    def start(self):
        """Запуск сервера"""
        try:
            self.server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
            self.server_socket.bind((self.host, self.port))
            self.server_socket.listen(10)
            self.running = True

            print("=" * 50)
            print(f"     PyCraft Server для Minecraft 1.8.0")
            print("=" * 50)
            print(f"Хост: {self.host}")
            print(f"Порт: {self.port}")
            print(f"Версия протокола: {self.protocol_version}")
            print("-" * 50)
            print("Сервер запущен! Ожидание подключений...")
            print("=" * 50)

            while self.running:
                try:
                    client_socket, address = self.server_socket.accept()
                    client_thread = threading.Thread(
                        target=self.handle_client,
                        args=(client_socket, address)
                    )
                    client_thread.daemon = True
                    client_thread.start()

                except KeyboardInterrupt:
                    break

        except Exception as e:
            print(f"Ошибка запуска сервера: {e}")
        finally:
            self.stop()

    def stop(self):
        """Остановка сервера"""
        print("\nОстановка сервера...")
        self.running = False

        for player_socket in list(self.online_players.keys()):
            try:
                player_socket.close()
            except:
                pass

        if self.server_socket:
            self.server_socket.close()

        print("Сервер остановлен")


if __name__ == "__main__":
    server = MinecraftServer(host='0.0.0.0', port=40477)

    try:
        server.start()
    except KeyboardInterrupt:
        print("\nПолучен сигнал остановки")
        server.stop()