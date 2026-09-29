import java.util.Scanner;

public class Task_1 {
    public static void main(String[] args) {
        Scanner sc = new Scanner(System.in);
        System.out.println("Зарегистрируйтесь!");

        System.out.println("Введите логин:");
        String login = sc.nextLine();
        while (login.length() < 4) {
            System.out.println("Неверный логин");
            login = sc.nextLine();
        }

        System.out.println("Введите email:");
        String email;
        int index = -2;
        do {
            if (index != -2)
                System.out.println("Неверный email");
            email = sc.nextLine();
            index = email.indexOf("@");
        } while (index < 1);

        while (true){
            System.out.println("Введите пароль:");
            String password = sc.nextLine();
            int d = 0;
            if (password.length() > 8)
                d++;
            for (char c : password.toCharArray())
                if (Character.isDigit(c)){
                    d++;
                    break;
                }
            for (char c : password.toCharArray())
                if (Character.isUpperCase(c)){
                    d++;
                    break;
                }
            for (char c : password.toCharArray())
                if (!Character.isLetterOrDigit(c)){
                    d++;
                    break;
                }

            System.out.println("Сложность пароля:");
            switch (d){
                case 0:
                    System.out.println("Ужасный");
                    break;
                case 1:
                    System.out.println("Низкий");
                    break;
                case 2:
                    System.out.println("Средний");
                    break;
                case 3:
                    System.out.println("Высокий");
                    break;
                case 4:
                    System.out.println("Супер надёжный");
                    break;
            }
            System.out.println("Вы уверены в этом пароле ? Д/Н");
            boolean accept = sc.nextLine().toLowerCase().contains("д");
            if (accept == false)
                continue;

            break;


        }


        System.out.println("Регистрация прошла успешно!");
        sc.close();
    }
}
