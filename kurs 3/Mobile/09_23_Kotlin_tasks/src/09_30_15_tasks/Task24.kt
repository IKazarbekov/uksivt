fun main(){

    val purchases: Map<String, String> = mapOf(
        "Яблоко"      to "15.03.2024",
        "Суп"         to "02.04.2024",
        "Фундук"      to "18.05.2024",
        "Орех"        to "07.06.2024",
        "Печенька"    to "21.07.2024",
        "Морковь"     to "03.08.2024",
        "Молоко"      to "14.09.2024",
        "Хлеб"        to "27.10.2024",
        "Сыр"         to "11.11.2024",
        "Кофе"        to "25.12.2024"
    )

    println("Введите слово для поиска")
    val word = readln()
    for((k, v) in purchases){
        if (k.lowercase().contains(word.lowercase()) or v.lowercase().contains(word.lowercase())){
            println("$k $v")
        }
    }

}