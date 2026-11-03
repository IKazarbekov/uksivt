enum class City{
    Moscow, SPb, Novosibirsk
}

class Profile(val Name: String, var city: City, var balance: Int, var listBonus: MutableList<String>){
    fun addMoney(money: Int){
        balance += money
    }
}