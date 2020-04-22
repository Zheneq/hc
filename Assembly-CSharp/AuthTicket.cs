using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;

public class AuthTicket
{
	public const string TICKET_CORRUPT = "TICKET_CORRUPT";

	public const string INVALID_IP_ADDRESS = "INVALID_IP_ADDRESS";

	public const string INVALID_PROTOCOL_VERSION = "INVALID_PROTOCOL_VERSION";

	public const string USD = "USD";

	public const string ACTIVE = "ACTIVE";

	public const string ACCOUNT_BANNED = "ACCOUNT_BANNED";

	public Dictionary<long, AuthEntitlement> m_entitlementsByAccountEntitlementId;

	public Dictionary<string, AuthEntitlement> m_entitlementsByCode;

	private static Mutex s_userNameMutex;

	private static int s_userIndex;

	public long ChannelId
	{
		get;
		set;
	}

	public AuthInfo AuthInfo
	{
		get;
		set;
	}

	public long AccountId
	{
		get
		{
			long result;
			if (AuthInfo == null)
			{
				result = 0L;
			}
			else
			{
				result = AuthInfo.AccountId;
			}
			return result;
		}
	}

	public string UserName
	{
		get
		{
			object result;
			if (AuthInfo == null)
			{
				result = null;
			}
			else
			{
				result = AuthInfo.UserName;
			}
			return (string)result;
		}
	}

	public string Handle => (AuthInfo != null) ? AuthInfo.Handle : null;

	public string TicketData => (AuthInfo != null) ? AuthInfo.TicketData : null;

	public string AccountStatus => (AuthInfo != null) ? AuthInfo.AccountStatus : null;

	public string AccountCurrency
	{
		get
		{
			object result;
			if (AuthInfo == null)
			{
				result = null;
			}
			else
			{
				result = AuthInfo.AccountCurrency;
			}
			return (string)result;
		}
	}

	public bool IsRealTicket
	{
		get
		{
			int result;
			if (ChannelId != 0)
			{
				if (AccountId != 0 && !UserName.IsNullOrEmpty())
				{
					result = ((!Handle.IsNullOrEmpty()) ? 1 : 0);
					goto IL_0055;
				}
			}
			result = 0;
			goto IL_0055;
			IL_0055:
			return (byte)result != 0;
		}
	}

	static AuthTicket()
	{
		s_userIndex = -1;
	}

	public AuthTicket()
	{
		m_entitlementsByAccountEntitlementId = new Dictionary<long, AuthEntitlement>();
		m_entitlementsByCode = new Dictionary<string, AuthEntitlement>();
	}

	public static AuthTicket Parse(string ticketData, string channelName = null)
	{
		AuthTicket authTicket = new AuthTicket();
		string xml;
		if (ticketData.StartsWith("Signature: "))
		{
			int num = ticketData.IndexOf('\n');
			if (num >= 0)
			{
				if (num <= 1024)
				{
					xml = ticketData.Substring(num + 1);
					goto IL_006a;
				}
			}
			throw new Exception("Could not parse signature for ticket");
		}
		xml = ticketData;
		goto IL_006a;
		IL_006a:
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("authTicket/ticket");
		XmlNode xmlNode2 = xmlDocument.SelectSingleNode("authTicket/account");
		if (xmlNode2 == null)
		{
			xmlNode2 = xmlDocument.SelectSingleNode("authAccount/account");
		}
		if (xmlNode2 == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					throw new Exception("Could not find account node in XML");
				}
			}
		}
		if (xmlNode != null)
		{
			authTicket.ChannelId = xmlNode.GetChildNodeAsInt64("channelId");
		}
		authTicket.AuthInfo = new AuthInfo();
		authTicket.AuthInfo.Type = AuthType.Ticket;
		authTicket.AuthInfo.TicketData = ticketData;
		try
		{
			authTicket.AuthInfo.UserName = xmlNode2.GetChildNodeAsString("email");
			authTicket.AuthInfo.AccountId = xmlNode2.GetChildNodeAsInt64("accountId");
			authTicket.AuthInfo.Handle = xmlNode2.GetChildNodeAsString("glyphTag");
			authTicket.AuthInfo.AccountCurrency = xmlNode2.GetChildNodeAsString("accountCurrency");
			authTicket.AuthInfo.AccountStatus = xmlNode2.GetChildNodeAsString("accountStatus");
		}
		catch (Exception ex)
		{
			object obj;
			if (authTicket.AuthInfo.UserName.IsNullOrEmpty())
			{
				obj = "???";
			}
			else
			{
				obj = authTicket.AuthInfo.UserName;
			}
			string text = (string)obj;
			if (authTicket.AuthInfo.AccountId != 0)
			{
				text += $" ({authTicket.AuthInfo.AccountId})";
			}
			throw new Exception($"Malformed account node for {text}: {ex.Message}");
		}
		XmlNodeList xmlNodeList = xmlNode2.SelectNodes("accountEntitlements/accountEntitlement");
		IEnumerator enumerator = xmlNodeList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				XmlNode xmlNode3 = (XmlNode)enumerator.Current;
				string a = Convert.ToString(xmlNode3.SelectSingleNode("accountEntitlementStatus").InnerText);
				if (a != "ACTIVE")
				{
				}
				else
				{
					string lhs = Convert.ToString(xmlNode3.SelectSingleNode("channel").InnerText);
					if (channelName != null)
					{
						if (!lhs.EqualsIgnoreCase(channelName))
						{
							continue;
						}
					}
					AuthEntitlement entitlement = default(AuthEntitlement);
					entitlement.accountEntitlementId = xmlNode3.GetChildNodeAsInt64("accountEntitlementId");
					entitlement.entitlementId = xmlNode3.GetChildNodeAsInt64("entitlementId");
					entitlement.entitlementCode = xmlNode3.GetChildNodeAsString("entitlementCode");
					entitlement.entitlementAmount = xmlNode3.GetChildNodeAsInt32("entitlementAmount");
					string childNodeAsString = xmlNode3.GetChildNodeAsString("modifiedDate", null);
					if (!childNodeAsString.IsNullOrEmpty())
					{
						entitlement.modifiedDate = DateTimeOffset.Parse(childNodeAsString).UtcDateTime;
					}
					else
					{
						entitlement.modifiedDate = DateTime.MaxValue;
					}
					string childNodeAsString2 = xmlNode3.GetChildNodeAsString("entitlementExpirationTime", null);
					if (!childNodeAsString2.IsNullOrEmpty())
					{
						entitlement.expirationDate = DateTimeOffset.Parse(childNodeAsString2).UtcDateTime;
					}
					else
					{
						entitlement.expirationDate = DateTime.MaxValue;
					}
					authTicket.AddEntitlement(entitlement);
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return authTicket;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						disposable.Dispose();
						goto end_IL_040e;
					}
				}
			}
			end_IL_040e:;
		}
	}

	public static AuthTicket Load(string path, string channelName = null)
	{
		string ticketData = File.ReadAllText(path);
		return Parse(ticketData, channelName);
	}

	public static AuthTicket ParseFakeTicket(AuthInfo authInfo)
	{
		if (authInfo.AccountId != 0)
		{
			if (!authInfo.Handle.IsNullOrEmpty())
			{
				if (!authInfo.UserName.IsNullOrEmpty())
				{
					AuthTicket authTicket = new AuthTicket();
					authTicket.AuthInfo = authInfo;
					string ticketData = authInfo.TicketData;
					authTicket.AddFakeEntitlements(ticketData);
					return authTicket;
				}
			}
		}
		throw new Exception("Account id, handle, or user name not set in ticket");
	}

	public static AuthTicket CreateFakeTicket(string userName, string resourceName, int userIndex, params string[] fakeEntitlements)
	{
		AuthTicket authTicket = new AuthTicket();
		authTicket.AuthInfo = new AuthInfo();
		authTicket.AuthInfo.Type = AuthType.FakeTicket;
		if (!resourceName.IsNullOrEmpty())
		{
			userIndex = GetUserIndex(resourceName);
		}
		authTicket.AuthInfo.UserName = $"{userName}{userIndex + 1}";
		string text = authTicket.AuthInfo.UserName + NetUtil.GetHostName().ToLower();
		int num = text.GetHashCode();
		if (num < 0)
		{
			num = -num;
		}
		authTicket.AuthInfo.AccountId = num + 1000000000000000L;
		authTicket.AuthInfo.Handle = $"{authTicket.UserName}#{authTicket.AccountId % 900 + 100}";
		authTicket.AuthInfo.AccountCurrency = "USD";
		authTicket.AuthInfo.AccountStatus = "ACTIVE";
		authTicket.AuthInfo.TicketData = string.Join(",", fakeEntitlements);
		return authTicket;
	}

	public static AuthTicket CreateRequestTicket(string userName, string password, string resourceName = null)
	{
		AuthTicket authTicket = new AuthTicket();
		authTicket.AuthInfo = new AuthInfo();
		authTicket.AuthInfo.Type = AuthType.RequestTicket;
		authTicket.AuthInfo.TicketData = password;
		authTicket.AuthInfo.Password = password;
		authTicket.AuthInfo.AccountId = 0L;
		authTicket.AuthInfo.UserName = userName;
		authTicket.AuthInfo.Handle = userName;
		authTicket.AuthInfo.AccountStatus = "ACTIVE";
		authTicket.AuthInfo.AccountCurrency = "USD";
		if (!resourceName.IsNullOrEmpty())
		{
			if (!userName.Contains("+"))
			{
				int userIndex = GetUserIndex(resourceName);
				if (userIndex != 0)
				{
					authTicket.AuthInfo.UserName = userName.Replace("@", $"+{userIndex}@");
				}
			}
		}
		int num = authTicket.AuthInfo.UserName.IndexOf('@');
		if (num >= 0)
		{
			authTicket.AuthInfo.Handle = authTicket.AuthInfo.UserName.Substring(0, num);
		}
		return authTicket;
	}

	public string GetFormattedHandle(int poundNumberFontSize = -1)
	{
		string text = Handle;
		if (!text.IsNullOrEmpty())
		{
			int num = text.IndexOf('#');
			if (num > -1)
			{
				string text2 = text.Substring(0, num);
				string text3 = text.Substring(num, text.Length - num);
				if (poundNumberFontSize > 0)
				{
					text = text2 + "<size=" + poundNumberFontSize + ">" + text3 + "</size>";
				}
			}
		}
		return text;
	}

	public static string GetFakeHandle(string userName)
	{
		string[] array = userName.Split('@');
		if (array.Length == 2)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					int num = array[1].GetHashCode() % 900 + 100;
					if (num < 0)
					{
						num = -num;
					}
					return $"{array[0]}#{num}";
				}
				}
			}
		}
		throw new Exception("Could not parse email address");
	}

	public void AddEntitlement(AuthEntitlement entitlement)
	{
		m_entitlementsByAccountEntitlementId.Add(entitlement.accountEntitlementId, entitlement);
		if (!m_entitlementsByCode.TryGetValue(entitlement.entitlementCode, out AuthEntitlement value))
		{
			value = default(AuthEntitlement);
			value.accountEntitlementId = entitlement.accountEntitlementId;
			value.entitlementId = entitlement.entitlementId;
			value.entitlementCode = entitlement.entitlementCode;
		}
		value.entitlementAmount += entitlement.entitlementAmount;
		if (value.expirationDate < entitlement.expirationDate)
		{
			value.expirationDate = entitlement.expirationDate;
		}
		if (value.modifiedDate < entitlement.modifiedDate)
		{
			value.modifiedDate = entitlement.modifiedDate;
		}
		m_entitlementsByCode[value.entitlementCode] = value;
	}

	public void AddFakeEntitlement(string entitlementCode)
	{
		AuthEntitlement entitlement = default(AuthEntitlement);
		entitlement.accountEntitlementId = m_entitlementsByAccountEntitlementId.Count;
		entitlement.entitlementId = 0L;
		entitlement.entitlementCode = entitlementCode;
		entitlement.entitlementAmount = 1;
		entitlement.modifiedDate = DateTime.UtcNow;
		entitlement.expirationDate = DateTime.MaxValue;
		AddEntitlement(entitlement);
	}

	public void AddFakeEntitlements(string entitlementCodes)
	{
		string[] array = entitlementCodes.Split(' ', ',', ';');
		foreach (string text in array)
		{
			if (!text.IsNullOrEmpty())
			{
				AddFakeEntitlement(text);
			}
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public AuthEntitlement? GetEntitlement(string entitlementCode)
	{
		if (m_entitlementsByCode.TryGetValue(entitlementCode, out AuthEntitlement value))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return value;
				}
			}
		}
		return null;
	}

	public bool HasEntitlement(string entitlementCode)
	{
		return GetEntitlement(entitlementCode).HasValue;
	}

	private static int GetUserIndex(string resourceName)
	{
		if (s_userIndex != -1)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return s_userIndex;
				}
			}
		}
		for (int i = 0; i < 10; i++)
		{
			string name = $"Hydrogen.{resourceName}.{i}";
			s_userNameMutex = new Mutex(true, name, out bool createdNew);
			if (!createdNew || s_userNameMutex == null)
			{
				continue;
			}
			while (true)
			{
				s_userIndex = i;
				return s_userIndex;
			}
		}
		while (true)
		{
			s_userIndex = -1;
			return s_userIndex;
		}
	}
}
