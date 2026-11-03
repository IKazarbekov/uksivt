# МОДУЛЬ 5 - КАМЕРЫ

from ursina import *
from main4_cab import *
import main4_cab

# -------------------- ЭКРАН КАМЕР --------------------
game_cam = Entity(parent=camera.ui, enabled=False)
game_cam.y = 0
cam_im = Entity(
    parent=game_cam,
    model='quad',
    texture=cam_images['cam1_1'],
    scale=(camera.aspect_ratio, 1),
    z=1
)

# Текст энергии
power_text = Text(
    parent=camera.ui,
    text='Энергия: 100%',
    position=(-0.86, 0.45),
    scale=1.5,
    color=color.white,
    enabled=False
)

# -------------------- ЭФФЕКТ СМЕНЫ КАМЕРЫ --------------------
def change_cam(cam_name):
    def changer():
        if cam_name not in cam_images:
            print(f'Не найдена камера: {cam_name}')
            return

        original_color = cam_im.color
        original_pos = cam_im.position
        original_scale = cam_im.scale

        def glitch_step():
            if not game_cam.enabled:
                return
            cam_im.color = color.rgba(
                random.randint(100, 255),
                random.randint(100, 255),
                random.randint(100, 255),
                255
            )
            cam_im.x = original_pos.x + random.uniform(-0.03, 0.03)
            cam_im.y = original_pos.y + random.uniform(-0.02, 0.02)
            cam_im.scale = (
                original_scale.x * random.uniform(0.97, 1.03),
                original_scale.y * random.uniform(0.97, 1.03)
            )

        def finish():
            cam_im.texture = cam_images[cam_name]
            cam_im.color = original_color
            cam_im.position = original_pos
            cam_im.scale = original_scale
            tv_sound.stop()

        tv_sound.stop()
        tv_sound.play()
        for i in range(10):
            invoke(glitch_step, delay=i * 0.05)
        invoke(finish, delay=0.5)

    return changer

# -------------------- ПЛАНШЕТ: ПРОБЕЛ --------------------
def finish_tablet_open():
    global tablet_animating
    tablet_animating = False


def close_tablet_finished():
    global tablet_animating
    game_cam.enabled = False
    game_cam.y = 0
    game_cab.enabled = True
    tablet_animating = False

# Включение камер
def toggle_tablet():
    global tablet_open, tablet_animating

    if not is_game or tablet_animating or power <= 0:
        return

    tablet_animating = True
    tablet_open = not tablet_open

    if tablet_open:
        game_cab.enabled = False
        game_cam.enabled = True
        game_cam.y = -1.15
        change_cam('cam1_1')()
        game_cam.animate_y(0, duration=0.3, curve=curve.out_cubic)
        invoke(finish_tablet_open, delay=0.3)
    else:
        game_cam.animate_y(-1.15, duration=0.3, curve=curve.in_cubic)
        invoke(close_tablet_finished, delay=0.3)


def to_cam():
    if not tablet_open:
        toggle_tablet()


def to_cab():
    if tablet_open:
        toggle_tablet()

# -------------------- ПЕРЕХОДЫ МЕЖДУ ЭКРАНАМИ --------------------
def begin():
    global is_game, power, tablet_open, tablet_animating

    title_menu.enabled = False
    start_menu.enabled = False
    game_cam.enabled = False
    game_cab.enabled = True
    game_cam.y = 0

    is_game = True
    tablet_open = False
    tablet_animating = False
    power = 100.0
    power_text.text = 'Энергия: 100%'
    power_text.enabled = True

    menu_sound.stop()
    cab_sound.play()


def play():
    start_menu.enabled = False
    title_menu.enabled = True
    info.text = 'Ночь 1'
    menu_sound.stop()

# -------------------- КНОПКИ МЕНЮ И КАБИНЕТА --------------------
play_button = Button(
    text='Играть',
    parent=start_menu,
    on_click=play,
    scale=(0.2, 0.1)
)
cam_bt.on_click = to_cam

begin_button = Button(
    text='Нажмите, чтобы начать',
    parent=title_menu,
    position=(0, 0),
    on_click=begin,
    scale=2,
)

cab_bt = Button(
    text='В кабинет',
    parent=game_cam,
    position=(0, -0.45),
    scale=(0.2, 0.08),
    on_click=to_cab,
)

# -------------------- КНОПКИ КАМЕР --------------------
map_im = Entity(
    parent=game_cam,
    model='quad',
    texture='map.png',
    position=(0.63, 0.15),
    scale=(0.5, 0.65)
)

cam_b1 = Button('1-1', parent=game_cam, position=(0.83, 0.03), scale=(0.10, 0.05), on_click=change_cam('cam1_1'))
cam_b2 = Button('1-2', parent=game_cam, position=(0.83, -0.03), scale=(0.10, 0.05), on_click=change_cam('cam1_2'))
cam_b3 = Button('1-3', parent=game_cam, position=(0.83, -0.1), scale=(0.10, 0.05), on_click=change_cam('cam1_3'))
floor1_tx = Text('Этаж 1', parent=game_cam, position=(0.77, -0.18))

cam2_b1 = Button('2-1', parent=game_cam, position=(0.44, 0.36), scale=(0.10, 0.05), on_click=change_cam('cam2_1'))
cam2_b2 = Button('2-2', parent=game_cam, position=(0.73, 0.3), scale=(0.10, 0.05), on_click=change_cam('cam2_2'))
cam2_b3 = Button('2-3', parent=game_cam, position=(0.73, -0.1), scale=(0.10, 0.05), on_click=change_cam('cam2_3'))
floor2_tx = Text('Этаж 2', parent=game_cam, position=(0.67, -0.18))

cam3_b1 = Button('3-1', parent=game_cam, position=(0.44, 0.4), scale=(0.10, 0.05), on_click=change_cam('cam3_1'))
cam3_b2 = Button('3-2', parent=game_cam, position=(0.57, 0.3), scale=(0.10, 0.05), on_click=change_cam('cam3_2'))
cam3_b3 = Button('3-3', parent=game_cam, position=(0.57, -0.1), scale=(0.10, 0.05), on_click=change_cam('cam3_3'))
floor3_tx = Text('Этаж 3', parent=game_cam, position=(0.57, -0.18))

you = Button(
    text='ВЫ',
    parent=game_cam,
    position=(0.83, -0.15),
    scale=(0.10, 0.05),
)

# -------------------- УПРАВЛЕНИЕ И ЭНЕРГИЯ --------------------
def input(key):
    if key == 'space' and main4_cab.cab_index == 0:
        toggle_tablet()


def update_buttons():
    global power, is_game, tablet_open, tablet_animating

    if not is_game:
        return

    power_text.text = f'Энергия: {int(power)}%'
