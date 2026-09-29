import json, os
FILE = "data.json"

class Sharing_car:
    def __init__(self, model, mark, price):
        self.model = model
        self.mark = mark
        self.price = price
        self.sr_price = sum(price) / len(price) if price else 0

def load_data():
    if not os.path.isfile(FILE):
        return [
            Sharing_car("Шестёрка", "Res2", [34,35,23]),
            Sharing_car("Rega", "qws2", [34,23,46]),
            Sharing_car("Laos", "52", [1,23,34,23]),
        ]
    with open(FILE, "r", encoding="utf-8") as f:
        data = json.load(f)
        return [Sharing_car(car["model"], car["mark"], car["price"]) for car in data]

def save_data(data):
    with open(FILE, "w", encoding="utf-8") as f:
        dict_data = [d.__dict__ for d in data]
        json.dump(dict_data, f, indent=4, ensure_ascii=False)

def process_data(request: dict):
    command = request["command"]
    data = load_data()

    match command:
        case "GET":
            result = [d.__dict__ for d in data]
            return {"data": result}
        case "PUT":
            payload = request["payload"]
            car = Sharing_car(**payload)
            data.append(car)
            save_data(data)
            return {"status": True}
        case "FIND":
            pattern = request["pattern"]
            filter_data = list(filter(lambda c: pattern.upper() in c.model.upper() or pattern.upper() in c.mark.upper(), data))
            result = [d.__dict__ for d in filter_data]
            return {"data": result}
        case _:
            print("ERROR COMMAND")
            return {"error": True}

if __name__ == "__main__":
    save_data(load_data())