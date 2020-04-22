using LobbyGameClientMessages;

public class SlashCommand_GroupLeave : SlashCommand
{
	public SlashCommand_GroupLeave()
		: base("/leave", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		
		clientGameManager.LeaveGroup(delegate(GroupLeaveResponse r)
			{
				if (!r.Success)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
						{
							string arg = (r.LocalizedFailure != null) ? r.LocalizedFailure.ToString() : ((r.ErrorMessage == null) ? StringUtil.TR("UnknownError", "Global") : $"{r.ErrorMessage}#needsLocalization");
							TextConsole.Get().Write(new TextConsole.Message
							{
								Text = string.Format(StringUtil.TR("FailedMessage", "Global"), arg),
								MessageType = ConsoleMessageType.SystemMessage
							});
							return;
						}
						}
					}
				}
				ClientGameManager clientGameManager2 = ClientGameManager.Get();
				if (clientGameManager2 != null && clientGameManager2.GroupInfo != null)
				{
					ClientGameManager.Get().GroupInfo.InAGroup = false;
					ClientGameManager.Get().GroupInfo.IsLeader = false;
					ClientGameManager.Get().GroupInfo.Members.Clear();
				}
				if (UICharacterSelectScreen.Get() != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							UICharacterSelectScreenController.Get().NotifyGroupUpdate();
							return;
						}
					}
				}
			});
	}
}
