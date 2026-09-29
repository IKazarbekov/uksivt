import java.util.Scanner;

public class Task_5 {
    static Scanner sc = new Scanner(System.in);
    static int level = 1;
    static String email = "";
    static String phone = "";

    public static void main(String[] args) {
        while (true) {
            Print("\n1 - Мой уровень");
            Print("2 - Повысить уровень");
            Print("0 - Выход");
            String choice = Input();

            if (choice.equals("0")) break;

            if (choice.equals("1")) {
                Print("Текущий уровень: " + level);
            } else if (choice.equals("2")) {
                upgrade();
            } else {
                Print("Неверный выбор");
            }
        }
    }

    static void upgrade() {
        if (level == 1) {
            Print("Введите почту:");
            email = Input();
            Print("Почта " + email + " подтверждена. Уровень 2!");
            level = 2;
        } else if (level == 2) {
            Print("Введите телефон:");
            phone = Input();
            Print("Телефон " + phone + " добавлен. Уровень 3!");
            level = 3;
        } else {
            Print("Максимальный уровень");
        }
    }

    static void Print(String s) { System.out.println(s); }
    static String Input() { return sc.nextLine(); }
}