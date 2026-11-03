# МОДУЛЬ 3 - ГЛАВНОЕ МЕНЮ и ИНТРО МЕНЮ

from ursina import *
from main2_res import *

# -------------------- ГЛАВНОЕ МЕНЮ --------------------
start_menu = Entity(parent=camera.ui)
start_im = Entity(
    parent=start_menu,
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

# -------------------- МЕНЮ НОЧИ --------------------
title_menu = Entity(parent=camera.ui)
title_menu.enabled = False
info = Text(
    '---',
    parent=title_menu,
    position=(0, 0.2),
    scale=4,
    origin=(0, 0),
    color=color.white,
)

