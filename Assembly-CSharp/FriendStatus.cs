public enum FriendStatus
{
	Unknown,
	Friend,
	RequestSent,
	RequestReceived,
	Removed,
	Blocked,
#if EVOS
	OnlineNonFriend,
#endif
}
