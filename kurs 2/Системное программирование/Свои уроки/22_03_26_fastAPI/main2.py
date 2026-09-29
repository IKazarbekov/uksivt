# шаг 1
from fastapi import FastAPI

app = FastAPI()

fake_tasks = list()

from pydantic import BaseModel
class Task(BaseModel):
    title: str
    description: str | None = None
    is_completed: bool = False

@app.post('/tasks')
def create_task(task: Task):
    fake_tasks.append(task)
    return {'message':'task created'}

@app.get('/tasks')
def read_task(id: int):
    try:
        result_task = fake_tasks[id]
        return {'task':result_task}
    except:
        return {'task': None}

@app.get('/tasks')
def read_task():
    return {'tasks' : fake_tasks}

@app.put('/tasks')
def update_task(task_id: int, is_completed: bool):
    try:
        fake_tasks[task_id].is_completed = is_completed
        return {'message':'OK'}
    except:
        return {'message':'Not found'}

@app.delete('/tasks')
def delete_task(task_id: int):
    try:
        fake_tasks.pop(task_id)
        return {'message', 'ok'}
    except:
        return {'message', 'not found'}