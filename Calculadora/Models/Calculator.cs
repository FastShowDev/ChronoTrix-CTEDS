﻿using System;
using System.Collections.Generic;
using System.Windows;

namespace Calculadora.Models
{
    public class Calculator
    {
        #region CONSTANTS
        public readonly string DECIMAL_SPERATOR = ",";

        public readonly double PI = Math.PI;
        public readonly double E = Math.E;
        #endregion

        #region Properties
        public bool isNumber { get; set; }
        public bool isFloat { get; set; }
        public bool isOperation { get; set; }
        public bool hasConst { get; set; }
        public bool hasCalculate { get; set; } = false;
        public string displayContent { get; set; } = "0";
        public string result { get; set; } = "";
        public Stack<string> buttonsTypePressed { get; } = new Stack<string>();
        public string? lastButtonTypePressed { get; set; }
        #endregion

        public Calculator()
        {
            buttonsTypePressed.Push("number");
        }
        

        /// <summary>
        /// Método que insere um número no display.
        /// Caso o botão de casa decimal for apertado, só permite a adição de vírgula caso for a primeira vez (isFloar == false)
        /// Faz o tramento necessário caso o display tenha apenas valor zero.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="pressedButtonValue"></param>
        /// <returns></returns>
        public void InsertNumberInDisplay(string buttonName, string pressedButtonValue)
        {
            isNumber = true;
            hasCalculate = false;
            if (displayContent == "0" && pressedButtonValue != DECIMAL_SPERATOR)
            {
                displayContent = "";
            }

            if (buttonName == "button_float")
            {
                if (buttonsTypePressed.Peek() == "float")
                {
                    return;
                }
                isFloat = true;
                buttonsTypePressed.Push("float");
                displayContent += pressedButtonValue;
                return;
            }
            if(buttonsTypePressed.Peek() == "right_parenthesis")
            {
                pressedButtonValue = "*" + pressedButtonValue;
            }

            displayContent += pressedButtonValue;
            buttonsTypePressed.Push("number");
            return;
        }


        /// <summary>
        /// Método que insere um operador no display:
        /// Caso o último botão pressionado foi um operador, ou seja, o último caracter do display for um operador aritmético atualiza para o novo operador pressionado.
        /// Caso o último botão pressionado foi um número, apenas adiciona o operador pressionado ao display.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="pressedButtonValue"></param>
        /// <returns></returns>
        public void InsertOperatorInDisplay(string buttonName, string pressedButtonValue)
        {
            hasCalculate = false;
            if(buttonsTypePressed.Peek() == "left_parenthesis")
            {
                return;
            }


            if (buttonsTypePressed.Peek() == "operator")
            {
                int newDisplayLenght = displayContent.Length - 1;
                string display = displayContent.Substring(0, newDisplayLenght);
                display += pressedButtonValue;
                displayContent = display;
            }
            else
            {
                buttonsTypePressed.Push("operator");
                displayContent += pressedButtonValue;
            }

            return;
        }


        public void InsertParenthesisInDisplay(string value)
        {
            hasCalculate = false;
            if(buttonsTypePressed.Peek() == "number" && value != ")")
            {
                displayContent += "*(";
                buttonsTypePressed.Push("left_parenthesis");
                return;
            }

            if(displayContent == "0" && value == "(")
            {
                displayContent = value;
                buttonsTypePressed.Push("left_parenthesis");
                return;
            }

            if(buttonsTypePressed.Peek() == "operator" && value == ")")
            {
                return;
            }

            if(buttonsTypePressed.Peek() == "right_parenthesis" && value == "(")
            {
                buttonsTypePressed.Push("left_parenthesis");
                value = "*(";
                displayContent += value;
                return;
            }

            displayContent += value;

            if(value == "(")
            {
                buttonsTypePressed.Push("left_parenthesis");
            }
            else
            {
                buttonsTypePressed.Push("right_parenthesis");
            }

        }


        public void InsertConstInDisplay(string constValue)
        {
            if (buttonsTypePressed.Peek() == "float")
            {
                return;
            }

            isFloat = true;
            hasCalculate = false;
            isNumber = true;

            if (displayContent == "0")
            {
                displayContent = constValue;
                buttonsTypePressed.Push("const");
                return;
            }

            if(buttonsTypePressed.Peek() != "operator")
            {
                constValue = "*" + constValue;
            }

            buttonsTypePressed.Push("const");
            displayContent += constValue;
        }


        public void BackspaceDisplay()
        {
            buttonsTypePressed.Pop();

            if (hasCalculate)
            {
                displayContent = "0";
                result = "";
                buttonsTypePressed.Clear();
                buttonsTypePressed.Push("number");
                return;
            }

            int lenght = displayContent.Length - 1;
            if (lenght > 0)
            {
                displayContent = displayContent.Substring(0, lenght);
            }
            else
            {
                displayContent = "0";
                buttonsTypePressed.Clear();
                buttonsTypePressed.Push("number");
            }
            return;
        }



        /// <summary>
        /// Método que limpa o display e atualiza as flags
        /// </summary>
        public void ClearDisplay()
        {
            buttonsTypePressed.Clear();
            buttonsTypePressed.Push("number");
            displayContent = "0";
            result = "";
            isFloat = false;
            isNumber = true;
            isOperation = false;
            hasConst = false;
            hasCalculate = false;
        }


        /// <summary>
        /// Método que recebe uma expressão matemática simples, contendo apenas operadores aritiméticos elementares e parênteses e faz seu cálculo.
        /// É necessário que a expressão esteja corretamente digitada, caso contrário mostra um MessageBox contendo um aviso de erro.
        /// 
        /// Exemplo de expressão correta: (10 + 3)*3 -4/3 + 5 ou (10+3)*3-4/3+5
        /// Exemplo de expressão incorreta: (10+3)*3--4/3( ou (10+3)*3 4/3++5
        /// 
        /// O cálculo é feito utilizando uma propriedade do DataTable de calcular expressões matemáticas simples
        /// Outro modo possível é utilizando o método Compute() do DataTable
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Um decimal contendo o valor da expressão corretamente calculada, respeitando precedência de parênteses e operadores</returns>
        public decimal CalculateExpression(string expression)
        {
            try
            {
                if (hasConst)
                {
                    expression = expression.Replace("e", E.ToString());
                    expression = expression.Replace("π", PI.ToString());
                }
                expression = expression.Replace(",", ".");
                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("expression", string.Empty.GetType(), expression);
                System.Data.DataRow row = table.NewRow();
                table.Rows.Add(row);

                return decimal.Parse((string)row["expression"]);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return 0;
            }
        }

        public double CalculateSquareRoot(string expression)
        {
            double rooting = Convert.ToDouble(CalculateExpression(expression));

            try
            {
                return Math.Sqrt(rooting);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }

        }

        public double CalculateLog10(string expression)
        {
            double rooting = Convert.ToDouble(CalculateExpression(expression));

            try
            {
                return Math.Log10(rooting);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
        }

        public double CalculateLn(string expression)
        {
            double rooting = Convert.ToDouble(CalculateExpression(expression));

            try
            {
                return Math.Log(rooting);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
        }

        public double CalculateInversion(string expression)
        {
            double rooting = Convert.ToDouble(CalculateExpression(expression));

            try
            {
                return 1 / rooting;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
        }

        public void InsertSquareRootInDisplay(string expression)
        {
            displayContent = CalculateSquareRoot(expression).ToString();
            InsertFunctionSymbolInDisplay("sqrt", expression);
            return;
        }

        public void InsertLog10InDisplay(string expression)
        {
            displayContent = CalculateLog10(expression).ToString();
            InsertFunctionSymbolInDisplay("log", expression);
            return;
        }

        public void InsertLnInDisplay(string expression)
        {
            displayContent = CalculateLn(expression).ToString();
            InsertFunctionSymbolInDisplay("ln", expression);
            return;
        }

        public void InsertInversionInDisplay(string expression)
        {
            displayContent = CalculateInversion(expression).ToString();
            InsertFunctionSymbolInDisplay("1/", expression);
            return;
        }

        public void InsertAbsoluteInDisplay(string expression)
        {
            displayContent = CalculateAbsolute(expression).ToString();
            InsertFunctionSymbolInDisplay("Abs", expression);
            return;
        }

        public double CalculateSquare(string expression)
        {
            double value = Convert.ToDouble(CalculateExpression(expression));
            try
            {
                return Math.Pow(value, 2);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
        }

        public double CalculateAbsolute(string expression)
        {
            double rooting = Convert.ToDouble(CalculateExpression(expression));

            try
            {
                return Math.Abs(rooting);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
        }

        public void InsertSquareInDisplay(string expression)
        {
            displayContent = CalculateSquare(expression).ToString();
            InsertFunctionSymbolInDisplay("sqr", expression);
            return;
        }


        public void InsertFunctionSymbolInDisplay(string symbol, string expression)
        {
            if (hasCalculate)
            {
                result = String.Concat(symbol, "(", result, ")");

            }
            else
            {
                result = String.Concat(symbol, "(", expression, ")");
            }
        }


    }
}
