import unittest
from matem import Matem

class TestsCalculator(unittest.TestCase):
    
    def setUp(self):
        # Перед каждым тестом создаем чистый калькулятор
        self.calc = Matem()

    def simulate_typing(self, *buttons):
        """Вспомогательный метод: нажимает кнопки по очереди и возвращает результат после '=' """
        for btn in buttons:
            self.calc.calculate(btn)
        return self.calc.calculate("=")

    def test_01_addition_simple(self):
        self.assertEqual(self.simulate_typing("5", "+", "4"), "9")

    def test_02_addition_float(self):
        self.assertEqual(self.simulate_typing("1", ",", "5", "+", "2", ",", "5"), "4")

    def test_03_subtraction_simple(self):
        self.assertEqual(self.simulate_typing("1", "0", "-", "4"), "6")

    def test_04_subtraction_negative(self):
        self.assertEqual(self.simulate_typing("2", "-", "5"), "-3")

    def test_05_multiplication_simple(self):
        self.assertEqual(self.simulate_typing("6", "x", "6"), "36")

    def test_06_multiplication_zero(self):
        self.assertEqual(self.simulate_typing("9", "9", "x", "0"), "0")

    def test_07_division_simple(self):
        self.assertEqual(self.simulate_typing("2", "0", "/", "4"), "5")

    def test_08_division_float(self):
        self.assertEqual(self.simulate_typing("1", "0", "/", "4"), "2.5")

    def test_09_division_by_zero(self):
        self.assertEqual(self.simulate_typing("5", "/", "0"), "Деление на 0")

    def test_10_decimal_only(self):
        self.assertEqual(self.simulate_typing("3", ",", "1", "4"), "3.14")

    def test_11_percent_addition(self):
        self.assertEqual(self.simulate_typing("5", "0", "+", "1", "0", "%"), "50.1")

    def test_12_percent_standalone(self):
        self.assertEqual(self.simulate_typing("1", "0", "%"), "0.1")

    def test_13_power_xy(self):
        self.assertEqual(self.simulate_typing("2", "x^y", "3"), "8")

    def test_14_power_x2(self):
        self.assertEqual(self.simulate_typing("5", "x^2"), "25")

    def test_15_square_root(self):
        self.assertEqual(self.simulate_typing("sqrt", "1", "6", "()"), "4")

    def test_16_sine(self):
        self.assertEqual(self.simulate_typing("sin", "0", "()"), "0")

    def test_17_cosine(self):
        self.assertEqual(self.simulate_typing("cos", "0", "()"), "1")

    def test_18_logarithm_10(self):
       self.assertEqual(self.simulate_typing("log", "1", "0", "0"), "2")

    def test_19_natural_logarithm(self):
        self.assertEqual(self.simulate_typing("ln", "e", "()"), "1")

    def test_20_absolute_value(self):
        self.assertEqual(self.simulate_typing("|x|", "-", "5", "()"), "5")

    def test_21_chained_operations(self):
        self.assertEqual(self.simulate_typing("2", "+", "3", "x", "4"), "14")

    def test_22_clear_button(self):
        self.calc.calculate("5")
        self.assertEqual(self.calc.calculate("C"), "0")



if __name__ == "__main__":
    unittest.main()