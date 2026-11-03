fun main() {
    var orders = arrayOf("Eggs", "Bread", "Milk", "Drink", "Scarf", "Jacket")
    var trash: MutableList<String> = mutableListOf("Bread", "Milk", "Drink")

    while (true) {
        println("Выберите действие: 1) Посмотреть доступные товары, 2) Удалить, 3) Добавить, 4) Посмотреть корзину, 5) Очитстить корзину полностью")
        val n_action = readln().toIntOrNull() ?: 0
        when (n_action) {
            1 -> {
                for (i in 0..orders.lastIndex) {
                    println("$i : ${orders[i]}")
                }
            }
            2 -> {
                println("Введите цифру товара из корзины")
                var i = readln()?.toIntOrNull() ?: 0
                if ((0..trash.lastIndex).toList().contains(i)) {
                    trash.removeAt(i)
                    println("УДАЛЕН")
                }
                else println("Нету такого в корзине")
            }
            3 -> {
                if (trash.count() > 8)
                {
                    println("Корзина полная")
                    continue
                }
                println("Введите цифру товара из корзины")
                var i = readln()?.toIntOrNull() ?: 0
                if ((0..orders.lastIndex).toList().contains(i)) {
                    trash.add(orders[i])
                    println("Добавлено")
                }else println("нет товара с таким индексом")
            }
            4 -> {
                if (trash.count() == 0)
                    println("Корзина пуста")
                else
                    for (i in 0..trash.lastIndex) {
                        println("$i : ${trash[i]}")
                    }
            }
            5 -> {
                trash.clear()
                println("Корзина очищена")
            }
            else -> println("Не понял")
        }
    }
}