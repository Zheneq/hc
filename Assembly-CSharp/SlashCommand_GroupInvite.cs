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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_GroupInvite.OnSlashCommand(string)).MethodHandle;
			}
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
								RuntimeMethodHandle runtimeMethodHandle2 = methodof(SlashCommand_GroupInvite.<OnSlashCommand>c__AnonStorey0.<>m__0(GroupInviteResponse)).MethodHandle;
							}
							bool flag;
							if (ClientGameManager.Get().GroupInfo != null)
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
								flag = ClientGameManager.Get().GroupInfo.InAGroup;
							}
							else
							{
								flag = false;
							}
							if (!flag && r.LocalizedFailure.Context == "Invite" && r.LocalizedFailure.Term == "OtherPlayerInOtherGroup")
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
								string empty = string.Empty;
								string description = string.Format(StringUtil.TR("OtherPlayerInOtherGroupSendJoinRequest", "Invite"), arguments);
								string leftButtonLabel = StringUtil.TR("Yes", "Global");
								string rightButtonLabel = StringUtil.TR("No", "Global");
								UIDialogBox.DialogButtonCallback leftButtonCallback = delegate(UIDialogBox UIDialogBox)
								{
									ClientGameManager clientGameManager = ClientGameManager.Get();
									string arguments2 = arguments;
									if (SlashCommand_GroupInvite.<OnSlashCommand>c__AnonStorey0.<>f__am$cache1 == null)
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
										if (!true)
										{
											RuntimeMethodHandle runtimeMethodHandle3 = methodof(SlashCommand_GroupInvite.<OnSlashCommand>c__AnonStorey0.<>m__1(UIDialogBox)).MethodHandle;
										}
										SlashCommand_GroupInvite.<OnSlashCommand>c__AnonStorey0.<>f__am$cache1 = delegate(GroupJoinResponse response)
										{
											if (response.Success)
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
													RuntimeMethodHandle runtimeMethodHandle4 = methodof(SlashCommand_GroupInvite.<OnSlashCommand>c__AnonStorey0.<>m__3(GroupJoinResponse)).MethodHandle;
												}
												return;
											}
											string text2;
											if (response.LocalizedFailure != null)
											{
												text2 = response.LocalizedFailure.ToString();
											}
											else if (!response.ErrorMessage.IsNullOrEmpty())
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
										};
									}
									clientGameManager.RequestToJoinGroup(arguments2, SlashCommand_GroupInvite.<OnSlashCommand>c__AnonStorey0.<>f__am$cache1);
								};
								if (SlashCommand_GroupInvite.<OnSlashCommand>c__AnonStorey0.<>f__am$cache0 == null)
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
									SlashCommand_GroupInvite.<OnSlashCommand>c__AnonStorey0.<>f__am$cache0 = delegate(UIDialogBox UIDialogBox)
									{
									};
								}
								UIDialogPopupManager.OpenTwoButtonDialog(empty, description, leftButtonLabel, rightButtonLabel, leftButtonCallback, SlashCommand_GroupInvite.<OnSlashCommand>c__AnonStorey0.<>f__am$cache0, false, false);
								return;
							}
							text = r.LocalizedFailure.ToString();
						}
						else if (!r.ErrorMessage.IsNullOrEmpty())
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = StringUtil.TR("InviteNameError", "SlashCommand"),
			MessageType = ConsoleMessageType.SystemMessage
		}, null);
	}
}
