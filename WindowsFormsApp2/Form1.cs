using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WindowsFormsApp2
{
    public partial class Calculator : Form
    {
        public Calculator()
        {
            InitializeComponent();
        }

        private void allClearButton_Click(object sender, EventArgs e)
        {
            outputResultTextBox.Text = "";
            outputTextBox.Text = "";
        }

        private void clearLastButton_Click(object sender, EventArgs e)
        {
            int length = outputTextBox.Text.Length;

            if (length > 0)
            {
                outputTextBox.Text = outputTextBox.Text.Remove(length - 1);
            }
        }

        private void zeroButton_Click(object sender, EventArgs e)
        {
            outputTextBox.Text += "0";
        }

        private void oneButton_Click(object sender, EventArgs e)
        {
            outputTextBox.Text += "1";
        }

        private void twoButton_Click(object sender, EventArgs e)
        {
            outputTextBox.Text += "2";
        }

        private void threeButton_Click(object sender, EventArgs e)
        {
            outputTextBox.Text += "3";
        }

        private void fourButton_Click(object sender, EventArgs e)
        {
            outputTextBox.Text += "4";
        }

        private void fiveButton_Click(object sender, EventArgs e)
        {
            outputTextBox.Text += "5";
        }

        private void sixButton_Click(object sender, EventArgs e)
        {
            outputTextBox.Text += "6";
        }

        private void sevenButton_Click(object sender, EventArgs e)
        {
            outputTextBox.Text += "7";
        }

        private void eightButton_Click(object sender, EventArgs e)
        {
            outputTextBox.Text += "8";
        }

        private void nineButton_Click(object sender, EventArgs e)
        {
            outputTextBox.Text += "9";
        }

        private void pointButton_Click(object sender, EventArgs e)
        {
            isLastSymbolSign();
            outputTextBox.Text += ",";
        }

        private void divideButton_Click(object sender, EventArgs e)
        {
            isLastSymbolSign();
            replaceResult();
            outputTextBox.Text += "/";
        }

        private void plusButton_Click(object sender, EventArgs e)
        {
            isLastSymbolSign();
            replaceResult();
            outputTextBox.Text += "+";
        }

        private void minusButton_Click(object sender, EventArgs e)
        {
            isLastSymbolSign();
            replaceResult();
            outputTextBox.Text += "-";
        }

        private void multiplyButton_Click(object sender, EventArgs e)
        {
            isLastSymbolSign();
            replaceResult();
            outputTextBox.Text += "*";
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            outputResultTextBox.Text = evaluate(outputTextBox.Text);
        }

        private void degreeButton_Click(object sender, EventArgs e)
        {
            isLastSymbolSign();
            replaceResult();
            outputTextBox.Text += "^";
        }

        private void openBracketButton_Click(object sender, EventArgs e)
        {
            outputTextBox.Text += "(";
        }

        private void closeBracketButton_Click(object sender, EventArgs e)
        {
            outputTextBox.Text += ")";
        }

        private void replaceResult()
        {
            if (!outputResultTextBox.Text.Equals(""))
            {
                outputTextBox.Text = outputResultTextBox.Text;
                outputResultTextBox.Text = "";
            }
        }

        private bool isLastSymbolSign()
        {
            Regex regex = new Regex(".*\\+$|.*-$|.*\\*$|.*/$|.*\\,$|.*\\^$");
            MatchCollection matches = regex.Matches(outputTextBox.Text);

            if (matches.Count != 0)
            {
                outputTextBox.Text = outputTextBox.Text.Remove(outputTextBox.Text.Length - 1);
                return true;
            }

            return false;
        }

        private String evaluate(String statement)
        {
            if (!checkStatement(statement))
            {
                return null;
            }

            String result = getCalculatedString(statement);

            if (result == null)
            {
                return null;
            }

            return result;
        }

        private String getCalculatedString(String statement)
        {
            String result = "";

            if (statement.Contains("("))
            {
                result = statement.Substring(0, statement.IndexOf('('));
                String str1 = getCalculatedString(statement.Substring(statement.IndexOf('(') + 1));

                if (str1 == null)
                {
                    return null;
                }

                result += str1;
            }
            else
            {
                result = statement;
            }

            String calculated = "";

            if (result.Contains(")"))
            {
                calculated = calculate(result.Substring(0, result.IndexOf(')')));

                if (calculated == null)
                {
                    return null;
                }

                result = result.Replace(result.Substring(0, result.IndexOf(')') + 1), calculated);
            }
            else
            {
                result = calculate(result);
            }

            return result;
        }

        private String calculate(String statement)
        {
            List<String> list = parseStringIntoList(statement);

            list = calculateList(list, "^");
            list = calculateList(list, "*");
            list = calculateList(list, "/");
            list = calculateList(list, "-");
            list = calculateList(list, "+");

            return list[0];
        }

        private List<String> parseStringIntoList(String statement)
        {
            StringBuilder sb = new StringBuilder();
            List<String> list = new List<string>();
            int twoSignsInRowCounter = 0;

            if (statement.StartsWith("-"))
            {
                sb.Append("-");
                statement = statement.Substring(1);
            }

            
            for (int i = 0; i < statement.Length; i++)
            {
                if (isMathSign(statement[i]))
                {
                    twoSignsInRowCounter++;

                    if (twoSignsInRowCounter != 2)
                    {
                        list.Add(statement[i].ToString());
                    }
                    continue;
                }

                if (twoSignsInRowCounter == 2)
                {
                    sb.Append('-');
                }

                twoSignsInRowCounter = 0;

                while (i < statement.Length && (statement[i] >= '0' && statement[i] <= '9'
                        || statement[i] == ','))
                {
                    sb.Append(statement[i]);
                    i++;
                }

                list.Add(sb.ToString());
                i--;
                sb.Length = 0;
            }
            return list;
        }

        private List<String> calculateList(List<String> list, String symbol)
        {
            int index = 0;

            while (list.IndexOf(symbol) != -1)
            {
                index = list.IndexOf(symbol);
                list[index] = calculateTwoNumbers(list[index - 1], list[index], list[index + 1]);
                list.RemoveAt(index - 1);
                list.RemoveAt(index);
            }

            return list;
        }

        private String calculateTwoNumbers(String firstNumber, String sign, String secondNumber)
        {
            double num1 = double.Parse(firstNumber);
            double num2 = double.Parse(secondNumber);

            switch (sign)
            {
                case "*":
                    return (num1 * num2).ToString();
                case "/":
                    if (num2 == 0)
                    {
                        return null;
                    }
                    return (num1 / num2).ToString();
                case "^":
                    return Math.Pow(num1, num2).ToString();
                case "+":
                    return (num1 + num2).ToString();
                case "-":
                    return (num1 - num2).ToString();
                default:
                    return null;
            }
        }

        private bool isMathSign(char symbol)
        {
            switch (symbol)
            {
                case '+':
                case '-':
                case '/':
                case '*':
                case '^':
                    return true;

                default:
                    return false;
            }
        }

        private bool checkBrackets(String statement)
        {
            LinkedList<char> list = new LinkedList<char>();

            for (int i = 0; i < statement.Length; i++)
            {
                switch (statement[i])
                {
                    case '(':
                        list.AddLast(statement[i]);
                        break;
                    case ')':
                        if (list.Count == 0)
                        {
                            return false;
                        }

                        list.RemoveLast();
                        break;
                }
            }

            return list.Count == 0;
        }

        private bool checkDoubleSigns(String statement)
        {
            Regex regex = new Regex(".*\\*\\*+.*|.*\\+\\++.*|.*--+.*|.*//+.*|.*,,+.*");
            MatchCollection matches = regex.Matches(statement);

            if (matches.Count > 0)
            {
                return true;
            }

            return false;
        }

        private bool checkStatement(String statement)
        {
            if (statement.Equals(""))
            {
                return false;
            }

            Regex regex = new Regex(".*\\+$|.*-$|.*\\*$|.*/$|.*\\,$|.*\\($");
            MatchCollection matches = regex.Matches(statement);

            if (matches.Count > 0)
            {
                return false;
            }

            if (!checkBrackets(statement))
            {
                return false;
            }

            if (checkDoubleSigns(statement))
            {
                return false;
            }

            return true;
        }
    }
}
