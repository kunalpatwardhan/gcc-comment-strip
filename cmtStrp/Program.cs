﻿using System;
using System.IO;
using System.Threading;
using System.Linq;
namespace cmtStrp
{
	public class MainClass
	{
		public static long totalFileCount = 0;
		public static long errCount = 0;
		public static long skippedCount = 0;
		public enum ProcessStatus
		{
			Idle,
			Progress,
			Complete
		};
		public static ProcessStatus pStatus = ProcessStatus.Idle;

		static Mutex mtx = new Mutex ();

		/// &lt;span class="code-SummaryComment">&lt;summary>&lt;/span>
		/// Executes a shell command synchronously.
		/// &lt;span class="code-SummaryComment">&lt;/summary>&lt;/span>
		/// &lt;span class="code-SummaryComment">&lt;param name="command">string command&lt;/param>&lt;/span>
		/// &lt;span class="code-SummaryComment">&lt;returns>string, as output of the command.&lt;/returns>&lt;/span>
		public static void ExecuteCommandSync(object command)
		{
			string result="";

			try
			{

				string commandStr = "\"" + command + "\""; 

				// create the ProcessStartInfo using "cmd" as the program to be run,
				// and "/c " as the parameters.
				// Incidentally, /c tells cmd that we want it to execute the command that follows,
				// and then exit.
				System.Diagnostics.ProcessStartInfo procStartInfo =
					new System.Diagnostics.ProcessStartInfo("gcc", " -fpreprocessed -dD -E -P " + commandStr);

				// The following commands are needed to redirect the standard output.
				// This means that it will be redirected to the Process.StandardOutput StreamReader.
				procStartInfo.RedirectStandardOutput = true;
				procStartInfo.RedirectStandardError = true;
				procStartInfo.UseShellExecute = false;
				// Do not create the black window.
				procStartInfo.CreateNoWindow = true;
				// Now we create a process, assign its ProcessStartInfo and start it
				System.Diagnostics.Process proc = new System.Diagnostics.Process();
				proc.StartInfo = procStartInfo;
				proc.Start();
				//proc.WaitForExit();
				// Get the output into a string
				//proc.WaitForExit();
				//proc.StandardError.ReadToEnd();
				result = proc.StandardOutput.ReadToEnd();
				if(result != "")
				{
					if(File.ReadAllText((string)command)==result)
					{
						mtx.WaitOne();
						skippedCount++;
						mtx.ReleaseMutex();
					}
					else
					{
						File.WriteAllText((string)command, result);
					}

				}
				else
				{
					
					mtx.WaitOne();
					errCount++;
					mtx.ReleaseMutex();
				}
				mtx.WaitOne();
				totalFileCount++;
				mtx.ReleaseMutex();
				// Display the command output.

				//Console.WriteLine(result);
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
		public static void ExecuteCommandAsync(string command)
		{
			try
			{
				//Asynchronously start the Thread to process the Execute command request.
				Thread objThread = new Thread(new ParameterizedThreadStart(ExecuteCommandSync));
				//Make the thread as background thread.
				objThread.IsBackground = true;
				//Set the Priority of the thread.
	//			objThread.Priority = ThreadPriority.AboveNormal;
				//Start the thread.
				objThread.Start(command);
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

		public static void DirSearch(string sDir)
		{
			pStatus = ProcessStatus.Progress;
			try
			{
				foreach (string d in Directory.GetDirectories(sDir))
				{
					var filteredFiles = Directory
						.GetFiles(d, "*.*")
						.Where(file => file.ToLower().EndsWith("c") || file.ToLower().EndsWith("cpp") 
							|| file.ToLower().EndsWith("h"))
						.ToList();
					foreach (string f in filteredFiles)
					{

						Console.WriteLine( d.Substring(d.Length-10,10));
						if(File.Exists(f))
						{
							ExecuteCommandAsync(f);
						}
					}
					DirSearch(d);
				}
			}
			catch (System.Exception excpt)
			{
				Console.WriteLine(excpt.Message);
			}


		}

		public static void Main (string[] args)
		{
			
			Console.WriteLine ("Let's Begin!");

			DirSearch ("Enetr Path Here!!");

		}
	}
}
