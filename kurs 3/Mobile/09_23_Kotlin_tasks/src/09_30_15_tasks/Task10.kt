fun main() {
    var totalBonuses = 0

    while (true) {
        println("\nБонусов на счету: $totalBonuses")
        println("1) Купить  0) Выход")
        when (readln().toIntOrNull() ?: 0) {
            1 -> {
                print("Сумма заказа: ")
                val sum = readln().toDoubleOrNull() ?: 0.0
                if (sum <= 0) {
                    println("Некорректная сумма")
                    continue
                }

                var bonus = (sum * 0.03).toInt()
                if (bonus > 500) bonus = 500

                totalBonuses += bonus
                println("Начислено бонусов: $bonus")
                println("Итого бонусов: $totalBonuses")
            }
            0 -> break
            else -> println("Не понял")
        }
    }
}