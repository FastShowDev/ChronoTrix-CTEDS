﻿using Calculadora.Models;
using Calculadora.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculadora.Commands
{
    public class OperatorClickCommand : BaseCommand
    {
        private readonly StandardCalculatorViewModel _viewModel;
        public OperatorClickCommand(StandardCalculatorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if(parameter != null)
            {
                string pressedButtonValue = (parameter as Button).Content.ToString();

                if (pressedButtonValue == "(" || pressedButtonValue == ")")
                {
                    _viewModel.calculator.InsertParenthesisInDisplay(pressedButtonValue);
                    _viewModel.displayContent = _viewModel.calculator.displayContent;
                    return;
                }

                if(pressedButtonValue == "mod")
                {
                    pressedButtonValue = "%";
                }

                _viewModel.calculator.InsertOperatorInDisplay(pressedButtonValue);
                _viewModel.displayContent = _viewModel.calculator.displayContent;
            }
        }
    }
}
