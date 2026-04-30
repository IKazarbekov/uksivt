class Item:
    def __init__(self, id: int, name: str, buff, description: str):
        self.id = id
        self.name = name
        self.buff = buff
        self.description = description

class Inventory:
    def __init__(self, size: int, default_items: list[Item] = []):
        self.size = size
        self.defaultItems = default_items
        self._items = list()

class Player:
    def __init__(self, name: str,
                 hp: float = 10,
                 exp: float = 5,
                 lvl: int = 1,
                 mana: float = 5,
                 spell_damage: float = 3,
                 damage: float = 3,
                 strenght: int = 3,
                 agility: int = 3,
                 intellect: int = 3,
                 inventory: Inventory = Inventory(3),
                 speed: int = 3):
        self.name = name
        self.hp = hp
        self.exp = exp
        self.lvl = lvl
        self.mana = mana
        self.spellDamage = spell_damage
        self.damage = damage
        self.strenght = strenght
        self.agility = agility
        self.intellect = intellect
        self.inventory = inventory
        self.speed = speed

    @staticmethod
    def get_default_player():
        return (
            Player("Джагернаут", hp=30, damage=10, speed=1),
            Player("Разведчик", hp=5, damage=3, speed=6),
            Player("Ведьма", hp=10, damage=1, spell_damage=5),
            Player("Колдун", hp=10, damage=1, spell_damage=8),
        )

    def __str__(self):
        return (f"Игрок: {self.name}\n"
                f"  ❤️ HP: {self.hp}\n"
                f"  ✨ Опыт: {self.exp}\n"
                f"  📊 Уровень: {self.lvl}\n"
                f"  💙 Мана: {self.mana}\n"
                f"  🔮 Урон заклинаниями: {self.spellDamage}\n"
                f"  ⚔️ Физический урон: {self.damage}\n"
                f"  💪 Сила: {self.strenght}\n"
                f"  🏃 Ловкость: {self.agility}\n"
                f"  🧠 Интеллект: {self.intellect}\n"
                f"  📦 Инвентарь: {self.inventory}\n"
                f"  ⚡ Скорость: {self.speed}")

class Enemy:
    def __init__(self, name: str, hp: float, spell_damage: float, text: list[str]):
        self.name = name
        self.hp = hp
        self.spellDamage = spell_damage
        self.text = text  # список фраз/диалогов врага