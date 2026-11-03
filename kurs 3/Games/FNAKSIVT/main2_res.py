# МОДУЛЬ 2 - ТЕКСТУРЫ, ЗВУКИ, СОСТОЯНИЕ
from main_init import *

# Текстура кабинетов
cab_images = {
    'cab1': 'cab1.jpeg',
    'cab1l': 'cab1_l.jpeg',
}

# Текстуры камер
cam_images = {
    'cam1_1': 'cam1_1.png',
    'cam1_2': 'cam1_2.png',
    'cam1_3': 'cam1_3.png',
    'cam2_1': 'cam2_1.png',
    'cam2_2': 'cam2_2.png',
    'cam2_3': 'cam2_3.png',
    'cam3_1': 'cam3_1.png',
    'cam3_2': 'cam3_2.png',
    'cam3_3': 'cam3_3.png',
}

# -------------------- ЗВУКИ --------------------
cab_sound = Audio('cab.mp3', loop=True, autoplay=False, volume=1)
menu_sound = Audio('menu.mp3', loop=True, autoplay=False, volume=1)
tv_sound = Audio('tv.mp3', loop=False, autoplay=False, volume=0.3)

cab_sound.stop()
tv_sound.stop()
menu_sound.play()

# -------------------- СОСТОЯНИЕ ИГРЫ --------------------
is_game = False
tablet_open = False
tablet_animating = False
power = 100.0

# Расход энергии в секунду
BASE_DRAIN = 0.10
DOOR_DRAIN = 0.18
LIGHT_DRAIN = 0.20
CAMERA_DRAIN = 0.15

