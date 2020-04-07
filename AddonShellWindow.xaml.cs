#region
using NinjaTrader;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Gui.Tools;
using NinjaTrader.NinjaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Automation;
using System.Xml.Serialization;
using NinjaTrader.Core;
using NinjaTrader.Gui;
using System.IO;
using System.Windows.Markup;
using IInstrumentProvider = NinjaTrader.Gui.Tools.IInstrumentProvider;
#endregion

namespace NinjaTrader.NinjaScript.AddOns
{
	/// <summary>
	/// Interaction logic for xaml
	/// </summary>
	public partial class AddonShellWindow : NTTabPage
	{
		#region Variables
		private Order entryOrder;
		private OrderType orderType;
		#endregion
		#region construtor da classe
		public AddonShellWindow() 
		{
			// the content of the page will go here.
			InitializeComponent();
			RadioButtonMkt.Checked += (o, args) =>
			{
				outputBox.Text = "Market";
				orderType = OrderType.Market;
			};
			RadioButtonLmt.Checked += (o, args) =>
			{
				outputBox.Text = "Limit";
				orderType = OrderType.Limit;
			};
		}
        #endregion
		#region outros
		// Cleanup() is called when the script is ending, this is to remove used resources or event handlers. 
		// Called by TabControl when tab is being removed or window is closed
		public override void Cleanup()
		{
			// a call to base.Cleanup() will loop through the visual tree looking for all ICleanable children
			// i.e., AccountSelector, AtmStrategySelector, InstrumentSelector, IntervalSelector, TifSelector,
			// as well as unregister any link control events
			base.Cleanup();
		}

		// This returns the default Tab Header text.
		protected override string GetHeaderPart(string variable)
		{
			return "Tab";
		}

		// These are used for when the workspace is Restoring or Saving.
		// NTTabPage member. Required for restoring elements from workspace
		protected override void Restore(XElement element)
		{
			if (element == null)
				return;
		}

		// NTTabPage member. Required for storing elements to workspace
		protected override void Save(XElement element)
		{
			if (element == null)
				return;
		}
		#endregion
		private void acctValuesButton_Click(object sender, RoutedEventArgs e)
		{
		}

		private void marketDataButton_Click(object sender, RoutedEventArgs e)
		{
			outputBox.Text = "Market Data Subscription Button";
			NinjaTrader.Code.Output.Process("Market Data Subscription Button", PrintTo.OutputTab1);
			NinjaTrader.NinjaScript.NinjaScript.Log("Market Data Subscription Button", LogLevel.Information);
		}

		private void buyButton_Click(object sender, RoutedEventArgs e)
		{
			entryOrder = accountSelector.SelectedAccount.CreateOrder(
					instrumentSelector.Instrument,// Order instrument

					OrderAction.Buy,           // Possible values: 
											   //  OrderAction.Buy
											   //  OrderAction.BuyToCover
											   //  OrderAction.Sell
											   //  OrderAction.SellShort

					orderType,          // Possible values:
										//  OrderType.Limit
										//  OrderType.Market
										//  OrderType.MIT
										//  OrderType.StopMarket
										//  OrderType.StopLimit

					OrderEntry.Automated,       // Possible values:
												//  OrderEntry.Automated
												//  OrderEntry.Manual
												// Allows setting the tag for orders submitted manually or via automated trading logic

					TimeInForce.Day,            // Possible values:
												//  TimeInForce.Day
												//  TimeInForce.Gtc
												//  TimeInForce.Gtd
												//  TimeInForce.Ioc
												//  TimeInForce.Opg

					qudSelector.Value,                          // Order quantity

					0,                          // Order limit price. Use "0" should this parameter be irrelevant for the OrderType being submitted.

					Convert.ToDouble(Value.Text),// Order stop price.Use "0" should this parameter be irrelevant for the OrderType being submitted.

				   string.Empty,                     // A string representing the OCO ID used to link OCO orders together

				   string.Empty,                // A string representing the name of the order. Max 50 characters.

					NinjaTrader.Core.Globals.MaxDate,// A DateTime value to be used with TimeInForce.Gtd - for all other cases you can pass in Core.Globals.MaxDate

					null                        // Custom order if it is being used
            );
			accountSelector.SelectedAccount.Submit(new[] { entryOrder });
		}

		private void sellButton_Click(object sender, RoutedEventArgs e)
		{
			entryOrder = accountSelector.SelectedAccount.CreateOrder(
					instrumentSelector.Instrument,// Order instrument

					OrderAction.Sell,           // Possible values: 
												//  OrderAction.Buy
												//  OrderAction.BuyToCover
												//  OrderAction.Sell
												//  OrderAction.SellShort

					orderType,           // Possible values:
												//  OrderType.Limit
												//  OrderType.Market
												//  OrderType.MIT
												//  OrderType.StopMarket
												//  OrderType.StopLimit

					OrderEntry.Automated,       // Possible values:
												//  OrderEntry.Automated
												//  OrderEntry.Manual
												// Allows setting the tag for orders submitted manually or via automated trading logic

					TimeInForce.Day,            // Possible values:
												//  TimeInForce.Day
												//  TimeInForce.Gtc
												//  TimeInForce.Gtd
												//  TimeInForce.Ioc
												//  TimeInForce.Opg

					qudSelector.Value,                          // Order quantity

					0,                          // Order limit price. Use "0" should this parameter be irrelevant for the OrderType being submitted.

					Convert.ToDouble(Value.Text),// Order stop price.Use "0" should this parameter be irrelevant for the OrderType being submitted.

				   string.Empty,                     // A string representing the OCO ID used to link OCO orders together

				   string.Empty,                // A string representing the name of the order. Max 50 characters.

					NinjaTrader.Core.Globals.MaxDate,// A DateTime value to be used with TimeInForce.Gtd - for all other cases you can pass in Core.Globals.MaxDate

					null                        // Custom order if it is being used
			);
			accountSelector.SelectedAccount.Submit(new[] { entryOrder });

		}

		private void marketDepthButton_Click(object sender, RoutedEventArgs e)
		{
			
		}
	}
}
