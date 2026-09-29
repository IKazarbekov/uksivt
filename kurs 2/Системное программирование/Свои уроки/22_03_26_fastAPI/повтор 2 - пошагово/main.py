from pyexpat.errors import messages

from fastapi import FastAPI

app = FastAPI()

@app.get("/")
def read_root():
    return {'message': 'hello'}

@app.get("/tasks/{task_id}")
def tasks(task_id: int):
    return {'message':f"task {task_id * 4}"}

@app.get("/tasks")
def tasks(task_id: int):
    return {'message':f"task {task_id * 4}"}

from pydantic import BaseModel

class Task(BaseModel):
    title: str
    description: str | None = None
    is_completed: bool = False

@app.post('/tasks')
def create_task(task: Task):
    return {
        'message':f'created task {task}'
    }