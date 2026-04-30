import subprocess


"""
1
subprocess.call(['featherpad'])

2
subprocess.call(['firefox','https://translate.yandex.ru'])

3
subprocess.call(['python', 'задачи по subprocess #2.py'])

4
print('open')
popen = subprocess.Popen(['featherpad'])
popen.wait()
print('close')

5
print('open')
popen = subprocess.Popen(['featherpad'])
popen1 = subprocess.Popen(['featherpad'])
popen2 = subprocess.Popen(['featherpad'])
popen3 = subprocess.Popen(['featherpad'])
popen4 = subprocess.Popen(['featherpad'])
popen5 = subprocess.Popen(['featherpad'])
while popen.poll() is None or popen1.poll() is None or popen2.poll() is None or popen3.poll() is None or popen4.poll() is None or popen5.poll() is None:
    pass
print('close')
"""


