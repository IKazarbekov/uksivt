fun main() {
    var balance = 0

    while (true) {
        println("\nБаланс: $balance")
        println("1) Пополнить  0) Выход")
        when (readln().toIntOrNull() ?: 0) {
            1 -> {
                print("Сумма пополнения: ")
                val sum = readln().toIntOrNull() ?: 0
                if (sum < 10 || sum > 10000) {
                    println("Пополнение отклонено: сумма должна быть от 10 до 10000")
                } else {
                    balance += sum
                    println("Баланс пополнен на $sum")
                }
            }
            0 -> break
            else -> println("Не понял")
        }
    }
}