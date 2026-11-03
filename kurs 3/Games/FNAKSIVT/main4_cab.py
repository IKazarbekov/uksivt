# МОДУЛЬ 4 - КАБИНЕТ ОХРАНИКА

from ursina import *
from main3_menu import *

# -------------------- КАБИНЕТ --------------------
game_cab = Entity(parent=camera.ui, enabled=False)

# Контейнер для изображений кабинета
cab_views = Entity(parent=game_cab)

# Кнопка включения камер(Событие в main5_cam.py)
cam_bt = Button(
    text='КАМЕРЫ / SPACE',
    parent=game_cab,
    position=(0, -0.43),
    scale=(0.28, 0.09),
)

# Первая сторона кабинета
cab_im = Entity(
    parent=cab_views,
    model='quad',
    texture='cab0.png',
    scale=(camera.aspect_ratio, 1),
    position=(0, 0, 1)
)

# Вторая сторона кабинета — справа от первой
cab1_im = Entity(
    parent=cab_views,
    model='quad',
    texture='cab1.jpeg',
    scale=(camera.aspect_ratio, 1),
    position=(camera.aspect_ratio, 0, 1)
)
# Свет в кабинете 1
is_light_cab1 = False
def click_light_cab1():
    global is_light_cab1, cab1_l_image
    is_light_cab1 = not is_light_cab1
    if is_light_cab1: cab1_im.texture = cab_images["cab1l"]
    else: cab1_im.texture = cab_images["cab1"]
light_bt = Button(
    text='LLLL',
    parent = cab1_im,
    position = (0.3, 0.15),
    scale=(0.01, 0.02),
    on_click = click_light_cab1
)

# Дверь кабинета 2
door_bt = Button(
    text='DDDD',
    parent = cab1_im,
    position = (0.3, 0),
    scale=0.4,
    on_click = click_light_cab1
)

# Состояние перемещения
cab_index = 0
cab_moving = False


def finish_cab_move():
    global cab_moving
    cab_moving = False

    # Показываем только нужную кнопку
    btn_cab_left.enabled = cab_index == 1
    btn_cab_right.enabled = cab_index == 0


def cab_right():
    global cab_index, cab_moving, cam_bt

    if cab_index == 0 and not cab_moving:
        cam_bt.enabled = False
        cab_moving = True
        cab_index = 1

        btn_cab_left.enabled = False
        btn_cab_right.enabled = False

        # Плавно перемещаемся вправо
        cab_views.animate_x(
            -camera.aspect_ratio,
            duration=0.5,
            curve=curve.in_out_quad
        )

        invoke(finish_cab_move, delay=0.5)


def cab_left():
    global cab_index, cab_moving, cam_bt

    if cab_index == 1 and not cab_moving:
        cam_bt.enabled = True
        cab_moving = True
        cab_index = 0

        btn_cab_left.enabled = False
        btn_cab_right.enabled = False

        # Возвращаемся обратно
        cab_views.animate_x(
            0,
            duration=0.5,
            curve=curve.in_out_quad
        )

        invoke(finish_cab_move, delay=0.5)


# Кнопка движения вправо
btn_cab_right = Button(
    parent=game_cab,
    text='▶',
    position=(camera.aspect_ratio / 2 - 0.08, 0, -2),
    scale=(0.08, 0.12),
    color=color.dark_gray,
    on_click=cab_right
)

# Кнопка движения влево
btn_cab_left = Button(
    parent=game_cab,
    text='◀',
    position=(-camera.aspect_ratio / 2 + 0.08, 0, -2),
    scale=(0.08, 0.12),
    color=color.dark_gray,
    on_click=cab_left,
    enabled=False
)