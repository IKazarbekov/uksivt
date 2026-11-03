import math
import re

class Matem:
    def __init__(self):
        self.expression = ""

    def calculate(self, msg):
        # 1. Обработка очистки и удаления
        if msg == "C":
            self.expression = ""
            return "0"

        if msg == "delete":
            self.expression = self.expression[:-1]
            return self.expression if self.expression else "0"

        # Проверяем, есть ли в названии кнопки "enter" (например, "%enter")
        is_enter = "enter" in msg.lower()
        # Очищаем название кнопки от "enter", чтобы получить саму команду (например, "%")
        clean_msg = msg.lower().replace("enter", "") if is_enter else msg

        # 2. СПЕЦИАЛЬНАЯ ЛОГИКА ДЛЯ ПРОЦЕНТОВ
        if clean_msg == "%":
            # Ищем последнее число
            match = re.search(r'(\d+\.?\d*)$', self.expression)
            if match:
                last_number_str = match.group(1)
                new_number = float(last_number_str) / 100
                
                if new_number.is_integer():
                    new_number = int(new_number)
                
                self.expression = self.expression[:-len(last_number_str)] + str(new_number)

            # Если была нажата кнопка "%enter", сразу вычисляем результат
            if is_enter:
                return self._evaluate()
            # Иначе просто показываем обновленное выражение
            return self.expression

        # 3. Обработка Enter и Равно
        # 3. Обработка Enter и Равно
        if clean_msg == "=" or is_enter:
            
            return self._evaluate()

        # 4. Словарь замен
        mapping = {
            "sin": "sin(", "cos": "cos(", "tan": "tan(",
            "ln": "log(", "log": "log10(", "e^x": "exp(",
            "|x|": "abs(", "sqrt": "sqrt(", "1/x": "1/(",
            "x^2": "**2", "x^y": "**", "p": "pi", "e": "e",
            "x": "*", ",": ".", "+/-": "*(-1)"
        }

        # 5. Скобки
        if clean_msg == "x^2" or clean_msg == "x^y":
            if not(self.expression and (self.expression[-1].isdigit() or self.expression[-1] == ')')):
                return self.expression
        if clean_msg == "()":
            mapped_msg = ")" if self.expression.count("(") > self.expression.count(")") else "("
        else:
            mapped_msg = mapping.get(clean_msg, clean_msg)

        # 6. Защита от ввода после ошибки
        if self.expression == "Ошибка" or self.expression.startswith("Деление на"):
            self.expression = ""

        # 7. Добавляем символ
        if self.expression == "0" and clean_msg.isdigit():
            self.expression = mapped_msg
        else:
            self.expression += mapped_msg

        return self.expression

    def _evaluate(self):
        if not self.expression:
            return "0"
        
        # --- БЛОК АВТО-УМНОЖЕНИЯ ---
        expr = self.expression
        
        # 1. Ставим * между числом и скобкой/буквой.
        # (?<![a-zA-Z0-9]) — защита, чтобы не ломать функции вроде log10
        expr = re.sub(r'(?<![a-zA-Z0-9])(\d+(?:\.\d+)?)(\(|[a-zA-Z])', r'\1*\2', expr)
        
        # 2. Ставим * между закрывающей скобкой и числом, буквой или другой скобкой
        expr = re.sub(r'\)(\(|[a-zA-Z]|\d)', r')*\1', expr)
        
        # --- НОВЫЙ БЛОК: Автодобавление недостающих скобок ---
        open_count = expr.count("(")
        close_count = expr.count(")")
        
        # Если открытых скобок больше, чем закрытых, дописываем ")" в конец
        if open_count > close_count:
            expr += ")" * (open_count - close_count)

        # Обновляем выражение, чтобы пользователь видел, как калькулятор его понял
        self.expression = expr 

        allowed_names = {
            "sin": math.sin, "cos": math.cos, "tan": math.tan,
            "log": math.log, "log10": math.log10, "exp": math.exp,
            "sqrt": math.sqrt, "abs": abs, "pi": math.pi, "e": math.e
        }
        
        try:
            # Вычисляем уже исправленное выражение
            result = eval(expr, {"__builtins__": {}}, allowed_names)
            if isinstance(result, float) and result.is_integer():
                result = int(result)
            self.expression = str(result)
            return self.expression
        except ZeroDivisionError:
            self.expression = ""
            return "Деление на 0"
        except Exception:
            self.expression = ""
            return "Ошибка"