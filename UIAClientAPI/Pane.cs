// Pane.cs: Pane control class wrapper.
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
	public class Pane : Element
	{
		public static readonly ControlType UIAType = ControlType.Pane;
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
