// Utils.cs: Utility functions
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
using System.Threading;
using System.Timers;
using System.Drawing;
using System.Collections;
using System.Data;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Windows.Forms;

namespace UIAClientAPI
{
	class Utils
	{
		//Takes a screenshot of the desktop
		public static void TakeScreenshot (string path)
		{
			Config config = new Config ();

			// pause before taking screenshots, otherwise we get half-drawn widgets
			//Thread.Sleep ((Int) config.shortDelay * 1000);

			// do the screenshot, and save it to disk
			int width = Screen.PrimaryScreen.Bounds.Width;
			int height = Screen.PrimaryScreen.Bounds.Height;
			Bitmap bmp = new Bitmap (width, height);
			using (Graphics g = Graphics.FromImage (bmp)) {
				g.CopyFromScreen (0, 0, 0, 0, new Size (width, height));
			}
			bmp.Save (path, ImageFormat.Png);
		}

		// runs until it gets True.
		public static bool RetryUntilTrue (Func<bool> d)
		{
			for (int i = 0; i < Config.RETRY_TIMES; i++) {
				if (d ())
					return true;

				Thread.Sleep (Config.RETRY_INTERVAL);
			}

			return false;
		}
	}
}
