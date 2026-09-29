text = input().lower().split()
articles = ['a', 'an', 'the']
count = sum(1 for word in text if word in articles)
print(f"Общее количество артиклей: {count}")
