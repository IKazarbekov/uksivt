from ursina import *
import random

app = Ursina()
window.title = 'Night Shift'
window.color = color.black
Text.default_font = 'font.ttf'

# Statistic
is_game = False
is_cam = False
current_cam = 'camb1'

cam_images = {
    "camb1": "camb1.png",
    "camb2": "camb2.jpeg",
    "camb3": "camb3.jpeg",
}