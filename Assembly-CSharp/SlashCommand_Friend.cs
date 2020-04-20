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
			if (!(ClientGameManager.Get() == null))
			{
				string[] array = arguments.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
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
					friendOperation = FriendOperation.Reject;
					message = StringUtil.TR("FriendRequestRejected", "SlashCommand");
				}
				else if (array[0] == StringUtil.TR("NoteFriend", "SlashCommand"))
				{
					friendOperation = FriendOperation.Note;
					message = StringUtil.TR("NoteRecorded", "SlashCommand");
					for (int i = 2; i < array.Length; i++)
					{
						strData = strData + array[i] + " ";
					}
					strData = strData.Trim();
					using (Dictionary<long, FriendInfo>.ValueCollection.Enumerator enumerator = ClientGameManager.Get().FriendList.Friends.Values.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							FriendInfo friendInfo = enumerator.Current;
							if (friendInfo.FriendHandle.StartsWith(text))
							{
								if (friendAccountId > 0L)
								{
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
					}
					if (friendAccountId == 0L)
					{
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
								r.ErrorMessage = StringUtil.TR("UnknownError", "Global");
							}
							message = string.Format(StringUtil.TR("FailedMessage", "Global"), r.ErrorMessage);
						}
						else if (friendOperation == FriendOperation.Note)
						{
							foreach (FriendInfo friendInfo2 in ClientGameManager.Get().FriendList.Friends.Values)
							{
								if (friendInfo2.FriendAccountId == friendAccountId)
								{
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
