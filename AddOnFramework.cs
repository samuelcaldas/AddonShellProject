#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Gui.Tools;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core;
using NinjaTrader.Gui;
using System.IO;
using System.Windows.Markup;
using IInstrumentProvider = NinjaTrader.Gui.Tools.IInstrumentProvider;
using NinjaTrader;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
#endregion

//This namespace holds GUI items and is required.
namespace NinjaTrader.Gui.NinjaScript
{
	/*
		* This is the Primary script in the set of scripts in this example, This script will be the first called script in NinjaTrader. 
		* Unlike other NinjaScript documents, Debugging in an addon can be done unsing the folling statement
		* Output.Process("SomeString", PrintTo.OutputTab1);
    */
	// NT creates an instance of each class derived from "AddOnBase" and call OnWindowCreated/OnWindowDestroyed for every instance and every NTWindow which is created or destroyed...
	public class AddonShell : AddOnBase
	{
		//These variables will be used in checking if the current Addon already has a menu item, and also stores the new menu item to be added.
		private NTMenuItem existingMenuItemInControlCenter;
		private NTMenuItem AddonShellMenuItem;
		
		// Same as other NS objects. However there's a difference: this event could be called in any thread
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description = "Example AddOn demonstrating some of the framework's capabilities";
				Name = "Addon Shell";
			}
		}

		// Will be called as a new NTWindow is created. It will be called in the thread of that window
		protected override void OnWindowCreated(Window window)
		{
			/*
				* The following checks if the control center window is present.
				* If the control center is found, the MainMenu is checked for existing menu items with the same name as this addon. 
				* If no existing items are found, a new menu item is added for this addon. 
			*/
			// We want to place our AddOn in the Control Center's menus
			ControlCenter controlCenter = window as ControlCenter;
			if (controlCenter == null)
				return;

			/* Determine we want to place our AddOn in the Control Center's "New" menu
            Other menus can be accessed via the control's "Automation ID". For example: toolsMenuItem, workspacesMenuItem, connectionsMenuItem, helpMenuItem. */
			existingMenuItemInControlCenter = controlCenter.FindFirst("ControlCenterMenuItemNew") as NTMenuItem;
			if (existingMenuItemInControlCenter == null)
				return;

			// This is the new menu item to be created, this assigns the menu text and will be used to Add this item to the Main Menu. 
			// 'Header' sets the name of our AddOn seen in the menu structure
			AddonShellMenuItem = new NTMenuItem { Header = "Addon Shell", Style = Application.Current.TryFindResource("MainMenuItem") as Style };

			// Add our AddOn into the "New" menu
			existingMenuItemInControlCenter.Items.Add(AddonShellMenuItem);

			// The new menu item will do nothing by its self, a click handler is added to complete the menu item and allow for clicks.
			// Subscribe to the event for when the user presses our AddOn's menu item
			AddonShellMenuItem.Click += OnMenuItemClick;
		}

		// Will be called as a new NTWindow is destroyed. It will be called in the thread of that window
		protected override void OnWindowDestroyed(Window window)
		{
			// This checks if there is not a menu item or if the destroyed window is not the control center.
			if (AddonShellMenuItem != null && window is ControlCenter)
			{
				if (existingMenuItemInControlCenter != null && existingMenuItemInControlCenter.Items.Contains(AddonShellMenuItem))
					existingMenuItemInControlCenter.Items.Remove(AddonShellMenuItem);
				
				// if the destroyed window was the control center, we clean up the click handler and remove the custom menu item and set it to null.
				AddonShellMenuItem.Click -= OnMenuItemClick;
				AddonShellMenuItem = null;
			}
		}

		// Open our AddOn's window when the menu item is clicked on
		private void OnMenuItemClick(object sender, RoutedEventArgs e)
		{
			//Core.Globals.RandomDispatcher.BeginInvoke(new Action(() => new AddonShellWindow().Show()));
			Core.Globals.RandomDispatcher.InvokeAsync(new Action(() => new AddonShellWindow().Show()));
		}
	}

	/* Class which implements Tools.INTTabFactory must be created and set as an attached property for TabControl
    in order to use tab page add/remove/move/duplicate functionality */
	public class AddonShellWindowFactory : INTTabFactory
	{
		// INTTabFactory member. Required to create parent window
		public NTWindow CreateParentWindow()
		{
			return new AddonShellWindow();
		}

		// INTTabFactory member. Required to create tabs
		public NTTabPage CreateTabPage(string typeName, bool isTrue)
		{
			return new NinjaTrader.NinjaScript.AddOns.AddonShellWindow();
		}
	}

	/* This is where we define our AddOn window. The actual content is contained inside the tabs of the window defined in public class AddonShellTab below.
        We have to create a new window class which inherits from Tools.NTWindow for styling and implements IWorkspacePersistence interface for ability to save/restore from workspaces. */
	public class AddonShellWindow : NTWindow, IWorkspacePersistence
	{
		/*
			* This is the constructor for the new NTWindow.
			* This document sets up the basic window before it gets displayed.
			* This also defines what TabPage will be used and the Window Factory that will be used in the window creation process.
			* This document is also where you would set Tab defaults like if this window will have a tab control, if the tabs are movable etc..
		*/
		public AddonShellWindow()
		{

			// set Caption property (not Title), since Title is managed internally to properly combine selected Tab Header and Caption for display in the windows taskbar
			// This is the name displayed in the top-left of the window
			Caption = "Addon Shell Window";

			// Set the default dimensions of the window
			//Width = 800;
			//Height = 600;

			// TabControl should be created for window content if tab features are wanted
			TabControl tabControl = new TabControl();

			// Attached properties defined in TabControlManager class should be set to achieve tab moving, adding/removing tabs
			TabControlManager.SetIsMovable(tabControl, false);
			TabControlManager.SetCanAddTabs(tabControl, false);
			TabControlManager.SetCanRemoveTabs(tabControl, false);

			// if ability to add new tabs is desired, TabControl has to have attached property "Factory" set.
			TabControlManager.SetFactory(tabControl, new AddonShellWindowFactory());
			Content = tabControl;

			/* In order to have link buttons functionality, tab control items must be derived from Tools.NTTabPage
            They can be added using extention method AddNTTabPage(NTTabPage page) */
			//tabControl.AddNTTabPage(new AddonShellWindowTabPage());
			tabControl.AddNTTabPage(new NinjaTrader.NinjaScript.AddOns.AddonShellWindow());

			// WorkspaceOptions property must be set
			// This is a inline Window Loaded handler, once the window loads, if the WorkspaceOptions are not present,
			// a new WorkspaceOptions object is created for this window using its GUID.
			Loaded += (o, e) =>
			{
				if (WorkspaceOptions == null)
				{
					WorkspaceOptions = new WorkspaceOptions("AddonShellWindow" + Guid.NewGuid().ToString("N"), this);
				}
			};
		}

		// IWorkspacePersistence member. Required for restoring window from workspace
		public void Restore(XDocument document, XElement element)
		{
			// This is used for restoring the elements of the document with the workspace. 
			if (MainTabControl != null)
				MainTabControl.RestoreFromXElement(element);
		}

		// IWorkspacePersistence member. Required for saving window to workspace
		public void Save(XDocument document, XElement element)
		{
			// This is used for saving the elements of the document with the workspace.
			if (MainTabControl != null)
				MainTabControl.SaveToXElement(element);
		}

		// IWorkspacePersistence member
		public WorkspaceOptions WorkspaceOptions
		{ get; set; }
	}
}