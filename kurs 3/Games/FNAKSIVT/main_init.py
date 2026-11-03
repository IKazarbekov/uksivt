# МОДУЛЬ 1 - ПЕРВИЧНЫЕ НАСТРОЙКИ

from ursina import *
import random

app = Ursina(size=(800, 600))
window.title = 'Night Shift'
window.color = color.black
Text.default_font = 'font.ttf'

# Statistic
is_game = False
status_enemy_shadow = 0
is_cam = False