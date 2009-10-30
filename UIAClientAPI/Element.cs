// Element.cs: the base class of each control class wrapper.
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
//	Ray Wang  (rawang@novell.com)
//	Felicia Mu  (fxmu@novell.com)


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

			for (int i = 0; i < Config.Instance.RetryTimes; i++) {
				AutomationElement control = element.FindFirst (TreeScope.Descendants, cond);
				if (control != null)
					return Promote (control);

				Thread.Sleep (Config.Instance.RetryInterval);
			}

			return null;
		}

		public T [] FindAll<T> (ControlType type) where T : Element
		{
			for (int i = 0; i < Config.Instance.RetryTimes; i++) {
				var cond = new PropertyCondition (AutomationElementIdentifiers.ControlTypeProperty, type);
				AutomationElementCollection controls = element.FindAll (TreeScope.Descendants, cond);
				if (controls != null)
					return Promote<T> (controls);

				Thread.Sleep (Config.Instance.RetryInterval);
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
			else if (elm.Current.ControlType == ControlType.DataGrid)
				return new DataGrid (elm);
			else if (elm.Current.ControlType == ControlType.Document)
				return new Document (elm);
			else if (elm.Current.ControlType == ControlType.ScrollBar)
				return new ScrollBar (elm);
			else if (elm.Current.ControlType == ControlType.Text)
				return new Text (elm);
			else if (elm.Current.ControlType == ControlType.Pane)
				return new Pane (elm);

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

		public DataGrid FindDataGrid (string name)
		{
			return (DataGrid) Find (ControlType.DataGrid, name);
		}

		public Document FindDocument (string name)
		{
			return (Document) Find (ControlType.Document, name);
		}

		public ScrollBar FindScrollBar (string name)
		{
			return (ScrollBar) Find (ControlType.ScrollBar, name);
		}

		public Text FindText (string name)
		{
			return (Text) Find (ControlType.Text, name);
		}

		public Pane FindPane (string name)
		{
			return (Pane) Find (ControlType.Pane, name);
		}
	}
}
