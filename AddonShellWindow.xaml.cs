#region
using NinjaTrader;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Gui.Tools;
using NinjaTrader.NinjaScript;
using System;
using System.Collections.Generic;
using System.Linq;
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
        TextBoxWriter writer;
        private string orderType;
        private NinjaTrader.Cbi.Instrument instrument = Instrument.GetInstrument("ES 03-20");
        private MarketData marketData;
        #endregion
        public AddonShellWindow()
        {
            // the content of the page will go here.
            InitializeComponent();

            // Set Console output to textbox
            writer = new TextBoxWriter(this.output);
            Console.SetOut(writer);
            Console.SetError(writer);
            // Initialize market data
            marketData = new MarketData(instrument);
            marketData.Update += OnMarketData;
        }

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
            return "Addon Tab";
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

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            orderType = (sender as RadioButton).Name;
        }

        // This method is fired on market data events

        private void OnMarketData(object sender, MarketDataEventArgs e)
        {
            // Do something with market data events
        }

        private void order_Click(object sender, RoutedEventArgs e)
        {
            if (instrumentSelector.Instrument == null)
            {
                Console.WriteLine("Select an Instrument");
                return;
            }
            string Btn = (sender as Button).Name;
            
            Order entryOrder;
            OrderAction orderAction = OrderAction.Buy;
            OrderType type = OrderType.Market;
            double price = 0;

            if (Btn == "buy")
            {
                orderAction = OrderAction.Buy;
            }
            else if (Btn == "sell")
            {
                orderAction = OrderAction.Sell;
            }
            else if (Btn == "close")
            {
                return;
            }
            else
            {
                return;
            }

            if (orderType == "bid")
            {
                marketData = new MarketData(instrumentSelector.Instrument);
                type = OrderType.Limit;
                price = marketData.Bid.Price;
            }
            else if (orderType == "ask")
            {
                marketData = new MarketData(instrumentSelector.Instrument);
                type = OrderType.Limit;
                price = marketData.Ask.Price;
            }
            else if (orderType == "mkt")
            {
                type = OrderType.Market;
                price = 0;
            }
            else
            {
                type = OrderType.Market;
                price = 0;
            }
            entryOrder = accountSelector.SelectedAccount.CreateOrder(
                instrumentSelector.Instrument,  // Order instrument
                orderAction,                    // Possible values:
                                                //  OrderAction.Buy
                                                //  OrderAction.BuyToCover
                                                //  OrderAction.Sell
                                                //  OrderAction.SellShort

                type,                           // Possible values:
                                                //  OrderType.Limit
                                                //  OrderType.Market
                                                //  OrderType.MIT
                                                //  OrderType.StopMarket
                                                //  OrderType.StopLimit

                OrderEntry.Automated,           // Possible values:
                                                //  OrderEntry.Automated
                                                //  OrderEntry.Manual
                                                // Allows setting the tag for orders submitted manually or via automated trading logic
                TimeInForce.Day,                // Possible values:
                                                //  TimeInForce.Day
                                                //  TimeInForce.Gtc
                                                //  TimeInForce.Gtd
                                                //  TimeInForce.Ioc
                                                //  TimeInForce.Opg

                qudSelector.Value,              // Order quantity

                0,                              // Order limit price. Use "0" should this parameter be irrelevant for the OrderType being submitted.

                price,                          // Order stop price.Use "0" should this parameter be irrelevant for the OrderType being submitted.

                string.Empty,                   // A string representing the OCO ID used to link OCO orders together

                string.Empty,                   // A string representing the name of the order. Max 50 characters.

                NinjaTrader.Core.Globals.MaxDate,// A DateTime value to be used with TimeInForce.Gtd - for all other cases you can pass in Core.Globals.MaxDate

                null                            // Custom order if it is being used
            );
            accountSelector.SelectedAccount.Submit(new[] { entryOrder });
            Console.WriteLine("Order Type: "+orderType+" Action: "+orderAction+" Price: "+Convert.ToString(price));
        }

        private void action_Click(object sender, RoutedEventArgs e)
        {
            string Btn = (sender as Button).Name;
            if (Btn=="start")
            {
                Console.WriteLine("Insert here your addon start actions");
            }
            if (Btn == "stop")
            {
                Console.WriteLine("Insert here your addon stop actions");
            }
            if (Btn == "clearconsole")
            {
                this.output.Text = "";
            }
        }
    }
}
