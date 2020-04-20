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
			if (text.IndexOf('>') != -1)
			{
				text = text.Replace("<", "< ");
			}
		}
		return text;
	}

	public void OnInputSubmitted(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return;
		}
		bool flag = false;
		if (GameManager.Get() != null && GameManager.Get().GameInfo != null)
		{
			if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
			{
				flag = true;
			}
		}
		input = ChatEmojiManager.Get().UnlocalizeEmojis(input);
		input = this.RemoveRichTextTags(input);
		string text;
		string arguments;
		if (input[0] != '/')
		{
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
			if (DebugCommands.Get() != null)
			{
				bool flag2 = DebugCommands.Get().RunDebugCommand(text, arguments);
			}
		}
	}

	public void HandleSetDevTagResponse(SetDevTagResponse response)
	{
		string text = string.Empty;
		if (response.Success)
		{
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
			if (Options_UI.Get().GetEnableProfanityFilter() && notification.ConsoleMessageType != ConsoleMessageType.BroadcastMessage)
			{
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
				if (!(clientGameManager.Handle != notification.SenderHandle))
				{
					goto IL_148;
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
