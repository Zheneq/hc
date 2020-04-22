using I2.Loc;
using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Threading;

public class TextConsole
{
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

	private static TextConsole s_instance;

	public string LastWhisperSenderHandle
	{
		get;
		private set;
	}

	public event Action<Message, AllowedEmojis> OnMessage
	{
		add
		{
			Action<Message, AllowedEmojis> action = this.OnMessage;
			Action<Message, AllowedEmojis> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnMessage, (Action<Message, AllowedEmojis>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		remove
		{
			Action<Message, AllowedEmojis> action = this.OnMessage;
			Action<Message, AllowedEmojis> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnMessage, (Action<Message, AllowedEmojis>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public TextConsole()
	{
		this.OnMessage = delegate
		{
		};
		base._002Ector();
		ClientGameManager.Get().OnChatNotification += HandleChatNotification;
	}

	public static TextConsole Get()
	{
		return s_instance;
	}

	public static void Instantiate()
	{
		s_instance = new TextConsole();
	}

	public void Write(Message message, List<int> EmojisAllowed = null)
	{
		AllowedEmojis allowedEmojis = default(AllowedEmojis);
		allowedEmojis.emojis = EmojisAllowed;
		this.OnMessage(message, allowedEmojis);
		UITextConsole.StoreMessage(message, allowedEmojis);
	}

	public void Write(string text, ConsoleMessageType messageType = ConsoleMessageType.SystemMessage)
	{
		Message message = default(Message);
		message.MessageType = messageType;
		message.Text = text;
		Write(message);
	}

	public string RemoveRichTextTags(string theString)
	{
		string text = theString;
		if (text.IndexOf('<') != -1)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (text.IndexOf('>') != -1)
			{
				while (true)
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		bool flag = false;
		if (GameManager.Get() != null && GameManager.Get().GameInfo != null)
		{
			while (true)
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
				while (true)
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
		input = RemoveRichTextTags(input);
		string arguments;
		string text;
		if (input[0] != '/')
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			text = ((!flag) ? "/global" : "/team");
			arguments = input;
		}
		else
		{
			string[] array = input.Split((string[])null, 2, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length >= 2)
			{
				while (true)
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
		if (SlashCommands.Get().RunSlashCommand(text, arguments))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (DebugCommands.Get() != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					bool flag2 = DebugCommands.Get().RunDebugCommand(text, arguments);
					return;
				}
			}
			return;
		}
	}

	public void HandleSetDevTagResponse(SetDevTagResponse response)
	{
		string empty = string.Empty;
		if (response.Success)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			empty = "Success";
		}
		else
		{
			empty = "Failed";
		}
		Write(new Message
		{
			Text = empty,
			MessageType = ConsoleMessageType.SystemMessage
		});
	}

	public void HandleChatNotification(ChatNotification notification)
	{
		if (Options_UI.Get() != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (Options_UI.Get().GetEnableProfanityFilter() && notification.ConsoleMessageType != ConsoleMessageType.BroadcastMessage)
			{
				while (true)
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
		Write(new Message
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
				while (true)
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
					goto IL_0148;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			LastWhisperSenderHandle = notification.SenderHandle;
		}
		goto IL_0148;
		IL_0148:
		if (notification.ConsoleMessageType == ConsoleMessageType.BroadcastMessage)
		{
			SystemMenuBroadcast.Get().DisplaySystemMessage(notification);
		}
	}
}
