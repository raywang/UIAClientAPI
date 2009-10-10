using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace UIAClientAPI
{
	// Logger class which provides Actions logger and ExpectedResults logger.
	public class ProcedureLogger
	{
		static string actionBuffer = string.Empty;
		static string expectedResultButter = string.Empty;

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
		}

		public void flushBuffer ()
		{
		}
	}
}
