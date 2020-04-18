#region
using System;
using System.Text;
using System.Windows.Controls;
using System.IO;
#endregion

namespace NinjaTrader.NinjaScript.AddOns
{
    public class TextBoxWriter : TextWriter
	{
		// The control where we will write text.
		private TextBox MyControl;
		public TextBoxWriter(TextBox control)
		{
			MyControl = control;
		}

		public override void Write(char value)
		{
			// MyControl.Text = Convert.ToString(value) + Environment.NewLine + MyControl.Text;
			//MyControl.Text += value;
			MyControl.AppendText(Convert.ToString(value));
		}

		public override void Write(string value)
		{
			//MyControl.Text = value+Environment.NewLine+ MyControl.Text;
			//MyControl.Text += value;
			MyControl.AppendText(value);
		}

		public override Encoding Encoding
		{
			get { return Encoding.Unicode; }
		}
	}
}
