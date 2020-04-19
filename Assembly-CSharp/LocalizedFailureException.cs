using System;

public class LocalizedFailureException : Exception
{
	public LocalizedFailureException(string term, string context)
	{
		this.LocalizationPayload = new LocalizationPayload
		{
			Term = term,
			Context = context
		};
	}

	public LocalizedFailureException(string term, string context, params LocalizationArg[] args)
	{
		this.LocalizationPayload = LocalizationPayload.Create(term, context, args);
	}

	public LocalizedFailureException(LocalizationPayload payload)
	{
		this.LocalizationPayload = payload;
	}

	public LocalizationPayload LocalizationPayload { get; private set; }

	public override string Message
	{
		get
		{
			return this.LocalizationPayload.ToString();
		}
	}
}
