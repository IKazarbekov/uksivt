Vehicle[] vehicles =
{
    new Car("Moelr", 5),
    new Plane("Roet", 15),
    new Boat("Tomis", 20)
};
foreach (var vehicle in vehicles)
{
    vehicle.Move();
    Console.WriteLine($"Топливо для него: {vehicle.CalculateFuelNeeded(1000)}");
}

class Vehicle
{
    protected string Brand;
    protected double FielConsumprion;
    public Vehicle(string Brand, double FielConsumprion)
    {
        this.Brand = Brand;
        this.FielConsumprion = FielConsumprion;
    }
    public virtual void Move()
    {
        Console.WriteLine("Транспорт движется");
    }
    public double CalculateFuelNeeded(double distance)
    {
        return FielConsumprion * distance / 100;
    }
}

class Car : Vehicle
{
    public Car(string Brand, double FielConsumprion) : base(Brand, FielConsumprion)
    {
    }

    public override void Move()
    {
        Console.WriteLine("Машина едет");

    }
}

class Plane : Vehicle
{
    public Plane(string Brand, double FielConsumprion) : base(Brand, FielConsumprion)
    {
    }

    public override void Move()
    {
        Console.WriteLine("Самолет летит в небе");

    }
}

class Boat : Vehicle
{
    public Boat(string Brand, double FielConsumprion) : base(Brand, FielConsumprion)
    {
    }

    public override void Move()
    {
        Console.WriteLine("Корабль плывет по воде");

    }
}

