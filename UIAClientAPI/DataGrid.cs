// DataGrid.cs: DataGrid control class wrapper.
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
			mvp.SetCurrentView (viewId);
		}

		//the GridPattern's property
		public int RowCount
		{
			get { return (int) element.GetCurrentPropertyValue (GridPattern.RowCountProperty); }
		}

		public int ColumnCount
		{
			get { return (int) element.GetCurrentPropertyValue (GridPattern.ColumnCountProperty); }
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
}
