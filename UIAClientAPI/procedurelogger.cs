// procedurelogger.cs: ouput the info to screen and write it into a xml file
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
using System.Xml;
using System.IO;

namespace UIAClientAPI
{
	// Logger class which provides Actions logger and ExpectedResults logger.
	public class ProcedureLogger
	{
		static string actionBuffer = string.Empty;
		static string expectedResultButter = string.Empty;
		static XmlDocument xmldoc;
		static XmlNode xmlnode;
		static XmlText xmltext;
		static XmlElement xmlelem;
		static XmlElement xmlelem2;
		static XmlElement xmlelem3;
		static XmlElement xmlelem4;
		static XmlElement xmlelem5;
		static XmlElement xmlelem5_1;
		static XmlElement xmlelem5_2;
		static XmlElement xmlelem5_3;
		static XmlElement xmlelem5_4;

		/**
		 *  Log an action, e.g., Click Cancel
		 *  
		 * Multiple calls to action() (without a call to expectedResult() in between)
		 * will cause the message from each call to be concatenated to the message from
		 * previous calls.
		 */
		public void Action (string action)
		{
			// take a screenshot;
			//flushBuffer();

			actionBuffer += action + "  ";
			Console.WriteLine ("Action: %s", action);
		}

		/**
		 * Log an expected result, e.g., The dialog closes
		 * 
		 * Multiple calls to expectedResult() (without a call to action() in between)
		 * will cause the message from each call to be concatenated to the message from
		 * previous calls.
		 */
		public void ExpectedResult (string expectedResult)
		{
			expectedResultButter += expectedResult + "  ";
			Console.WriteLine ("Expected result: %s", expectedResult);
		}

		// Save logged actions and expected results to an XML file
		public void Save ()
		{
			xmldoc = new XmlDocument ();
			//add XML declaration
			xmlnode = xmldoc.CreateNode (XmlNodeType.XmlDeclaration, "", "");
			xmldoc.AppendChild (xmlnode);
			//add a root element
			xmlelem = xmldoc.CreateElement ("", "test", "");
			//xmltext = xmldoc.CreateTextNode ("Root Text");
			//xmlelem.AppendChild (xmltext);
			xmldoc.AppendChild (xmlelem);
			//add <name> element
			xmlelem2 = xmldoc.CreateElement ("name");
			xmltext = xmldoc.CreateTextNode ("button-regression");
			xmlelem2.AppendChild (xmltext);
			xmldoc.ChildNodes.Item (1).AppendChild (xmlelem2);
			//add <description> element
			xmlelem3 = xmldoc.CreateElement ("description");
			xmltext = xmldoc.CreateTextNode ("Test accessibility of button widget");
			xmlelem3.AppendChild (xmltext);
			xmldoc.ChildNodes.Item (1).AppendChild (xmlelem3);
			//add <parameters> element
			xmlelem4 = xmldoc.CreateElement ("parameters");
			xmltext = xmldoc.CreateTextNode ("environments");
			xmlelem4.AppendChild (xmltext);
			xmldoc.ChildNodes.Item (1).AppendChild (xmlelem4);

			/*add <parameters> subelement, like the following sturcture
			*<procedures>
			*	<step>
			*	<action>.........................</action>
			*	<expectedResult>.........</expectedResult>
			*	<screenshot>.................</screenshot>
			*	</step>
			*</procedures>
			*/
			xmlelem5 = xmldoc.CreateElement ("procedures");
			xmldoc.ChildNodes.Item (1).AppendChild (xmlelem5);
	
			for (int i = 0; i < actionBuffer.Length; i++) {
				// <step>
				xmlelem5_1 = xmldoc.CreateElement ("step");
				xmltext = xmldoc.CreateTextNode (Convert.ToString (i));
				xmlelem5_1.AppendChild (xmltext);
				xmldoc.ChildNodes.Item (1).ChildNodes [3].AppendChild (xmlelem5_1);

				// <action>
				xmlelem5_2 = xmldoc.CreateElement ("action");
				xmltext = xmldoc.CreateTextNode (actionBuffer);
				xmlelem5_2.AppendChild (xmltext);
				xmldoc.ChildNodes.Item (1).ChildNodes [3].AppendChild (xmlelem5_2);

				// <expectedResult>
				xmlelem5_3 = xmldoc.CreateElement ("expectedResult");
				xmltext = xmldoc.CreateTextNode (expectedResultButter);
				xmlelem5_3.AppendChild (xmltext);
				xmldoc.ChildNodes.Item (1).ChildNodes [3].AppendChild (xmlelem5_3);

				/* add <screenshot>
				* TODO: add a judge, if config.TAKE_SCREENSHOTS == TRUE
				* then do the following step
				*/
				xmlelem5_4 = xmldoc.CreateElement ("screenshot");
				xmltext = xmldoc.CreateTextNode ("screenshot");
				xmlelem5_4.AppendChild (xmltext);
				xmldoc.ChildNodes.Item (1).ChildNodes [3].AppendChild (xmlelem5_4);
			}
			// add the content for <parameters>
			xmltext = xmldoc.CreateTextNode ("。。。。。。。");
			xmlelem5.AppendChild (xmltext);

			//save the created xml 
			try {
				xmldoc.Save ("c:\\data.xml");
			} catch (Exception e) {
				Console.WriteLine (e.Message);
			}

			//write the xml comment to the xml file
			string lines = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<?xml-stylesheet type=\"text/xsl\" href=\"procedures.xsl\"?>";

			StreamReader sr = new StreamReader (@"C:\procedures.xml");
			string s1 = sr.ReadToEnd ();
			string s2 = s1.Insert (0, lines);
			sr.Close ();
			StreamWriter sw = new StreamWriter (@"C:\procedures.xml");
			sw.Write (s2);
			sw.Flush ();
			sw.Close ();





			//Console.ReadLine ();
		}
		public void flushBuffer ()
		{
		}

	}
	
}

		
	

