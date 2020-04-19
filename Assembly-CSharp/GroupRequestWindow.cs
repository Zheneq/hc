using System;
using LobbyGameClientMessages;

internal class GroupRequestWindow
{
	private GroupConfirmationRequest m_request;

	private UIPartyInvitePopDialogBox m_dialogBox;

	private bool m_done;

	private DateTime m_expiration;

	internal GroupRequestWindow(GroupConfirmationRequest request)
	{
		this.m_request = request;
		this.m_expiration = DateTime.UtcNow + request.ExpirationTime;
		this.SpawnNewPopup();
	}

	private void SpawnNewPopup()
	{
		if (this.m_request.Type == GroupConfirmationRequest.JoinType.\u001D)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GroupRequestWindow.SpawnNewPopup()).MethodHandle;
			}
			string description = string.Format(StringUtil.TR("InviteToFormGroup", "Global"), this.m_request.LeaderName);
			this.m_dialogBox = UIDialogPopupManager.OpenPartyInviteDialog(StringUtil.TR("GroupRequest", "Global"), description, StringUtil.TR("Join", "Global"), StringUtil.TR("Reject", "Global"), delegate(UIDialogBox UIDialogBox)
			{
				this.BlockPlayerFromGroupRequest();
			}, delegate(UIDialogBox UIDialogBox)
			{
				this.Join();
			}, delegate(UIDialogBox UIDialogBox)
			{
				this.Reject();
			});
		}
		else if (this.m_request.Type == GroupConfirmationRequest.JoinType.\u000E)
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
			string description2 = string.Format(StringUtil.TR("RequestToJoinGroup", "Invite"), this.m_request.JoinerName);
			this.m_dialogBox = UIDialogPopupManager.OpenPartyInviteDialog(StringUtil.TR("GroupRequest", "Global"), description2, StringUtil.TR("Approve", "Global"), StringUtil.TR("Reject", "Global"), delegate(UIDialogBox UIDialogBox)
			{
				this.BlockPlayerFromGroupRequest();
			}, delegate(UIDialogBox UIDialogBox)
			{
				this.Join();
			}, delegate(UIDialogBox UIDialogBox)
			{
				this.Reject();
			});
		}
	}

	public void CleanupWindow()
	{
		if (!this.m_done)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GroupRequestWindow.CleanupWindow()).MethodHandle;
			}
			this.MarkDone();
			this.m_dialogBox.Close();
			GroupJoinManager.Get().SendGroupConfirmation(GroupInviteResponseType.OfferExpired, this.m_request);
		}
	}

	private void MarkDone()
	{
		this.m_done = true;
		GroupJoinManager.Get().RemoveRequest(this.m_request);
	}

	private void Join()
	{
		this.MarkDone();
		GroupJoinManager.Get().SendGroupConfirmation(GroupInviteResponseType.PlayerAccepted, this.m_request);
	}

	private void Reject()
	{
		this.MarkDone();
		GroupJoinManager.Get().SendGroupConfirmation(GroupInviteResponseType.PlayerRejected, this.m_request);
	}

	private void BlockPlayerFromGroupRequest()
	{
		string title = StringUtil.TR("BlockPlayer", "FriendList");
		string description = string.Format(StringUtil.TR("DoYouWantToBlock", "FriendList"), this.m_request.LeaderName);
		UIDialogPopupManager.OpenTwoButtonDialog(title, description, StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate(UIDialogBox dialogReference)
		{
			this.MarkDone();
			GroupJoinManager.Get().SendGroupConfirmation(GroupInviteResponseType.PlayerRejected, this.m_request);
			SlashCommands.Get().RunSlashCommand("/block", this.m_request.LeaderFullHandle);
		}, delegate(UIDialogBox dialogReference)
		{
			this.SpawnNewPopup();
		}, false, false);
	}

	public bool HasExpired(DateTime time)
	{
		bool result;
		if (!this.m_done)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GroupRequestWindow.HasExpired(DateTime)).MethodHandle;
			}
			result = (this.m_expiration < time);
		}
		else
		{
			result = false;
		}
		return result;
	}
}
