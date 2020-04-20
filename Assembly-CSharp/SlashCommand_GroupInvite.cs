using System;
using LobbyGameClientMessages;

public class SlashCommand_GroupInvite : SlashCommand
{
	public SlashCommand_GroupInvite() : base("/invite", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
		{
			if (!(ClientGameManager.Get() == null))
			{
				TextConsole.Get().Write(new TextConsole.Message
				{
					Text = string.Format(StringUtil.TR("InvitedFriendToGroup", "Global"), arguments),
					MessageType = ConsoleMessageType.SystemMessage
				}, null);
				ClientGameManager.Get().InviteToGroup(arguments, delegate(GroupInviteResponse r)
				{
					if (!r.Success)
					{
						string text;
						if (r.LocalizedFailure != null)
						{
							bool flag;
							if (ClientGameManager.Get().GroupInfo != null)
							{
								flag = ClientGameManager.Get().GroupInfo.InAGroup;
							}
							else
							{
								flag = false;
							}
							if (!flag && r.LocalizedFailure.Context == "Invite" && r.LocalizedFailure.Term == "OtherPlayerInOtherGroup")
							{
								string empty = string.Empty;
								string description = string.Format(StringUtil.TR("OtherPlayerInOtherGroupSendJoinRequest", "Invite"), arguments);
								string leftButtonLabel = StringUtil.TR("Yes", "Global");
								string rightButtonLabel = StringUtil.TR("No", "Global");
								UIDialogBox.DialogButtonCallback leftButtonCallback = delegate(UIDialogBox UIDialogBox)
								{
									ClientGameManager clientGameManager = ClientGameManager.Get();
									string arguments2 = arguments;
									
									clientGameManager.RequestToJoinGroup(arguments2, delegate(GroupJoinResponse response)
										{
											if (response.Success)
											{
												return;
											}
											string text2;
											if (response.LocalizedFailure != null)
											{
												text2 = response.LocalizedFailure.ToString();
											}
											else if (!response.ErrorMessage.IsNullOrEmpty())
											{
												text2 = string.Format("Failed: {0}#NeedsLocalization", response.ErrorMessage);
											}
											else
											{
												text2 = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
											}
											TextConsole.Get().Write(new TextConsole.Message
											{
												Text = text2,
												MessageType = ConsoleMessageType.SystemMessage
											}, null);
										});
								};
								
								UIDialogPopupManager.OpenTwoButtonDialog(empty, description, leftButtonLabel, rightButtonLabel, leftButtonCallback, delegate(UIDialogBox UIDialogBox)
									{
									}, false, false);
								return;
							}
							text = r.LocalizedFailure.ToString();
						}
						else if (!r.ErrorMessage.IsNullOrEmpty())
						{
							text = string.Format("Failed: {0}#NeedsLocalization", r.ErrorMessage);
						}
						else
						{
							text = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
						}
						TextConsole.Get().Write(new TextConsole.Message
						{
							Text = text,
							MessageType = ConsoleMessageType.SystemMessage
						}, null);
					}
				});
				return;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = StringUtil.TR("InviteNameError", "SlashCommand"),
			MessageType = ConsoleMessageType.SystemMessage
		}, null);
	}
}
