using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Xamarin.Forms;

namespace TwentyFour
{
    public partial class MainPage : ContentPage
    {
        private string _solution;

        private int[] _numbers = new int[4];
        private List<int> _signs = new List<int>();
        private List<int> _guesses = new List<int>();

        private int _numberPosition = 0;
        private int _signPosition = 0;

        private int ADD = 0;
        private int MINUS = 1;
        private int DIVIDE = 2;
        private int MULTIPLY = 3;

        private bool _error = false;

        private bool _btn1Checked = false;
        public bool Btn1Checked
        {
            get { return _btn1Checked; }
            set
            {
                if (value != _btn1Checked)
                {
                    _btn1Checked = value;
                }
            }
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            GenerateNumbers();
            SetDefaultVisual();
            _error = false;
            btnReset_Click(null, null);
        }

        private void SetDefaultVisual()
        {
            btnNumber1.BackgroundColor = Color.FromRgb(51, 204, 255);
            btnNumber1.TextColor = Color.White;
            btnNumber1.BorderColor = Color.White;
            btnNumber1.BorderWidth = 2;

            btnNumber2.BackgroundColor = Color.FromRgb(51, 204, 255);
            btnNumber2.TextColor = Color.White;
            btnNumber2.BorderColor = Color.White;
            btnNumber2.BorderWidth = 2;

            btnNumber3.BackgroundColor = Color.FromRgb(51, 204, 255);
            btnNumber3.TextColor = Color.White;
            btnNumber3.BorderColor = Color.White;
            btnNumber3.BorderWidth = 2;

            btnNumber4.BackgroundColor = Color.FromRgb(51, 204, 255);
            btnNumber4.TextColor = Color.White;
            btnNumber4.BorderColor = Color.White;
            btnNumber4.BorderWidth = 2;

            btnNewGame.BackgroundColor = Color.FromRgb(51, 204, 255);
            btnNewGame.TextColor = Color.White;
            btnNewGame.BorderColor = Color.White;
            btnNewGame.BorderWidth = 2;

            btnReset.BackgroundColor = Color.FromRgb(51, 204, 255);
            btnReset.TextColor = Color.White;
            btnReset.BorderColor = Color.White;
            btnReset.BorderWidth = 2;

            btnSolution.BackgroundColor = Color.FromRgb(51, 204, 255);
            btnSolution.TextColor = Color.White;
            btnSolution.BorderColor = Color.White;
            btnSolution.BorderWidth = 2;

            btnPlus.BackgroundColor = Color.FromRgb(0, 153, 51);
            btnPlus.TextColor = Color.White;
            btnPlus.BorderColor = Color.White;
            btnPlus.BorderWidth = 2;

            btnMinus.BackgroundColor = Color.Maroon;
            btnMinus.TextColor = Color.White;
            btnMinus.BorderColor = Color.White;
            btnMinus.BorderWidth = 2;

            btnDivide.BackgroundColor = Color.Blue;
            btnDivide.TextColor = Color.White;
            btnDivide.BorderColor = Color.White;
            btnDivide.BorderWidth = 2;

            btnMultiply.BackgroundColor = Color.Orange;
            btnMultiply.TextColor = Color.White;
            btnMultiply.BorderColor = Color.White;
            btnMultiply.BorderWidth = 2;

        }

        private void GenerateNumbers()
        {
            _solution = "";

            int balance = 24;
            int lastRnd = -1;

            Random rand = new Random(DateTime.Now.Millisecond);
            int rnd = rand.Next(4);

            for (int ix = 0; ix < 3; ix++)
            {

                _numbers[ix] = rand.Next(4) + 2;
                switch (rnd)
                {
                    case 0:
                        balance += _numbers[ix];
                        _solution += string.Format("{0} - ", _numbers[ix]);
                        break;
                    case 1:
                        balance -= _numbers[ix];
                        _solution += string.Format("{0} + ", _numbers[ix]);
                        break;
                    case 2:
                        balance *= _numbers[ix];
                        _solution += string.Format("{0} / ", _numbers[ix]);
                        break;
                    case 3:
                        while (balance % _numbers[ix] != 0)
                        {
                            _numbers[ix] -= 1;
                        }
                        balance /= _numbers[ix];
                        _solution += string.Format("{0} * ", _numbers[ix]);
                        break;
                }

                lastRnd = rnd;
                while (lastRnd == rnd)
                {
                    rnd = rand.Next(4);
                }
            }

            _numbers[3] = balance;
            _solution += string.Format("{0}", balance);

            string[] split = _solution.Split(' ');
            _solution = string.Format("(({0} {1} {2}) {3} {4}) {5} {6} = 24", split[6], split[5], split[4], split[3], split[2], split[1], split[0]);

            rnd = rand.Next(4);
            switch (rnd)
            { 
                case 0:
                    btnNumber1.Text = _numbers[0].ToString();
                    btnNumber2.Text = _numbers[1].ToString();
                    btnNumber3.Text = _numbers[2].ToString();
                    btnNumber4.Text = _numbers[3].ToString();


                    break;
                case 1:
                    btnNumber4.Text = _numbers[0].ToString();
                    btnNumber2.Text = _numbers[1].ToString();
                    btnNumber3.Text = _numbers[2].ToString();
                    btnNumber1.Text = _numbers[3].ToString();
                    break;
                case 2:
                    btnNumber4.Text = _numbers[0].ToString();
                    btnNumber1.Text = _numbers[1].ToString();
                    btnNumber2.Text = _numbers[2].ToString();
                    btnNumber3.Text = _numbers[3].ToString();
                    break;
                case 3:
                    btnNumber1.Text = _numbers[0].ToString();
                    btnNumber4.Text = _numbers[1].ToString();
                    btnNumber2.Text = _numbers[2].ToString();
                    btnNumber3.Text = _numbers[3].ToString();
                    break;
            }


        }

        private int Sum()
        {
            int balance = 0;

            if (_guesses.Count == 1)
            {
                balance = _guesses[0];
            }
            if (_guesses.Count == 2)
            {
                balance = UseSign(_guesses[0], _guesses[1], 0);
            }
            if (_guesses.Count == 3)
            {
                balance = UseSign(UseSign(_guesses[0], _guesses[1], 0), _guesses[2], 1);
            }
            if (_guesses.Count == 4)
            {
                balance = UseSign(UseSign(UseSign(_guesses[0], _guesses[1], 0), _guesses[2], 1), _guesses[3], 2);
            }
            return balance;
        }

        private int UseSign(int number1, int number2, int signPosition)
        {
            _error = false;

            int sum = 0;
            int sign = -1;

            if (_signs.Count >= signPosition)
            {
                sign = _signs[signPosition];
            }

            switch (sign)
            {
                case 0:
                    sum = number1 + number2;
                    break;
                case 1:
                    sum = number1 - number2;
                    break;
                case 2:
                    if (number2 == 0)
                    {
                        sum = 0;
                    }
                    else
                    {
                        sum = number1 / number2;
                        if (number1 % number2 != 0)
                        {
                            _error = true;
                        }
                    }
                    break;
                case 3:
                    sum = number1 * number2;
                    break;
            }
            return sum;
        }

        private void BuildLabel()
        {
            string[] signs = new string[3];

            signs[0] = string.Empty;
            signs[1] = string.Empty;
            signs[2] = string.Empty;

            for (int ix = 0; ix < _signs.Count; ix++)
            {
                switch (_signs[ix])
                {
                    case 0:
                        signs[ix] = "+";
                        break;
                    case 1:
                        signs[ix] = "-";
                        break;
                    case 2:
                        signs[ix] = "/";
                        break;
                    case 3:
                        signs[ix] = "*";
                        break;
                }
            }

            int balance = Sum();

            if (_error)
            {
                txtSum.TextColor = Color.Red;
                txtCalc.TextColor = Color.Red;
            }
            else
            {
                txtSum.TextColor = Color.White;
                txtCalc.TextColor = Color.White;
            }

            if (_guesses.Count == 0)
            {
                txtCalc.Text = string.Empty;
            }
            if (_guesses.Count == 1)
            {
                txtCalc.Text = string.Format("{0} {1} =", _guesses[0], signs[0]);
            }
            if (_guesses.Count == 2)
            {
                txtCalc.Text = string.Format("({0} {1} {2}) {3} =", _guesses[0], signs[0], _guesses[1], signs[1]);
            }
            if (_guesses.Count == 3)
            {
                txtCalc.Text = string.Format("(({0} {1} {2}) {3} {4}) {5} =", _guesses[0], signs[0], _guesses[1], signs[1], _guesses[2], signs[2]);
            }
            if (_guesses.Count == 4)
            {
                txtCalc.Text = string.Format("(({0} {1} {2}) {3} {4}) {5} {6} =", _guesses[0], signs[0], _guesses[1], signs[1], _guesses[2], signs[2], _guesses[3]);
            }
            txtSum.Text = balance.ToString();
        }

        private void UpdateStats(bool won)
        {

        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            btnReset_Click(sender, e);
            GenerateNumbers();
        }

        private void ToggleButton(Button button)
        {
            if (button.BackgroundColor.Equals(Color.White))
            {
                button.BackgroundColor = Color.FromRgb(51, 204, 255);
                button.TextColor = Color.White;
            }
            else
            {
                button.BackgroundColor = Color.White;
                button.TextColor = Color.Black;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtCalc.Text = "Use all #'s to get";
            txtSum.Text = "24";
           
            _signPosition = 0;
            _numberPosition = 0;

            txtSum.TextColor = Color.White;
            txtCalc.TextColor = Color.White;

            _guesses.Clear();
            _signs.Clear();

            _error = false;

            SetDefaultVisual();
        }

        private void AddNumber(object sender, EventArgs e)
        {
            Button number = (Button)sender;

            if (number.BackgroundColor.Equals(Color.White))
            {
                _guesses.Remove(Convert.ToInt32(number.Text));
                if (_signs.Count > 0)
                {
                    _signs.RemoveAt(_signs.Count - 1);
                    _signPosition--;
                }
                BuildLabel();
                _numberPosition--;

                ToggleButton(number);

                return;
            }

            if (_signPosition < _numberPosition)
            {
                return;
            }

            _guesses.Add(Convert.ToInt32(number.Text));
            _numberPosition++;

            ToggleButton(number);

            BuildLabel();

            if (_guesses.Count == 4)
            {
                if (Sum() == 24)
                {
                    string message = "Congratulations!\r\nYour Solution\r\n" + txtCalc.Text + txtSum.Text + "\r\nComputed Solution\r\n" + _solution;
                    UpdateStats(true);
                    DisplayAlert("You Win!", message, "OK");
                    btnReset_Click(null, null);
                    btnNewGame_Click(null, null);
                }
                else
                {
                    // UpdateStats(false);
                    DisplayAlert("Try Again", "Not quite the correct solution, try again!", "OK");
                    btnReset_Click(null, null);
                }
            }

        }

        private void AddSign(object sender, EventArgs e)
        {
            if (_guesses.Count == 0)
            {
                return;
            }
            if (_guesses.Count == _signs.Count)
            {
                _signs.RemoveAt(_signs.Count - 1);
                BuildLabel();
                return;
            }

            Button sign = (Button)sender;
            switch (sign.Text)
            {
                case "+":
                    _signs.Add(this.ADD);
                    break;
                case "-":
                    _signs.Add(this.MINUS);
                    break;
                case "/":
                    _signs.Add(this.DIVIDE);
                    break;
                case "*":
                    _signs.Add(this.MULTIPLY);
                    break;
            }
            _signPosition++;

            BuildLabel();
        }

        private void btnSolution_Click(object sender, EventArgs e)
        {
            DisplayAlert("Solution", "Computed Solution\r\n" + _solution, "OK");
            btnNewGame_Click(null, null);
        }

    }
}