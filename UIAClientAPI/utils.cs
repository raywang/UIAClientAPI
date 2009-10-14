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

namespace UIAClientAPI
{
	class Utils
	{
		//'Takes a screenshot of the desktop
		public void takeScreenshot (string path)
		{
			Config config = new Config ();
			// pause before taking screenshots, otherwise we get half-drawn widgets
			Thread.Sleep (config.TakeScreenShots);

			/*
			 * assert os.path.isdir(os.path.dirname(path))

    fileExt = os.path.splitext(path)[1][1:]

    rootWindow = gtk.gdk.get_default_root_window()
    geometry = rootWindow.get_geometry()
    pixbuf = gtk.gdk.Pixbuf(gtk.gdk.COLORSPACE_RGB, False, 8, geometry[2], geometry[3])
    gtk.gdk.Pixbuf.get_from_drawable(pixbuf, rootWindow, rootWindow.get_colormap(), 0, 0, 0, 0, geometry[2], geometry[3])

    # gtk.gdk.Pixbuf.save() needs 'jpeg' and not 'jpg'
    if fileExt == 'jpg': fileExt = 'jpeg'

    try:
        pixbuf.save(path, fileExt)
    except gobject.GError:
        raise ValueError, "Failed to save screenshot in %s format" % fileExt

    assert os.path.exists(path)
			 */
		}
	}
}
