// Elements.cs: test framework and the elements which used in the tests.
//
// This program is free software; you can redistribute it and/or modify it under
// the terms of the GNU General Public License version 2 as published by the
// Free Software Foundation.
//
// This program is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more
// details.
//
// You should have received a copy of the GNU General Public License along with
// this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
//
// Copyright (c) 2009 Novell, Inc (http://www.novell.com)
//
// Authors:
//	Felicia Mu  (fxmu@novell.com)
//	Ray Wang  (rawang@novell.com)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Threading;

namespace UIAClientAPI
{
	// Basic instance which provides Find methods (e.g. FindButton, FindEdit..
	public class Element
	{
		protected AutomationElement element;

		// AutomationElement Name property.>
		public string Name
		{
			get { return element.Current.Name; }
		}

		public Element (Core.UIItems.IUIItem item)
			: this (item.AutomationElement)
		{
		}

		public Element (AutomationElement element)
		{
			this.element = element;
		}

		// Find a Element by name.
		public Element Find (ControlType type, string name)
		{
			return Find (type, name, string.Empty);
		}

		public Element Find (ControlType type, string name, string automationId)
		{
			AndCondition cond;

			if (automationId == string.Empty) {
				cond = new AndCondition (new PropertyCondition (AutomationElementIdentifiers.ControlTypeProperty, type),
					new PropertyCondition (AutomationElementIdentifiers.NameProperty, name));
			} else {
				cond = new AndCondition (new PropertyCondition (AutomationElementIdentifiers.ControlTypeProperty, type),
					new PropertyCondition (AutomationElementIdentifiers.NameProperty, name),
					new PropertyCondition (AutomationElementIdentifiers.AutomationIdProperty, automationId));
			}

			for (int i = 0; i < Config.retryTimes; i++) {
				AutomationElement control = element.FindFirst (TreeScope.Descendants, cond);
				if (control != null)
					return Promote (control);

				Thread.Sleep (Config.retryInterval);
			}

			return null;
		}

		public T [] FindAll<T> (ControlType type) where T : Element
		{
			for (int i = 0; i < Config.retryTimes; i++) {
				var cond = new PropertyCondition (AutomationElementIdentifiers.ControlTypeProperty, type);
				AutomationElementCollection controls = element.FindAll (TreeScope.Descendants, cond);
				if (controls != null)
					return Promote<T> (controls);

				Thread.Sleep (Config.retryInterval);
			}
			return null;
		}

		// To promote a AutomationElement to a certain instance of a class,
		// in order to get more specific mothods.
		protected Element Promote (AutomationElement elm)
		{
			if (elm.Current.ControlType == ControlType.Window)
				return new Window (elm);
			else if (elm.Current.ControlType == ControlType.Button)
				return new Button (elm);
			else if (elm.Current.ControlType == ControlType.Edit)
				return new Edit (elm);
			else if (elm.Current.ControlType == ControlType.CheckBox)
				return new CheckBox (elm);
			else if (elm.Current.ControlType == ControlType.RadioButton)
				return new RadioButton (elm);
			else if (elm.Current.ControlType == ControlType.TabItem)
				return new TabItem (elm);
			else if (elm.Current.ControlType == ControlType.Spinner)
				return new Spinner (elm);
			else if (elm.Current.ControlType == ControlType.ComboBox)
				return new ComboBox (elm);
			else if (elm.Current.ControlType == ControlType.MenuBar)
				return new MenuBar (elm);
			else if (elm.Current.ControlType == ControlType.MenuItem)
				return new MenuItem (elm);
			else if (elm.Current.ControlType == ControlType.List)
				return new List (elm);
			else if (elm.Current.ControlType == ControlType.ListItem)
				return new ListItem (elm);
			else if (elm.Current.ControlType == ControlType.ToolBar)
				return new ToolBar (elm);

			return new Element (elm);
		}

		protected T [] Promote<T> (AutomationElementCollection elm) where T : Element
		{
			T [] ret = new T [elm.Count];
			for (int i = 0; i < elm.Count; i++)
				ret [i] = Promote (elm [i]) as T;
			return ret;
		}

		public Window FindWindow (string name)
		{
			return (Window) Find (ControlType.Window, name);
		}

		public Button FindButton (string name)
		{
			return (Button) Find (ControlType.Button, name);
		}

		public Edit FindEdit (string name)
		{
			return (Edit) Find (ControlType.Edit, name);
		}

		public Edit FindEdit (string name, string automationId)
		{
			return (Edit) Find (ControlType.Edit, name, automationId);
		}

		public CheckBox FindCheckBox (string name)
		{
			return (CheckBox) Find (ControlType.CheckBox, name);
		}

		public RadioButton FindRadioButton (string name)
		{
			return (RadioButton) Find (ControlType.RadioButton, name);
		}

		public TabItem FindTabItem (string name)
		{
			return (TabItem) Find (ControlType.TabItem, name);
		}

		public Spinner FindSpinner (string name)
		{
			return (Spinner) Find (ControlType.Spinner, name);
		}

		public ComboBox FindComboBox (string name)
		{
			return (ComboBox) Find (ControlType.ComboBox, name);
		}

		public MenuBar FindMenuBar ()
		{
			return (MenuBar) Find (ControlType.MenuBar, "");
		}

		public MenuItem FindMenuItem (string name)
		{
			return (MenuItem) Find (ControlType.MenuItem, name);
		}

		public List FindList (string name)
		{
			return FindList (name, string.Empty);
		}

		public List FindList (string name, string automationId)
		{
			return (List) Find (ControlType.List, name, automationId);
		}

		public ListItem FindListItem (string name)
		{
			return (ListItem) Find (ControlType.ListItem, name);
		}

		public ToolBar FindToolBar (string name)
		{
			return (ToolBar) Find (ControlType.ToolBar, name);
		}

	}

	// The wrapper class of Window class.
	public class Window : Element
	{
		protected ProcedureLogger procedureLogger = new ProcedureLogger ();
		public Window (Core.UIItems.WindowItems.Window elm)
			: base (elm)
		{
		}

		public Window (AutomationElement elm)
			: base (elm)
		{
		}

		//moves the control
		public void Move (double x, double y)
		{
			TransformPattern sip = (TransformPattern) element.GetCurrentPattern (TransformPattern.Pattern);
			sip.Move (x, y);
		}

		public void Resize (double weight, double height)
		{
			TransformPattern sip = (TransformPattern) element.GetCurrentPattern (TransformPattern.Pattern);
			sip.Resize(weight, height);
		}

		public void Rotate (double degress)
		{
			TransformPattern sip = (TransformPattern) element.GetCurrentPattern (TransformPattern.Pattern);
			sip.Rotate(degress);
		}

		// Click "OK" button of the window.
		public void OK ()
		{
			ClickButton ("OK");
		}

		// Click "Cancel" button of the window.
		public void Cancel ()
		{
			ClickButton ("Cancel");
		}

		// Click "Save" button of the window.
		public void Save ()
		{
			ClickButton ("Save");
		}

		// Click "Yes" button of the window
		public void Yes ()
		{
			ClickButton ("Yes");
		}

		// Click "No" button of the window
		public void No ()
		{
			ClickButton ("No");
		}

		// Click button by its name.
		public void ClickButton (string name)
		{
			try {
				Button button = FindButton (name);
				button.Click ();
			} catch (NullReferenceException e) {
				Console.WriteLine (e);
			}
		}
	}

	// The wrapper class of Button class.
	public class Button : Element
	{
		protected ProcedureLogger procedureLogger = new ProcedureLogger ();

		public Button (AutomationElement elm)
			: base (elm)
		{
		}

		// Perform "Click" action.
		public void Click ()
		{
			procedureLogger.Action (string.Format("Click the \"{0}\" button.", this.Name));
			InvokePattern ip = (InvokePattern) element.GetCurrentPattern (InvokePattern.Pattern);
			ip.Invoke ();
		}
	}

	public class Edit : Element
	{
		protected ProcedureLogger procedureLogger = new ProcedureLogger ();

		public Edit (AutomationElement elm)
			: base (elm)
		{
		}

		// wrapper SetValue method as a property to set value to the Edit control.
		public string Value
		{
			get
			{
				ValuePattern vp = (ValuePattern) element.GetCurrentPattern (ValuePattern.Pattern);
				return vp.Current.Value;
			}
			set
			{
				procedureLogger.Action (string.Format("Set \"{0}\" to the \"{1}\".", value, this.Name));
				ValuePattern vp = (ValuePattern) element.GetCurrentPattern (ValuePattern.Pattern);
				vp.SetValue (value);
			}
		}
	}

	public class CheckBox : Element
	{
		public CheckBox (AutomationElement elm)
			: base (elm)
		{
		}

		// Perform "Toggle" action.
		public void Toggle ()
		{
			TogglePattern tp = (TogglePattern) element.GetCurrentPattern (TogglePattern.Pattern);
			tp.Toggle ();
		}
	}

	public class RadioButton : Element
	{
		protected ProcedureLogger procedureLogger = new ProcedureLogger ();

		public RadioButton (AutomationElement elm)
			: base (elm)
		{
		}

		public void Select ()
		{
			procedureLogger.Action (string.Format("Select the \"{0}\" radio button.", this.Name));
			SelectionItemPattern sp = (SelectionItemPattern) element.GetCurrentPattern (SelectionItemPattern.Pattern);
			sp.Select ();
		}
	}

	public class TabItem : Element
	{
		protected ProcedureLogger procedureLogger = new ProcedureLogger ();

		public TabItem (AutomationElement elm)
			: base (elm)
		{
		}

		public void Select ()
		{
			procedureLogger.Action (string.Format ("Select the \"{0}\" tab item.", this.Name));
			SelectionItemPattern sp = (SelectionItemPattern) element.GetCurrentPattern (SelectionItemPattern.Pattern);
			sp.Select ();
		}
	}

	public class Spinner : Element
	{
		public Spinner (AutomationElement elm)
			: base (elm)
		{
		}

		public double Value
		{
			get
			{
				RangeValuePattern rp = (RangeValuePattern) element.GetCurrentPattern (RangeValuePattern.Pattern);
				return rp.Current.Value;
			}
			set
			{
				RangeValuePattern rp = (RangeValuePattern) element.GetCurrentPattern (RangeValuePattern.Pattern);
				rp.SetValue (value);
			}
		}
	}

	public class ComboBox : Element
	{
		protected ProcedureLogger procedureLogger = new ProcedureLogger ();

		public ComboBox (AutomationElement elm)
			: base (elm)
		{
		}

		public void Expand ()
		{
			procedureLogger.Action (string.Format("Expand the \"{0}\".", this.Name));
			ExpandCollapsePattern ecp = (ExpandCollapsePattern) element.GetCurrentPattern (ExpandCollapsePattern.Pattern);
			ecp.Expand ();
		}

		public void Collapse ()
		{
			procedureLogger.Action (string.Format ("Collapse the \"{0}\".", this.Name));
			ExpandCollapsePattern ecp = (ExpandCollapsePattern) element.GetCurrentPattern (ExpandCollapsePattern.Pattern);
			ecp.Collapse ();
		}
	}

	public class MenuBar : Element
	{
		public MenuBar (AutomationElement elm)
			: base (elm)
		{
		}
	}

	public class MenuItem : Element
	{
		protected ProcedureLogger procedureLogger = new ProcedureLogger ();

		public MenuItem (AutomationElement elm)
			: base (elm)
		{
		}

		public void Click ()
		{
			procedureLogger.Action (string.Format ("Click the \"{0}\" menu item.", this.Name));
			InvokePattern ip = (InvokePattern) element.GetCurrentPattern (InvokePattern.Pattern);
			ip.Invoke ();
		}
	}

	public class List : Element
	{
		public List (AutomationElement elm)
			: base (elm)
		{
		}
	}

	public class ListItem : Element
	{
		public ListItem (AutomationElement elm)
			: base (elm)
		{
		}

		public void Show ()
		{
			ScrollItemPattern sip = (ScrollItemPattern) element.GetCurrentPattern (ScrollItemPattern.Pattern);
			sip.ScrollIntoView ();
		}

		public void Select ()
		{
			SelectionItemPattern sip = (SelectionItemPattern) element.GetCurrentPattern (SelectionItemPattern.Pattern);
			sip.Select ();
		}
	}

	public class ToolBar : Element
	{
		public ToolBar (AutomationElement elm)
			: base (elm)
		{
		}
	}



	
}
