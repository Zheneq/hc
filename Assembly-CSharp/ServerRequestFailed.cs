using System;

public class ServerRequestFailed : Exception
{
	public ServerRequestFailed(string errorMessage, Exception innerException = null) : base(errorMessage, innerException)
	{
	}

	public ServerRequestFailed(string context, string remoteMessage, string remoteStackTrace) : base(string.Format("{0}: {1}\n{2}", context, remoteMessage, remoteStackTrace))
	{
	}
}
