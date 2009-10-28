﻿// Elements.cs: test framework and the elements which used in the tests.
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

		//the TransformPattern's method
		public void Move (double x, double y)
		{
			TransformPattern tp = (TransformPattern) element.GetCurrentPattern (TransformPattern.Pattern);
			tp.Move (x, y);
		}

		public void Resize (double width, double height)
		{
			TransformPattern tp = (TransformPattern) element.GetCurrentPattern (TransformPattern.Pattern);
			tp.Resize (width, height);
		}

		public void Rotate (double degrees)
		{
			TransformPattern tp = (TransformPattern) element.GetCurrentPattern (TransformPattern.Pattern);
			tp.Rotate (degrees);
		}

		//the TransformPattern's property
		public bool CanMove
		{
			get { return (bool) element.GetCurrentPropertyValue (TransformPattern.CanMoveProperty); }
		}

		public bool CanResize
		{
			get { return (bool) element.GetCurrentPropertyValue (TransformPattern.CanResizeProperty); }
		}

		public bool CanRotate
		{
			get { return (bool) element.GetCurrentPropertyValue (TransformPattern.CanRotateProperty); }
		}

		//the WindowPattern's method
		public void SetWindowVisualState (WindowVisualState state)
		{
			procedureLogger.Action (string.Format ("Set \"{0}\" to be \"{1}\".", this.Name, state));
			WindowPattern wp = (WindowPattern) element.GetCurrentPattern (WindowPattern.Pattern);
			wp.SetWindowVisualState (state);
		}

		public void Maximized ()
		{
			SetWindowVisualState (WindowVisualState.Maximized);
		}

		public void Minimized ()
		{
			SetWindowVisualState (WindowVisualState.Minimized);
		}

		public void Normal ()
		{
			SetWindowVisualState (WindowVisualState.Normal);
		}

		//the WindowPattern's property
		public bool CanMaximize
		{
			get { return (bool) element.GetCurrentPropertyValue (WindowPattern.CanMaximizeProperty); }
		}

		public bool CanMinimize
		{
			get { return (bool) element.GetCurrentPropertyValue (WindowPattern.CanMinimizeProperty); }
		}

		public bool IsModal
		{
			get { return (bool) element.GetCurrentPropertyValue (WindowPattern.IsModalProperty); }
		}

		public bool IsTopmost
		{
			get { return (bool) element.GetCurrentPropertyValue (WindowPattern.IsTopmostProperty); }
		}

		public WindowVisualState WindowVisualState
		{
			get { return (WindowVisualState) element.GetCurrentPropertyValue (WindowPattern.WindowInteractionStateProperty); }
		}

		public WindowInteractionState WindowInteractionState
		{
			get { return (WindowInteractionState) element.GetCurrentPropertyValue (WindowPattern.WindowInteractionStateProperty); }
		}		

		// Click "OK" button of the window.
		public void OK ()
		{
			ClickButton ("OK", true);
		}

		public void OK (bool log)
		{
			ClickButton ("OK", log);
		}

		// Click "Cancel" button of the window.
		public void Cancel ()
		{
			ClickButton ("Cancel", true);
		}

		public void Cancel (bool log)
		{
			ClickButton ("Cancel", log);
		}

		// Click "Save" button of the window.
		public void Save ()
		{
			ClickButton ("Save", true);
		}

		public void Save (bool log)
		{
			ClickButton ("Save", log);
		}

		// Click "Yes" button of the window
		public void Yes ()
		{
			ClickButton ("Yes", true);
		}

		public void Yes (bool log)
		{
			ClickButton ("Yes", log);
		}

		// Click "No" button of the window
		public void No ()
		{
			ClickButton ("No", true);
		}

		public void No (bool log)
		{
			ClickButton ("No", log);
		}

		// Click button by its name.
		private void ClickButton (string name, bool log)
		{
			try {
				Button button = FindButton (name);
				button.Click (log);
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
			Click (true);
		}

		public void Click (bool log)
		{
			if (log == true) 
				procedureLogger.Action (string.Format ("Click the \"{0}\" button.", this.Name));

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
			Select (true);
		}

		public void Select (bool log)
		{
			if (log == true)
				procedureLogger.Action (string.Format ("Select the \"{0}\" radio button.", this.Name));

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
			Select (true);
		}

		public void Select (bool log)
		{
			if (log == true)
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
			Expand (true);
		}

		public void Expand (bool log)
		{
			if (log == true)
				procedureLogger.Action (string.Format ("Expand the \"{0}\".", this.Name));

			ExpandCollapsePattern ecp = (ExpandCollapsePattern) element.GetCurrentPattern (ExpandCollapsePattern.Pattern);
			ecp.Expand ();
		}

		public void Collapse ()
		{
			Collapse (true);
		}

		public void Collapse (bool log)
		{
			if (log == true)
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
			Click (true);
		}

		public void Click (bool log)
		{
			if (log == true)
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
		protected ProcedureLogger procedureLogger = new ProcedureLogger ();

		public ListItem (AutomationElement elm)
			: base (elm)
		{
		}

		public void Show ()
		{
			Show (true);
		}

		public void Show (bool log)
		{
			if (log == true)
				procedureLogger.Action (string.Format("Scroll {0} Into View.", this.Name));

			ScrollItemPattern sip = (ScrollItemPattern) element.GetCurrentPattern (ScrollItemPattern.Pattern);
			sip.ScrollIntoView ();
		}

		public void Select ()
		{
			Select (true);
		}

		public void Select (bool log)
		{
			if (log == true)
				procedureLogger.Action (string.Format ("{0} is selected.", this.Name));

			SelectionItemPattern sip = (SelectionItemPattern) element.GetCurrentPattern (SelectionItemPattern.Pattern);
			sip.Select ();
		}

		public void RemoveFromSelection ()
		{
			RemoveFromSelection (true);
		}

		public void RemoveFromSelection (bool log)
		{
			if (log == true)
				procedureLogger.Action (string.Format ("{0} is removed from selection.", this.Name));

			SelectionItemPattern sip = (SelectionItemPattern) element.GetCurrentPattern (SelectionItemPattern.Pattern);
			sip.RemoveFromSelection ();
		}

		public void AddToSelection ()
		{
			AddToSelection (true);
		}

		public void AddToSelection (bool log)
		{
			if (log == true)
				procedureLogger.Action (string.Format ("{0} is added to selection.", this.Name));

			SelectionItemPattern sip = (SelectionItemPattern) element.GetCurrentPattern (SelectionItemPattern.Pattern);
			sip.AddToSelection ();
		}
	}

	public class ToolBar : Element
	{
		public ToolBar (AutomationElement elm)
			: base (elm)
		{
		}
	}

	public class DataGrid : Element
	{
		public DataGrid (AutomationElement elm)
			: base (elm)
		{
		}

		//the GridPattern 's method
		public void GetItem (int row, int column)
		{
			GridPattern gp = (GridPattern) element.GetCurrentPattern (GridPattern.Pattern);
			gp.GetItem (row, column);
		}

		//the MultipleViewPattern 's method
		public void GetViewName (int viewId)
		{
			MultipleViewPattern mvp = (MultipleViewPattern) element.GetCurrentPattern (MultipleViewPattern.Pattern);
			mvp.GetViewName (viewId);
		}


		public void SetCurrentView (int viewId)
		{
			MultipleViewPattern mvp = (MultipleViewPattern) element.GetCurrentPattern (MultipleViewPattern.Pattern);
			mvp.SetCurrentView(viewId);
		}

		//the GridPattern's property
		public AutomationProperty RowCount
		{
			get { return (AutomationProperty) element.GetCurrentPropertyValue (GridPattern.RowCountProperty); }
		}

		public AutomationProperty ColumnCount
		{
			get { return (AutomationProperty) element.GetCurrentPropertyValue (GridPattern.ColumnCountProperty); }
		}

		//the GridItemPattern's property
		public int Column
		{
			get { return (int) element.GetCurrentPropertyValue (GridItemPattern.ColumnProperty); }
		}

		public int ColumnSpan
		{
			get { return (int) element.GetCurrentPropertyValue (GridItemPattern.ColumnSpanProperty); }
		}

		public AutomationElement ContainingGrid
		{
			get { return (AutomationElement) element.GetCurrentPropertyValue (GridItemPattern.ContainingGridProperty); }
		}

		public int Row
		{
			get { return (int) element.GetCurrentPropertyValue (GridItemPattern.RowProperty); }
		}

		public int RowSpan
		{
			get { return (int) element.GetCurrentPropertyValue (GridItemPattern.RowSpanProperty); }
		}

		//the MultipleViewPattern 's property
		public int CurrentViewProperty
		{
			get { return (int) element.GetCurrentPropertyValue (MultipleViewPattern.CurrentViewProperty); }
		}

	}

	public class Document : Element
	{
		public Document (AutomationElement elm)
			: base (elm)
		{
		}

		public void Scroll (ScrollAmount horizontalPercent,ScrollAmount verticalPercent)
		{
			ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
			sp.Scroll(horizontalPercent, verticalPercent);
		}

		public void ScrollVertical (ScrollAmount amount)
		{
			ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
			sp.ScrollVertical (amount);
		}

		public void ScrollHorizontal (ScrollAmount amount)
		{
			ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
			sp.ScrollHorizontal (amount);
		}

		public void SetScrollPercent (double horizontalPercent, double verticalPercent)
		{
			ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
			sp.SetScrollPercent (horizontalPercent, verticalPercent);
		}
	}

	public class ScrollBar : Element
	{
		public ScrollBar (AutomationElement elm)
			: base (elm)
		{
		}

		//the RangeValuePattern's method
		public void SetValue (double value)
		{
			RangeValuePattern rp = (RangeValuePattern) element.GetCurrentPattern (RangeValuePattern.Pattern);
			rp.SetValue (value);
		}

		//the RangeValuePattern's property
		public bool IsReadOnly
		{
			get { return (bool) element.GetCurrentPropertyValue (RangeValuePattern.IsReadOnlyProperty); }
		}

		public double LargeChange
		{
			get { return (double) element.GetCurrentPropertyValue (RangeValuePattern.LargeChangeProperty); }
		}

		public double SmallChange
		{
			get { return (double) element.GetCurrentPropertyValue (RangeValuePattern.SmallChangeProperty); }
		}

		public double Maximum
		{
			get { return (double) element.GetCurrentPropertyValue (RangeValuePattern.MaximumProperty); }
		}

		public double Minimum
		{
			get { return (double) element.GetCurrentPropertyValue (RangeValuePattern.MinimumProperty); }
		}

		public double Value
		{
			get { return (double) element.GetCurrentPropertyValue (RangeValuePattern.ValueProperty); }
		}
	}

	public class Text : Element
	{
		public Text (AutomationElement elm)
			: base (elm)
		{
		}

		//the RangeValuePattern's property
		public int Column
		{
			get { return (int) element.GetCurrentPropertyValue (TableItemPattern.ColumnProperty); }
		}

		public int ColumnSpan
		{
			get { return (int) element.GetCurrentPropertyValue (TableItemPattern.ColumnSpanProperty); }
		}

		public AutomationElement ContainingGrid
		{
			get { return (AutomationElement) element.GetCurrentPropertyValue (TableItemPattern.ContainingGridProperty); }
		}

		public int Row
		{
			get { return (int) element.GetCurrentPropertyValue (TableItemPattern.RowProperty); }
		}

		public int RowSpan
		{
			get { return (int) element.GetCurrentPropertyValue (TableItemPattern.RowSpanProperty); }
		}

		public int Value
		{
			get { return (int) element.GetCurrentPropertyValue (RangeValuePattern.ValueProperty); }
		}

		//the TableItem's property
		public AutomationProperty ColumnHeaderItems
		{
			get { return (AutomationProperty) element.GetCurrentPropertyValue (TableItemPattern.ColumnHeaderItemsProperty); }
		}

		public int TableItemColumn
		{
			get { return (int) element.GetCurrentPropertyValue (TableItemPattern.ColumnProperty); }
		}

		public int TableItemColumnSpan
		{
			get { return (int) element.GetCurrentPropertyValue (TableItemPattern.ColumnSpanProperty); }
		}

		public AutomationElement TableItemContainingGrid
		{
			get { return (AutomationElement) element.GetCurrentPropertyValue (TableItemPattern.ContainingGridProperty); }
		}

		public AutomationProperty TableItemRowHeaderItems
		{
			get { return (AutomationProperty) element.GetCurrentPropertyValue (TableItemPattern.RowHeaderItemsProperty); }
		}

		public int TableItemRow
		{
			get { return (int) element.GetCurrentPropertyValue (TableItemPattern.RowProperty); }
		}

		public int TableItemRowSpan
		{
			get { return (int) element.GetCurrentPropertyValue (TableItemPattern.RowSpanProperty); }
		}
	}

	public class Pane : Element
	{
		ProcedureLogger procedureLogger = new ProcedureLogger ();

		public Pane (AutomationElement elm)
			: base (elm)
		{
		}

		public DockPosition DockPosition
		{
			get
			{
				DockPattern dp = (DockPattern) element.GetCurrentPattern (DockPattern.Pattern);
				return dp.Current.DockPosition;
			}
			set
			{
				procedureLogger.Action (string.Format ("Set \"{0}\" control to be docked to \"{1}\".", this.Name, value));
				DockPattern dp = (DockPattern) element.GetCurrentPattern (DockPattern.Pattern);
				dp.SetDockPosition (value);
			}
		}

		public void Rotate (double degree)
		{
			procedureLogger.Action (string.Format ("Rotate \"{0}\" for {1} degree.", this.Name, degree));
			TransformPattern tp = (TransformPattern) element.GetCurrentPattern (TransformPattern.Pattern);
			tp.Rotate (degree);
		}
	}
}
