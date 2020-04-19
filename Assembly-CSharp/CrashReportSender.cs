using System;
using System.IO;
using System.Net;
using System.Text;

public static class CrashReportSender
{
	internal static bool Send(string crashReportFilePath)
	{
		bool result = false;
		try
		{
			Log.Info("Attempting to build URL to send crash report at path {0}", new object[]
			{
				crashReportFilePath
			});
			string fileName = Path.GetFileName(crashReportFilePath);
			string text = "http://debug.triongames.com/";
			text = text + "v2/archive/" + fileName;
			Log.Info("Attempting to start WebClient to send crash report to URL {0}", new object[]
			{
				text
			});
			WebClient webClient = new WebClient();
			try
			{
				webClient.Headers.Add("User-Agent", "TrionHTTP/1.0");
				webClient.Headers.Add("Content-Type", "application/zip");
				Log.Info("Attempting to read file bytes for crash report from {0}", new object[]
				{
					crashReportFilePath
				});
				byte[] array = File.ReadAllBytes(crashReportFilePath);
				string message = "Attempting to upload {0} bytes to crash report URL {1}";
				object[] array2 = new object[2];
				int num = 0;
				object obj;
				if (array == null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(CrashReportSender.Send(string)).MethodHandle;
					}
					obj = "NULL";
				}
				else
				{
					obj = array.Length.ToString();
				}
				array2[num] = obj;
				array2[1] = text;
				Log.Info(message, array2);
				byte[] bytes = webClient.UploadData(text, "POST", array);
				Log.Info("\nResponse from Crash Service received was {0}", new object[]
				{
					Encoding.ASCII.GetString(bytes)
				});
				result = true;
			}
			finally
			{
				if (webClient != null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					((IDisposable)webClient).Dispose();
				}
			}
		}
		catch (WebException ex)
		{
			string arg = string.Empty;
			if (ex != null && ex.Response != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				Stream responseStream = ex.Response.GetResponseStream();
				try
				{
					if (responseStream != null)
					{
						StreamReader streamReader = new StreamReader(responseStream);
						try
						{
							arg = streamReader.ReadToEnd();
						}
						finally
						{
							if (streamReader != null)
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								((IDisposable)streamReader).Dispose();
							}
						}
					}
				}
				finally
				{
					if (responseStream != null)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						((IDisposable)responseStream).Dispose();
					}
				}
			}
			Log.Error(string.Format("{0}, status {1}, response: {2}", ex.ToString(), ex.Status.ToString(), arg), new object[0]);
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		return result;
	}
}
