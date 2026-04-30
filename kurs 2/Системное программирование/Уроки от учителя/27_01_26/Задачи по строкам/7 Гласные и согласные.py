s = input().lower()
vowels = "ауоыизяеё"
consonants = "бвгджйклмнпрстфхцчшщ"
vowel_count = 0
consonant_count = 0
for char in s:
    if char in vowels:
        vowel_count += 1
    elif char in consonants:
        consonant_count += 1
print(f"Количество гласных букв равно {vowel_count}")
print(f"Количество согласных букв равно {consonant_count}")