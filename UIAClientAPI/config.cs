// Config.cs: save the configuration for UIAClinet test
//
// Author:
//   Felicia Mu  (fxmu@novell.com)
//
// Copyright (c) 2009 Novell, Inc (http://www.novell.com)
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIAClientAPI
{
	class Config
	{
		// whether or not to take screenshots
		private bool takeScreenShots = true;
		public bool TakeScreenShots
		{
			get { return this.takeScreenShots; }
			set { this.takeScreenShots = value; }
		}

		// where to write procedure logger output, screenshots, etc
		private string output_dir = " ";
		public string Output_Dir
		{
			get { return this.output_dir; }
			set { this.output_dir = value; }
		}

		//  the shotr time to delay
		private string short_delay = "0.5";
		public string Short_Delay
		{
			get { return this.short_delay; }
			set { this.short_delay = value; }
		}

		//  the middle time to delay
		private string medium_delay = "4";
		public string Medium_Delay
		{
			get { return this.medium_delay; }
			set { this.medium_delay = value; }
		}

		//  the long time to delay
		private string long_delay = "10";
		public string Long_Delay
		{
			get { return this.long_delay; }
			set { this.long_delay = value; }
		}


	}
}
