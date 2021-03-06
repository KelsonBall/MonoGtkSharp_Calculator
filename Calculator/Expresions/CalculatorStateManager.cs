﻿using System;
using System.Text;
using System.Collections.Generic;
using Vanderbilt.Biostatistics.Wfccm2;

namespace Calculator.Expresions
{
	using Events;

    /// <summary>
    /// Manages current number and current expresion
    /// Subscribes to UI events and Publishes results as text.
    /// </summary>
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
            this._events.GetEvent<ExpresionUpdateEvent>().Subscribe (this.ExpresionAction);			
			this._events.GetEvent<ClearEvent> ().Subscribe (this.ClearState);
			
		}

	    private void ClearState(int action)
	    {
            if (action == 2)
                this.lines.Clear();

            this._expresion.Clear();
            this._numVal.Clear();
            this._numValToggle = false;
            this._isDec = false;

            this.PostStateUpdate();
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

            this.PostStateUpdate();
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

            this.PostStateUpdate();
		}

		private void Evaluate()
		{
			string submit = this._expresion.ToString ();
			this.lines.Add (submit);
			Expression func = new Expression ("");
			try
			{				
				func.Function = submit;
				double result = func.EvaluateNumeric ();
				if (double.IsNaN(result))
					throw new ExpressionException("Division by 0! ;-;");
				
				this.lines.Add (">> " + result.ToString ());
			}
			catch (ExpressionException e)
			{
				this.lines.Add ("X> " + e.Message + @" ಠ╭╮ಠ");
			}
			catch (OverflowException e)
			{
				this.lines.Add ("NUMBERS TOO BIG ;-;");
			}
			this._expresion.Clear ();
		}

	    private void PostStateUpdate()
	    {
	        this._events.GetEvent<OutputTextUpdateEvent>().Publish(this.ToString());
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

