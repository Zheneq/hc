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

	private static int s_userIndex = -1;

	public AuthTicket()
	{
		this.m_entitlementsByAccountEntitlementId = new Dictionary<long, AuthEntitlement>();
		this.m_entitlementsByCode = new Dictionary<string, AuthEntitlement>();
	}

	public long ChannelId { get; set; }

	public AuthInfo AuthInfo { get; set; }

	public long AccountId
	{
		get
		{
			long result;
			if (this.AuthInfo == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AuthTicket.get_AccountId()).MethodHandle;
				}
				result = 0L;
			}
			else
			{
				result = this.AuthInfo.AccountId;
			}
			return result;
		}
	}

	public string UserName
	{
		get
		{
			string result;
			if (this.AuthInfo == null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AuthTicket.get_UserName()).MethodHandle;
				}
				result = null;
			}
			else
			{
				result = this.AuthInfo.UserName;
			}
			return result;
		}
	}

	public string Handle
	{
		get
		{
			return (this.AuthInfo != null) ? this.AuthInfo.Handle : null;
		}
	}

	public string TicketData
	{
		get
		{
			return (this.AuthInfo != null) ? this.AuthInfo.TicketData : null;
		}
	}

	public string AccountStatus
	{
		get
		{
			return (this.AuthInfo != null) ? this.AuthInfo.AccountStatus : null;
		}
	}

	public string AccountCurrency
	{
		get
		{
			string result;
			if (this.AuthInfo == null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AuthTicket.get_AccountCurrency()).MethodHandle;
				}
				result = null;
			}
			else
			{
				result = this.AuthInfo.AccountCurrency;
			}
			return result;
		}
	}

	public bool IsRealTicket
	{
		get
		{
			if (this.ChannelId != 0L)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AuthTicket.get_IsRealTicket()).MethodHandle;
				}
				if (this.AccountId != 0L && !this.UserName.IsNullOrEmpty())
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					return !this.Handle.IsNullOrEmpty();
				}
			}
			return false;
		}
	}

	public static AuthTicket Parse(string ticketData, string channelName = null)
	{
		AuthTicket authTicket = new AuthTicket();
		string xml;
		if (ticketData.StartsWith("Signature: "))
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AuthTicket.Parse(string, string)).MethodHandle;
			}
			int num = ticketData.IndexOf('\n');
			if (num >= 0)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (num <= 0x400)
				{
					xml = ticketData.Substring(num + 1);
					goto IL_6A;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			throw new Exception("Could not parse signature for ticket");
		}
		xml = ticketData;
		IL_6A:
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("authTicket/ticket");
		XmlNode xmlNode2 = xmlDocument.SelectSingleNode("authTicket/account");
		if (xmlNode2 == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			xmlNode2 = xmlDocument.SelectSingleNode("authAccount/account");
		}
		if (xmlNode2 == null)
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
			throw new Exception("Could not find account node in XML");
		}
		if (xmlNode != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			authTicket.ChannelId = xmlNode.GetChildNodeAsInt64("channelId", null);
		}
		authTicket.AuthInfo = new AuthInfo();
		authTicket.AuthInfo.Type = AuthType.Ticket;
		authTicket.AuthInfo.TicketData = ticketData;
		try
		{
			authTicket.AuthInfo.UserName = xmlNode2.GetChildNodeAsString("email");
			authTicket.AuthInfo.AccountId = xmlNode2.GetChildNodeAsInt64("accountId", null);
			authTicket.AuthInfo.Handle = xmlNode2.GetChildNodeAsString("glyphTag");
			authTicket.AuthInfo.AccountCurrency = xmlNode2.GetChildNodeAsString("accountCurrency");
			authTicket.AuthInfo.AccountStatus = xmlNode2.GetChildNodeAsString("accountStatus");
		}
		catch (Exception ex)
		{
			string text;
			if (authTicket.AuthInfo.UserName.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				text = "???";
			}
			else
			{
				text = authTicket.AuthInfo.UserName;
			}
			string text2 = text;
			if (authTicket.AuthInfo.AccountId != 0L)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				text2 += string.Format(" ({0})", authTicket.AuthInfo.AccountId);
			}
			throw new Exception(string.Format("Malformed account node for {0}: {1}", text2, ex.Message));
		}
		XmlNodeList xmlNodeList = xmlNode2.SelectNodes("accountEntitlements/accountEntitlement");
		IEnumerator enumerator = xmlNodeList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode3 = (XmlNode)obj;
				string a = Convert.ToString(xmlNode3.SelectSingleNode("accountEntitlementStatus").InnerText);
				if (a != "ACTIVE")
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				else
				{
					string lhs = Convert.ToString(xmlNode3.SelectSingleNode("channel").InnerText);
					if (channelName != null)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!lhs.EqualsIgnoreCase(channelName))
						{
							continue;
						}
					}
					AuthEntitlement entitlement = default(AuthEntitlement);
					entitlement.accountEntitlementId = xmlNode3.GetChildNodeAsInt64("accountEntitlementId", null);
					entitlement.entitlementId = xmlNode3.GetChildNodeAsInt64("entitlementId", null);
					entitlement.entitlementCode = xmlNode3.GetChildNodeAsString("entitlementCode");
					entitlement.entitlementAmount = xmlNode3.GetChildNodeAsInt32("entitlementAmount", null);
					string childNodeAsString = xmlNode3.GetChildNodeAsString("modifiedDate", null);
					if (!childNodeAsString.IsNullOrEmpty())
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				disposable.Dispose();
			}
		}
		return authTicket;
	}

	public static AuthTicket Load(string path, string channelName = null)
	{
		string ticketData = File.ReadAllText(path);
		return AuthTicket.Parse(ticketData, channelName);
	}

	public static AuthTicket ParseFakeTicket(AuthInfo authInfo)
	{
		if (authInfo.AccountId != 0L)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AuthTicket.ParseFakeTicket(AuthInfo)).MethodHandle;
			}
			if (!authInfo.Handle.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!authInfo.UserName.IsNullOrEmpty())
				{
					AuthTicket authTicket = new AuthTicket();
					authTicket.AuthInfo = authInfo;
					string ticketData = authInfo.TicketData;
					authTicket.AddFakeEntitlements(ticketData);
					return authTicket;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AuthTicket.CreateFakeTicket(string, string, int, string[])).MethodHandle;
			}
			userIndex = AuthTicket.GetUserIndex(resourceName);
		}
		authTicket.AuthInfo.UserName = string.Format("{0}{1}", userName, userIndex + 1);
		string text = authTicket.AuthInfo.UserName + NetUtil.GetHostName().ToLower();
		int num = text.GetHashCode();
		if (num < 0)
		{
			num = -num;
		}
		authTicket.AuthInfo.AccountId = (long)num + 0x38D7EA4C68000L;
		authTicket.AuthInfo.Handle = string.Format("{0}#{1}", authTicket.UserName, authTicket.AccountId % 0x384L + 0x64L);
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AuthTicket.CreateRequestTicket(string, string, string)).MethodHandle;
			}
			if (!userName.Contains("+"))
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
				int userIndex = AuthTicket.GetUserIndex(resourceName);
				if (userIndex != 0)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					authTicket.AuthInfo.UserName = userName.Replace("@", string.Format("+{0}@", userIndex));
				}
			}
		}
		int num = authTicket.AuthInfo.UserName.IndexOf('@');
		if (num >= 0)
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
			authTicket.AuthInfo.Handle = authTicket.AuthInfo.UserName.Substring(0, num);
		}
		return authTicket;
	}

	public string GetFormattedHandle(int poundNumberFontSize = -1)
	{
		string text = this.Handle;
		if (!text.IsNullOrEmpty())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AuthTicket.GetFormattedHandle(int)).MethodHandle;
			}
			int num = text.IndexOf('#');
			if (num > -1)
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
				string text2 = text.Substring(0, num);
				string text3 = text.Substring(num, text.Length - num);
				if (poundNumberFontSize > 0)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					text = string.Concat(new object[]
					{
						text2,
						"<size=",
						poundNumberFontSize,
						">",
						text3,
						"</size>"
					});
				}
			}
		}
		return text;
	}

	public static string GetFakeHandle(string userName)
	{
		string[] array = userName.Split(new char[]
		{
			'@'
		});
		if (array.Length == 2)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AuthTicket.GetFakeHandle(string)).MethodHandle;
			}
			int num = array[1].GetHashCode() % 0x384 + 0x64;
			if (num < 0)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				num = -num;
			}
			return string.Format("{0}#{1}", array[0], num);
		}
		throw new Exception("Could not parse email address");
	}

	public void AddEntitlement(AuthEntitlement entitlement)
	{
		this.m_entitlementsByAccountEntitlementId.Add(entitlement.accountEntitlementId, entitlement);
		AuthEntitlement value;
		if (!this.m_entitlementsByCode.TryGetValue(entitlement.entitlementCode, out value))
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AuthTicket.AddEntitlement(AuthEntitlement)).MethodHandle;
			}
			value = default(AuthEntitlement);
			value.accountEntitlementId = entitlement.accountEntitlementId;
			value.entitlementId = entitlement.entitlementId;
			value.entitlementCode = entitlement.entitlementCode;
		}
		value.entitlementAmount += entitlement.entitlementAmount;
		if (value.expirationDate < entitlement.expirationDate)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			value.expirationDate = entitlement.expirationDate;
		}
		if (value.modifiedDate < entitlement.modifiedDate)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			value.modifiedDate = entitlement.modifiedDate;
		}
		this.m_entitlementsByCode[value.entitlementCode] = value;
	}

	public void AddFakeEntitlement(string entitlementCode)
	{
		this.AddEntitlement(new AuthEntitlement
		{
			accountEntitlementId = (long)this.m_entitlementsByAccountEntitlementId.Count,
			entitlementId = 0L,
			entitlementCode = entitlementCode,
			entitlementAmount = 1,
			modifiedDate = DateTime.UtcNow,
			expirationDate = DateTime.MaxValue
		});
	}

	public void AddFakeEntitlements(string entitlementCodes)
	{
		foreach (string text in entitlementCodes.Split(new char[]
		{
			' ',
			',',
			';'
		}))
		{
			if (!text.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AuthTicket.AddFakeEntitlements(string)).MethodHandle;
				}
				this.AddFakeEntitlement(text);
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	public AuthEntitlement? GetEntitlement(string entitlementCode)
	{
		AuthEntitlement value;
		if (this.m_entitlementsByCode.TryGetValue(entitlementCode, out value))
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AuthTicket.GetEntitlement(string)).MethodHandle;
			}
			return new AuthEntitlement?(value);
		}
		return null;
	}

	public bool HasEntitlement(string entitlementCode)
	{
		return this.GetEntitlement(entitlementCode) != null;
	}

	private static int GetUserIndex(string resourceName)
	{
		if (AuthTicket.s_userIndex != -1)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AuthTicket.GetUserIndex(string)).MethodHandle;
			}
			return AuthTicket.s_userIndex;
		}
		for (int i = 0; i < 0xA; i++)
		{
			string name = string.Format("Hydrogen.{0}.{1}", resourceName, i);
			bool flag;
			AuthTicket.s_userNameMutex = new Mutex(true, name, ref flag);
			if (flag && AuthTicket.s_userNameMutex != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				AuthTicket.s_userIndex = i;
				return AuthTicket.s_userIndex;
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		AuthTicket.s_userIndex = -1;
		return AuthTicket.s_userIndex;
	}
}
