using System;
using System.Collections.Generic;

namespace Calculator.Events
{
    /// <summary>
    /// Strongly typed event object manager. 
    /// </summary>
	public class EventAggregator
	{
		private Dictionary<string,AppEvent> events;

		public EventAggregator ()
		{
			this.events = new Dictionary<string, AppEvent> ();
		}

		public TEventType GetEvent<TEventType>() where TEventType : AppEvent
		{
			string eventName = typeof(TEventType).Name;
			if (!this.events.ContainsKey (eventName))
			{
				this.events.Add (eventName, Activator.CreateInstance<TEventType>());
			}
			return (TEventType)this.events [ eventName ];
		}
	}
}

