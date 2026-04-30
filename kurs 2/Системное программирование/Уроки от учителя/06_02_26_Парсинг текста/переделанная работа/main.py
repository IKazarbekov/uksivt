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
                print('Автоматически создать ? Д/Н')
                if input() == 'Д':
                    lines = ['3 + 2', '1 - 5']
                    print('Входной текст:\n','\n'.join(lines))
                    result = ps.parsing_lines(lines)
                    del lines
                    print('Обработанный файл', str(result))
                else:
                    lines = fl.write_file_user()
                    result = ps.parsing_skobla(lines)
                    del lines
                    print('Обработанный файл',result, type(result))
            case '2':
                path = input('Введите путь к файлу:')
                try:
                    with open(path, 'r') as file:
                        lines = file.readlines()
                    result = ps.parsing_lines(lines)
                    del lines
                    print(result)
                    with open(input('Введите путь к выходному файлу'), 'xt') as file:
                        file.writelines(result)
                    print('Файл записан. Внимание! PyCharm не всегда отображает файл, открой проводник')
                except FileNotFoundError:
                    print("Файл не найден:")
                except FileExistsError:
                    print("Файл уже существует!")
            case '3':
                print('Программа для обработки текста\nПрограмма способна:\n'
                      '1. Читает текстовый файл, содержащий математические выражения, смешанные с обычным текстом\n'
                    ' Находит и вычисляет только корректные математические выражения, избегая ложных срабатываний\n'
                    ' Добавляет результаты вычислений после соответствующих выражений\n'
                    ' Сохраняет обработанный текст в новый файл, оставляя исходный файл неизменным\n'
                    )
                input()
            case '4':
                text = ['12.12.2022', '(3*4)/2', '3.14*2', '192.168.0.1']
                print('Входной текст:', *text)
                result = ps.parsing_lines(text)
                print('Результат:', *result)
                if result[1] == '6.0' and result[2] == '6.28':
                    print('+ вычисляет хорошо')
                else:
                    print('- вычисляет плохо')
                if result[0] == '12.12.2022' and result[3] == '192.168.0.1':
                    print('+ На даты и адреса не реагирует')
                else:
                    print('- На даты и адреса реагирует')
            case '5':
                break
            case _:
                print('Не известная команда')