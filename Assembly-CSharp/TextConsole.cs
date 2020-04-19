using System;
using System.Collections.Generic;
using System.Threading;
using I2.Loc;
using LobbyGameClientMessages;

public class TextConsole
{
	private static TextConsole s_instance;

	public TextConsole()
	{
		this.OnMessage = delegate(TextConsole.Message A_0, TextConsole.AllowedEmojis A_1)
		{
		};
		base..ctor();
		ClientGameManager.Get().OnChatNotification += this.HandleChatNotification;
	}

	public static TextConsole Get()
	{
		return TextConsole.s_instance;
	}

	public static void Instantiate()
	{
		TextConsole.s_instance = new TextConsole();
	}

	public string LastWhisperSenderHandle { get; private set; }

	public event Action<TextConsole.Message, TextConsole.AllowedEmojis> OnMessage
	{
		add
		{
			Action<TextConsole.Message, TextConsole.AllowedEmojis> action = this.OnMessage;
			Action<TextConsole.Message, TextConsole.AllowedEmojis> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<TextConsole.Message, TextConsole.AllowedEmojis>>(ref this.OnMessage, (Action<TextConsole.Message, TextConsole.AllowedEmojis>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TextConsole.add_OnMessage(Action<TextConsole.Message, TextConsole.AllowedEmojis>)).MethodHandle;
			}
		}
		remove
		{
			Action<TextConsole.Message, TextConsole.AllowedEmojis> action = this.OnMessage;
			Action<TextConsole.Message, TextConsole.AllowedEmojis> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<TextConsole.Message, TextConsole.AllowedEmojis>>(ref this.OnMessage, (Action<TextConsole.Message, TextConsole.AllowedEmojis>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public void Write(TextConsole.Message message, List<int> EmojisAllowed = null)
	{
		TextConsole.AllowedEmojis allowedEmojis;
		allowedEmojis.emojis = EmojisAllowed;
		this.OnMessage(message, allowedEmojis);
		UITextConsole.StoreMessage(message, allowedEmojis);
	}

	public void Write(string text, ConsoleMessageType messageType = ConsoleMessageType.SystemMessage)
	{
		this.Write(new TextConsole.Message
		{
			MessageType = messageType,
			Text = text
		}, null);
	}

	public string RemoveRichTextTags(string theString)
	{
		string text = theString;
		if (text.IndexOf('<') != -1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TextConsole.RemoveRichTextTags(string)).MethodHandle;
			}
			if (text.IndexOf('>') != -1)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				text = text.Replace("<", "< ");
			}
		}
		return text;
	}

	public void OnInputSubmitted(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TextConsole.OnInputSubmitted(string)).MethodHandle;
			}
			return;
		}
		bool flag = false;
		if (GameManager.Get() != null && GameManager.Get().GameInfo != null)
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
			if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
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
				flag = true;
			}
		}
		input = ChatEmojiManager.Get().UnlocalizeEmojis(input);
		input = this.RemoveRichTextTags(input);
		string text;
		string arguments;
		if (input[0] != '/')
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
			if (flag)
			{
				text = "/team";
			}
			else
			{
				text = "/global";
			}
			arguments = input;
		}
		else
		{
			string[] array = input.Split(null, 2, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length >= 2)
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
				text = array[0];
				arguments = array[1];
			}
			else
			{
				text = input;
				arguments = string.Empty;
			}
		}
		text = text.Trim();
		if (!SlashCommands.Get().RunSlashCommand(text, arguments))
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
			if (DebugCommands.Get() != null)
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
				bool flag2 = DebugCommands.Get().RunDebugCommand(text, arguments);
			}
		}
	}

	public void HandleSetDevTagResponse(SetDevTagResponse response)
	{
		string text = string.Empty;
		if (response.Success)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TextConsole.HandleSetDevTagResponse(SetDevTagResponse)).MethodHandle;
			}
			text = "Success";
		}
		else
		{
			text = "Failed";
		}
		this.Write(new TextConsole.Message
		{
			Text = text,
			MessageType = ConsoleMessageType.SystemMessage
		}, null);
	}

	public void HandleChatNotification(ChatNotification notification)
	{
		if (Options_UI.Get() != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TextConsole.HandleChatNotification(ChatNotification)).MethodHandle;
			}
			if (Options_UI.Get().GetEnableProfanityFilter() && notification.ConsoleMessageType != ConsoleMessageType.BroadcastMessage)
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
				notification.Text = BannedWords.Get().FilterPhrase(notification.Text, LocalizationManager.CurrentLanguageCode);
			}
		}
		this.Write(new TextConsole.Message
		{
			Text = ((notification.LocalizedText == null) ? notification.Text : notification.LocalizedText.ToString()),
			MessageType = notification.ConsoleMessageType,
			SenderAccountId = notification.SenderAccountId,
			SenderHandle = notification.SenderHandle,
			SenderTeam = notification.SenderTeam,
			RecipientHandle = notification.RecipientHandle,
			CharacterType = notification.CharacterType,
			DisplayDevTag = notification.DisplayDevTag
		}, notification.EmojisAllowed);
		if (notification.ConsoleMessageType == ConsoleMessageType.WhisperChat)
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (!(clientGameManager == null))
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(clientGameManager.Handle != notification.SenderHandle))
				{
					goto IL_148;
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.LastWhisperSenderHandle = notification.SenderHandle;
		}
		IL_148:
		if (notification.ConsoleMessageType == ConsoleMessageType.BroadcastMessage)
		{
			SystemMenuBroadcast.Get().DisplaySystemMessage(notification);
		}
	}

	public struct Message
	{
		public string Text;

		public ConsoleMessageType MessageType;

		public CharacterType CharacterType;

		public bool DisplayDevTag;

		public long SenderAccountId;

		public string SenderHandle;

		public Team SenderTeam;

		public string RecipientHandle;

		public Team RestrictVisibiltyToTeam;
	}

	public struct AllowedEmojis
	{
		public List<int> emojis;
	}
}
