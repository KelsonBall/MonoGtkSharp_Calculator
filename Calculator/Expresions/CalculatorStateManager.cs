using System;

namespace Calculator.Expresions
{
	using Events;

	public class CalculatorStateManager
	{
		private EventAggregator _events;

		public CalculatorStateManager (EventAggregator events)
		{
			this._events = events;
			this._events.GetEvent<NumberUpdateEvent>().Subscribe (this.UpdateNumState);
		}

		public void UpdateNumState(string numVal)
		{
			Console.WriteLine (numVal);
		}

	}
}

