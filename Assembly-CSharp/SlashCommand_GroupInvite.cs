using System.Text;
using LobbyGameClientMessages;

public class SlashCommand_GroupInvite : SlashCommand
{
	public SlashCommand_GroupInvite()
		: base("/invite", SlashCommandType.Everywhere)
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
				});
				ClientGameManager.Get().InviteToGroup(arguments, delegate(GroupInviteResponse r)
				{
					if (!r.Success)
					{
						string text;
						if (r.LocalizedFailure != null)
						{
							int num;
							if (ClientGameManager.Get().GroupInfo != null)
							{
								num = (ClientGameManager.Get().GroupInfo.InAGroup ? 1 : 0);
							}
							else
							{
								num = 0;
							}
							if (num == 0 && r.LocalizedFailure.Context == "Invite" && r.LocalizedFailure.Term == "OtherPlayerInOtherGroup")
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
									{
										string empty = string.Empty;
										string description = string.Format(StringUtil.TR("OtherPlayerInOtherGroupSendJoinRequest", "Invite"), arguments);
										string leftButtonLabel = StringUtil.TR("Yes", "Global");
										string rightButtonLabel = StringUtil.TR("No", "Global");
										UIDialogBox.DialogButtonCallback leftButtonCallback = delegate
										{
											ClientGameManager clientGameManager = ClientGameManager.Get();
											string friendHandle = arguments;
											
											clientGameManager.RequestToJoinGroup(friendHandle, delegate(GroupJoinResponse response)
												{
													if (response.Success)
													{
														while (true)
														{
															switch (7)
															{
															case 0:
																break;
															default:
																return;
															}
														}
													}
													string text2;
													if (response.LocalizedFailure != null)
													{
														text2 = response.LocalizedFailure.ToString();
													}
													else if (!response.ErrorMessage.IsNullOrEmpty())
													{
														text2 = new StringBuilder().Append("Failed: ").Append(response.ErrorMessage).Append("#NeedsLocalization").ToString();
													}
													else
													{
														text2 = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
													}
													TextConsole.Get().Write(new TextConsole.Message
													{
														Text = text2,
														MessageType = ConsoleMessageType.SystemMessage
													});
												});
										};
										
										UIDialogPopupManager.OpenTwoButtonDialog(empty, description, leftButtonLabel, rightButtonLabel, leftButtonCallback, delegate
											{
											});
										return;
									}
									}
								}
							}
							text = r.LocalizedFailure.ToString();
						}
						else if (!r.ErrorMessage.IsNullOrEmpty())
						{
							text = new StringBuilder().Append("Failed: ").Append(r.ErrorMessage).Append("#NeedsLocalization").ToString();
						}
						else
						{
							text = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
						}
						TextConsole.Get().Write(new TextConsole.Message
						{
							Text = text,
							MessageType = ConsoleMessageType.SystemMessage
						});
					}
				});
				return;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = StringUtil.TR("InviteNameError", "SlashCommand"),
			MessageType = ConsoleMessageType.SystemMessage
		});
	}
}
