fun isValidCode(code: String): Boolean =
    code.length == 6 && code.all { it.isLetter() }

fun main() {
    var balance = 0
    val certificates = mutableListOf<String>()

    while (true) {
        println("\nБаланс: $balance")
        println("1) Купить сертификат  2) Активировать код  0) Выход")
        when (readln().toIntOrNull() ?: 0) {
            1 -> {
                val code = (1..6).map { ('A'..'Z').random() }.joinToString("")
                certificates.add(code)
                println("Куплен сертификат. Код: $code")
            }
            2 -> {
                print("Введите код: ")
                val input = readln().uppercase()
                if (!isValidCode(input)) {
                    println("Неверный формат: 6 букв")
                } else if (input !in certificates) {
                    println("Такого сертификата нет")
                } else {
                    certificates.remove(input)
                    balance += 1000
                    println("Код активирован. Баланс +1000")
                }
            }
            0 -> break
            else -> println("Не понял")
        }
    }
}