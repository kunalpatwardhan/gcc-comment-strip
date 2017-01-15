using System;
using Gtk;
using cmtStrp;
using System.Threading;

public partial class MainWindow: Gtk.Window
{
	Timer t1 = new Timer (timerCallback,null,1000,1000);

	static Label sl2, sl6, sl7, sl8;
	static Button bt1;
	static void timerCallback(object state)
	{
		if (MainClass.pStatus == MainClass.ProcessStatus.Complete) {
			sl2.Text = "Done";
			bt1.Sensitive = true;
		} else if (MainClass.pStatus == MainClass.ProcessStatus.Progress) {
			bt1.Sensitive = false;
			sl2.Text = "Processing ...";
		} else {
			sl2.Text = "Idle";
			bt1.Sensitive = true;
		}
		sl6.Text = MainClass.totalFileCount.ToString ();
		sl7.Text = MainClass.skippedCount.ToString ();
		sl8.Text = MainClass.errCount.ToString ();
	}

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		sl2 = label2;
		sl6 = label6;
		sl7 = label7;
		sl8 = label8;
		bt1 = button1;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	public static void ExecuteFunctionAsynce(object path)
	{
		try
		{
			MainClass.DirSearch ((string)path);
			MainClass.pStatus = MainClass.ProcessStatus.Complete;
		}
		catch (Exception objException)
		{
			// Log the exception
		}

		//	return result;
	}

	/// <span class="code-SummaryComment"><summary></span>
	/// Execute the command Asynchronously.
	/// <span class="code-SummaryComment"></summary></span>
	/// <span class="code-SummaryComment"><param name="command">string command.</param></span>
	public static void ExecuteFunctionAsync(string path)
	{
		try
		{
			//Asynchronously start the Thread to process the Execute command request.
			Thread objThread = new Thread(new ParameterizedThreadStart(ExecuteFunctionAsynce));
			//Make the thread as background thread.
			objThread.IsBackground = true;
			//Set the Priority of the thread.
			//			objThread.Priority = ThreadPriority.AboveNormal;
			//Start the thread.
			objThread.Start(path);
		}
		catch (ThreadStartException objException)
		{
			// Log the exception
		}
		catch (ThreadAbortException objException)
		{
			// Log the exception
		}
		catch (Exception objException)
		{
			// Log the exception
		}
	}

	protected void OnButton1Clicked (object sender, EventArgs e)
	{
		button1.Sensitive = false;
		System.Windows.Forms.FolderBrowserDialog fd = new System.Windows.Forms.FolderBrowserDialog ();
		fd.ShowDialog ();

		//System.Windows.Forms.MessageBox.Show (fd.SelectedPath);
		label2.Text="Processing ...";
		while (Gtk.Application.EventsPending ())
			Gtk.Application.RunIteration ();

		MainClass.totalFileCount = 0;
		MainClass.skippedCount = 0;
		MainClass.errCount = 0;
		MainClass.pStatus = MainClass.ProcessStatus.Idle;


		ExecuteFunctionAsync(fd.SelectedPath);

		fd.Dispose ();


		//fd.Container.Dispose ();
	}
}
