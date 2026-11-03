enum class Category {
    FRUITS, VEGETABLES, DAIRY, MEAT, DRINKS
}

data class Product(val name: String, val category: Category, val price: Double)

val products = listOf(
    Product("Яблоко", Category.FRUITS, 50.0),
    Product("Банан", Category.FRUITS, 80.0),
    Product("Апельсин", Category.FRUITS, 120.0),
    Product("Морковь", Category.VEGETABLES, 30.0),
    Product("Картофель", Category.VEGETABLES, 25.0),
    Product("Огурец", Category.VEGETABLES, 60.0),
    Product("Молоко", Category.DAIRY, 70.0),
    Product("Сыр", Category.DAIRY, 350.0),
    Product("Йогурт", Category.DAIRY, 90.0),
    Product("Курица", Category.MEAT, 250.0),
    Product("Говядина", Category.MEAT, 550.0),
    Product("Свинина", Category.MEAT, 400.0),
    Product("Кола", Category.DRINKS, 100.0),
    Product("Сок", Category.DRINKS, 150.0),
    Product("Вода", Category.DRINKS, 40.0)
)

fun filterProducts(
    category: Category? = null,
    minPrice: Double? = null,
    maxPrice: Double? = null
): List<Product> = products.filter { p ->
    (category == null || p.category == category) &&
            (minPrice == null || p.price >= minPrice) &&
            (maxPrice == null || p.price <= maxPrice)
}

fun printProducts(list: List<Product>) {
    if (list.isEmpty()) {
        println("Ничего не найдено")
        return
    }
    list.forEach { println("${it.name} | ${it.category} | ${it.price}") }
}

fun main() {
    while (true) {
        println("\n1) По категории  2) По цене  3) Всё  0) Выход")
        when (readln().toIntOrNull() ?: 0) {
            1 -> {
                println("Категории: ${Category.values().joinToString()}")
                val cat = Category.values().find { it.name == readln().uppercase() }
                if (cat == null) println("Неизвестная категория")
                else printProducts(filterProducts(category = cat))
            }
            2 -> {
                print("Мин. цена: ")
                val min = readln().toDoubleOrNull()
                print("Макс. цена: ")
                val max = readln().toDoubleOrNull()
                printProducts(filterProducts(minPrice = min, maxPrice = max))
            }
            3 -> {
                println("Категория (Enter — пропустить):")
                val cat = Category.values().find { it.name == readln().uppercase() }
                print("Мин. цена: ")
                val min = readln().toDoubleOrNull()
                print("Макс. цена: ")
                val max = readln().toDoubleOrNull()
                printProducts(filterProducts(cat, min, max))
            }
            0 -> break
            else -> println("Не понял")
        }
    }
}