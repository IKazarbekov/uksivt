city1 = input()
city2 = input()
city3 = input()
cities = [city1, city2, city3]
shortest = min(cities, key=len)
longest = max(cities, key=len)
print(shortest)
print(longest)