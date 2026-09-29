using System.Windows.Controls;

namespace РЕГИСТРАЦИЯ_НА_МАРАФОН
{
    public partial class BottomPanel : UserControl
    {
        DateTime date = new DateTime(2027, 1, 1, 10, 10, 10);
        public BottomPanel()
        {
            InitializeComponent();
            var now = DateTime.Now;
            var subtime = date - now;
            string text = $"{subtime.Days} дней {subtime.Hours} часов и {subtime.Minutes} минут до старта марафона!";
            textBlock.Text = text;
        }
    }
}