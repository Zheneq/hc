using System;

[Serializable]
public class ServerMessageOverrides
{
	public ServerMessage MOTDText { get; set; }

	public ServerMessage MOTDPopUpText { get; set; }

	public ServerMessage ReleaseNotesText { get; set; }

	public ServerMessage ReleaseNotesHeader { get; set; }

	public ServerMessage ReleaseNotesDescription { get; set; }

	public ServerMessage WhatsNewText { get; set; }

	public ServerMessage WhatsNewHeader { get; set; }

	public ServerMessage WhatsNewDescription { get; set; }

	public ServerMessage LockScreenText { get; set; }

	public ServerMessage LockScreenButtonText { get; set; }

	public string FreeUpsellExternalBrowserUrl { get; set; }

	public string FreeUpsellExternalBrowserSteamUrl { get; set; }

	public ServerMessage FacebookOAuthRedirectUriContent { get; set; }

	public void SetValue(ServerMessageType type, ServerMessage value)
	{
		base.GetType().GetProperty(type.ToString()).SetValue(this, value, null);
	}

	public string GetValue(ServerMessageType type, string language)
	{
		ServerMessage serverMessage = (ServerMessage)base.GetType().GetProperty(type.ToString()).GetValue(this, null);
		if (serverMessage == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ServerMessageOverrides.GetValue(ServerMessageType, string)).MethodHandle;
			}
			return null;
		}
		return serverMessage.GetValue(language);
	}

	public string GetValueOrDefault(ServerMessageType type, string language)
	{
		string value = this.GetValue(type, language);
		if (value.IsNullOrEmpty())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ServerMessageOverrides.GetValueOrDefault(ServerMessageType, string)).MethodHandle;
			}
			value = this.GetValue(type, ServerMessageLanguage.EN);
		}
		return value;
	}

	public string GetValue(ServerMessageType type, ServerMessageLanguage language)
	{
		ServerMessage serverMessage = (ServerMessage)base.GetType().GetProperty(type.ToString()).GetValue(this, null);
		if (serverMessage == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ServerMessageOverrides.GetValue(ServerMessageType, ServerMessageLanguage)).MethodHandle;
			}
			return null;
		}
		return serverMessage.GetValue(language);
	}

	public string GetValueOrDefault(ServerMessageType type, ServerMessageLanguage language)
	{
		string value = this.GetValue(type, language);
		if (value.IsNullOrEmpty())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ServerMessageOverrides.GetValueOrDefault(ServerMessageType, ServerMessageLanguage)).MethodHandle;
			}
			value = this.GetValue(type, ServerMessageLanguage.EN);
		}
		return value;
	}
}
