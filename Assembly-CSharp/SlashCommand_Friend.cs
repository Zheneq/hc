using LobbyGameClientMessages;
using System;
using System.Collections.Generic;

public class SlashCommand_Friend : SlashCommand
{
	public SlashCommand_Friend()
		: base("/friend", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (arguments.IsNullOrEmpty())
		{
			return;
		}
		FriendOperation friendOperation;
		string message;
		string strData;
		long friendAccountId;
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (ClientGameManager.Get() == null)
			{
				return;
			}
			string[] array = arguments.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length < 2)
			{
				return;
			}
			string text = array[1];
			friendOperation = FriendOperation.Unknown;
			message = string.Empty;
			strData = string.Empty;
			friendAccountId = 0L;
			if (array[0] == StringUtil.TR("AddFriend", "SlashCommand"))
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
				friendOperation = FriendOperation.Add;
				message = StringUtil.TR("AddFriendRequest", "SlashCommand");
			}
			else if (array[0] == StringUtil.TR("AcceptFriend", "SlashCommand"))
			{
				friendOperation = FriendOperation.Accept;
				message = StringUtil.TR("AcceptFriendRequest", "SlashCommand");
			}
			else if (array[0] == StringUtil.TR("RemoveFriend", "SlashCommand"))
			{
				friendOperation = FriendOperation.Remove;
				message = StringUtil.TR("FriendRemoved", "SlashCommand");
			}
			else if (array[0] == StringUtil.TR("RejectFriend", "SlashCommand"))
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
				friendOperation = FriendOperation.Reject;
				message = StringUtil.TR("FriendRequestRejected", "SlashCommand");
			}
			else if (array[0] == StringUtil.TR("NoteFriend", "SlashCommand"))
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
				friendOperation = FriendOperation.Note;
				message = StringUtil.TR("NoteRecorded", "SlashCommand");
				for (int i = 2; i < array.Length; i++)
				{
					strData = strData + array[i] + " ";
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				strData = strData.Trim();
				using (Dictionary<long, FriendInfo>.ValueCollection.Enumerator enumerator = ClientGameManager.Get().FriendList.Friends.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FriendInfo current = enumerator.Current;
						if (current.FriendHandle.StartsWith(text))
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
							if (friendAccountId > 0)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
										TextConsole.Get().Write(new TextConsole.Message
										{
											Text = StringUtil.TR("AmbiguousFriendName", "SlashCommand"),
											MessageType = ConsoleMessageType.SystemMessage
										});
										return;
									}
								}
							}
							friendAccountId = current.FriendAccountId;
						}
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (friendAccountId == 0)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							TextConsole.Get().Write(new TextConsole.Message
							{
								Text = StringUtil.TR("YouAreNotFriends", "SlashCommand"),
								MessageType = ConsoleMessageType.SystemMessage
							});
							return;
						}
					}
				}
			}
			else
			{
				TextConsole.Get().Write(new TextConsole.Message
				{
					Text = StringUtil.TR("FriendSyntax", "SlashCommand"),
					MessageType = ConsoleMessageType.SystemMessage
				});
			}
			if (friendOperation == FriendOperation.Unknown)
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
				ClientGameManager.Get().UpdateFriend(text, friendAccountId, friendOperation, strData, delegate(FriendUpdateResponse r)
				{
					if (!r.Success)
					{
						if (r.LocalizedFailure != null)
						{
							r.ErrorMessage = r.LocalizedFailure.ToString();
						}
						else if (r.ErrorMessage.IsNullOrEmpty())
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
							r.ErrorMessage = StringUtil.TR("UnknownError", "Global");
						}
						message = string.Format(StringUtil.TR("FailedMessage", "Global"), r.ErrorMessage);
					}
					else if (friendOperation == FriendOperation.Note)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						foreach (FriendInfo value in ClientGameManager.Get().FriendList.Friends.Values)
						{
							if (value.FriendAccountId == friendAccountId)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										value.FriendNote = strData;
										FriendListPanel.Get().UpdateFriendBannerNote(value);
										goto end_IL_00ba;
									}
								}
							}
						}
					}
					TextConsole.Get().Write(new TextConsole.Message
					{
						Text = message,
						MessageType = ConsoleMessageType.SystemMessage
					});
				});
				return;
			}
		}
	}
}
