﻿using Restaurant.Models;
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

namespace Restaurant.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для DishCard.xaml
    /// </summary>
    public partial class DishCard : UserControl
    {
        public DishCard(Dish dish)
        {
            InitializeComponent();
            DataContext = dish;
            dishInfo = dish;
        }

        public Dish dishInfo { get; set; }
    }
}