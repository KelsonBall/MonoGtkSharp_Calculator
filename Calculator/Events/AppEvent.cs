using System;

namespace Calculator.Events
{
	public abstract class AppEvent{}

	public abstract class AppEvent<TPayload> : AppEvent
	{		
		public Action<TPayload> Listeners { get; set; }

		public void Subscribe(Action<TPayload> action)
		{
			this.Listeners += action;
		}

		public void Publish(TPayload payload)
		{
			this.Listeners (payload);
		}
	}
}

