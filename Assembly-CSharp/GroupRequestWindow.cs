using LobbyGameClientMessages;
using System;

internal class GroupRequestWindow
{
	private GroupConfirmationRequest m_request;

	private UIPartyInvitePopDialogBox m_dialogBox;

	private bool m_done;

	private DateTime m_expiration;

	internal GroupRequestWindow(GroupConfirmationRequest request)
	{
		m_request = request;
		m_expiration = DateTime.UtcNow + request.ExpirationTime;
		SpawnNewPopup();
	}

	private void SpawnNewPopup()
	{
		if (m_request.Type == GroupConfirmationRequest.JoinType._001D)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					string description = string.Format(StringUtil.TR("InviteToFormGroup", "Global"), m_request.LeaderName);
					m_dialogBox = UIDialogPopupManager.OpenPartyInviteDialog(StringUtil.TR("GroupRequest", "Global"), description, StringUtil.TR("Join", "Global"), StringUtil.TR("Reject", "Global"), delegate
					{
						BlockPlayerFromGroupRequest();
					}, delegate
					{
						Join();
					}, delegate
					{
						Reject();
					});
					return;
				}
				}
			}
		}
		if (m_request.Type != GroupConfirmationRequest.JoinType._000E)
		{
			return;
		}
		while (true)
		{
			string description2 = string.Format(StringUtil.TR("RequestToJoinGroup", "Invite"), m_request.JoinerName);
			m_dialogBox = UIDialogPopupManager.OpenPartyInviteDialog(StringUtil.TR("GroupRequest", "Global"), description2, StringUtil.TR("Approve", "Global"), StringUtil.TR("Reject", "Global"), delegate
			{
				BlockPlayerFromGroupRequest();
			}, delegate
			{
				Join();
			}, delegate
			{
				Reject();
			});
			return;
		}
	}

	public void CleanupWindow()
	{
		if (m_done)
		{
			return;
		}
		while (true)
		{
			MarkDone();
			m_dialogBox.Close();
			GroupJoinManager.Get().SendGroupConfirmation(GroupInviteResponseType.OfferExpired, m_request);
			return;
		}
	}

	private void MarkDone()
	{
		m_done = true;
		GroupJoinManager.Get().RemoveRequest(m_request);
	}

	private void Join()
	{
		MarkDone();
		GroupJoinManager.Get().SendGroupConfirmation(GroupInviteResponseType.PlayerAccepted, m_request);
	}

	private void Reject()
	{
		MarkDone();
		GroupJoinManager.Get().SendGroupConfirmation(GroupInviteResponseType.PlayerRejected, m_request);
	}

	private void BlockPlayerFromGroupRequest()
	{
		string title = StringUtil.TR("BlockPlayer", "FriendList");
		string description = string.Format(StringUtil.TR("DoYouWantToBlock", "FriendList"), m_request.LeaderName);
		UIDialogPopupManager.OpenTwoButtonDialog(title, description, StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate
		{
			MarkDone();
			GroupJoinManager.Get().SendGroupConfirmation(GroupInviteResponseType.PlayerRejected, m_request);
			SlashCommands.Get().RunSlashCommand("/block", m_request.LeaderFullHandle);
		}, delegate
		{
			SpawnNewPopup();
		});
	}

	public bool HasExpired(DateTime time)
	{
		int result;
		if (!m_done)
		{
			result = ((m_expiration < time) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}
}
