using System;

[Serializable]
public class LobbyGameClientProxyInfo : ICloneable
{
	public long AccountId;

	public long SessionToken;

	public long AssignmentTime;

	public string Handle;

	public LobbyGameClientProxyStatus Status;

	public bool IsHigherPriorityThan(LobbyGameClientProxyInfo other)
	{
		if (AssignmentTime > other.AssignmentTime)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return true;
				}
			}
		}
		if (AssignmentTime < other.AssignmentTime)
		{
			return false;
		}
		return AccountId < other.AccountId;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
