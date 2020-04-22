using System;

public class ClientRequestFailed : Exception
{
	public string ClientErrorMessage
	{
		get;
		set;
	}

	public ClientRequestFailed(string clientErrorMessage = null)
		: base(clientErrorMessage)
	{
		ClientErrorMessage = clientErrorMessage;
	}

	public ClientRequestFailed(string clientErrorMessage, string serverErrorMessage)
		: base(serverErrorMessage)
	{
		ClientErrorMessage = clientErrorMessage;
	}

	public ClientRequestFailed(string clientErrorMessage, Exception innerException)
		: base(innerException.Message, innerException)
	{
		ClientErrorMessage = clientErrorMessage;
	}
}
