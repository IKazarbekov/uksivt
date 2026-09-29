import random
from ensurepip import bootstrap
from turtle import Screen, Turtle


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
                 hp: float = 100,
                 exp: float = 5,
                 lvl: int = 1,
                 mana: float = 5,
                 spell_damage: float = 3,
                 damage: float = 3,
                 strenght: int = 3,
                 agility: int = 3,
                 intellect: int = 3,
                 inventory: Inventory = Inventory(3),
                 speed: int = 5):
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
        turtle = Turtle()
        turtle.up()
        turtle.shapesize(3)
        self.turtle = turtle

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
    def __init__(self, name: str, hp: float = 100, spell_damage: float = 3, text: list[str] = None, speed = 3):
        self.name = name
        self.hp = hp
        self.spellDamage = spell_damage
        self.text = text  # список фраз/диалогов врага
        self.speed = speed
        turtle = Turtle()
        turtle.up()
        self.turtle = turtle

class Bullet:
    def __init__(self, distance = 200, damage = 5, speed = 5):
        self.distance = distance
        self.damage = damage
        self.speed = speed
        turtle = Turtle()
        turtle.shape('arrow')
        self.turtle = turtle

if __name__ == '__main__':
    screen = Screen()
    player = Player("default")
    turtle = player.turtle
    enemies = list()

    # start select player
    '''players = Player.get_default_player()
    for i in range(len(players)):
        player = players[i]
        print(i,str(player))
    n_player = int(input("выберите игрока:"))
    player = players[2]'''

    # movement
    def move_forward():
        turtle.forward(player.speed)
    def move_backward():
        turtle.backward(player.speed)
    def turn_left():
        turtle.left(30)
    def turn_right():
        turtle.right(30)
    screen.onkey(move_forward,'w')
    screen.onkey(move_backward,'s')
    screen.onkey(turn_left,'a')
    screen.onkey(turn_right,'d')

    # attack
    def attack():
        bullet = Bullet()
        bullet.turtle.setheading(player.turtle.heading() + random.randint(-40, 40))
        bullet.turtle.teleport(player.turtle.xcor(), player.turtle.ycor())
        def move():
            bullet.turtle.forward(bullet.speed)
            bullet.distance -= bullet.speed
            for enemy in enemies:
                d = bullet.turtle.distance(enemy.turtle)
                print(d)
                if d < 50:
                    enemy.hp -= bullet.damage
                    if enemy.hp <= 0:
                        enemy.turtle.clear()
                        enemy.turtle.hideturtle()
                        enemies.remove(enemy)
            if bullet.distance > 0:
                screen.ontimer(move, 10)
            else:
                bullet.turtle.clear()
                bullet.turtle.hideturtle()
        move()
    screen.onkey(attack, 'space')
    screen.listen()

    # timers
    def enemy_timer():
        for enemy in enemies:
            turtle = enemy.turtle
            angle = turtle.towards(player.turtle)
            turtle.setheading(angle)
            turtle.forward(enemy.speed)


        screen.ontimer(enemy_timer, 10)

    enemy_timer()
    def info_timer():
        for enemy in enemies:
            turtle = enemy.turtle
            turtle.clear()
            turtle.write(f"{enemy.name}, hp{enemy.hp}", align='left', font=('Arial',10,'bold'))
        screen.ontimer(info_timer, 1000)
        screen.update()
    info_timer()

    # Start settings
    enemies.append(Enemy("Ильяс"))

    screen.mainloop()