using System;
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

	public long ChannelId { get; set; }
	public AuthInfo AuthInfo { get; set; }
	public long AccountId => AuthInfo?.AccountId ?? 0L;
	public string UserName => AuthInfo?.UserName;
	public string Handle => AuthInfo?.Handle;
	public string TicketData => AuthInfo?.TicketData;
	public string AccountStatus => AuthInfo?.AccountStatus;
	public string AccountCurrency => AuthInfo?.AccountCurrency;
	public bool IsRealTicket => ChannelId != 0
	                            && AccountId != 0
	                            && !UserName.IsNullOrEmpty()
	                            && !Handle.IsNullOrEmpty();

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
			if (num < 0 || num > 1024)
			{
				throw new Exception("Could not parse signature for ticket");
			}
			xml = ticketData.Substring(num + 1);
		}
		else
		{
			xml = ticketData;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("authTicket/ticket");
		XmlNode xmlNode2 = xmlDocument.SelectSingleNode("authTicket/account")
		                   ?? xmlDocument.SelectSingleNode("authAccount/account");
		if (xmlNode2 == null)
		{
			throw new Exception("Could not find account node in XML");
		}
		if (xmlNode != null)
		{
			authTicket.ChannelId = xmlNode.GetChildNodeAsInt64("channelId");
		}
		authTicket.AuthInfo = new AuthInfo
		{
			Type = AuthType.Ticket,
			TicketData = ticketData
		};
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
			string username = !authTicket.AuthInfo.UserName.IsNullOrEmpty()
				? authTicket.AuthInfo.UserName
				: "???";
			if (authTicket.AuthInfo.AccountId != 0)
			{
				username += $" ({authTicket.AuthInfo.AccountId})";
			}
			throw new Exception($"Malformed account node for {username}: {ex.Message}");
		}
		XmlNodeList xmlNodeList = xmlNode2.SelectNodes("accountEntitlements/accountEntitlement");
		foreach (XmlNode xmlNode3 in xmlNodeList)
		{
			string active = Convert.ToString(xmlNode3.SelectSingleNode("accountEntitlementStatus").InnerText);
			if (active == "ACTIVE")
			{
				string channel = Convert.ToString(xmlNode3.SelectSingleNode("channel").InnerText);
				if (channelName != null && !channel.EqualsIgnoreCase(channelName))
				{
					continue;
				}

				AuthEntitlement entitlement = new AuthEntitlement
				{
					accountEntitlementId = xmlNode3.GetChildNodeAsInt64("accountEntitlementId"),
					entitlementId = xmlNode3.GetChildNodeAsInt64("entitlementId"),
					entitlementCode = xmlNode3.GetChildNodeAsString("entitlementCode"),
					entitlementAmount = xmlNode3.GetChildNodeAsInt32("entitlementAmount")
				};
				string modifiedDate = xmlNode3.GetChildNodeAsString("modifiedDate", null);
				entitlement.modifiedDate = modifiedDate.IsNullOrEmpty()
					? DateTime.MaxValue
					: DateTimeOffset.Parse(modifiedDate).UtcDateTime;
				string expirationDate = xmlNode3.GetChildNodeAsString("entitlementExpirationTime", null);
				entitlement.expirationDate = expirationDate.IsNullOrEmpty()
					? DateTime.MaxValue
					: DateTimeOffset.Parse(expirationDate).UtcDateTime;
				authTicket.AddEntitlement(entitlement);
			}
		}
		return authTicket;
	}

	public static AuthTicket Load(string path, string channelName = null)
	{
		string ticketData = File.ReadAllText(path);
		return Parse(ticketData, channelName);
	}

	public static AuthTicket ParseFakeTicket(AuthInfo authInfo)
	{
		if (authInfo.AccountId == 0
		    || authInfo.Handle.IsNullOrEmpty()
		    || authInfo.UserName.IsNullOrEmpty())
		{
			throw new Exception("Account id, handle, or user name not set in ticket");
		}
		AuthTicket authTicket = new AuthTicket
		{
			AuthInfo = authInfo
		};
		authTicket.AddFakeEntitlements(authInfo.TicketData);
		return authTicket;
	}

	public static AuthTicket CreateFakeTicket(string userName, string resourceName, int userIndex, params string[] fakeEntitlements)
	{
		AuthTicket authTicket = new AuthTicket
		{
			AuthInfo = new AuthInfo
			{
				Type = AuthType.FakeTicket
			}
		};
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
		AuthTicket authTicket = new AuthTicket
		{
			AuthInfo = new AuthInfo
			{
				Type = AuthType.RequestTicket,
				TicketData = password,
				Password = password,
				AccountId = 0L,
				UserName = userName,
				Handle = userName,
				AccountStatus = "ACTIVE",
				AccountCurrency = "USD"
			}
		};
		if (!resourceName.IsNullOrEmpty() && !userName.Contains("+"))
		{
			int userIndex = GetUserIndex(resourceName);
			if (userIndex != 0)
			{
				authTicket.AuthInfo.UserName = userName.Replace("@", $"+{userIndex}@");
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
		string handle = Handle;
		if (!handle.IsNullOrEmpty())
		{
			int num = handle.IndexOf('#');
			if (num > -1)
			{
				string username = handle.Substring(0, num);
				string poundNumber = handle.Substring(num, handle.Length - num);
				if (poundNumberFontSize > 0)
				{
					handle = username + "<size=" + poundNumberFontSize + ">" + poundNumber + "</size>";
				}
			}
		}
		return handle;
	}

	public static string GetFakeHandle(string userName)
	{
		string[] array = userName.Split('@');
		if (array.Length != 2)
		{
			throw new Exception("Could not parse email address");
		}
		int poundNumber = array[1].GetHashCode() % 900 + 100;
		if (poundNumber < 0)
		{
			poundNumber = -poundNumber;
		}
		return $"{array[0]}#{poundNumber}";
	}

	public void AddEntitlement(AuthEntitlement entitlement)
	{
		m_entitlementsByAccountEntitlementId.Add(entitlement.accountEntitlementId, entitlement);
		if (!m_entitlementsByCode.TryGetValue(entitlement.entitlementCode, out AuthEntitlement value))
		{
			value = new AuthEntitlement
			{
				accountEntitlementId = entitlement.accountEntitlementId,
				entitlementId = entitlement.entitlementId,
				entitlementCode = entitlement.entitlementCode
			};
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
		AuthEntitlement entitlement = new AuthEntitlement
		{
			accountEntitlementId = m_entitlementsByAccountEntitlementId.Count,
			entitlementId = 0L,
			entitlementCode = entitlementCode,
			entitlementAmount = 1,
			modifiedDate = DateTime.UtcNow,
			expirationDate = DateTime.MaxValue
		};
		AddEntitlement(entitlement);
	}

	public void AddFakeEntitlements(string entitlementCodes)
	{
		string[] entitlements = entitlementCodes.Split(' ', ',', ';');
		foreach (string entitlement in entitlements)
		{
			if (!entitlement.IsNullOrEmpty())
			{
				AddFakeEntitlement(entitlement);
			}
		}
	}

	public AuthEntitlement? GetEntitlement(string entitlementCode)
	{
		if (m_entitlementsByCode.TryGetValue(entitlementCode, out AuthEntitlement value))
		{
			return value;
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
			return s_userIndex;
		}
		for (int i = 0; i < 10; i++)
		{
			string name = $"Hydrogen.{resourceName}.{i}";
			s_userNameMutex = new Mutex(true, name, out bool createdNew);
			if (!createdNew || s_userNameMutex == null)
			{
				continue;
			}
			s_userIndex = i;
			return s_userIndex;
		}
		s_userIndex = -1;
		return s_userIndex;
	}
}
