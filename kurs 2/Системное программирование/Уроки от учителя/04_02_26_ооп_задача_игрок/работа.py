import random
from typing import List, Optional


# Базовые классы
class Item:

    def __init__(self, id: int, name: str, buff: str, description: str):
        self.id = id
        self.name = name
        self.buff = buff  # Например: "+5 к силе"
        self.description = description

    def __str__(self):
        return f"{self.name} | {self.buff} | {self.description}"


class Inventory:

    def __init__(self, size: int, default_items: List[Item] = None):
        self.size = size
        self.items = default_items if default_items else []

    def add_item(self, item: Item) -> bool:
        if len(self.items) < self.size:
            self.items.append(item)
            return True
        return False

    def remove_item(self, item_id: int) -> Optional[Item]:
        for i, item in enumerate(self.items):
            if item.id == item_id:
                return self.items.pop(i)
        return None

    def show_inventory(self):
        if not self.items:
            print("  Инвентарь пуст")
        else:
            print("  Содержимое инвентаря:")
            for item in self.items:
                print(f"    {item}")


class Player:

    def __init__(self, name: str, hp: float, exp: float, lvl: int, mana: float,
                 spell_damage: float, damage: float, strength: int, agility: int,
                 intelligence: int, inventory_size: int = 5):
        self.name = name
        self.hp = hp
        self.max_hp = hp
        self.exp = exp
        self.lvl = lvl
        self.mana = mana
        self.max_mana = mana
        self.spell_damage = spell_damage
        self.damage = damage
        self.strength = strength
        self.agility = agility
        self.intelligence = intelligence
        self.inventory = Inventory(inventory_size)

        # Счетчик убийств
        self.kills = 0

    def take_damage(self, damage: float):
        self.hp -= damage
        if self.hp < 0:
            self.hp = 0

    def is_alive(self) -> bool:
        return self.hp > 0

    def heal(self, amount: float):
        self.hp = min(self.hp + amount, self.max_hp)

    def add_exp(self, amount: float) -> bool:
        self.exp += amount

        # Простая формула: каждый уровень требует на 50% больше опыта
        exp_needed = 100 * (1.5 ** (self.lvl - 1))

        if self.exp >= exp_needed:
            self.lvl += 1
            self.exp -= exp_needed
            return True  # Уровень повышен
        return False

    def level_up(self):
        print(f"\n{'=' * 50}")
        print(f"🎉 ПОЗДРАВЛЯЕМ! Уровень повышен до {self.lvl}!")
        print("Выберите атрибут для улучшения:")
        print("1. Здоровье (+20 к макс. HP)")
        print("2. Мана (+15 к макс. MP)")
        print("3. Сила (+3 к урону и силе)")
        print("4. Ловкость (+3 к урону и ловкости)")
        print("5. Интеллект (+3 к урону заклинаний и интеллекту)")
        print("6. Восстановление (полное исцеление)")

        choice = input("Ваш выбор (1-6): ")

        if choice == "1":
            self.max_hp += 20
            self.hp = self.max_hp
            print("❤️ Максимальное здоровье увеличено на 20!")
        elif choice == "2":
            self.max_mana += 15
            self.mana = self.max_mana
            print("💙 Максимальная мана увеличена на 15!")
        elif choice == "3":
            self.strength += 3
            self.damage += 3
            print("💪 Сила увеличена на 3!")
        elif choice == "4":
            self.agility += 3
            self.damage += 3
            print("⚡ Ловкость увеличена на 3!")
        elif choice == "5":
            self.intelligence += 3
            self.spell_damage += 3
            print("🧠 Интеллект увеличен на 3!")
        elif choice == "6":
            self.hp = self.max_hp
            self.mana = self.max_mana
            print("💚 Полное восстановление!")
        else:
            print("Неверный выбор, но вы получаете +5 к здоровью")
            self.max_hp += 5
            self.hp = self.max_hp

    def show_stats(self):
        print(f"\n=== {self.name} (Уровень {self.lvl}) ===")
        print(f"❤️ HP: {self.hp:.1f}/{self.max_hp:.1f}")
        print(f"💙 MP: {self.mana:.1f}/{self.max_mana:.1f}")
        print(f"⚔️ Урон: {self.damage:.1f} | Маг.урон: {self.spell_damage:.1f}")
        print(f"💪 Сила: {self.strength} | ⚡ Ловкость: {self.agility} | 🧠 Интеллект: {self.intelligence}")
        print(f"⭐ Опыт: {self.exp:.1f}")
        print(f"💀 Убийств: {self.kills}")
        self.inventory.show_inventory()


class Enemy:
    """Враг"""

    def __init__(self, name: str, hp: float, spell_damage: float, texts: List[str]):
        self.name = name
        self.hp = hp
        self.max_hp = hp
        self.spell_damage = spell_damage
        self.texts = texts  # Фразы, которые враг говорит при атаке

    def take_damage(self, damage: float):
        """Получить урон"""
        self.hp -= damage
        if self.hp < 0:
            self.hp = 0

    def is_alive(self) -> bool:
        """Проверить, жив ли враг"""
        return self.hp > 0

    def get_attack_phrase(self) -> str:
        """Получить случайную фразу для атаки"""
        return random.choice(self.texts)

    def show_status(self):
        """Показать статус врага"""
        hp_percent = (self.hp / self.max_hp) * 100
        bar_length = 20
        filled = int(bar_length * self.hp / self.max_hp)
        bar = '█' * filled + '░' * (bar_length - filled)
        print(f"\n👾 {self.name}: [{bar}] {self.hp:.1f}/{self.max_hp:.1f} HP")


# Создаем предметы (минимум 15)
def create_items():
    items = [
        Item(1, "Меч героя", "+10 к урону", "Легендарный меч древнего воина"),
        Item(2, "Посох мага", "+8 к урону заклинаний", "Усиливает магические способности"),
        Item(3, "Кожаная броня", "+15 к здоровью", "Легкая и прочная броня"),
        Item(4, "Амулет силы", "+5 к силе", "Древний амулет с рунами"),
        Item(5, "Сапоги скорости", "+7 к ловкости", "Позволяют двигаться быстрее"),
        Item(6, "Книга знаний", "+6 к интеллекту", "Содержит древние знания"),
        Item(7, "Зелье здоровья", "+30 HP при использовании", "Восстанавливает здоровье"),
        Item(8, "Эликсир маны", "+25 MP при использовании", "Восстанавливает ману"),
        Item(9, "Кольцо огня", "+12 к урону заклинаний", "Пылает магическим огнем"),
        Item(10, "Щит стража", "+20 к здоровью", "Надежный защитник"),
        Item(11, "Кинжал теней", "+9 к урону", "Идеален для скрытных атак"),
        Item(12, "Кристалл маны", "+10 к мане", "Увеличивает запас магии"),
        Item(13, "Пояс великана", "+5 к силе и здоровью", "Дает силу великана"),
        Item(14, "Перчатки ловкача", "+8 к ловкости", "Улучшают ловкость рук"),
        Item(15, "Талисман удачи", "+3 ко всем характеристикам", "Приносит удачу в бою"),
        Item(16, "Шлем мудреца", "+7 к интеллекту", "Повышает мудрость"),
        Item(17, "Плащ невидимости", "+10 к ловкости", "Делает почти невидимым"),
        Item(18, "Меч дракона", "+15 к урону", "Выкован из зуба дракона"),
        Item(19, "Книга заклинаний", "+12 к интеллекту", "Содержит мощные заклинания"),
        Item(20, "Зелье силы", "+20 к силе временно", "Дает временную силу")
    ]
    return items


# Создаем готовых персонажей
def create_players():
    players = [
        Player("Артур", 120, 0, 1, 50, 15, 25, 15, 10, 8, 5),
        Player("Мерлин", 80, 0, 1, 100, 30, 10, 5, 8, 20, 5),
        Player("Робин", 100, 0, 1, 40, 12, 20, 10, 18, 8, 5),
        Player("Громобой", 150, 0, 1, 30, 8, 30, 20, 8, 5, 5),
        Player("Эльвира", 90, 0, 1, 80, 25, 15, 8, 15, 15, 5)
    ]
    return players


# Создаем врагов
def create_enemies():
    enemies = [
        Enemy("Гоблин", 50, 5, ["Бей его!", "За мной, братья!", "Получи!"]),
        Enemy("Орк", 120, 8, ["Сила орков!", "Ты слаб!", "Умри!"]),
        Enemy("Тролль", 200, 10, ["Тролль сокрушит!", "Мясо!", "Глупый человечек!"]),
        Enemy("Тёмный маг", 80, 25, ["Тьма поглотит тебя!", "Силы зла!", "Трепещи!"]),
        Enemy("Скелет", 60, 12, ["Кости хрустят!", "Смерть неизбежна!", "Ха-ха-ха!"]),
        Enemy("Дракон", 300, 30, ["Огонь!", "Ты посмел прийти?", "Сгори!"]),
        Enemy("Бандит", 90, 6, ["Кошелек или жизнь!", "Отдай добро!", "Попался!"]),
        Enemy("Вампир", 150, 18, ["Кровь!", "Ты будешь моим ужином!", "Вечная ночь!"])
    ]
    return enemies


# Игровой процесс
class Game:
    def __init__(self):
        self.items = create_items()
        self.players = create_players()
        self.enemies = create_enemies()
        self.player = None
        self.running = True

    def choose_character(self):
        """Выбор персонажа"""
        print("\n" + "=" * 60)
        print("           🎮 ДОБРО ПОЖАЛОВАТЬ В МИНИ-ИГРУ!")
        print("=" * 60)
        print("\nВыберите персонажа:")

        for i, player in enumerate(self.players, 1):
            print(f"\n{i}. {player.name}")
            print(f"   ❤️ HP: {player.max_hp} | ⚔️ Урон: {player.damage} | 🧠 Интеллект: {player.intelligence}")

        while True:
            try:
                choice = int(input("\nВведите номер персонажа: "))
                if 1 <= choice <= len(self.players):
                    self.player = self.players[choice - 1]
                    print(f"\n✅ Вы выбрали {self.player.name}!")
                    break
                else:
                    print("❌ Неверный номер. Попробуйте снова.")
            except ValueError:
                print("❌ Введите число.")

    def battle(self, enemy: Enemy) -> bool:
        """Сражение с врагом. Возвращает True, если игрок победил"""
        print(f"\n{'=' * 50}")
        print(f"⚔️ БИТВА: {self.player.name} vs {enemy.name} ⚔️")

        while self.player.is_alive() and enemy.is_alive():
            # Показываем статус
            self.player.show_stats()
            enemy.show_status()

            # Ход игрока
            print("\nВаш ход:")
            print("1. Обычная атака")
            print("2. Магическая атака (тратит 15 маны)")
            print("3. Использовать зелье (если есть)")

            action = input("Выберите действие (1-3): ")

            # Обработка действия игрока
            if action == "1":
                damage = self.player.damage + random.uniform(-2, 5)
                enemy.take_damage(damage)
                print(f"⚔️ Вы нанесли {damage:.1f} урона!")

            elif action == "2":
                if self.player.mana >= 15:
                    damage = self.player.spell_damage + random.uniform(0, 10)
                    enemy.take_damage(damage)
                    self.player.mana -= 15
                    print(f"🔮 Вы нанесли {damage:.1f} магического урона!")
                else:
                    print("❌ Недостаточно маны!")
                    continue

            elif action == "3":
                # Поиск зелья здоровья в инвентаре
                found_potion = None
                for item in self.player.inventory.items:
                    if "Зелье здоровья" in item.name:
                        found_potion = item
                        break

                if found_potion:
                    self.player.inventory.remove_item(found_potion.id)
                    self.player.heal(30)
                    print("💚 Вы использовали зелье здоровья и восстановили 30 HP!")
                else:
                    print("❌ У вас нет зелья здоровья!")
                    continue
            else:
                print("❌ Неверный выбор, вы пропускаете ход!")

            # Проверка, жив ли враг
            if not enemy.is_alive():
                print(f"\n🎉 Враг {enemy.name} повержен!")
                return True

            # Ход врага
            print(f"\n👾 Ход {enemy.name}:")
            phrase = enemy.get_attack_phrase()
            print(f'"{phrase}"')

            damage = enemy.spell_damage + random.uniform(-3, 3)
            self.player.take_damage(damage)
            print(f"💢 Враг нанес {damage:.1f} урона!")

            if not self.player.is_alive():
                print(f"\n💔 {self.player.name} пал в бою...")
                return False

        return False

    def handle_victory(self, enemy: Enemy):
        """Обработка победы над врагом"""
        self.player.kills += 1

        # Получение опыта
        exp_gain = 50 + enemy.max_hp * 0.2
        print(f"\n✨ Вы получили {exp_gain:.1f} опыта!")

        if self.player.add_exp(exp_gain):
            self.player.level_up()

        # Выпадение предмета
        if random.random() < 0.7:  # 70% шанс выпадения предмета
            item = random.choice(self.items)
            print(f"\n📦 С врага выпал предмет: {item.name}!")

            if self.player.inventory.add_item(item):
                print(f"✅ Предмет добавлен в инвентарь")
            else:
                print("❌ Инвентарь полон! Предмет не может быть подобран")
                print("   Чтобы освободить место, можно выбросить предмет в следующий раз")

    def run(self):
        """Запуск игры"""
        self.choose_character()

        while self.running and self.player.is_alive():
            print(f"\n{'=' * 50}")
            print("Что делаем дальше?")
            print("1. 🔍 Искать врага")
            print("2. 📊 Показать статистику")
            print("3. 🚪 Выйти из игры")

            choice = input("Ваш выбор: ")

            if choice == "1":
                # Выбираем случайного врага
                enemy = random.choice(self.enemies).__class__(
                    random.choice(self.enemies).name,
                    random.choice(self.enemies).hp,
                    random.choice(self.enemies).spell_damage,
                    random.choice(self.enemies).texts
                )

                print(f"\n🔍 Вы встретили {enemy.name}!")

                if self.battle(enemy):
                    self.handle_victory(enemy)
                else:
                    print("\n💀 ИГРА ОКОНЧЕНА 💀")
                    self.running = False

            elif choice == "2":
                self.player.show_stats()

            elif choice == "3":
                print("\n👋 Спасибо за игру! До встречи!")
                self.running = False

            else:
                print("❌ Неверный выбор")


# Запуск игры
if __name__ == "__main__":
    game = Game()
    game.run()