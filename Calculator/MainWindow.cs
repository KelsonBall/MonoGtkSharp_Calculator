using System;
using Gtk;
using Calculator.Events;

public partial class MainWindow: Gtk.Window
{	
	private EventAggregator _events;

	public MainWindow (EventAggregator events) : base (Gtk.WindowType.Toplevel)
	{
		this._events = events;
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void numClicked (object sender, EventArgs e)
	{
		Button bSender = sender as Button;
		if (bSender == null)
			return;
		this._events.GetEvent<NumberUpdateEvent> ().Publish (bSender.Label);
	}		
}
