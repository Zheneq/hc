using LobbyGameClientMessages;
using System;
using System.Collections.Generic;

public class SlashCommand_InviteToGame : SlashCommand
{
    public SlashCommand_InviteToGame()
        : base("/invitetogame", SlashCommandType.InFrontEnd)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        Action<GameInvitationResponse> onResponseCallback = delegate(GameInvitationResponse response)
        {
            TextConsole.Message message = new TextConsole.Message
            {
                MessageType = ConsoleMessageType.SystemMessage
            };

            if (response.Success)
            {
                message.Text = string.Format(StringUtil.TR("InviteSentTo", "SlashCommand"), response.InviteeHandle);
            }
            else if (response.LocalizedFailure != null)
            {
                message.Text = response.LocalizedFailure.ToString();
            }
            else if (!response.ErrorMessage.IsNullOrEmpty())
            {
                message.Text = $"Failed: {response.ErrorMessage}#NeedsLocalization";
            }
            else
            {
                message.Text = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
            }

            TextConsole.Get().Write(message);
        };

        if (!arguments.IsNullOrEmpty() && ClientGameManager.Get() != null)
        {
            ClientGameManager.Get().InvitePlayerToGame(arguments, onResponseCallback);
            return;
        }

        FriendList friendList = ClientGameManager.Get().FriendList;
        foreach (KeyValuePair<long, FriendInfo> friend in friendList.Friends)
        {
            if (friend.Value.IsOnline)
            {
                ClientGameManager.Get().InvitePlayerToGame(friend.Value.FriendHandle, onResponseCallback);
            }
        }
    }
}