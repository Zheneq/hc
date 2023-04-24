using System;

public class LocalizedFailureException : Exception
{
	public LocalizationPayload LocalizationPayload { get; private set; }
	public override string Message => LocalizationPayload.ToString();

	public LocalizedFailureException(string term, string context)
	{
		LocalizationPayload = new LocalizationPayload
		{
			Term = term,
			Context = context
		};
	}

	public LocalizedFailureException(string term, string context, params LocalizationArg[] args)
	{
		LocalizationPayload = LocalizationPayload.Create(term, context, args);
	}

	public LocalizedFailureException(LocalizationPayload payload)
	{
		LocalizationPayload = payload;
	}
}
