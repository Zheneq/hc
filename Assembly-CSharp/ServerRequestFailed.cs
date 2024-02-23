using System;
using System.Text;

public class ServerRequestFailed : Exception
{
	public ServerRequestFailed(string errorMessage, Exception innerException = null)
		: base(errorMessage, innerException)
	{
	}

	public ServerRequestFailed(string context, string remoteMessage, string remoteStackTrace)
		: base(new StringBuilder().Append(context).Append(": ").Append(remoteMessage).Append("\n").Append(remoteStackTrace).ToString())
	{
	}
}
