using System;
using System.Collections.Generic;
using LobbyGameClientMessages;

internal class GroupJoinManager
{
	private static GroupJoinManager s_instance;

	private Dictionary<string, GroupRequestWindow> m_pendingWindows = new Dictionary<string, GroupRequestWindow>();

	private Dictionary<string, LeakyBucket> m_restrictSpammers = new Dictionary<string, LeakyBucket>();

	public static GroupJoinManager Get()
	{
		if (GroupJoinManager.s_instance == null)
		{
			GroupJoinManager.s_instance = new GroupJoinManager();
		}
		return GroupJoinManager.s_instance;
	}

	public void Update()
	{
		DateTime utcNow = DateTime.UtcNow;
		List<GroupRequestWindow> list = new List<GroupRequestWindow>();
		foreach (GroupRequestWindow groupRequestWindow in this.m_pendingWindows.Values)
		{
			if (groupRequestWindow.HasExpired(utcNow))
			{
				list.Add(groupRequestWindow);
			}
		}
		List<GroupRequestWindow> list2 = list;
		
		list2.ForEach(delegate(GroupRequestWindow p)
			{
				p.CleanupWindow();
			});
	}

	internal void AddRequest(GroupConfirmationRequest request)
	{
		if (UIFrontEnd.Get() == null)
		{
			return;
		}
		if (UIFrontEnd.Get().m_landingPageScreen != null)
		{
			if (UIFrontEnd.Get().m_landingPageScreen.m_inCustomGame)
			{
				this.SendGroupConfirmation(GroupInviteResponseType.PlayerInCustomMatch, request);
				return;
			}
		}
		if (this.m_pendingWindows.ContainsKey(request.LeaderFullHandle))
		{
			this.SendGroupConfirmation(GroupInviteResponseType.PlayerStillAwaitingPreviousQuery, request);
			return;
		}
		if (request == null)
		{
			throw new Exception("request is null");
		}
		LeakyBucket leakyBucket;
		if (this.m_restrictSpammers.TryGetValue(request.LeaderFullHandle, out leakyBucket) && leakyBucket != null)
		{
			if (!leakyBucket.CanAdd(1.0))
			{
				this.SendGroupConfirmation(GroupInviteResponseType.RequestorSpamming, request);
				return;
			}
		}
		this.m_pendingWindows.Add(request.LeaderFullHandle, new GroupRequestWindow(request));
	}

	internal void RemoveRequest(GroupConfirmationRequest request)
	{
		this.m_pendingWindows.Remove(request.LeaderFullHandle);
	}

	internal void SendGroupConfirmation(GroupInviteResponseType status, GroupConfirmationRequest request)
	{
		if (status == GroupInviteResponseType.PlayerRejected)
		{
			LeakyBucket leakyBucket;
			if (!this.m_restrictSpammers.TryGetValue(request.LeaderFullHandle, out leakyBucket))
			{
				leakyBucket = new LeakyBucket(2.0, TimeSpan.FromMinutes(10.0));
				this.m_restrictSpammers.Add(request.LeaderFullHandle, leakyBucket);
			}
			leakyBucket.Add(1.0);
		}
		GroupConfirmationResponse groupConfirmationResponse = new GroupConfirmationResponse();
		groupConfirmationResponse.Acceptance = status;
		groupConfirmationResponse.GroupId = request.GroupId;
		groupConfirmationResponse.ConfirmationNumber = request.ConfirmationNumber;
		groupConfirmationResponse.JoinerAccountId = request.JoinerAccountId;
		ClientGameManager.Get().LobbyInterface.SendMessage(groupConfirmationResponse);
	}
}
