package `09_30_15_tasks`

fun main(args: Array<String>) {
    println("Регистрация")

    var name: String
    var age: Int
    var email: String
    var password: String

    do {
        print("введите имя:")
        name = readln()
        if (name.count() < 3)
            println("Не менее 3 символов")
        else break
    } while (true)

    do {
        print("введите возраст:")
        age = readln()?.toIntOrNull() ?: 0
        if (16 > age || age > 100)
            println("Не корректный возраст")
        else break
    } while (true)

    do {
        print("введите email:")
        email = readln()
        if (!email.contains("@"))
            println("Не корректный email")
        else break
    } while (true)

    do {
        print("введите пароль:")
        password = readln()
        if (password.count() < 8)
            println("Лёгкий пароль")
        else break
    } while (true)

    print("УСПЕШНО !")
}