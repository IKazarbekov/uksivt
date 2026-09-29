import json
import os

FILE = "data.json"

class Product:
    def __init__(self, name, price, count, sr_price):
        self.name = name
        self.price = price
        self.count = count
        self.sr_price = sr_price

def load_data():
    if not os.path.exists(FILE):
        return [
            Product("car", 1000, 3, 90),
            Product("bicycle", 100, 2, 90),
            Product("motorcycle", 140, 10, 120),
        ]
    with open(FILE, "r") as f:
        data = json.load(f)
        return [Product(**d) for d in data]

def save_data(data):
    with open(FILE, "w") as f:
        json.dump(data, f, indent=4, default=lambda o: o.__dict__)

def process(request):
    command = request["command"]
    products = load_data()

    match command:
        case "GET":
            result = list(map(lambda product: product.__dict__, products))
            return {"data": result}
        case "FIND":
            pattern = request["pattern"]
            products = list(filter(lambda p: pattern in p.name, products))
            result = list(map(lambda product: product.__dict__, products))
            return {"data": result}
        case "PUT":
            payload = request["payload"]
            products.append(Product(price=0, sr_price=0, **payload))
            save_data(products)
            result = list(map(lambda product: product.__dict__, products))
            return {"data": result}