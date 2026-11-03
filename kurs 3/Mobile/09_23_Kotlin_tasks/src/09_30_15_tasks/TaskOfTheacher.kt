fun main(){
    var orders: Array<String> = arrayOf("ноутбук",
        "компьютер",
        "телефон",
        "смартфон",
        "роутер",
        "планшет",
        )

    println("Введите строку для поиска товара.")
    var str = readln()
    var count = 0
    for(order in orders){
        if (order.contains(str.lowercase())) {
            println(order)
            count++
        }
    }
    println("Кол-во элементов: $count")
}