using LobbyGameClientMessages;
using System;
using System.Collections.Generic;

internal class GroupJoinManager
{
	private static GroupJoinManager s_instance;

	private Dictionary<string, GroupRequestWindow> m_pendingWindows = new Dictionary<string, GroupRequestWindow>();

	private Dictionary<string, LeakyBucket> m_restrictSpammers = new Dictionary<string, LeakyBucket>();

	public static GroupJoinManager Get()
	{
		if (s_instance == null)
		{
			while (true)
			{
				switch (1)
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
			s_instance = new GroupJoinManager();
		}
		return s_instance;
	}

	public void Update()
	{
		DateTime utcNow = DateTime.UtcNow;
		List<GroupRequestWindow> list = new List<GroupRequestWindow>();
		foreach (GroupRequestWindow value in m_pendingWindows.Values)
		{
			if (value.HasExpired(utcNow))
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
				list.Add(value);
			}
		}
		if (_003C_003Ef__am_0024cache0 == null)
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
			_003C_003Ef__am_0024cache0 = delegate(GroupRequestWindow p)
			{
				p.CleanupWindow();
			};
		}
		list.ForEach(_003C_003Ef__am_0024cache0);
	}

	internal void AddRequest(GroupConfirmationRequest request)
	{
		if (UIFrontEnd.Get() == null)
		{
			while (true)
			{
				switch (5)
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
		if (UIFrontEnd.Get().m_landingPageScreen != null)
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
			if (UIFrontEnd.Get().m_landingPageScreen.m_inCustomGame)
			{
				SendGroupConfirmation(GroupInviteResponseType.PlayerInCustomMatch, request);
				return;
			}
		}
		if (m_pendingWindows.ContainsKey(request.LeaderFullHandle))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					SendGroupConfirmation(GroupInviteResponseType.PlayerStillAwaitingPreviousQuery, request);
					return;
				}
			}
		}
		if (request == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					throw new Exception("request is null");
				}
			}
		}
		if (m_restrictSpammers.TryGetValue(request.LeaderFullHandle, out LeakyBucket value) && value != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!value.CanAdd())
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						SendGroupConfirmation(GroupInviteResponseType.RequestorSpamming, request);
						return;
					}
				}
			}
		}
		m_pendingWindows.Add(request.LeaderFullHandle, new GroupRequestWindow(request));
	}

	internal void RemoveRequest(GroupConfirmationRequest request)
	{
		m_pendingWindows.Remove(request.LeaderFullHandle);
	}

	internal void SendGroupConfirmation(GroupInviteResponseType status, GroupConfirmationRequest request)
	{
		if (status == GroupInviteResponseType.PlayerRejected)
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
			if (!m_restrictSpammers.TryGetValue(request.LeaderFullHandle, out LeakyBucket value))
			{
				value = new LeakyBucket(2.0, TimeSpan.FromMinutes(10.0));
				m_restrictSpammers.Add(request.LeaderFullHandle, value);
			}
			value.Add();
		}
		GroupConfirmationResponse groupConfirmationResponse = new GroupConfirmationResponse();
		groupConfirmationResponse.Acceptance = status;
		groupConfirmationResponse.GroupId = request.GroupId;
		groupConfirmationResponse.ConfirmationNumber = request.ConfirmationNumber;
		groupConfirmationResponse.JoinerAccountId = request.JoinerAccountId;
		ClientGameManager.Get().LobbyInterface.SendMessage(groupConfirmationResponse);
	}
}
