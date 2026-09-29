import java.util.Scanner;

public class Task_2 {
    static Scanner sc = new Scanner(System.in);
    static String trueLogin = "kay";
    static String truePassword = "1234";
    public static void main(String[] args) {

        for (int i = 3; i > 0; i--) {
            Print("Введите логин:");
            var login = Input();

            Print("Введите пароль:");
            var password = Input();

            if (login.contains(trueLogin) && password.contains(truePassword)){
                Print("Вы вошли!");
                break;
            }
            else{
                Print("Неверный логин или пароль");
                if (i == 1){
                    Print("Попытки исчерпаны");
                    break;
                }
            }
        }

        sc.close();

    }

    public static void Print(String string){
        System.out.println(string);
    }

    public static String Input(){
        return sc.nextLine();
    }
}