from fastapi import FastAPI

app = FastAPI()

@app.get('/')
def read_root():
    return {'message':'HELLOOOOOOOO HAHA'}

@app.get('/tasks/{task_id}')
def get_task(task_id: int):
    return {'message':'get task HAHA', 'id':task_id * 5}

@app.get('/tasks')
def get_task(task_id: int):
    return {'message':'get task HAHA', 'id':task_id**5000}

from pydantic import BaseModel

class Task(BaseModel):
    title: str
    description: str | None = None
    is_completed:bool = False

@app.post('/tasks')
def create_task(task: Task):
    return {'message': 'task HAHAHAH', 'task' : task}