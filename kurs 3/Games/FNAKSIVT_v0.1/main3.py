import main2
import random as random
from ursina import time
from main2 import app, is_game, time, cam_images
app = main2.app

# Logic
rnd = random.Random()
timer_enemy = 0
PROB_SHADOW = 20
status_shadow = 0

def update():
    global timer_enemy, status_shadow, cam_images

    if main2.is_game:
        timer_enemy += time.dt

        # Active
        if timer_enemy >= 2:

            # Shadow
            prob = rnd.randint(0, PROB_SHADOW)
            if prob == 0:
                status_shadow += 1

            # Image 1_2
            if status_shadow == 1: cam_images["camb2"] = "camb2_e1.jpeg"
            else: cam_images["camb2"] = "camb2.jpeg"

            # Image 2_2
            if status_shadow == 2: cam_images["camb3"] = "camb3_e1.jpeg"
            else: cam_images["camb3"] = "camb3.jpeg"

            timer_enemy = 0
            print(f"Shadow: {status_shadow}")

app.run()