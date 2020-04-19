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
		if (SlashCommand_GroupLeave.<>f__am$cache0 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_GroupLeave.OnSlashCommand(string)).MethodHandle;
			}
			SlashCommand_GroupLeave.<>f__am$cache0 = delegate(GroupLeaveResponse r)
			{
				if (!r.Success)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(SlashCommand_GroupLeave.<OnSlashCommand>m__0(GroupLeaveResponse)).MethodHandle;
					}
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
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						ClientGameManager.Get().GroupInfo.InAGroup = false;
						ClientGameManager.Get().GroupInfo.IsLeader = false;
						ClientGameManager.Get().GroupInfo.Members.Clear();
					}
					if (UICharacterSelectScreen.Get() != null)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						UICharacterSelectScreenController.Get().NotifyGroupUpdate();
					}
				}
			};
		}
		clientGameManager.LeaveGroup(SlashCommand_GroupLeave.<>f__am$cache0);
	}
}
