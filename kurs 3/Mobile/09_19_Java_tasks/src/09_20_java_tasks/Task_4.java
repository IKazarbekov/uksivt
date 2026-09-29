import java.util.Random;
import java.util.Scanner;

public class Task_4 {
    static Scanner sc = new Scanner(System.in);

    public static void main(String[] args) {
        Print("Логин:");
        String login = Input();
        Print("Код на " + login + "***@mail.com");
        String code = String.valueOf(1000 + new Random().nextInt(9000));
        Print("Код: " + code);

        Print("Введите код:");
        if (!Input().equals(code)) { Print("Неверно"); return; }

        while (true) {
            Print("Новый пароль:");
            String p = Input();

            boolean big = false, small = false, digit = false;
            for (char c : p.toCharArray()) {
                if (Character.isUpperCase(c)) big = true;
                else if (Character.isLowerCase(c)) small = true;
                else if (Character.isDigit(c)) digit = true;
            }

            if (p.length() < 8 || !big || !small || !digit) {
                Print("Слабый пароль");
                continue;
            }

            Print("Повторите:");
            if (!p.equals(Input())) { Print("Не совпадает"); continue; }

            Print("Готово!");
            break;
        }
    }

    static void Print(String s) { System.out.println(s); }
    static String Input() { return sc.nextLine(); }
}