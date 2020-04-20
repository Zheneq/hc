using System;
using LobbyGameClientMessages;

public class SlashCommand_GroupLeave : SlashCommand
{
	public SlashCommand_GroupLeave() : base("/leave", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		
		clientGameManager.LeaveGroup(delegate(GroupLeaveResponse r)
			{
				if (!r.Success)
				{
					string arg = (r.LocalizedFailure == null) ? ((r.ErrorMessage == null) ? StringUtil.TR("UnknownError", "Global") : string.Format("{0}#needsLocalization", r.ErrorMessage)) : r.LocalizedFailure.ToString();
					TextConsole.Get().Write(new TextConsole.Message
					{
						Text = string.Format(StringUtil.TR("FailedMessage", "Global"), arg),
						MessageType = ConsoleMessageType.SystemMessage
					}, null);
				}
				else
				{
					ClientGameManager clientGameManager2 = ClientGameManager.Get();
					if (clientGameManager2 != null && clientGameManager2.GroupInfo != null)
					{
						ClientGameManager.Get().GroupInfo.InAGroup = false;
						ClientGameManager.Get().GroupInfo.IsLeader = false;
						ClientGameManager.Get().GroupInfo.Members.Clear();
					}
					if (UICharacterSelectScreen.Get() != null)
					{
						UICharacterSelectScreenController.Get().NotifyGroupUpdate();
					}
				}
			});
	}
}
