fun main() {
    var map = mapOf("jacket" to 3, "shirt" to 5, "skarf" to 4, "hard" to 3, "drink" to 2, "short" to 4)

    var sortMap = map.entries.sortedByDescending { it.value }

    for ((k, v) in sortMap) println("Товар:$k с рейтингом $v/5")

}