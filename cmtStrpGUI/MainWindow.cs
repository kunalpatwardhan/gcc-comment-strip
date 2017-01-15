using System;
using Gtk;
using cmtStrp;
public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnButton1Clicked (object sender, EventArgs e)
	{
		
		System.Windows.Forms.FolderBrowserDialog fd = new System.Windows.Forms.FolderBrowserDialog ();
		fd.ShowDialog ();

		//System.Windows.Forms.MessageBox.Show (fd.SelectedPath);
		label2.Text="Processing ...";
		while (Gtk.Application.EventsPending ())
			Gtk.Application.RunIteration ();
		MainClass.DirSearch (fd.SelectedPath);
		label2.Text = "Done";

		fd.Dispose ();

	}
}
