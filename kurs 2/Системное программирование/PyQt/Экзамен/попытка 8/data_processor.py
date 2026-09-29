import json, os

FILE = "data.json"

class Delivery_salary:
    def __init__(self, name, salary, bonus, sr_salary):
        self.name = name
        self.salary = salary
        self.bonus = bonus
        self.sr_salary = sr_salary

def save_data(data):
    with open(FILE, "w") as f:
        json.dump(data, f, default=lambda o: o.__dict__, indent=4)

def read_data(FILE):
    if not os.path.exists(FILE):
        return [
            Delivery_salary("runner", 30, 4, 27),
            Delivery_salary("swanner", 53, 6, 45),
            Delivery_salary("kitchen", 38, 3, 45),
        ]
    with open(FILE, "r") as f:
        data = json.load(f)
        return [Delivery_salary(**d) for d in data]

def process_request(request):
    command = request['command']
    data = read_data(FILE)

    match command:
        case "PUT":
            payload = request['payload']
            delivery_salary = Delivery_salary(**payload)
            data.append(delivery_salary)
            save_data(data)
            return {"status":"ok"}
        case "GET":
            result = []
            for id, d in enumerate(data):
                dct = {"id":id}
                dct.update(d.__dict__)
                result.append(dct)
            return { "data" : result }
        case "DELETE":
            id = request['id']
            data.pop(id)
            save_data(data)
            return {"status":"ok"}
