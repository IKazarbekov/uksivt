# МОДУЛЬ 6 - ЛОГИКА ВРАГОВ И ЗАПУСК
import main5_cam
import random as random
from ursina import time
from main5_cam import *

# Logic
rnd = random.Random()
timer_enemy = 0
PROB_SHADOW = 1
status_shadow = 0

def update():
    global timer_enemy, status_shadow, cam_images, cab_im
    update_buttons()

    if main5_cam.is_game:
        timer_enemy += time.dt

        # Active
        if timer_enemy >= 2:

            # Shadow
            prob = rnd.randint(0, PROB_SHADOW)
            if prob == 0:
                status_shadow += 1

            # Cab 1
            if status_shadow == 3: cab_im.texture = 'camb0_0e.png'
            else: cab_im.texture = 'cab0.png'

            # Cab 2
            if status_shadow == 4: 
                cab_images["cab1_l"] = 'cab1_le1.jpeg'
            else: cab_images["cab1_l"] = 'cab1_l.jpeg'

            # Image 1_2
            if status_shadow == 1: cam_images["cam1_2"] = "cam1_2e.png"
            else: cam_images["cam1_2"] = "cam1_2.png"

            # Image 2_2
            if status_shadow == 2: cam_images["cam1_3"] = "cam1_3e.png"
            else: cam_images["cam1_3"] = "cam1_3.png"

            timer_enemy = 0
            print(f"Shadow: {status_shadow}")

app.run()