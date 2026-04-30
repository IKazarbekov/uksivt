# шаг 1
from fastapi import FastAPI

app = FastAPI()

@app.get('/')
def read_root():
    return {'message':'Привет Хабр!'}
# шаг 2
@app.get('/tasks/{task_id}')
def tasks(task_id: int):
    return {'task_id' : task_id * 5, 'name':f'Задача номер {task_id}'}

# шаг 3
@app.get('/tasks')
def tasks(skip: int = 0, limit: int = 10):
    return {
        'message' : 'Возвращаю список',
        'skip' : skip,
        'limit' : limit
    }

# шаг 4
from pydantic import BaseModel
class Task(BaseModel):
    title: str
    description: str | None = None
    is_completed: bool = False

@app.post('/tasks')
def create_task(task: Task):
    return {'Message': ' Задание создано',
            'task' : task}
