using System;

public class ClientRequestFailed : Exception
{
	public ClientRequestFailed(string clientErrorMessage = null) : base(clientErrorMessage)
	{
		this.ClientErrorMessage = clientErrorMessage;
	}

	public ClientRequestFailed(string clientErrorMessage, string serverErrorMessage) : base(serverErrorMessage)
	{
		this.ClientErrorMessage = clientErrorMessage;
	}

	public ClientRequestFailed(string clientErrorMessage, Exception innerException) : base(innerException.Message, innerException)
	{
		this.ClientErrorMessage = clientErrorMessage;
	}

	public string ClientErrorMessage { get; set; }
}
