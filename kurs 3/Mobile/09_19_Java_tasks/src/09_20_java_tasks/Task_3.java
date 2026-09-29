import java.util.Locale;
import java.util.Scanner;

public class Task_3 {
    static String[] array = new String[]{
            "Яблоко",
            "Суп",
            "Фундук",
            "Орех",
            "Печенька",
            "Морковь",
    };
    static int[] prices = new int[]{
            4,
            10,
            2,
            3,
            4,
            1
    };
    static Scanner sc = new Scanner(System.in);
    public static void main(String[] args) {
        Print("Введите товары через запятую");
        String data = Input();
        var myArray = data.split(", ");
        int price = 0;
        for (String d : myArray){
            for (int i = 0; i < array.length; i++){
                if (d.toLowerCase().equals(array[i].toLowerCase())){
                    price += prices[i];
                }
            }
        }
        Print("Итоговая цена: " + price);

        sc.close();
    }

    public static void Print(String string){
        System.out.println(string);
    }

    public static String Input(){
        return sc.nextLine();
    }
}
