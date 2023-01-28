using System;

[Serializable]
public class AuthInfo
{
	public AuthType Type;
	public long AccountId;
	public string UserName;
	public string Password;
	public string Handle;
	public string TicketData;
	public string AccountStatus;
	public string AccountCurrency;
	public long SteamId;
}
