# ======================== INIT ========================
from ursina.prefabs.first_person_controller import FirstPersonController
from ursina.shaders import lit_with_shadows_shader
from ursina import *
from ursina.models.procedural.cylinder import Cylinder
import random as rnd

# INIT
app = Ursina(vsync=False)
window.color=color.rgb(0, 0, 0)

# ======================== OBJECTS ========================
# class
class Object(Entity):
    def __init__(self, x, y, z):
        size = max(0.1, rnd.random())
        self.default_color = color.red
        self.bio_name = 'Сгусток крови'
        self.is_catch = False
        self.dx = self.dy = self.dz = 0
        self.tk = 0
        super().__init__(model='sphere', position=(x, y, z), scale=size,
                         color=self.default_color, collider='box')
    def update(self):
        if pause_menu.enabled: return
        if self.is_catch: return
        self.position += Vec3(self.dx, self.dy, self.dz)
        self.tk += 1
        if self.tk < n_skeep: return
        else: self.tk = 0

        if self.dx != 0 and self.dy != 0:
            velocity = Vec3(self.dx, self.dy, self.dz)
            distance_to_move = velocity.length() * time.dt
            if distance_to_move > 0:
                hit = raycast(
                    self.world_position,
                    direction=velocity.normalized(),
                    distance=distance_to_move + self.scale_x / 2,
                    ignore=[self],
                    debug=False
                )
                if hit.hit:
                    # Отражаем скорость от поверхности
                    normal = hit.normal
                    velocity = velocity - 2 * velocity.dot(normal) * normal

                    velocity *= 0.7
                    # Сохраняем новую скорость
                    self.dx = velocity.x
                    self.dy = velocity.y
                    self.dz = velocity.z
                    # Немного отодвигаем объект от стены
                    self.position += normal * 0.02
        
        if self.dz < speed_blood:
            self.dz += 0.1 * time.dt * n_skeep
        if self.z > 90:
            destroy(self)
        if self.dx > 0: self.dx -= 0.001
        if self.dy > 0: self.dy -= 0.001
        if self.dz > 0: self.dz -= 0.001

    def light(self, is_light):
        if is_light: self.color = self.default_color * 1.8
        else: self.color = self.default_color

class Bacteria(Object):
    def __init__(self, x, y, z):
        super().__init__(x, y, z)
        self.default_color = color.green
        self.color = self.default_color
        self.bio_name = 'Микроб'
        self.model='Bacteria.obj'

    def update(self):
        if pause_menu.enabled: return
        self.rotation_y += 10 * time.dt
        self.rotation_x += 10 * time.dt
        self.rotation_z += 10 * time.dt
        super().update()

class Oxygen(Object):
    def __init__(self, x, y, z):
        super().__init__(x, y, z)
        self.default_color = color.white
        self.color = self.default_color
        self.alpha = 0.5
        self.bio_name = 'Кислород'

class Remain(Object):
    def __init__(self, x, y, z):
        super().__init__(x, y, z)
        self.default_color = color.black
        self.color = self.default_color
        self.bio_name = 'Клеточный остаток'
        self.model = 'Remain.obj'
        self.double_sided = True

class CapillaryBox(Entity):
    def __init__(self, position):
        super().__init__(model='cube', color=color.clear, position=position, scale=10, collider='box')

class Eritrocit(Object):
    def __init__(self, x, y, z, has_oxygen = True):
        super().__init__(x, y, z)
        self.model='Eritrocit.obj'
        self.color=color.red
        self.collider='box'
        self.rotation_y = rnd.randint(0, 90)
        self.rotation_x = rnd.randint(0, 90)
        self.rotation_z = rnd.randint(0, 90)
        self.bio_name='Эритроцит'
        if has_oxygen: 
            o = Oxygen(0, 0, 0)
            o.parent = self
            o.position = (0, 0, 0)
            o.is_catch = True

    def update(self):
        if pause_menu.enabled: return
        self.rotation_y += 10 * time.dt
        self.rotation_x += 10 * time.dt
        self.rotation_z += 10 * time.dt
        super().update()

# ======================== STATISTIC ========================
speed_blood = 5000
spawn_info = { # count, tick
    Bacteria: [0.3, 0],
    Remain: [0.3, 0],
    Oxygen: [0.3, 0],
    Eritrocit: [0.3, 0],
    Object: [0.3, 0]
}
n_skeep = 4

# ======================== SOUND ========================
start_sound = Audio('start.mp3', valume=1, autoplay=True, loop=True)
play_sound = Audio('play.flac', valume=1, autoplay=False)
hit_sound = Audio('hit.mp3', valume=1, autoplay=False)
water_sound = Audio('water.mp3', valume=1, autoplay=False, loop=True)
ball_sound = Audio('ball.mp3', valume=1, autoplay=False)

# ======================== PLACE ========================
# LIGHT
Entity.default_shader = lit_with_shadows_shader
darkness = DirectionalLight(shadows=True)
darkness.color = color.rgb(0.4, 0.4, 0.4)
darkness.look_at(Vec3(1, -1, 1))

# ROOM BLOOD — ДЛИННЫЙ ТУННЕЛЬ
room_blood = Entity(position=(0, 0, 0))
tonel = Entity(
    model = 'tonel.obj',
    texture='perlin_noise',
    color=color.rgb(1, 0, 0),
    scale=1,
    collider='mesh',
)
capillary_box = CapillaryBox((-13, -4, 60))
class Tmp(Entity):
    def __init__(self):
        super().__init__(position=(-4, -3, 60), scale = 2, color=color.yellow, model='cube', collider='box')
    def update(self):
        self.rotation_y += time.dt * 20
        
Tmp()
# ======================== LOGIC ========================
# PLAYER — ПЛАВАНИЕ ПОД ВОДОЙ
camera.clip_plane_far = 130
player = Entity()
spheres_visibility = [Entity(model='sphere', parent=player, scale=i*3, alpha=i/100, double_sided=True, color=color.black) for i in range(10, 70, 10)]
current_obj = None
game_screen = Entity(parent=camera.ui, enabled=False)
current_obj_text = Text('o', parent=game_screen, scale=2, position=(0.1, 0))
target_tx = Text('', parent=game_screen, position=(0.3, 0.5), origin=(0.3, 0.3), scale=2)
catch_tx = Text('', parent=game_screen, position=(0, -0.4), origin=(0.5, 0.5), scale=2)
def set_target(target_text: str):
    target_tx.text = target_text
    target_tx.scale = 2
    target_tx.position = (0, 0)
catch_obj = None
def uncatch():
    global catch_obj
    if catch_obj:
        catch_obj.is_catch = False
        catch_obj.world_parent = scene
        catch_obj = None
        catch_tx.text = ''
def throw():
    global catch_obj
    if catch_obj:
        obj = catch_obj

        obj.is_catch = False
        obj.world_parent = scene
        obj.parent = scene

        obj.dx = camera.forward.x * 0.2
        obj.dy = camera.forward.y * 0.2
        obj.dz = camera.forward.z * 0.2

        catch_obj = None
        catch_tx.text = ''
def catch(obj: Entity):
    global catch_obj
    if catch_obj is None:
        catch_obj = obj
        catch_obj.is_catch = True
        catch_obj.parent = camera
        catch_obj.position = (0, -0.5, 1.5)
        catch_tx.text = catch_obj.bio_name
        catch_obj.dx = catch_obj.dy = catch_obj.dz = 0
    else: uncatch()

tk = 0
def update():
    global current_obj, target_tx, current_level_update, oxygen_tick, spawn_info, tk, n_skeep
    if pause_menu.enabled: return

    # level
    if current_level_update: current_level_update()

    # skeep
    tk += 1
    if tk < n_skeep: return
    else: tk = 0

    player.z += 1 * time.dt

    # spawn objects
    for cls in spawn_info.keys():
        spawn_info[cls][1] += time.dt
        if spawn_info[cls][0] == 0: continue
        if spawn_info[cls][1] > 1 / spawn_info[cls][0]:
            spawn_info[cls][1] = 0
            for _ in range(n_skeep):
                x, y = rnd.randint(-3, 3), rnd.randint(-5, 4)
                z = -100 + rnd.randint(-5, 5)
                cls(x, y, z)

    hit = raycast(
        camera.world_position,
        camera.forward,
        distance=5,
        ignore=[player]
    )
    # current object
    if current_obj:
        current_obj.light(False)
    current_obj = None
    if hit.hit:
        if isinstance(hit.entity, Object):
            current_obj = hit.entity
            current_obj.light(True)
            current_obj_text.text = current_obj.bio_name
        if isinstance(hit.entity, Tmp):
            destroy(hit.entity)
    else: current_obj_text.text = ''

    # target
    if target_tx.y < 0.4: target_tx.y += time.dt * 0.5
    if target_tx.scale[0] > 1: target_tx.scale -= time.dt * 0.5

    # move and torch
    move(Bacteria, Eritrocit, 3)
    move(Eritrocit, CapillaryBox, 20)
    erit = touch(Eritrocit, tonel)
    if erit and not erit.is_catch: destroy(erit)

    # end maps
    if player.z > 80: player.z = -90
    elif player.z < -90: player.z = 80

def input(key):
    if pause_menu.enabled: return
    # remove object
    match key:
        case 'left mouse down':
            if catch_obj: throw()
            else:
                hit = raycast(camera.world_position, camera.forward, distance=5, ignore=[player])
                if hit.hit and isinstance(hit.entity, Object):
                    e = hit.entity
                    Remain(*e.position).scale = e.scale
                    destroy(e)
                    hit_sound.play()
        case 'right mouse down':
            if catch_obj: uncatch()
            else: 
                hit = raycast(camera.world_position, camera.forward, distance=5, ignore=[player])
                if hit.hit and isinstance(hit.entity, Object): catch(hit.entity)
        # pause
        case 'escape': 
                if not start_menu.enabled: change_pause()

def update_spheres_visibility():
    global spheres_visibility
    for sphere in spheres_visibility: destroy(sphere)
    spheres_visibility = [Entity(model='cube', parent=player, scale=i*3, alpha=i/50, double_sided=True, color=color.black) for i in range(10, visibility_sld.value, visibility_sld.value // 20)]
        
# PAUSE MENU
def change_pause():
    global game_screen
    pause_menu.enabled = not pause_menu.enabled
    player.enabled = not pause_menu.enabled
    game_screen.enabled = not pause_menu.enabled
    camera.clip_plane_far = visibility_sld.value
    update_spheres_visibility()
    # degub info
    debug_tx.text = f'Отладка:\n player: {math.floor(player.x)}, {math.floor(player.y)}, {math.floor(player.z)}'

pause_menu = Entity(parent=camera.ui, enabled=False)
Entity(model='quad', parent=pause_menu, color=color.black, alpha=0.7, scale=5)
Text('Benefit Harm', parent=pause_menu, position=(-0.4, 0.45), scale=3, origin=(0.5, 0.5))
Text('BioStage', parent=pause_menu, position=(-0.5, 0.35), scale=2.5, origin=(0.5, 0.5))
Button(text = 'Продолжить', parent = pause_menu,position = (0.6, -0.3), scale = (0.4, 0.07), on_click = change_pause)
Button(text = 'Выйти', parent = pause_menu,position = (0.6, -0.4), scale = (0.4, 0.07), on_click = application.quit)
Text('Дальность прорисовки', parent=pause_menu,  position=(-0.4, -0.3), scale=1)
visibility_sld = Slider(max=200, min=20, default=camera.clip_plane_far, step=1, position=(-0.4, -0.4), dynamic=True, parent=pause_menu)
debug_tx = Text('Отладка:', parent=pause_menu,  position=(0.4, 0.4), scale=1)

# START MENU AND LEVELS
camera.rotation = (0, -55, 35)
camera.position = (20, 3, -70)
start_menu = Entity(parent=camera.ui)
tick_velev = 0
current_level_update = None
def start_level_1():
    global speed_blood, player, current_level_update, start_sound
    target_tx.text = 'Ознакомтесь со средой'
    current_level_update = level_1_update
    start_level_0()

def start_level_0():
    global speed_blood, player, current_level_update, start_sound
    start_menu.enabled = False
    player.enabled = True
    game_screen.enabled = True
    speed_blood = 0.1
    player = FirstPersonController(position=(0, 0, 0),gravity=0.1)
    start_sound.stop(destroy=False)
    play_sound.play()
    water_sound.play()
    update_spheres_visibility()

def touch(cls, to_obj):
    for obj in scene.entities:
        if isinstance(obj, cls) and obj.intersects(to_obj).hit:
            return obj
    return None

def move_to(it_cls, target, distance):
    if target is None or getattr(target, 'destroyed', False):
        return

    for obj in scene.entities:
        if not isinstance(obj, it_cls):
            continue

        if getattr(obj, 'destroyed', False):
            continue

        direction = target.position - obj.position

        if direction.length() <= distance and direction.length() > 0:
            direction = direction.normalized()

            obj.dx = direction.x
            obj.dy = direction.y
            obj.dz = direction.z

def move(it_cls, to_cls, distance):
    entities = [
        obj for obj in scene.entities
        if not obj in (None,)
        and not getattr(obj, 'destroyed', False)
    ]
    targets = [obj for obj in entities if isinstance(obj, to_cls)]
    movers = [obj for obj in entities if isinstance(obj, it_cls)]
    for obj in movers:
        nearby = []
        for target in targets:
            try:
                dist = (target.position - obj.position).length()
                if dist <= distance: nearby.append((dist, target))
            except Exception: continue
        if nearby:
            target = min(nearby, key=lambda item: item[0])[1]
            direction = (target.position - obj.position).normalized()
            obj.dx = direction.x * 0.1
            obj.dy = direction.y * 0.1
            obj.dz = direction.z * 0.3

def level_1_update():
    global tick_velev, touch
    match math.floor(tick_velev):
        case 2:
            set_target('Возьмите кислород(правая кнопка мыши)')
            tick_velev = 100
        case 100:
            if isinstance(catch_obj, Oxygen):
                set_target('Отправьте его в капиляр')
                ball_sound.play()
                tick_velev = 101
        case 101:
            oxygen = touch(Oxygen, capillary_box)
            if oxygen:
                ball_sound.play()
                destroy(oxygen)
                set_target('Отлично!')
                tick_velev = 200
        case 203:
            set_target('Отправь еще 5 кислорода')
            tick_velev = 204
        case t if 203 < t < 210:
            oxygen = touch(Oxygen, capillary_box)
            if oxygen: 
                ball_sound.play()
                tick_velev += 1
                destroy(oxygen)
            else:
                other = touch(Object, capillary_box)
                if other: destroy(other)
        case 210:
            ball_sound.play()
            set_target('Клетки оснощены кислородом!')
            tick_velev = 300
            spawn_info[Eritrocit][0] = 1.5
        case 310:
            set_target('Но всё ещё не хватает')
            tick_velev = 311
        case 317:
            set_target('Давай ещё 100 доставь!')
            tick_velev = 318
            spawn_info[Eritrocit][0] = 0.3
        case 320:
            set_target('ШУЧУ! Погоди ка!')
            tick_velev = 321
        case 330:
            set_target('Эритроциты доставляют кислород!')
            tick_velev = 331
        case 340:
            set_target('Теперь можно отдохнуть')
            spawn_info[Eritrocit][0] = 0.1
            spawn_info[Bacteria][0] = 1.5
            tick_velev = 341
        case 350:
            ball_sound.play()
            set_target('О нет! Бактерии убивают их')
            tick_velev = 401
        case 410:
            set_target('Убери бактерии(левая кнопка мыши)')
            spawn_info[Bacteria][0] = 0.3
            tick_velev = 411
        case 460:
            set_target('Ещё немного!')
            spawn_info[Bacteria][0] = 0
            tick_velev = 461
        case 481:
            ball_sound.play()
            set_target('Вы победили !')
            tick_velev = 482
        case 485:
            ball_sound.play()
            set_target('Спасибо, за демо-прохождение !')
            tick_velev = 458
        case _:
            tick_velev += time.dt

Text('Benefit Harm', parent=start_menu, position=(-0.4, 0.45), scale=3, origin=(0.5, 0.5))
Text('BioStage s2', parent=start_menu, position=(-0.4, 0.35), scale=2.5, origin=(0.5, 0.5))
Button('Уровень: Состав крови', parent = start_menu, position = (0.6, -0.2), scale = (0.4, 0.07), on_click=start_level_1)
Button('Симулятор', parent = start_menu, position = (0.6, -0.3), scale = (0.4, 0.07), on_click=start_level_0)
Button(text = 'Выйти', parent = start_menu,position = (0.6, -0.4), scale = (0.4, 0.07), on_click = application.quit)
Entity(model='quad', parent=start_menu, color=color.black, alpha=0.1, scale=5)

# RUN
app.run()