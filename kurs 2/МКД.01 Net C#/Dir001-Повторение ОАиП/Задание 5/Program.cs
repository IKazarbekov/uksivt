using System;
using System.Collections.Generic;
using System.Linq;

public enum OrderStatus
{
    Pending,   
    Cooking,    
    Ready,    
    Delivered,
    Cancelled  
}

public struct MenuItem
{
    public string Name;
    public decimal Price;
    public string Category;

    public MenuItem(string name, decimal price, string category)
    {
        Name = name;
        Price = price;
        Category = category;
    }

    public override string ToString()
    {
        return $"{Name} ({Category}) - {Price} руб.";
    }
}

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message) : base(message) { }
}

public class InvalidOrderOperationException : Exception
{
    public InvalidOrderOperationException(string message) : base(message) { }
}

public class Order
{
    private static int orderCounter = 1;

    public int OrderId { get; }
    public string CustomerName { get; }
    public List<MenuItem> Items { get; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedDate { get; }

    public Order(string customerName)
    {
        OrderId = orderCounter++;
        CustomerName = customerName;
        Items = new List<MenuItem>();
        Status = OrderStatus.Pending;
        CreatedDate = DateTime.Now;
    }

    public void AddItem(MenuItem item)
    {
        Items.Add(item);
        Console.WriteLine($"Добавлено: {item.Name} в заказ #{OrderId}");
    }

    public void RemoveItem(string itemName)
    {
        var item = Items.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

        if (item.Name == null)
        {
            throw new ItemNotFoundException($"Блюдо '{itemName}' не найдено в заказе #{OrderId}");
        }

        Items.Remove(item);
        Console.WriteLine($"Удалено: {item.Name} из заказа #{OrderId}");
    }

    public decimal GetTotalPrice()
    {
        return Items.Sum(item => item.Price);
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        if (Status == OrderStatus.Delivered || Status == OrderStatus.Cancelled)
        {
            throw new InvalidOrderOperationException(
                $"Нельзя изменить статус заказа #{OrderId} после его завершения (текущий статус: {Status})");
        }

        Status = newStatus;
        Console.WriteLine($"Статус заказа #{OrderId} изменен на: {GetStatusString()}");
    }

    public List<MenuItem> GetItemsByCategory(string category)
    {
        return Items.Where(item => item.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public void PrintOrder()
    {
        Console.WriteLine($"\n{'='.Repeat(50)}");
        Console.WriteLine($"Заказ #{OrderId:D3}");
        Console.WriteLine($"Клиент: {CustomerName}");
        Console.WriteLine($"Статус: {GetStatusString()}");
        Console.WriteLine("Позиции:");

        if (Items.Count == 0)
        {
            Console.WriteLine("- Заказ пуст");
        }
        else
        {
            foreach (var item in Items)
            {
                Console.WriteLine($"- {item}");
            }
        }

        Console.WriteLine($"Итого: {GetTotalPrice()} руб.");
        Console.WriteLine($"Дата: {CreatedDate:dd.MM.yyyy HH:mm}");
        Console.WriteLine($"{'='.Repeat(50)}");
    }

    private string GetStatusString()
    {
        return Status switch
        {
            OrderStatus.Pending => "В ожидании",
            OrderStatus.Cooking => "Готовится",
            OrderStatus.Ready => "Готово",
            OrderStatus.Delivered => "Доставлено",
            OrderStatus.Cancelled => "Отменено",
            _ => "Неизвестно"
        };
    }
}

public static class StringExtensions
{
    public static string Repeat(this char c, int count)
    {
        return new string(c, count);
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        try
        {
            Console.WriteLine("МЕНЮ РЕСТОРАНА");

            List<MenuItem> menu = new List<MenuItem>
            {
                new MenuItem("Цезарь", 350, "Салат"),
                new MenuItem("Греческий салат", 320, "Салат"),
                new MenuItem("Паста Болоньезе", 450, "Основное"),
                new MenuItem("Стейк Рибай", 1200, "Основное"),
                new MenuItem("Пицца Маргарита", 550, "Основное"),
                new MenuItem("Апельсиновый сок", 150, "Напиток"),
                new MenuItem("Кофе Латте", 200, "Напиток"),
                new MenuItem("Чай зеленый", 100, "Напиток"),
                new MenuItem("Тирамису", 280, "Десерт"),
                new MenuItem("Чизкейк", 350, "Десерт")
            };

            int index = 1;
            foreach (var item in menu)
            {
                Console.WriteLine($"{index}. {item}");
                index++;
            }


            Order order1 = new Order("Иван Петров");
            Order order2 = new Order("Анна Сидорова");
            Order order3 = new Order("Петр Иванов");

            Console.WriteLine("\nФОРМИРОВАНИЕ ЗАКАЗОВ");

            // Заказ 1
            order1.AddItem(menu[0]);  // Цезарь
            order1.AddItem(menu[2]);  // Паста Болоньезе
            order1.AddItem(menu[5]);  // Апельсиновый сок
            order1.AddItem(menu[8]);  // Тирамису

            // Заказ 2
            order2.AddItem(menu[3]);  // Стейк Рибай
            order2.AddItem(menu[1]);  // Греческий салат
            order2.AddItem(menu[6]);  // Кофе Латте

            // Заказ 3
            order3.AddItem(menu[4]);  // Пицца Маргарита
            order3.AddItem(menu[9]);  // Чизкейк

            // 4. Изменяем статусы заказов
            Console.WriteLine("\nИЗМЕНЕНИЕ СТАТУСОВ ЗАКАЗОВ");

            order1.ChangeStatus(OrderStatus.Cooking);
            order2.ChangeStatus(OrderStatus.Pending);
            order3.ChangeStatus(OrderStatus.Cooking);

            order1.ChangeStatus(OrderStatus.Ready);

            Console.WriteLine("\nПОИСК БЛЮД ПО КАТЕГОРИЯМ");

            Console.WriteLine("\nПоиск блюд по категории \"Салат\" в заказе #001:");
            var saladsInOrder1 = order1.GetItemsByCategory("Салат");
            if (saladsInOrder1.Count > 0)
            {
                foreach (var salad in saladsInOrder1)
                {
                    Console.WriteLine($"- {salad.Name} - {salad.Price} руб.");
                }
            }
            else
            {
                Console.WriteLine("Салаты не найдены в заказе");
            }

            Console.WriteLine("\nПоиск блюд по категории \"Напиток\" в заказе #002:");
            var drinksInOrder2 = order2.GetItemsByCategory("Напиток");
            foreach (var drink in drinksInOrder2)
            {
                Console.WriteLine($"- {drink.Name} - {drink.Price} руб.");
            }

            Console.WriteLine("\nОБРАБОТКА ИСКЛЮЧЕНИЙ");

            try
            {
                Console.WriteLine("\nПопытка удалить несуществующее блюдо:");
                order1.RemoveItem("Суши");
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            try
            {
                Console.WriteLine("\nПопытка изменить статус после отмены заказа:");
                order2.ChangeStatus(OrderStatus.Cancelled);
                order2.ChangeStatus(OrderStatus.Cooking); // Это вызовет исключение
            }
            catch (InvalidOrderOperationException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            try
            {
                Console.WriteLine("\nПопытка удалить существующее блюдо:");
                order3.RemoveItem("Пицца Маргарита");
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine("\nИТОГОВАЯ ИНФОРМАЦИЯ ПО ЗАКАЗАМ");

            order1.PrintOrder();
            order2.PrintOrder();
            order3.PrintOrder();

            Console.WriteLine("\nСТАТИСТИКА");

            List<Order> allOrders = new List<Order> { order1, order2, order3 };

            decimal totalRevenue = allOrders.Sum(o => o.GetTotalPrice());
            Console.WriteLine($"Общая выручка: {totalRevenue} руб.");

            var activeOrders = allOrders.Count(o =>
                o.Status != OrderStatus.Delivered &&
                o.Status != OrderStatus.Cancelled);
            Console.WriteLine($"Активных заказов: {activeOrders}");

            var avgOrderValue = allOrders.Average(o => o.GetTotalPrice());
            Console.WriteLine($"Средний чек: {avgOrderValue:F2} руб.");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}