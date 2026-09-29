import json, random
import os.path

FILE = "data.json"

class Medical_card:
    def __init__(self, nomber: str, familia, visits: list[int], sr: int):
        assert type(nomber) == str
        assert type(familia) == str
        assert isinstance(visits, list)
        assert type(sr) == int
        self.nomber = nomber
        self.familia = familia
        self.visits = visits
        self.sr = sr

def load_data():
    if not os.path.exists(FILE):
        return [
            Medical_card("234234234", "Tom", [2,5,8,23], 4),
            Medical_card("204892134", "Bob", [2, 2, 3, 23], 2)
        ]
    with open(FILE, "r") as f:
        data = json.load(f)
        return [Medical_card(**d) for d in data]

def save_data(cards: list[Medical_card]):
    assert type(cards) == list
    with open(FILE, "w") as f:
        json.dump(cards, f, default=lambda o: o.__dict__, indent=4)

def process(command: str, payload):
    cards = load_data()
    result = None

    match command:
        case "PUT":
            card = Medical_card(**payload)
            cards.append(card)
        case "GET-SORT":
            result = sorted(cards, key=lambda card: card.familia)
        case "GET-REVERSE":
            result = sorted(cards, key=lambda card: card.familia, reverse=True)
        case "GET-SHUFFLE":
            result = cards.copy()
            random.shuffle(result)
        case _:
            return {"status":"error"}

    save_data(cards)
    data_dict = [card.__dict__ for card in result]
    if command == "PUT":
        return {"status":"ok"}
    else:
        return {"data": data_dict, "status" : "ok"}