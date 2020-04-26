using System;

[Serializable]
public class ServerMessageOverrides
{
	public ServerMessage MOTDText
	{
		get;
		set;
	}

	public ServerMessage MOTDPopUpText
	{
		get;
		set;
	}

	public ServerMessage ReleaseNotesText
	{
		get;
		set;
	}

	public ServerMessage ReleaseNotesHeader
	{
		get;
		set;
	}

	public ServerMessage ReleaseNotesDescription
	{
		get;
		set;
	}

	public ServerMessage WhatsNewText
	{
		get;
		set;
	}

	public ServerMessage WhatsNewHeader
	{
		get;
		set;
	}

	public ServerMessage WhatsNewDescription
	{
		get;
		set;
	}

	public ServerMessage LockScreenText
	{
		get;
		set;
	}

	public ServerMessage LockScreenButtonText
	{
		get;
		set;
	}

	public string FreeUpsellExternalBrowserUrl
	{
		get;
		set;
	}

	public string FreeUpsellExternalBrowserSteamUrl
	{
		get;
		set;
	}

	public ServerMessage FacebookOAuthRedirectUriContent
	{
		get;
		set;
	}

	public void SetValue(ServerMessageType type, ServerMessage value)
	{
		GetType().GetProperty(type.ToString()).SetValue(this, value, null);
	}

	public string GetValue(ServerMessageType type, string language)
	{
		ServerMessage serverMessage = (ServerMessage)GetType().GetProperty(type.ToString()).GetValue(this, null);
		if (serverMessage == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		return serverMessage.GetValue(language);
	}

	public string GetValueOrDefault(ServerMessageType type, string language)
	{
		string value = GetValue(type, language);
		if (value.IsNullOrEmpty())
		{
			value = GetValue(type, ServerMessageLanguage.EN);
		}
		return value;
	}

	public string GetValue(ServerMessageType type, ServerMessageLanguage language)
	{
		ServerMessage serverMessage = (ServerMessage)GetType().GetProperty(type.ToString()).GetValue(this, null);
		if (serverMessage == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		return serverMessage.GetValue(language);
	}

	public string GetValueOrDefault(ServerMessageType type, ServerMessageLanguage language)
	{
		string value = GetValue(type, language);
		if (value.IsNullOrEmpty())
		{
			value = GetValue(type, ServerMessageLanguage.EN);
		}
		return value;
	}
}
