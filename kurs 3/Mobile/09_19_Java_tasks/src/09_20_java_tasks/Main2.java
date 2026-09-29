import java.util.Random;
import java.util.Scanner;

public class Main2 {
    static Scanner scanner = new Scanner(System.in);
    public static void main(String[] args) {
        System.out.println("Введите кол-во символов для генерации пароля");
        int l = scanner.nextInt();
        System.out.println(createPassword(l));
    }

    public static String createPassword(int lenght){
        var build = new StringBuilder();
        var rnd = new Random();

        for (int i = 0; i < lenght; i++){
            boolean isDigit = rnd.nextBoolean();
            if (isDigit){
                int digit = rnd.nextInt(9);
                build.append("" + digit);
            }else{
                char ch = (char)rnd.nextInt((int)'a', (int)'z');
                build.append("" + ch);
            }
        }

        return build.toString();
    }
}
