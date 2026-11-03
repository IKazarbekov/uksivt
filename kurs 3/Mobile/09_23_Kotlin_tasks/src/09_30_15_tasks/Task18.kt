enum class Status { НОВИЧОК, ОПЫТНЫЙ, ГУРУ }

fun main() {
    var purchases = 0
    var status = Status.НОВИЧОК

    while (true) {
        println("\nПокупок: $purchases | Статус: $status")
        println("1) Купить  0) Выход")
        when (readln().toIntOrNull() ?: 0) {
            1 -> {
                purchases++
                val newStatus = when {
                    purchases >= 15 -> Status.ГУРУ
                    purchases >= 5 -> Status.ОПЫТНЫЙ
                    else -> Status.НОВИЧОК
                }
                if (newStatus != status) {
                    println("Статус повышен: $status → $newStatus")
                    status = newStatus
                } else {
                    println("Покупка совершена")
                }
            }
            0 -> break
            else -> println("Не понял")
        }
    }
}