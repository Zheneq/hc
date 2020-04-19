using System;
using System.Collections.Generic;
using LobbyGameClientMessages;

public class SlashCommand_Friend : SlashCommand
{
	public SlashCommand_Friend() : base("/friend", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_Friend.OnSlashCommand(string)).MethodHandle;
			}
			if (!(ClientGameManager.Get() == null))
			{
				string[] array = arguments.Split(null, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length < 2)
				{
					return;
				}
				string text = array[1];
				FriendOperation friendOperation = FriendOperation.Unknown;
				string message = string.Empty;
				string strData = string.Empty;
				long friendAccountId = 0L;
				if (array[0] == StringUtil.TR("AddFriend", "SlashCommand"))
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
					for (;;)
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
					for (;;)
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
					for (;;)
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
							FriendInfo friendInfo = enumerator.Current;
							if (friendInfo.FriendHandle.StartsWith(text))
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
								if (friendAccountId > 0L)
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
									TextConsole.Get().Write(new TextConsole.Message
									{
										Text = StringUtil.TR("AmbiguousFriendName", "SlashCommand"),
										MessageType = ConsoleMessageType.SystemMessage
									}, null);
									return;
								}
								friendAccountId = friendInfo.FriendAccountId;
							}
						}
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (friendAccountId == 0L)
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
						TextConsole.Get().Write(new TextConsole.Message
						{
							Text = StringUtil.TR("YouAreNotFriends", "SlashCommand"),
							MessageType = ConsoleMessageType.SystemMessage
						}, null);
						return;
					}
				}
				else
				{
					TextConsole.Get().Write(new TextConsole.Message
					{
						Text = StringUtil.TR("FriendSyntax", "SlashCommand"),
						MessageType = ConsoleMessageType.SystemMessage
					}, null);
				}
				if (friendOperation != FriendOperation.Unknown)
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
									RuntimeMethodHandle runtimeMethodHandle2 = methodof(SlashCommand_Friend.<OnSlashCommand>c__AnonStorey0.<>m__0(FriendUpdateResponse)).MethodHandle;
								}
								r.ErrorMessage = StringUtil.TR("UnknownError", "Global");
							}
							message = string.Format(StringUtil.TR("FailedMessage", "Global"), r.ErrorMessage);
						}
						else if (friendOperation == FriendOperation.Note)
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
							foreach (FriendInfo friendInfo2 in ClientGameManager.Get().FriendList.Friends.Values)
							{
								if (friendInfo2.FriendAccountId == friendAccountId)
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
									friendInfo2.FriendNote = strData;
									FriendListPanel.Get().UpdateFriendBannerNote(friendInfo2);
									break;
								}
							}
						}
						TextConsole.Get().Write(new TextConsole.Message
						{
							Text = message,
							MessageType = ConsoleMessageType.SystemMessage
						}, null);
					});
				}
				return;
			}
		}
	}
}
