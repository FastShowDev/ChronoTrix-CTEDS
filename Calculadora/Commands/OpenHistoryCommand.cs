﻿using Calculadora.Stores;
using Calculadora.ViewModels;
using System.Windows.Controls;
using System.Windows;

namespace Calculadora.Commands
{
    public class OpenHistoryCommand : BaseCommand
    {
        StandardCalculatorViewModel _viewModel;
        public OpenHistoryCommand(StandardCalculatorViewModel viewModel)
        {
            _viewModel = viewModel;
        }
#nullable enable
        public override bool CanExecute(object? parameter)
        {
            return true;
        }
#nullable enable
        public override void Execute(object? parameter)
        {
            if(parameter != null)
            {
                var values = (object[])parameter;
                Border? historyMenu = values[0] as Border;
                DataGrid? dataGrid = values[1] as DataGrid;

                historyMenu.Visibility = Visibility.Visible;
                dataGrid.ItemsSource = _viewModel.GetAllHistories();
                NavigationStore.CurrentViewModel.IsMenuOpen = true;
            }
        }
    }
}
