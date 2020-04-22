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
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							int num;
							if (ClientGameManager.Get().GroupInfo != null)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
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
											if (_003COnSlashCommand_003Ec__AnonStorey0._003C_003Ef__am_0024cache1 == null)
											{
												while (true)
												{
													switch (4)
													{
													case 0:
														continue;
													}
													break;
												}
												if (1 == 0)
												{
													/*OpCode not supported: LdMemberToken*/;
												}
												_003COnSlashCommand_003Ec__AnonStorey0._003C_003Ef__am_0024cache1 = delegate(GroupJoinResponse response)
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
																if (1 == 0)
																{
																	/*OpCode not supported: LdMemberToken*/;
																}
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
														while (true)
														{
															switch (6)
															{
															case 0:
																continue;
															}
															break;
														}
														text2 = $"Failed: {response.ErrorMessage}#NeedsLocalization";
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
												};
											}
											clientGameManager.RequestToJoinGroup(friendHandle, _003COnSlashCommand_003Ec__AnonStorey0._003C_003Ef__am_0024cache1);
										};
										if (_003COnSlashCommand_003Ec__AnonStorey0._003C_003Ef__am_0024cache0 == null)
										{
											while (true)
											{
												switch (5)
												{
												case 0:
													continue;
												}
												break;
											}
											_003COnSlashCommand_003Ec__AnonStorey0._003C_003Ef__am_0024cache0 = delegate
											{
											};
										}
										UIDialogPopupManager.OpenTwoButtonDialog(empty, description, leftButtonLabel, rightButtonLabel, leftButtonCallback, _003COnSlashCommand_003Ec__AnonStorey0._003C_003Ef__am_0024cache0);
										return;
									}
									}
								}
							}
							text = r.LocalizedFailure.ToString();
						}
						else if (!r.ErrorMessage.IsNullOrEmpty())
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							text = $"Failed: {r.ErrorMessage}#NeedsLocalization";
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
			while (true)
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
		});
	}
}
