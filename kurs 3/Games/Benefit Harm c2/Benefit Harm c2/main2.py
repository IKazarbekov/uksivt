from ursina import *
from ursina.prefabs.first_person_controller import FirstPersonController
from ursina.shaders import unlit_shader
from ursina.shaders import lit_with_shadows_shader
app = Ursina()

window.title="HMMM"
window.color = color.black
Entity.default_shader = lit_with_shadows_shader

darkness = DirectionalLight(shadows=True)
darkness.color = color.rgb(0.1, 0.1, 0.1)
darkness.look_at(Vec3(1, -1, 1))

flashlight = SpotLight(position=(0, 2, 0), shadow=True)
flashlight.look_at(Vec3(0, 0, 1))

plane = Entity(
    model='plane',
    position=(0, -2, 0),
    color=color.gray,
    scale=30,
    collider='box',
)

wall = Entity(
    model='plane',
    position=(-4.5, 2.5, 0),
    rotation=(0, 0, 90),
    scale=9,
    color=color.green,
    collider='box',
)

cube = Entity(
    model='cube',
    scale=(3, 1, 2),
    position=(0, -2, 0),
    color=color.blue,
    collider='box',
)

"""
model = Entity(
    model="model.glb",
    position = (0, 0, 0),
    scale=1,
)"""

player = FirstPersonController(
    position=(0, 2, 0),
)

def input(key):
    if key == 'f':
        print('фонарь включён')

def update():
    global cube
    if held_keys['w']:
        print('ты идёшь вперёт')

    cube.x += 1 * time.dt

app.run()