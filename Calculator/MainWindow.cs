using System;
using System.Linq;
using Gtk;
using Calculator.Events;

public partial class MainWindow: Gtk.Window
{	
	private EventAggregator _events;

	public MainWindow (EventAggregator events) : base (Gtk.WindowType.Toplevel)
	{
		this._events = events;
		this._events.GetEvent<OutputTextUpdateEvent> ().Subscribe (lines =>
		{			
			this.outputTextView.Buffer.Text = lines;
		});
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

	protected void opClicked(object sender, EventArgs e)
	{
		Button bSender = sender as Button;
		if (bSender == null)
			return;
		this._events.GetEvent<ExpresionUpdateEvent> ().Publish (bSender.Label);
	}
		
	protected void onClear (object sender, EventArgs e)
	{		
		this._events.GetEvent<ClearEvent> ().Publish (1);
	}
}
