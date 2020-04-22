using System;

public class ServerRequestFailed : Exception
{
	public ServerRequestFailed(string errorMessage, Exception innerException = null)
		: base(errorMessage, innerException)
	{
	}

	public ServerRequestFailed(string context, string remoteMessage, string remoteStackTrace)
		: base($"{context}: {remoteMessage}\n{remoteStackTrace}")
	{
	}
}
