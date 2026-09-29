using System;
using System.Collections.Generic;
using System.Text;

namespace Work_Doc_Моё_решение
{
    public class Problem
    {
        public string Describe {  get; set; }
        public int Room {  get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public bool IsAccept {  get; set; }
        public override string ToString()
        {
            if (IsAccept)
                return $"ПРИНЯТО: Сотрудник {UserName}, жалуется: {Describe}, в кабинете: {Room}, в {Date.ToShortDateString()}";
            return $"Сотрудник {UserName}, жалуется: {Describe}, в кабинете: {Room}, в {Date.ToShortDateString()}";
        }
    }
}
