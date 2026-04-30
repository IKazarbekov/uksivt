List<Employee> list = new List<Employee>();
list.Add(new SoftwareDeveloper("Иван", 80000, "разработчик"));
list.Add(new SoftwareDeveloper("Мария", 100000, "менеджер"));
list.Add(new SoftwareDeveloper("Петр", 70000, "дизайнер"));

foreach (Employee e in list)
{
    Console.WriteLine($"{e.name} ({e.Department},{e.salary})");
    e.PerfomDuties();
}

Console.WriteLine(list[0] is  IWorker);



interface IWorker
{
    void DoWork();
}
interface ITeamLead
{
    void ManageTeam();
}
interface IDeveloper
{
    void WriteCode();
}

abstract class Employee
{
    public string name;
    public int salary;
    public string Department;

    public Employee(string name, int salary, string department)
    {
        this.name = name;
        this.salary = salary;
        this.Department = department;
    }

    public abstract void PerfomDuties();
}

class SoftwareDeveloper : Employee, IDeveloper
{
    public SoftwareDeveloper(string name, int salary, string department) : base(name, salary, department)
    {
    }

    public void WriteCode()
    {
        Console.WriteLine("- Пишет код на C#");
    }

    public override void PerfomDuties()
    {
        WriteCode();
        Console.WriteLine("- Может быть лидом команды");
    }
}

class Manager : Employee, ITeamLead
{
    public Manager(string name, int salary, string department) : base(name, salary, department)
    {
    }

    public void ManageTeam()
    {
        Console.WriteLine("- Управляет командой");
    }

    public override void PerfomDuties()
    {
        ManageTeam();
        Console.WriteLine("- Работает в отделе управления");
    }
}

class Designer : Employee, IWorker
{
    public Designer(string name, int salary, string department) : base(name, salary, department)
    {
    }

    public void DoWork()
    {
        Console.WriteLine("- Создает дизайн");
    }

    public override void PerfomDuties()
    {
        DoWork();
    }
}