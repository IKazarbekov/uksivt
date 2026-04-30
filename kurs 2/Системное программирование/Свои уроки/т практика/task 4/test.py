from solution import get_answer
tests = {"""5  2
1  2  1  3  5""":16,
         """3  1
99  5  85""":10,
         """1  10
9999""":0,
         """6 3
23 5 9 1534 9452""":8570}

for data, answer in tests.items():
    get_answer(data)

"""Кузнецов Дмитрий Юрич"""
