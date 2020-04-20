using System;
using System.IO;

public static class FileSystemUtils
{
	public unsafe static bool TryRead(string path, out string errorMessage)
	{
		errorMessage = null;
		bool result;
		try
		{
			FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read);
			try
			{
				result = true;
			}
			finally
			{
				if (fileStream != null)
				{
					((IDisposable)fileStream).Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			errorMessage = ex.Message;
			result = false;
		}
		return result;
	}
}
