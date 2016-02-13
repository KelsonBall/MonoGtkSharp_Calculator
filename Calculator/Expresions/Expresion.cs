using System;
using System.Collections.Generic;

namespace Calculator.Expresions
{	
	public class Expresion
	{
		public enum Operators 
		{
			Value,
			Variable,
			Add,
			Subtract,
			Multiply,
			Divide,
			Exponent,
			Mod,
			Log,
			Sin,
			Cos,
			Tan,
			ASin,
			ACos,
			ATan
		};

		public double? Value { get; set; }
		public string Variable { get; set; }
		public Expresion Left { get; set; }
		public Expresion Right { get; set; }
		public Operators Operator { get; set; }


		public Expresion ()
		{
		}

		public Expresion(Expresion left, Operators op, Expresion right)
		{
			this.Left = left;
			this.Operator = op;
			this.Right = right;
		}

		public Expresion(double value)
		{
			this.Value = value;
		}

		/// <summary>
		/// Evaluates an expression and returns a double or null in the case of an invalid operation.
		/// Pushes nulls up the stack.
		/// </summary>
		public double? Evaluate(Dictionary<string, double> variables = null)
		{	
			if (this.Operator == Operators.Value)
				return this.Value;
			if (this.Operator == Operators.Variable)
			{
				if (variables == null)
					return null;
				if (!variables.ContainsKey(this.Variable))
					return null;
				return variables [this.Variable];
			}
			
			double? l = this.Left.Evaluate (variables);
			double? r = this.Right.Evaluate (variables);
			if (l == null || r == null)
				return null;
			
			switch (this.Operator)
			{

			case Operators.Add:
				return l + r;
			case Operators.Subtract:
				return l - r;
			case Operators.Multiply:
				return l * r;
			case Operators.Divide:
				if (r == 0)
					return null;
				return l / r;
			case Operators.Mod:
				int mod = (int)r;
				if (mod == 0)
					return null;
				return l % mod;
			case Operators.Exponent:
				return Math.Pow ((double)l, (double)r);
			default:
				return null;
			}
		}
	}
}

