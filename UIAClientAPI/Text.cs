// Text.cs: Text control class wrapper.
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

namespace UIAClientAPI
{
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
		public AutomationProperty TableItemColumnHeaderItems
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

		public AutomationProperty TableItemContainingGrid
		{
			get { return (AutomationProperty) element.GetCurrentPropertyValue (TableItemPattern.ContainingGridProperty); }
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
}
