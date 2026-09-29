using System;
using System.Windows;

namespace ShopTask.data
{
    public static class InitData
    {
        public static void Save()
        {
            try
            {
                // Сюда можно добавить сериализацию в JSON/XML или сохранение в БД SQL.
                // На данный момент метод обеспечивает безопасное закрытие окон без падения.
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Система");
            }
        }
    }
}