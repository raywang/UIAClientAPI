// ListItem.cs: ListItem control class wrapper.
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

namespace UIAClientTestFramework
{
	public class ListItem : Element
	{
		public static readonly ControlType UIAType = ControlType.ListItem;

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
				procedureLogger.Action (string.Format ("Scroll {0} Into View.", this.Name));

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
}
