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

		MainClass.totalFileCount = 0;
		MainClass.skippedCount = 0;
		MainClass.errCount = 0;

		MainClass.DirSearch (fd.SelectedPath);
		label2.Text = "Done";

		label6.Text = MainClass.totalFileCount.ToString ();
		label7.Text = MainClass.skippedCount.ToString ();
		label8.Text = MainClass.errCount.ToString ();
		fd.Dispose ();

	}
}
