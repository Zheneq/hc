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
			Log.Info("Attempting to build URL to send crash report at path {0}", crashReportFilePath);
			string fileName = Path.GetFileName(crashReportFilePath);
			string str = "http://debug.triongames.com/";
			str = new StringBuilder().Append(str).Append("v2/archive/").Append(fileName).ToString();
			Log.Info("Attempting to start WebClient to send crash report to URL {0}", str);
			WebClient webClient = new WebClient();
			try
			{
				webClient.Headers.Add("User-Agent", "TrionHTTP/1.0");
				webClient.Headers.Add("Content-Type", "application/zip");
				Log.Info("Attempting to read file bytes for crash report from {0}", crashReportFilePath);
				byte[] array = File.ReadAllBytes(crashReportFilePath);
				object[] array2 = new object[2];
				object obj;
				if (array == null)
				{
					obj = "NULL";
				}
				else
				{
					obj = array.Length.ToString();
				}
				array2[0] = obj;
				array2[1] = str;
				Log.Info("Attempting to upload {0} bytes to crash report URL {1}", array2);
				byte[] bytes = webClient.UploadData(str, "POST", array);
				Log.Info("\nResponse from Crash Service received was {0}", Encoding.ASCII.GetString(bytes));
				result = true;
				return result;
			}
			finally
			{
				if (webClient != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							((IDisposable)webClient).Dispose();
							goto end_IL_0117;
						}
					}
				}
				end_IL_0117:;
			}
		}
		catch (WebException ex)
		{
			string arg = string.Empty;
			if (ex != null && ex.Response != null)
			{
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
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										((IDisposable)streamReader).Dispose();
										goto end_IL_017c;
									}
								}
							}
							end_IL_017c:;
						}
					}
				}
				finally
				{
					if (responseStream != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								((IDisposable)responseStream).Dispose();
								goto end_IL_0194;
							}
						}
					}
					end_IL_0194:;
				}
			}
			Log.Error(new StringBuilder().Append(ex.ToString()).Append(", status ").Append(ex.Status.ToString()).Append(", response: ").Append(arg).ToString());
			return result;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
			return result;
		}
	}
}
