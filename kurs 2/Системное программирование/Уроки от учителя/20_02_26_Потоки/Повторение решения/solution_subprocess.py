import subprocess

task = int(input())

#1
match task:
    case 1:
        subprocess.run(["featherpad"])
    case 2:
        subprocess.run(["firefox","rutube.ru"])
    case 3:
        subprocess.run(['python','/home/ikazarbekov/Документы/UKSIVT/Системное программирование/Уроки от учителя/20_02_26_Потоки/задачи по main.py'])
    case 4:
        pr = subprocess.Popen(['featherpad'])
        pr.wait()
    case 5:
        pr = subprocess.Popen(['featherpad'])
        pr2 = subprocess.Popen(['nano'])
        pr3 = subprocess.Popen(['firefox'])
        pr4 = subprocess.Popen(['cool-retro-term'])
        pr.wait()
        pr2.wait()
        pr3.wait()
        pr4.wait()