def main():
    letter = input("Введите букву, с которой должно начинаться имя: ").strip()
    length = int(input("Введите длину имени (количество букв): ").strip())

    try:
        file = open("students.txt", "r", encoding="utf-8")
        count = 0

        for line in file:
            line = line.strip()
            if not line:
                continue

            parts = line.split()
            if len(parts) < 1:
                continue

            name = parts[0]

            if name.startswith(letter) and len(name) == length:
                print(line)
                count += 1

        file.close()
        print(f"\nКоличество подходящих строк: {count}")

    except FileNotFoundError:
        print("Файл students.txt не найден.")

if __name__ == "__main__":
    main()
