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
		if (this.AssignmentTime > other.AssignmentTime)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyGameClientProxyInfo.IsHigherPriorityThan(LobbyGameClientProxyInfo)).MethodHandle;
			}
			return true;
		}
		return this.AssignmentTime >= other.AssignmentTime && this.AccountId < other.AccountId;
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
