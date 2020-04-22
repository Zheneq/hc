using System;
using System.Collections.Generic;

[Serializable]
public class FriendList
{
	public Dictionary<long, FriendInfo> Friends = new Dictionary<long, FriendInfo>();

	public bool IsDelta;

	public bool IsError;
}
