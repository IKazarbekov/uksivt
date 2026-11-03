fun main() {
    var foods: Array<String> = arrayOf("Eggs", "Bread", "Milk")
    var drinks: Array<String> = arrayOf("Drink", "Scarf", "Jacket")
    var tehniks: Array<String> = arrayOf("Car", "Bus")
    var all = arrayOf(foods, drinks, tehniks)

    while (true) {
        print("Выберите каталог: 1)еда, 2)одежда, 3)техника\nвведите цифру:")
        var number = readln()?.toIntOrNull()
        when (number) {
            in 1..2 -> for (item in all[number!! - 1]) println(item)
            else -> println("Нету такого")
        }
    }
}