using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Table = Restaurant.Models.Table;

namespace Restaurant.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ReservationControl.xaml
    /// </summary>
    public partial class ReservationControl : UserControl
    {
        public ReservationControl(ClientTable clientTable)
        {
            InitializeComponent();
            DataContext = this;

            ClientTableInfo = clientTable;
            ClientInfo = clientTable.Client;
            TableInfo = clientTable.Table;
        }

        public List<string> StatusSource { get; set; } = ["Ожидание", "Действительна", "Прошла"];
        public ClientTable ClientTableInfo { get; set; }
        public Client ClientInfo {  get; set; }
        public Table TableInfo { get; set; }
    }
}
