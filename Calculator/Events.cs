using System;
using System.Collections;
using System.Collections.Generic;

namespace Calculator
{
	partial class MainWindow
	{
		private void Click_Number(object sender, EventArgs e)
		{ //number controls
			if (calc_working)
			{
				return;
			}
			var str = sender.ToString();
			var value = str.Substring(32, ((str.Length) - 32));
			if (calc_curInput == "0")
			{
				if (value != "0")
				{
					calc_curInput = value;
				}
			} else {
				calc_curInput += value;
			}
			filterInput(calc_curInput);
		}

		private void Click_Operation(object sender, EventArgs e)
		{ //operation controls
			if (calc_working)
			{
				return;
			}
			var str = sender.ToString();
			var operation = str.Substring(32, ((str.Length) - 32));
			Dictionary<string, int> operations = new Dictionary<string, int>() { { "+", 0 }, { "-", 1 }, { "*", 2 }, { "/", 3 }, { "!", 4} };
			if (operation == "!")
			{
				buttonAvailability(false, "0123456789pmtdf");
			}
			switchOperations(operations[operation]);
		}

		private void buttonEquals_Click(object sender, EventArgs e)
		{
			//take the value of the second input and parse it into a long
			if (calc_working)
			{
				return;
			}
			try
			{
				calc_secondInpt = double.Parse(calc_curInput);
			}
			catch (FormatException)
			{
				calc_secondInpt = 0;
			}

			//disable all buttons except the clear button
			buttonAvailability(false, "0123456789e.n");

			//switch through the operations and perform the necessary actions for the user
			var result = 1.0;
			switch (calc_operation)
			{
				case 0:
				{ //add
					result = calc_firstInpt + calc_secondInpt;
					filterInput(result);
					break;
				}
				case 1:
				{ //subtract
					result = calc_firstInpt - calc_secondInpt;
					filterInput(result);
					break;
				}
				case 2:
				{ //multiply
					result = calc_firstInpt * calc_secondInpt;
					filterInput(result);
					break;
				}
				case 3:
				{ //divide
					try
					{ //divide by zero check
						if (calc_secondInpt == 0.0)
						{
							throw new DivideByZeroException();
						}
						result = calc_firstInpt / calc_secondInpt;
						filterInput(result);
					}
					catch (DivideByZeroException)
					{
						//lock this shit down
						buttonAvailability(false, "0123456789epmtdf");
						labelOutput.Content = "ERR: DIV BY 0"; //let the user know that they done fucked up
					}
					break;
				}
				case 4:
				{ //factorial
					for (var i = calc_firstInpt; i > 0; i--)
					{
						result *= i;
					}
					if (double.IsInfinity(result))
					{
						labelOutput.Content = "Infinity";
					}
					else
					{
						filterInput(result);
					}
					break;
				}
				default:
				{
					labelOutput.Content = "ERROR!";
					break;
				}
			}
		}

		private void buttonClear_Click(object sender, EventArgs e)
		{
			if (calc_working)
			{
				return;
			}
			//reset all current input
			calc_curInput = "";
			labelOutput.Content = "0";
			calc_firstInpt = 0;
			calc_secondInpt = 0;

			buttonAvailability(true, "0123456789pmtdf.n"); //re-enable all number buttons
			buttonAvailability(false, "e"); //disable equals button
		}

		private void buttonDec_Click(object sender, EventArgs e)
		{
			if (calc_working)
			{
				return;
			}
			if (calc_curInput == "")
			{
				calc_curInput = "0.";
			} else {
				calc_curInput += ".";
			}
			labelOutput.Content = calc_curInput;
			buttonAvailability(false, ".");
			filterInput(calc_curInput);
		}

		private void buttonPosNeg_Click(object sender, EventArgs e)
		{
			Console.WriteLine("Event Executed");
			if (calc_curInput.Equals("0")) { return; }
			if (!calc_Negative)
			{ //change to negative
				calc_curInput = calc_curInput.Insert(0, "-");
				Console.WriteLine($"Filtering input with {calc_curInput}");
				filterInput(calc_curInput);
				calc_Negative = true;
			} else { //change to positive
				calc_curInput = calc_curInput.Substring(1,(calc_curInput.Length - 1));
				Console.WriteLine($"Filtering input with {calc_curInput}");
				filterInput(calc_curInput);
				calc_Negative = false;
			}
		}
	}
}
