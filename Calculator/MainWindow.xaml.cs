using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace Calculator
{
	public partial class MainWindow : Window
	{
		private string calc_curInput = "0";
		private int calc_operation = -1; //0 - add / 1 - subtract / 2 - multiply / 3 - divide
		private double calc_firstInpt;
		private double calc_secondInpt;
		private bool calc_working = false;
		private bool calc_Negative = false;
		private ArrayList calc_inputs = new ArrayList();
		private ArrayList calc_operations = new ArrayList();

		public MainWindow()
		{
			InitializeComponent();
			buttonAvailability(false, "e");
		}

		private void buttonAvailability(bool enabled, string controls)
		{
			calc_working = true;
			ArrayList ctrlArr = new ArrayList();
			for (int i = 0; i < controls.Length; i++)
			{
				ctrlArr.Add(controls[i]);
			}
			Dictionary<char, dynamic> buttons = new Dictionary<char, dynamic>()
			{
				{'0',button0},
				{'1',button1},
				{'2',button2},
				{'3',button3},
				{'4',button4},
				{'5',button5},
				{'6',button6},
				{'7',button7},
				{'8',button8},
				{'9',button9},
				{'p',buttonPlus},
				{'m',buttonMinus},
				{'t',buttonTimes},
				{'d',buttonDivide},
				{'f',buttonFactorial},
				{'e',buttonEquals},
				{'c',buttonClear},
				{'.',buttonDec},
				{'n',buttonPosNeg}
			};
			foreach (char i in ctrlArr)
			{
				buttons[i].IsEnabled = enabled;
			}
			calc_working = false;
		}

		private void filterInput(string _inpt)
		{
			calc_working = true;
			string _commaSTR = _inpt;
			int decIndex = _inpt.IndexOf(".");
			int negIndex = _inpt.IndexOf("-");

			if (decIndex != -1)
			{
				_commaSTR = _inpt.Substring(0, decIndex);
			}

			if (negIndex != -1)
			{
				_commaSTR = _inpt.Substring(1,(_commaSTR.Length - 1));
			}

			if (_commaSTR.Length > 3)
			{
				ArrayList ARR = new ArrayList();

				int iterator = 3;
				int commaCount = 0;
				int commaCount_INT = _commaSTR.Length / 3;
				double commaCount_DOUBLE = _commaSTR.Length / 3.0;
				string commaCount_INT_STR = "" + commaCount_INT;
				string commaCount_DOUBLE_STR = "" + commaCount_DOUBLE;
				if (commaCount_INT_STR.Equals(commaCount_DOUBLE_STR))
				{ //if it's evenly divisable by 3
					commaCount = (_commaSTR.Length / 3) - 1;
				}
				else
				{
					commaCount = _commaSTR.Length / 3;
				}

				//populate the array list
				for (var i = 0; i < _commaSTR.Length; i++)
				{
					ARR.Add(_commaSTR[i]);
				}

				ARR.Reverse();

				//add the commas
				for (var i = 0; i < commaCount; i++)
				{
					ARR.Insert(iterator, ',');
					iterator += 4;
				}

				ARR.Reverse();

				string _newSTR = "";

				//finalize our end-result
				for (var i = 0; i < ARR.Count; i++)
				{
					_newSTR += ARR[i];
				}

				if (decIndex != -1)
				{
					labelOutput.Content = _newSTR + _inpt.Substring(decIndex, (_inpt.Length) - decIndex);
				}
				else
				{
					labelOutput.Content = _newSTR;
				}
				if (negIndex != -1)
				{
					labelOutput.Content = "-" + labelOutput.Content;
				}
			} else {
				if (decIndex != -1)
				{
					labelOutput.Content = _commaSTR + _inpt.Substring(decIndex, (_inpt.Length) - decIndex);
				} else {
					labelOutput.Content = _commaSTR;
				}
				if (negIndex != -1)
				{
					labelOutput.Content = "-" + labelOutput.Content;
				}
			}
			calc_working = false;
		}

		private void filterInput(double _input)
		{ //overload
			calc_working = true;
			string _inpt = "" + _input;
			string _commaSTR = _inpt;
			int decIndex = _inpt.IndexOf(".");
			int negIndex = _inpt.IndexOf("-");

			if (decIndex != -1)
			{
				_commaSTR = _inpt.Substring(0, decIndex);
			}

			if (negIndex != -1)
			{
				_commaSTR = _inpt.Substring(1, (_commaSTR.Length - 1));
			}

			if (_commaSTR.Length > 3)
			{
				ArrayList ARR = new ArrayList();

				int iterator = 3;
				int commaCount = 0;
				int commaCount_INT = _commaSTR.Length / 3;
				double commaCount_DOUBLE = _commaSTR.Length / 3.0;
				string commaCount_INT_STR = "" + commaCount_INT;
				string commaCount_DOUBLE_STR = "" + commaCount_DOUBLE;
				if (commaCount_INT_STR.Equals(commaCount_DOUBLE_STR))
				{ //if it's evenly divisable by 3
					commaCount = (_commaSTR.Length / 3) - 1;
				}
				else
				{
					commaCount = _commaSTR.Length / 3;
				}

				//populate the array list
				for (var i = 0; i < _commaSTR.Length; i++)
				{
					ARR.Add(_commaSTR[i]);
				}

				ARR.Reverse();

				//add the commas
				for (var i = 0; i < commaCount; i++)
				{
					ARR.Insert(iterator, ',');
					iterator += 4;
				}

				ARR.Reverse();

				string _newSTR = "";

				//finalize our end-result
				for (var i = 0; i < ARR.Count; i++)
				{
					_newSTR += ARR[i];
				}

				if (decIndex != -1)
				{
					labelOutput.Content = _newSTR + _inpt.Substring(decIndex, (_inpt.Length) - decIndex);
				}
				else
				{
					labelOutput.Content = _newSTR;
				}
				if (negIndex != -1)
				{
					labelOutput.Content = "-" + labelOutput.Content;
				}
			} else {
				if (decIndex != -1)
				{
					labelOutput.Content = _commaSTR + _inpt.Substring(decIndex, (_inpt.Length) - decIndex);
				} else {
					labelOutput.Content = _commaSTR;
				}
				if (negIndex != -1)
				{
					labelOutput.Content = "-" + labelOutput.Content;
				}
			}
			calc_working = false;
		}

		private void switchOperations(int oper)
		{
			buttonAvailability(false, "pmdtfn");

			//take the current input value and parse it into a long
			try
			{
				calc_firstInpt = double.Parse(calc_curInput);
			}
			catch (FormatException)
			{
				calc_firstInpt = 0;
			}

			calc_curInput = "0";
			calc_Negative = false;
			buttonAvailability(true, "e");

			calc_operation = oper;

			if (oper == 4)
			{
				//lock down all buttons except equals and clear
				//number buttons
				buttonAvailability(false, "0123456789pmtdf.");
				buttonAvailability(true, "ec");

				labelOutput.Content += "!";
			} else {
				buttonAvailability(true, ".");
			}
		}
	}
}