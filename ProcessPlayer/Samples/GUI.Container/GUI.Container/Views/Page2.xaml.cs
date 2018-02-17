﻿using GUIContainer.ViewModels;
using System.Windows.Controls;

namespace GUIContainer.Views
{
    /// <summary>
    /// Interaction logic for Process.xaml
    /// </summary>
    public partial class Page2 : UserControl
    {
        public Page2()
        {
            InitializeComponent();
            DataContext = new Page2ViewModel(this);
        }
    }
}
