using System;
using System.Collections.Generic;
using System.Linq;

namespace Задание_5_измненено
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var main = new MenuItem("Egg", 50, Category.Main);
            var salat = new MenuItem("EggSalat", 100, Category.Salat);
            var drink = new MenuItem("Egg in milk", 200, Category.Drink);

            List<MenuItem> menu = new List<MenuItem>();
            menu.Add(main);
            menu.Add(salat);
            menu.Add(drink);

            Order order1 = new Order();
            order1.AddItem(main);

            Order order2 = new Order();
            order1.AddItem(salat);

            Order order3 = new Order();
            order1.AddItem(main);
            order1.AddItem(drink);

            order1.ChangeStatus(OrderStatus.Cooking);
            order3.ChangeStatus(OrderStatus.Cooking);

            order1.GetItemsByCategory(Category.Main);

            try
            {
                order1.RemoveItem(drink.name);
            }
            catch
            {
                Console.WriteLine("Обработано исключение");
            }
        }

        enum OrderStatus
        {
            Pending,
            Cooking,
            Ready,
            Delivered,
            Cancelled
        }

        enum Category
        {
            Salat,
            Main,
            Drink
        }

        struct MenuItem
        {
            public string name;
            public int price;
            public Category category;

            public MenuItem(string name, int price, Category category)
            {
                this.name = name;
                this.price = price;
                this.category = category;
            }
        }

        class Order
        {
            private int number = 0;
            int OrderId { get; }
            string customerName;
            List<MenuItem> items = new List<MenuItem>();
            OrderStatus status;
            string createdData;
            public Order()
            {
                OrderId = number++;
                status = OrderStatus.Pending;
            }
            public void AddItem(MenuItem item)
            {
                items.Add(item);
            }

            public void RemoveItem(string itemName)
            {
                MenuItem[] item = items.Where(i => i.name == itemName).ToArray();
                Console.WriteLine(item[0].name);
                items.Remove(item[0]);
            }

            public int GetTotalPrice(string customerName)
            {

                int mount = 0;
                foreach (MenuItem item in items)
                {
                    mount += item.price;
                }
                return mount;
            }

            public void ChangeStatus(OrderStatus newStatus)
            {
                status = newStatus;
            }

            public void PrintOrder()
            {
                Console.WriteLine($"ID: {OrderId}, name:{customerName}, status: {status}, data: {createdData}");
            }

            public MenuItem[] GetItemsByCategory(Category categ)
            {
                return items.Where(i => i.category == categ).ToArray();
            }
        }
    }
}