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
		//declare an API
		[System.Runtime.InteropServices.DllImportAttribute ("gdi32.dll")]
		private static extern bool BitBlt (
		    IntPtr hdcDest, // the handle of target DC
		    int nXDest,
		    int nYDest,
		    int nWidth,
		    int nHeight,
		    IntPtr hdcSrc, // the handle of source DC
		    int nXSrc,
		    int nYSrc,
		    System.Int32 dwRop // the processing data of raster
		    );

		//Takes a screenshot of the desktop
		public void takeScreenshot (string path)
		{
			Config config = new Config ();
			// pause before taking screenshots, otherwise we get half-drawn widgets
			Thread.Sleep (TimeSpan.Parse (config.Short_Delay));//config.Short_Delay.to);

			// do the screen shot, and save it to disk
			int width = Screen.PrimaryScreen.Bounds.Width;
			int height = Screen.PrimaryScreen.Bounds.Height;
			Bitmap bmp = new Bitmap (width, height);
			using (Graphics g = Graphics.FromImage (bmp)) {
				g.CopyFromScreen (0, 0, 0, 0, new Size (width, height));
			}
			bmp.Save ("c:\\Capture.bmp");
		}
	}
}
