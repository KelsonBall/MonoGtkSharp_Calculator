using System;

namespace Calculator.Events
{
	public abstract class AppEvent{}

	public abstract class AppEvent<TPayload> : AppEvent
	{		
		public Action<TPayload> Listeners { get; set; }

        /// <summary>
        /// Appends a delegate to the subscribed delegates of this event.
        /// </summary>
        /// <param name="action">A delegate taking a parameter of type TPayload</param>
		public void Subscribe(Action<TPayload> action)
		{
			this.Listeners += action;
		}

        /// <summary>
        /// Fires all subscribed delegates with the specified paramter. 
        /// </summary>
        /// <param name="payload">Parameter to pass to all subscribed delegates.</param>
		public void Publish(TPayload payload)
		{
			this.Listeners (payload);
		}
	}
}

