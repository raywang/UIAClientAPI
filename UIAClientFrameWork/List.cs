﻿// List.cs: List control class wrapper.
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
	public class List : Element
	{
		public static readonly ControlType UIAType = ControlType.List;

		public List (AutomationElement elm)
			: base (elm)
		{
		}

		public void GetViewName (int viewId)
		{
			MultipleViewPattern mvp = (MultipleViewPattern) element.GetCurrentPattern (MultipleViewPattern.Pattern);
			mvp.GetViewName (viewId);
		}

		public void SetCurrentView (int viewId)
		{
			SetCurrentView (viewId, true);
		}

		public void SetCurrentView (int viewId, bool log)
		{
			if (log == true)
				procedureLogger.Action (string.Format ("Set current view to \"{0}\".", this.Name));

			MultipleViewPattern mvp = (MultipleViewPattern) element.GetCurrentPattern (MultipleViewPattern.Pattern);
			mvp.SetCurrentView (viewId);
		}

		public AutomationElement [] GetSelection ()
		{
			SelectionPattern sp = (SelectionPattern) element.GetCurrentPattern (SelectionPattern.Pattern);
			return (AutomationElement []) sp.Current.GetSelection ();
		}
	}
}