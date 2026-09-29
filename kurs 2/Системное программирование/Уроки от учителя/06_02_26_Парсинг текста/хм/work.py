import re
import ast
import math
import os


class MathExpressionParser:
    def __init__(self):
        # Основное регулярное выражение для поиска выражений
        # Ищем числа, операторы, скобки, игнорируем даты, IP и т.д.
        self.pattern = re.compile(r'''
            (?:^|[^\w.])  # Начало строки или не буква/точка
            (?!\d+[.:/-]\d+[.:/-]\d+)  # Исключаем даты/IP
            (?!\d+[%$€р])  # Исключаем проценты/деньги
            (?!\d{1,3}(?:\.\d{1,3}){3})  # Исключаем IP-адреса
            (?!\d+:\d+(?::\d+)?)  # Исключаем время
            (?!\d+(?:\.\d+)+[^\+\-\*/^])  # Исключаем версии ПО

            (\(?\s*  # Открывающая скобка
            -?\s*\d+(?:\.\d+)?  # Число (может быть отрицательным)
            (?:\s*[\+\-\*/^]\s*  # Оператор
            -?\s*\d+(?:\.\d+)?\s*)*  # Другие числа/операторы
            \)?)  # Закрывающая скобка

            (?=[^\w.]|$)  # Конец строки или не буква/точка
        ''', re.VERBOSE | re.MULTILINE)

    def is_valid_expression(self, expr):
        """Проверка валидности выражения"""
        expr = expr.strip()

        # Минимальная длина (без пробелов и скобок)
        clean_expr = re.sub(r'[()\s]', '', expr)
        if len(clean_expr) < 3:
            return False

        # Проверка баланса скобок
        if expr.count('(') != expr.count(')'):
            return False

        # Проверка на два оператора подряд
        if re.search(r'[\+\-\*/^]{2,}', clean_expr):
            return False

        # Проверка начала/конца на оператор (кроме унарного минуса)
        if re.match(r'^[\+\*/^]', clean_expr) or re.search(r'[\+\-\*/^]$', clean_expr):
            return False

        # Не должно быть букв (кроме 'e' для научной нотации)
        if re.search(r'[a-df-zA-Z]', expr):
            return False

        return True

    def safe_eval(self, expr):
        """Безопасное вычисление выражения"""
        try:
            # Заменяем ^ на ** для Python
            expr = expr.replace('^', '**')

            # Ограниченный eval через ast
            tree = ast.parse(expr, mode='eval')

            # Проверяем допустимые операции
            allowed_nodes = (
                ast.Expression, ast.Constant, ast.BinOp,
                ast.UnaryOp, ast.Operator, ast.Add, ast.Sub,
                ast.Mult, ast.Div, ast.Pow, ast.USub
            )

            for node in ast.walk(tree):
                if not isinstance(node, allowed_nodes):
                    raise ValueError(f"Недопустимая операция: {type(node).__name__}")

            # Вычисляем
            result = eval(compile(tree, filename='<ast>', mode='eval'))

            # Проверка на бесконечность
            if math.isinf(result) or math.isnan(result):
                return None, "Результат неопределен"

            return result, None

        except ZeroDivisionError:
            return None, "Деление на ноль"
        except (ValueError, SyntaxError, TypeError) as e:
            return None, f"Ошибка вычисления: {str(e)}"
        except Exception as e:
            return None, f"Неизвестная ошибка: {str(e)}"

    def format_number(self, num):
        """Форматирование числа для вывода"""
        if num is None:
            return "Ошибка"

        if isinstance(num, (int, float)):
            # Большие числа в научной нотации
            if abs(num) > 1e15 or (abs(num) < 1e-6 and num != 0):
                return f"{num:.2e}"

            # Целые числа с разделителями
            if isinstance(num, int) or num.is_integer():
                return f"{int(num):,}"

            # Числа с плавающей точкой
            return f"{num:.6f}".rstrip('0').rstrip('.')

        return str(num)

    def find_expressions(self, text):
        """Поиск всех потенциальных выражений в тексте"""
        expressions = []

        for match in self.pattern.finditer(text):
            expr = match.group(1)
            if self.is_valid_expression(expr):
                expressions.append((match.start(1), match.end(1), expr))

        return expressions

    def process_text(self, text):
        """Обработка текста - вычисление выражений"""
        expressions = self.find_expressions(text)

        # Сортируем по позиции (с конца к началу для корректной замены)
        expressions.sort(reverse=True)

        stats = {'found': len(expressions), 'calculated': 0, 'errors': 0}

        for start, end, expr in expressions:
            result, error = self.safe_eval(expr)

            if error is None:
                formatted_result = self.format_number(result)
                replacement = f"{expr} = {formatted_result}"
                stats['calculated'] += 1
            else:
                replacement = f"{expr} [Ошибка: {error}]"
                stats['errors'] += 1

            text = text[:start] + replacement + text[end:]

        return text, stats

    def process_file(self, input_file, output_file=None):
        """Обработка файла"""
        try:
            with open(input_file, 'r', encoding='utf-8') as f:
                text = f.read()

            processed_text, stats = self.process_text(text)

            if output_file is None:
                output_file = input_file.replace('.txt', '_processed.txt')

            with open(output_file, 'w', encoding='utf-8') as f:
                f.write(processed_text)

            return True, stats, output_file

        except FileNotFoundError:
            return False, None, "Файл не найден"
        except Exception as e:
            return False, None, str(e)


def create_test_file(filename="test_math.txt"):
    """Создание тестового файла с примерами"""
    test_content = """Примеры математических выражений:

2+2 = 4
(3*4)/2 = 6
2^10 = 1024
3.14*2 = 6.28
((5+3)*2-4)/2 = 6
((46727^6-547)*256)/29 = 8.97e+25
-5+3*2 = 1

Примеры ложных срабатываний (не должны обрабатываться):
Дата: 12.12.2022
Время: 12:30
IP: 192.168.1.1
Процент: 25%
Деньги: $100
Версия ПО: 1.2.3
Просто число: 42
Текст с точками: пример.com
"""

    with open(filename, 'w', encoding='utf-8') as f:
        f.write(test_content)

    return filename


def show_help():
    """Показать справку"""
    help_text = """
=== Справка по программе ===

Программа находит и вычисляет математические выражения в текстовых файлах.

Поддерживаемые операции:
- Числа: целые (42) и десятичные (3.14)
- Операторы: + - * / ^ (степень)
- Скобки: любые вложенные ( )

Игнорирует:
- Даты (12.12.2022, 2022-12-12)
- Время (12:30)
- IP-адреса (192.168.1.1)
- Проценты (25%)
- Денежные суммы ($100)
- Версии ПО (1.2.3)

Результаты:
- Большие числа: 8.97e+25
- Целые числа: 1,000,000
- Десятичные: 3.140000 (до 6 знаков)
"""
    print(help_text)


def self_test():
    """Самотестирование программы"""
    parser = MathExpressionParser()

    test_cases = [
        ("2+2", "2+2 = 4"),
        ("(3*4)/2", "(3*4)/2 = 6"),
        ("2^10", "2^10 = 1,024"),
        ("3.14*2", "3.14*2 = 6.280000"),
        ("-5+3*2", "-5+3*2 = 1"),
    ]

    false_positives = [
        "12.12.2022",
        "12:30",
        "192.168.1.1",
        "25%",
        "$100",
        "1.2.3",
        "42",
    ]

    print("=== Самотестирование ===")
    print("\nКорректные выражения:")
    passed = 0
    for expr, expected in test_cases:
        processed, _ = parser.process_text(expr)
        if processed.strip() == expected:
            print(f"✓ {expr} → {expected}")
            passed += 1
        else:
            print(f"✗ {expr} → {processed}")

    print("\nЛожные срабатывания (не должны обрабатываться):")
    for expr in false_positives:
        processed, _ = parser.process_text(expr)
        if expr == processed.strip():
            print(f"✓ {expr} → без изменений")
            passed += 1
        else:
            print(f"✗ {expr} → изменилось: {processed}")

    print(f"\nИтого: {passed}/12 тестов пройдено")
    return passed == 12


def main():
    """Основное меню программы"""
    parser = MathExpressionParser()

    while True:
        print("\n" + "=" * 50)
        print("Парсер математических выражений")
        print("=" * 50)
        print("1. Создать и обработать тестовый файл")
        print("2. Обработать пользовательский файл")
        print("3. Просмотреть справку")
        print("4. Выполнить самотестирование")
        print("5. Выход")
        print("-" * 50)

        choice = input("Выберите действие (1-5): ").strip()

        if choice == '1':
            filename = create_test_file()
            print(f"\nСоздан тестовый файл: {filename}")

            success, stats, output_file = parser.process_file(filename)
            if success:
                print(f"✓ Файл обработан: {output_file}")
                print(f"  Найдено выражений: {stats['found']}")
                print(f"  Вычислено: {stats['calculated']}")
                print(f"  Ошибок: {stats['errors']}")
            else:
                print(f"✗ Ошибка: {output_file}")

        elif choice == '2':
            filename = input("\nВведите путь к файлу: ").strip()

            if not os.path.exists(filename):
                print("✗ Файл не существует!")
                continue

            output_file = input("Введите путь для сохранения (Enter для авто): ").strip()
            if not output_file:
                output_file = None

            success, stats, result = parser.process_file(filename, output_file)
            if success:
                print(f"\n✓ Файл обработан: {result}")
                print(f"  Найдено выражений: {stats['found']}")
                print(f"  Вычислено: {stats['calculated']}")
                print(f"  Ошибок: {stats['errors']}")

                # Показать первые 5 строк результата
                try:
                    with open(result, 'r', encoding='utf-8') as f:
                        preview = ''.join(f.readlines()[:5])
                        print(f"\nПредпросмотр:\n{'-' * 30}")
                        print(preview)
                except:
                    pass
            else:
                print(f"✗ Ошибка: {result}")

        elif choice == '3':
            show_help()

        elif choice == '4':
            if self_test():
                print("\n✓ Все тесты пройдены успешно!")
            else:
                print("\n✗ Некоторые тесты не пройдены")

        elif choice == '5':
            print("\nВыход из программы...")
            break

        else:
            print("\n✗ Неверный выбор. Попробуйте снова.")


if __name__ == "__main__":
    main()