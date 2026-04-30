import orjson

from dataclasses import dataclass
from datetime import datetime

@dataclass()
class User:
    name:str
    signup_is:datetime

user = User('Tom', datetime.now())

json_bytes = orjson.dumps(user)
print(json_bytes)

data = orjson.loads(json_bytes)
print(data)