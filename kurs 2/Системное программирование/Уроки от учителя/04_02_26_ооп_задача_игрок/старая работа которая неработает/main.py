import time
import game
from interface import Interface
from game import *

if __name__ == '__main__':

    #input("Добро пожаловать в игру")

    players = Player.get_default_player()
    for i in range(len(players)):
        player = players[i]
        print(i,str(player))

    n_player = 2# int(input("выберите игрока:"))
    player = players[n_player]

    # change setting player
    Interface.speed = player.speed
    def timer():
        Interface.set_text("Hello")
    Interface.set_timer(timer, 1000)
    Interface.create_enemy()

    # info text
    Interface.start_window()
