from main import *

# SOUND
cab_sound = Audio(
    'cab.mp3',
    loop=True,
    autoplay=False,
    volume=1
)
menu_sound = Audio(
    'menu.mp3',
    loop=True,
    volume=1
)
tv_sound = Audio(
    'tv.mp3',
    volume=0.3
)
cab_sound.stop()
tv_sound.stop()
menu_sound.play()

# START MANU
start_menu = Entity(parent=camera.ui)
start_im = Entity(
    parent = start_menu,
    model='quad',
    texture='menu.png',
    scale=(camera.aspect_ratio, 1),
    z=1
)
title = Text(
    'Пять ночей в УКСИВТ',
    parent=start_menu,
    position=(0, 0.2),
    color=color.white,
    origin=(0, 0),
    scale=4
)
def play():
    start_menu.enabled = False
    title_menu.enabled = True
    info.text = "Ночь 1"
    menu_sound.stop()
play_button = Button(
    text='Играть',
    parent=start_menu,
    on_click=play,
    scale=(0.2, 0.1)
)

# TITLE MENU
title_menu = Entity(parent=camera.ui)
title_menu.enabled = False
info = Text(
    '---',
    parent = title_menu,
    position=(0, 0.2),
    scale=4,
    origin=(0, 0),
    color=color.white,
)
def begin():
    global is_game
    is_game = True
    title_menu.enabled = False
    game_cab.enabled = True
    cab_sound.play()
begin_button = Button(
    'Нажмите, чтобы начать',
    parent = title_menu,
    position = (0, 0),
    on_click = begin,
    scale = 2,
)

# GAME PLAY CABINET
game_cab = Entity(parent=camera.ui)
game_cab.enabled = False
cab_im = Entity(
    parent=game_cab,
    model = 'quad',
    texture = 'cab0.png',
    scale=(camera.aspect_ratio, 1),
    z=1
)
def to_cam():
    game_cab.enabled = False
    game_cam.enabled = True
    change_cam(current_cam)()
    is_cam = True
cam_bt = Button(
    parent=game_cab,
    position = (-0.45, -0.1),
    scale = (0.4, 0.2),
    on_click = to_cam,
)

# GAMEPLAY CAMERA
game_cam = Entity(parent=camera.ui)
game_cam.enabled = False
def to_cab():
    game_cam.enabled = False
    game_cab.enabled = True
    is_cam = False
    
cam_im = Entity(
    parent = game_cam,
    model='quad',
    texture='camb1',
    scale=(camera.aspect_ratio, 1),
    z=1
)
cab_bt = Button(
    parent=game_cam,
    position = (0, -0.45),
    scale = (0.7, 0.1),
    on_click = to_cab,
)
map_b = Entity(
    parent = game_cam,
    model='quad',
    texture='map_b.png',
    scale=(0.15, 0.3),
    position=(0.8, 0),
    z=0.9,
)
def change_cam(cam: str):
    def changer():

        original_color = cam_im.color
        original_pos = cam_im.position
        original_scale = cam_im.scale

        def glitch_step():
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
            cam_im.texture = cam_images[cam]
            cam_im.color = original_color
            cam_im.position = original_pos
            cam_im.scale = original_scale
            tv_sound.stop()

        tv_sound.play()
        # Глитч каждые 0.05 секунды в течение 2 секунд
        for i in range(10):
            invoke(glitch_step, delay=i * 0.05)

        invoke(finish, delay=0.5)

    return changer
cam_b1 = Button(
    'cam_b1',
    parent=game_cam,
    position=(0.8, 0.1),
    scale = (0.1, 0.05),
    on_click = change_cam('camb1'),
)
cam_b2 = Button(
    'cam_b2',
    parent=game_cam,
    position=(0.8, 0.0),
    scale = (0.1, 0.05),
    on_click = change_cam('camb2'),
)
cam_b3 = Button(
    'cam_b3',
    parent=game_cam,
    position=(0.8, -0.06),
    scale = (0.1, 0.05),
    on_click = change_cam('camb3'),
)
you = Button(
    'ВЫ',
    parent=game_cam,
    position=(0.8, -0.13),
    scale = (0.1, 0.05),
)
