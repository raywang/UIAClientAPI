// Driver.cs: main entry of the tests of KeePass
//
// Author:
//   Ray Wang  (rawang@novell.com)
//
// Copyright (c) 2009 Novell, Inc (http://www.novell.com)
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace UIAClientAPI
{
	class Driver
	{
		public static void Main ()
		{
			KeePassTests test = new KeePassTests ();
			test.Init ();
			test.TestCase101 ();
			test.TestCase102 ();
		}
	}
}
