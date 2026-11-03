fun main() {
    print("Логин: ")
    val login = readln()
    print("Пароль: ")
    val password = readln()

    if (login.isEmpty()) {
        println("Логин пустой — вход отклонён")
        return
    }
    if (password.length < 8) {
        println("Пароль короче 8 символов — вход отклонён")
        return
    }

    println("Вход выполнен. Добро пожаловать, $login!")

    if (password == login) {
        println("Ваш пароль совпадает с логином. Хотите поменять? (да/нет)")
        if (readln().lowercase() == "да") {
            print("Новый пароль: ")
            val newPassword = readln()
            if (newPassword.length < 8) {
                println("Новый пароль слишком короткий")
            } else if (newPassword == login) {
                println("Новый пароль не должен совпадать с логином")
            } else {
                println("Пароль успешно изменён")
            }
        } else {
            println("Пароль оставлен без изменений")
        }
    }
}