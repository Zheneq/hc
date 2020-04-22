using System;
using System.IO;

public static class FileSystemUtils
{
	public static bool TryRead(string path, out string errorMessage)
	{
		errorMessage = null;
		try
		{
			FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read);
			try
			{
				return true;
			}
			finally
			{
				if (fileStream != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							((IDisposable)fileStream).Dispose();
							goto end_IL_0010;
						}
					}
				}
				end_IL_0010:;
			}
		}
		catch (Exception ex)
		{
			errorMessage = ex.Message;
			return false;
		}
	}
}
