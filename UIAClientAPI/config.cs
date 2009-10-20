// Config.cs: save the configuration for UIAClinet test
//
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the 
// "Software"), to deal in the Software without restriction, including 
// without limitation the rights to use, copy, modify, merge, publish, 
// distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to 
// the following conditions: 
//  
// The above copyright notice and this permission notice shall be 
// included in all copies or substantial portions of the Software. 
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// 
// Copyright (c) 2009 Novell, Inc. (http://www.novell.com) 
// 
// Authors: 
//      Ray Wang <rawang@novell.com>
//	Felicia Mu <fxmu@novell.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIAClientAPI
{
	class Config
	{
		// You could donwload our KeePass sampe from http://downloads.sourceforge.net/keepass/KeePass-2.09.zip
		// Note that the vesionm we use is 2.09
		public const string appPath = "keePass-2.09\\KeePass.exe";
		public const int retryTimes = 20;
		public const int retryInterval = 500;

		// whether or not to take screenshots
		private bool take_screen_shots = true;
		public bool takeScreenShots
		{
			get { return this.take_screen_shots; }
			set { this.take_screen_shots = value; }
		}

		// where to write procedure logger output, screenshots, etc
		private string output_dir = "";
		public string outputDir
		{
			get { return this.output_dir; }
			set { this.output_dir = value; }
		}

		//  the shotr time to delay
		private double short_delay = 0.5;
		public double shortDelay
		{
			get { return this.short_delay; }
			set { this.short_delay = value; }
		}

		//  the middle time to delay
		private double medium_delay = 4;
		public double mediumDelay
		{
			get { return this.medium_delay; }
			set { this.medium_delay = value; }
		}

		//  the long time to delay
		private double long_delay = 10;
		public double longDelay
		{
			get { return this.long_delay; }
			set { this.long_delay = value; }
		}


	}
}
