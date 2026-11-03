fun main() {
    val history = mutableListOf<Pair<String, Double>>()

    while (true) {
        println("\n1) Пополнить  2) Купить  3) История  0) Выход")
        when (readln().toIntOrNull() ?: 0) {
            1 -> {
                print("Сумма пополнения: ")
                val sum = readln().toDoubleOrNull() ?: 0.0
                if (sum > 0) {
                    history.add("Пополнение" to sum)
                    println("Добавлено")
                } else println("Некорректная сумма")
            }
            2 -> {
                print("Сумма покупки: ")
                val sum = readln().toDoubleOrNull() ?: 0.0
                if (sum > 0) {
                    history.add("Покупка" to sum)
                    println("Добавлено")
                } else println("Некорректная сумма")
            }
            3 -> {
                if (history.isEmpty()) println("История пуста")
                else history.forEach { println("${it.first}: ${it.second}") }
            }
            0 -> break
            else -> println("Не понял")
        }
    }

    val income = history.filter { it.first == "Пополнение" }.sumOf { it.second }
    val expense = history.filter { it.first == "Покупка" }.sumOf { it.second }

    println("\n=== Итог ===")
    println("Пополнений: ${history.count { it.first == "Пополнение" }} на сумму $income")
    println("Покупок: ${history.count { it.first == "Покупка" }} на сумму $expense")
}