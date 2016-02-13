using System;
using Gtk;

namespace Calculator
{
	using Events;
	using Expresions;

	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			EventAggregator appEvents = new EventAggregator ();
			CalculatorStateManager stateManager = new CalculatorStateManager (appEvents);
			MainWindow win = new MainWindow (appEvents);
			win.Show ();
			Application.Run ();
		}
	}
}
