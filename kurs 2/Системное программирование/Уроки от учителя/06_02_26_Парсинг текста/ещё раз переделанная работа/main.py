from utils import files as fl, parsing as ps

if __name__ == '__main__':

    while True:

        print('1) Создать файл и обработать\n'
              '2) Обработать ваш файл\n'
              '3) Справка\n'
              '4) Самотестирование\n'
              '5) Выход\n')

        match input():
            case '1':
                pass
            case '2':
                try:
                    with open(input('Путь к файлу'), 'r') as file:
                        result_lines = ps.parsing_lines(file.readlines())
                        print("Результат:", result_lines)
                    with open(input('Путь к файлу куда сохранить результат'), 'w') as file:
                        file.writelines(result_lines)
                except FileNotFoundError:
                    print('Файл не найден')
            case '3':
                print('Программа для обработки текста\nПрограмма способна:\n'
                      '1. Читает текстовый файл, содержащий математические выражения, смешанные с обычным текстом'
                    ' Находит и вычисляет только корректные математические выражения, избегая ложных срабатываний'
                    ' Добавляет результаты вычислений после соответствующих выражений'
                    ' Сохраняет обработанный текст в новый файл, оставляя исходный файл неизменным'
                    )
            case '4':
                text = ['12.12.2022', '(3*4)/2', '3.14*2', '192.168.0.1']
                result = ps.parsing_lines(text)
                print(result)
            case '5':
                break
            case _:
                print('Не известная команда')