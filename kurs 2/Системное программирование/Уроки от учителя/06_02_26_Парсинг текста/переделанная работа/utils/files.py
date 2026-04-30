def write_file_user():
    '''
        User write text and name file
        :return: list of lines
    '''
    print('Чтобы закончить файл, напишите END\n'
          '===========НАЧАЛО ФАЙЛА==============')
    lines = list()
    while True:
        string = input()
        if string != 'END':
            lines.append(string)
        else:
            break
    name = input('Имя файла:')
    with open(name, 'wt') as file:
        file.writelines(lines)
    return lines