using System;
using System.Text;
using System.Collections.Generic;
using Vanderbilt.Biostatistics.Wfccm2;

namespace Calculator.Expresions
{
	using Events;

	public class CalculatorStateManager
	{
		private List<string> lines;

		private EventAggregator _events;

		private StringBuilder _expresion;
		private StringBuilder _numVal;
		private bool _numValToggle;
		private bool _isDec = false;

		public CalculatorStateManager (EventAggregator events)
		{
			this._expresion = new StringBuilder ();
			this._numVal = new StringBuilder ();

			this.lines = new List<string> ();

			this._events = events;
			this._events.GetEvent<NumberUpdateEvent>().Subscribe (this.UpdateNumState);
			this._events.GetEvent<NumberUpdateEvent> ().Subscribe (value => {				
				this._events.GetEvent<OutputTextUpdateEvent>().Publish(this.ToString());
			});
			this._events.GetEvent<ExpresionUpdateEvent>().Subscribe (this.ExpresionAction);
			this._events.GetEvent<ExpresionUpdateEvent> ().Subscribe (value => {				
				this._events.GetEvent<OutputTextUpdateEvent>().Publish(this.ToString());
			});

			this._events.GetEvent<ClearEvent> ().Subscribe (clicks =>
			{
				if (clicks == 2)
					this.lines.Clear();
				
				this._expresion.Clear();
				this._numVal.Clear();
				this._numValToggle = false;
				this._isDec = false;

			});

			this._events.GetEvent<ClearEvent> ().Subscribe (value => {				
				this._events.GetEvent<OutputTextUpdateEvent>().Publish(this.ToString());
			});
		}

		private void UpdateNumState(string numVal)
		{
			if (numVal.Equals ("+/-"))
			{
				this._numValToggle = !this._numValToggle;
				return;
			}

			if (numVal.Equals ("."))
			{
				if (!this._isDec)
					this._numVal.Append (numVal);				
				this._isDec = true;
				return;
			}

			this._numVal.Append (numVal);
		}

		private void ExpresionAction(string actionVal)
		{
			actionVal = actionVal.Equals ("X") ? "*" : actionVal;

			this._expresion.Append (this._numValToggle ? " -" : "  ");
			this._expresion.Append (this._numVal);
			this._numVal.Clear ();
			this._numValToggle = false;

			if (actionVal.Equals ("="))
			{
				this.Evaluate ();
				return;
			} 
			else
			{
				this._expresion.Append ("  " + actionVal + " ");
			}
		}

		private void Evaluate()
		{
			string submit = this._expresion.ToString ();
			this.lines.Add (submit);
			Expression func = new Expression ("");
			func.Function = submit;
			this.lines.Add (">> " + func.EvaluateNumeric ().ToString ());
			this._expresion.Clear ();
		}

		public override string ToString ()
		{
			StringBuilder workingExpresion = new StringBuilder (this._expresion.ToString());
			workingExpresion.Append (this._numValToggle ? " -" : "  ");
			workingExpresion.Append (this._numVal);
			return string.Join (System.Environment.NewLine, this.lines) + System.Environment.NewLine + workingExpresion.ToString();
		}
	}
}

