﻿// Document.cs: Document control class wrapper.
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
	public class Document : Element
	{
		public static readonly ControlType UIAType = ControlType.Document;

		public Document (AutomationElement elm)
			: base (elm)
		{
		}

		// The methods and properties of ScrollPattern
		public void Scroll (ScrollAmount horizontalAmount, ScrollAmount verticalAmount)
		{
			ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
			sp.Scroll (horizontalAmount, verticalAmount);
		}

		public void ScrollHorizontal (ScrollAmount amount)
		{
			ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
			sp.ScrollHorizontal (amount);
		}

		public void ScrollVertical (ScrollAmount amount)
		{
			ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
			sp.ScrollVertical (amount);
		}

		public void SetScrollPercent (double horizontalPercent, double verticalPercent)
		{
			ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
			sp.SetScrollPercent (horizontalPercent, verticalPercent);
		}

		public bool HorizontallyScrollable {
			get { return (bool) element.GetCurrentPropertyValue (ScrollPattern.HorizontallyScrollableProperty); }
		}

		public double HorizontalScrollPercent {
			get { return (double) element.GetCurrentPropertyValue (ScrollPattern.HorizontalScrollPercentProperty); }
		}

		public double HorizontalViewSize {
			get { return (double) element.GetCurrentPropertyValue (ScrollPattern.HorizontalViewSizeProperty); }
		}

		public bool VerticallyScrollable {
			get { return (bool) element.GetCurrentPropertyValue (ScrollPattern.VerticallyScrollableProperty); }
		}

		public double VerticalScrollPercent {
			get { return (double) element.GetCurrentPropertyValue (ScrollPattern.VerticalScrollPercentProperty); }
		}

		public double VerticalViewSize {
			get { return (double) element.GetCurrentPropertyValue (ScrollPattern.VerticalViewSizeProperty); }
		}
	}
}