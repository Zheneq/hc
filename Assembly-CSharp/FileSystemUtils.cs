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
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(FileSystemUtils.TryRead(string, string*)).MethodHandle;
					}
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
