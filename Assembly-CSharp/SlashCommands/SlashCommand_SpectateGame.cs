using LobbyGameClientMessages;
using System;

public class SlashCommand_SpectateGame : SlashCommand
{
    public SlashCommand_SpectateGame()
        : base("/spectategame", SlashCommandType.InFrontEnd)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        if (!GameManager.Get().GameplayOverrides.AllowSpectatorsOutsideCustom)
        {
            TextConsole.Get().Write(StringUtil.TR("FriendGameSpectatingNotAvailable", "Frontend"));
            return;
        }

        Action<GameSpectatorResponse> onResponseCallback = delegate(GameSpectatorResponse response)
        {
            if (response.Success)
            {
                return;
            }

            TextConsole.Get().Write(
                new TextConsole.Message
                {
                    MessageType = ConsoleMessageType.SystemMessage,
                    Text = response.LocalizedFailure != null
                        ? response.LocalizedFailure.ToString()
                        : !response.ErrorMessage.IsNullOrEmpty()
                            ? $"Failed: {response.ErrorMessage}#NeedsLocalization"
                            : StringUtil.TR("UnknownErrorTryAgain", "Frontend")
                });
        };

        if (!arguments.IsNullOrEmpty() && ClientGameManager.Get() != null)
        {
            ClientGameManager.Get().SpectateGame(arguments, onResponseCallback);
            return;
        }

        foreach (FriendInfo friendInfo in ClientGameManager.Get().FriendList.Friends.Values)
        {
            if (friendInfo.IsJoinable(GameManager.Get().GameplayOverrides))
            {
                ClientGameManager.Get().SpectateGame(friendInfo.FriendHandle, onResponseCallback);
                break;
            }
        }

        TextConsole.Get().Write(StringUtil.TR("NoFriendsIngame", "Frontend"));
    }
}