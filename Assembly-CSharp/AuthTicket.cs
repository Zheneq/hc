using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
	public long AccountId
	{
		get { return AuthInfo != null ? AuthInfo.AccountId : 0L; }
	}

	public string UserName
	{
		get { return AuthInfo != null ? AuthInfo.UserName : null; }
	}

	public string Handle
	{
		get { return AuthInfo != null ? AuthInfo.Handle : null; }
	}

	public string TicketData
	{
		get { return AuthInfo != null ? AuthInfo.TicketData : null; }
	}

	public string AccountStatus
	{
		get { return AuthInfo != null ? AuthInfo.AccountStatus : null; }
	}

	public string AccountCurrency
	{
		get { return AuthInfo != null ? AuthInfo.AccountCurrency : null; }
	}

	public bool IsRealTicket
	{
		get
		{
			return ChannelId != 0
			       && AccountId != 0
			       && !UserName.IsNullOrEmpty()
			       && !Handle.IsNullOrEmpty();
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
		XmlNode nodeTicket = xmlDocument.SelectSingleNode("authTicket/ticket");
		XmlNode nodeAccount = xmlDocument.SelectSingleNode("authTicket/account") != null ? xmlDocument.SelectSingleNode("authTicket/account") : xmlDocument.SelectSingleNode("authAccount/account");
		if (nodeAccount == null)
		{
			throw new Exception("Could not find account node in XML");
		}
		if (nodeTicket != null)
		{
			authTicket.ChannelId = nodeTicket.GetChildNodeAsInt64("channelId");
		}
		authTicket.AuthInfo = new AuthInfo
		{
			Type = AuthType.Ticket,
			TicketData = ticketData
		};
		try
		{
			authTicket.AuthInfo.UserName = nodeAccount.GetChildNodeAsString("email");
			authTicket.AuthInfo.AccountId = nodeAccount.GetChildNodeAsInt64("accountId");
			authTicket.AuthInfo.Handle = nodeAccount.GetChildNodeAsString("glyphTag");
			authTicket.AuthInfo.AccountCurrency = nodeAccount.GetChildNodeAsString("accountCurrency");
			authTicket.AuthInfo.AccountStatus = nodeAccount.GetChildNodeAsString("accountStatus");
		}
		catch (Exception ex)
		{
			string username = !authTicket.AuthInfo.UserName.IsNullOrEmpty()
				? authTicket.AuthInfo.UserName
				: "???";
			if (authTicket.AuthInfo.AccountId != 0)
			{
				username += new StringBuilder().Append(" (").Append(authTicket.AuthInfo.AccountId).Append(")").ToString();
			}
			throw new Exception(new StringBuilder().Append("Malformed account node for ").Append(username).Append(": ").Append(ex.Message).ToString());
		}
		XmlNodeList nodeEntitlements = nodeAccount.SelectNodes("accountEntitlements/accountEntitlement");
		foreach (XmlNode nodeEntitlement in nodeEntitlements)
		{
			string active = Convert.ToString(nodeEntitlement.SelectSingleNode("accountEntitlementStatus").InnerText);
			if (active == "ACTIVE")
			{
				string channel = Convert.ToString(nodeEntitlement.SelectSingleNode("channel").InnerText);
				if (channelName != null && !channel.EqualsIgnoreCase(channelName))
				{
					continue;
				}

				AuthEntitlement entitlement = new AuthEntitlement
				{
					accountEntitlementId = nodeEntitlement.GetChildNodeAsInt64("accountEntitlementId"),
					entitlementId = nodeEntitlement.GetChildNodeAsInt64("entitlementId"),
					entitlementCode = nodeEntitlement.GetChildNodeAsString("entitlementCode"),
					entitlementAmount = nodeEntitlement.GetChildNodeAsInt32("entitlementAmount")
				};
				string modifiedDate = nodeEntitlement.GetChildNodeAsString("modifiedDate", null);
				entitlement.modifiedDate = modifiedDate.IsNullOrEmpty()
					? DateTime.MaxValue
					: DateTimeOffset.Parse(modifiedDate).UtcDateTime;
				string expirationDate = nodeEntitlement.GetChildNodeAsString("entitlementExpirationTime", null);
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

		authTicket.AuthInfo.UserName = new StringBuilder().Append(userName).Append(userIndex + 1).ToString();
		string text = new StringBuilder().Append(authTicket.AuthInfo.UserName).Append(NetUtil.GetHostName().ToLower()).ToString();
		int num = text.GetHashCode();
		if (num < 0)
		{
			num = -num;
		}
		authTicket.AuthInfo.AccountId = num + 1000000000000000L;
		authTicket.AuthInfo.Handle = new StringBuilder().Append(authTicket.UserName).Append("#").Append(authTicket.AccountId % 900 + 100).ToString();
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
				authTicket.AuthInfo.UserName = userName.Replace("@", new StringBuilder().Append("+").Append(userIndex).Append("@").ToString());
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
					handle = new StringBuilder().Append(username).Append("<size=").Append(poundNumberFontSize).Append(">").Append(poundNumber).Append("</size>").ToString();
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
		return new StringBuilder().Append(array[0]).Append("#").Append(poundNumber).ToString();
	}

	public void AddEntitlement(AuthEntitlement entitlement)
	{
		m_entitlementsByAccountEntitlementId.Add(entitlement.accountEntitlementId, entitlement);
		AuthEntitlement authEntitlement;
		if (!m_entitlementsByCode.TryGetValue(entitlement.entitlementCode, out authEntitlement))
		{
			authEntitlement = new AuthEntitlement
			{
				accountEntitlementId = entitlement.accountEntitlementId,
				entitlementId = entitlement.entitlementId,
				entitlementCode = entitlement.entitlementCode
			};
		}
		authEntitlement.entitlementAmount += entitlement.entitlementAmount;
		if (authEntitlement.expirationDate < entitlement.expirationDate)
		{
			authEntitlement.expirationDate = entitlement.expirationDate;
		}
		if (authEntitlement.modifiedDate < entitlement.modifiedDate)
		{
			authEntitlement.modifiedDate = entitlement.modifiedDate;
		}
		m_entitlementsByCode[authEntitlement.entitlementCode] = authEntitlement;
	}

	public void AddFakeEntitlement(string entitlementCode)
	{
		AddEntitlement(new AuthEntitlement
		{
			accountEntitlementId = m_entitlementsByAccountEntitlementId.Count,
			entitlementId = 0L,
			entitlementCode = entitlementCode,
			entitlementAmount = 1,
			modifiedDate = DateTime.UtcNow,
			expirationDate = DateTime.MaxValue
		});
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
		AuthEntitlement value;
		if (m_entitlementsByCode.TryGetValue(entitlementCode, out value))
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
			string name = new StringBuilder().Append("Hydrogen.").Append(resourceName).Append(".").Append(i).ToString();
			bool createdNew;
			s_userNameMutex = new Mutex(true, name, out createdNew);
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
