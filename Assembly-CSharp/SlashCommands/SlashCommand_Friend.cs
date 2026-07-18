using System;
using LobbyGameClientMessages;

public class SlashCommand_Friend : SlashCommand
{
    public SlashCommand_Friend() : base("/friend", SlashCommandType.Everywhere)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        if (arguments.IsNullOrEmpty() || ClientGameManager.Get() == null)
        {
            return;
        }

        string[] array = arguments.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
        if (array.Length < 2)
        {
            return;
        }

        string friendHandle = array[1];
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

            foreach (FriendInfo friendInfo in ClientGameManager.Get().FriendList.Friends.Values)
            {
                if (!friendInfo.FriendHandle.StartsWith(friendHandle))
                {
                    continue;
                }

                if (friendAccountId > 0L)
                {
                    TextConsole.Get().Write(
                        new TextConsole.Message
                        {
                            Text = StringUtil.TR("AmbiguousFriendName", "SlashCommand"),
                            MessageType = ConsoleMessageType.SystemMessage
                        });
                    return;
                }

                friendAccountId = friendInfo.FriendAccountId;
            }

            if (friendAccountId == 0L)
            {
                TextConsole.Get().Write(
                    new TextConsole.Message
                    {
                        Text = StringUtil.TR("YouAreNotFriends", "SlashCommand"),
                        MessageType = ConsoleMessageType.SystemMessage
                    });
                return;
            }
        }
        else
        {
            TextConsole.Get().Write(
                new TextConsole.Message
                {
                    Text = StringUtil.TR("FriendSyntax", "SlashCommand"),
                    MessageType = ConsoleMessageType.SystemMessage
                });
        }

        if (friendOperation == FriendOperation.Unknown)
        {
            return;
        }


        ClientGameManager.Get().UpdateFriend(
            friendHandle,
            friendAccountId,
            friendOperation,
            strData,
            delegate(FriendUpdateResponse r)
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
                    foreach (FriendInfo friendInfo in ClientGameManager.Get().FriendList.Friends.Values)
                    {
                        if (friendInfo.FriendAccountId != friendAccountId)
                        {
                            continue;
                        }

                        friendInfo.FriendNote = strData;
                        FriendListPanel.Get().UpdateFriendBannerNote(friendInfo);
                        break;
                    }
                }

                TextConsole.Get().Write(
                    new TextConsole.Message
                    {
                        Text = message,
                        MessageType = ConsoleMessageType.SystemMessage
                    });
            });
    }
}