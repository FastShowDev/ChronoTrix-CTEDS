﻿using Calculadora.Models;
using Calculadora.ViewModels;
using System;
using System.Windows.Controls;

namespace Calculadora.Commands
{
    public class ConstClickCommand : BaseCommand
    {
        private StandardCalculatorViewModel _viewModel;
        public ConstClickCommand(StandardCalculatorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if(parameter != null)
            {
                string buttonName = ((Button)parameter).Name;

                if (buttonName == "button_pi")
                {
                    CalculatorDisplay.InsertConstInDisplay("π");
                    _viewModel.DisplayContent = CalculatorDisplay.DisplayContent;
                }
                else if(buttonName == "button_e")
                {
                    CalculatorDisplay.InsertConstInDisplay("e");
                    _viewModel.DisplayContent = CalculatorDisplay.DisplayContent;
                }
            }
            else throw new ArgumentNullException(nameof(parameter));
        }
    }
}
