import java.util.ArrayList;
import java.util.Scanner;

public class Task_6 {
    static Scanner sc = new Scanner(System.in);

    static String[] catalog = {"Яблоко", "Суп", "Фундук", "Орех", "Печенька", "Морковь"};
    static int[] prices = {4, 10, 2, 3, 4, 1};
    static ArrayList<Integer> cart = new ArrayList<>();

    // зарегистрированный пользователь (по умолчанию — kay/1234)
    static String regLogin = "kay";
    static String regPassword = "1234";
    static String regName = "Kay";

    // текущая сессия
    static String login = "";
    static String name = "";

    public static void main(String[] args) {
        while (true) {
            Print("\n1 - Каталог");
            Print("2 - Корзина");
            Print("3 - Личный кабинет");
            Print("0 - Выход");
            String choice = Input();

            if (choice.equals("0")) break;
            if (choice.equals("1")) showCatalog();
            else if (choice.equals("2")) showCart();
            else if (choice.equals("3")) profile();
            else Print("Неверный выбор");
        }
    }

    static void showCatalog() {
        Print("\n--- Каталог ---");
        for (int i = 0; i < catalog.length; i++) {
            Print((i + 1) + ". " + catalog[i] + " - " + prices[i]);
        }
        Print("Введите номер товара для добавления в корзину (0 - назад):");
        int n = Integer.parseInt(Input());
        if (n >= 1 && n <= catalog.length) {
            cart.add(n - 1);
            Print(catalog[n - 1] + " добавлен в корзину");
        }
    }

    static void showCart() {
        Print("\n--- Корзина ---");
        if (cart.isEmpty()) {
            Print("Корзина пуста");
            return;
        }

        int total = 0;
        for (int i = 0; i < cart.size(); i++) {
            int idx = cart.get(i);
            Print((i + 1) + ". " + catalog[idx] + " - " + prices[idx]);
            total += prices[idx];
        }
        Print("Итого: " + total);
        Print("Введите номер товара для удаления (0 - назад):");
        int n = Integer.parseInt(Input());
        if (n >= 1 && n <= cart.size()) {
            Print(cart.get(n - 1) + " удалён");
            cart.remove(n - 1);
        }
    }

    static void profile() {
        Print("\n--- Личный кабинет ---");

        // Если не залогинен — предлагаем войти или зарегистрироваться
        if (login.isEmpty()) {
            Print("1 - Регистрация");
            Print("2 - Авторизация");
            Print("0 - Назад");
            String choice = Input();

            if (choice.equals("1")) {
                Print("Логин:");
                regLogin = Input();
                Print("Пароль:");
                regPassword = Input();
                Print("Имя:");
                regName = Input();
                Print("Регистрация успешна! Теперь войдите.");
            } else if (choice.equals("2")) {
                Print("Логин:");
                String l = Input();
                Print("Пароль:");
                String p = Input();

                if (l.equals(regLogin) && p.equals(regPassword)) {
                    login = regLogin;
                    name = regName;
                    Print("Добро пожаловать, " + name + "!");
                } else {
                    Print("Неверный логин или пароль");
                }
            }
        } else {
            // Если залогинен — показываем профиль
            Print("Логин: " + login);
            Print("Имя: " + name);
            Print("Выйти из аккаунта? (1 - да, 0 - нет)");
            if (Input().equals("1")) {
                login = "";
                name = "";
                Print("Вы вышли из аккаунта");
            }
        }
    }

    static void Print(String s) { System.out.println(s); }
    static String Input() { return sc.nextLine(); }
}