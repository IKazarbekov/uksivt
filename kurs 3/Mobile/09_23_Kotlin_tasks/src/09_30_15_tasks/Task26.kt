class User(val name: String) {
    var purchases: Int = 0
    var promoCode: String? = null

    fun addPurchase() {
        purchases++
        if (purchases >= 10 && promoCode == null) {
            promoCode = "PROMO-${name.uppercase()}-${(1000..9999).random()}"
            println("Вам выдан промокод: $promoCode")
        }
    }
}

fun main() {
    print("Введите имя: ")
    val user = User(readln())

    while (true) {
        println("\n1) Купить  2) Профиль  0) Выход")
        when (readln().toIntOrNull() ?: 0) {
            1 -> {
                user.addPurchase()
                println("Покупок: ${user.purchases}")
            }
            2 -> {
                println("Имя: ${user.name}")
                println("Покупок: ${user.purchases}")
                println("Промокод: ${user.promoCode ?: "нет"}")
            }
            0 -> break
            else -> println("Не понял")
        }
    }
}