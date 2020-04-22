using System;
using System.IO;
using System.Security.Cryptography;

[Serializable]
public class PlatformConfig
{
	public int ChannelId;

	public string ChannelName;

	public string AuthServerUrl;

	public string GameDataServerUrl;

	[SensitiveData]
	public string GameDataServerKey;

	[SensitiveData]
	public string CommerceClientKey;

	public string CommerceClientAPIServerUrl;

	public string CommerceClientAPIClientUrl;

	public string CallbackServerUrl;

	public bool AllowFakeTickets;

	public bool AllowRequestTickets;

	public string RequiredAdminEntitlement;

	public string RequiredAgentEntitlement;

	public string RequiredLoadTestEntitlement;

	public string RequiredVIPEntitlement;

	public string RequiredTrionEntitlement;

	public string RequiredGamePurchaseEntitlement;

	public string[] AccessEntitlements;

	public string[] FullAccessEntitlements;

	public string Played10GamesProductCode;

	public string Played10GamesEntitlement;

	public string LoadTestPasswordPattern;

	[SensitiveData]
	public string LoadTestPassword;

	public PlatformConfig()
	{
		ChannelId = 150;
		ChannelName = "REACTOR";
		AllowFakeTickets = false;
		AllowRequestTickets = false;
		RequiredAdminEntitlement = "ADMIN_ACCESS";
		RequiredAgentEntitlement = "AGENT_ACCESS";
		RequiredLoadTestEntitlement = "LOADTEST_ACCESS";
		RequiredVIPEntitlement = "VIP_ACCESS";
		RequiredTrionEntitlement = "TRION_ACCESS";
		RequiredGamePurchaseEntitlement = "GAME_OWNERSHIP";
		AccessEntitlements = new string[7]
		{
			"*",
			"TRION_ACCESS",
			"FNF_ACCESS",
			"ALPHA_ACCESS",
			"BETA_ACCESS",
			"GAME_ACCESS",
			"VIP_ACCESS"
		};
		FullAccessEntitlements = new string[0];
		Played10GamesProductCode = "AR_PLAYED_10_GAMES_CONVEYANCE";
		Played10GamesEntitlement = "AR_PLAYED_10_GAMES";
	}

	private static int GetHexVal(char hex)
	{
		int num;
		if (hex < ':')
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
			num = 48;
		}
		else if (hex < 'a')
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
			num = 55;
		}
		else
		{
			num = 87;
		}
		return hex - num;
	}

	private static byte[] StringToByteArray(string hex)
	{
		if (hex.Length % 2 == 1)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					throw new Exception("The binary key cannot have an odd number of digits");
				}
			}
		}
		byte[] array = new byte[hex.Length >> 1];
		for (int i = 0; i < hex.Length >> 1; i++)
		{
			array[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + GetHexVal(hex[(i << 1) + 1]));
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			return array;
		}
	}

	public byte[] Encrypt(byte[] plainText)
	{
		byte[] iv = new byte[16];
		return Encrypt(plainText, GameDataServerKey, iv);
	}

	public byte[] Encrypt(byte[] plainText, string encryptionKey, byte[] iv)
	{
		byte[] array = null;
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		try
		{
			rijndaelManaged.KeySize = 256;
			rijndaelManaged.BlockSize = 128;
			rijndaelManaged.Mode = CipherMode.CBC;
			rijndaelManaged.Padding = PaddingMode.PKCS7;
			rijndaelManaged.Key = StringToByteArray(encryptionKey);
			rijndaelManaged.IV = iv;
			ICryptoTransform transform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
				try
				{
					BinaryWriter binaryWriter = new BinaryWriter(cryptoStream);
					try
					{
						binaryWriter.Write(plainText);
					}
					finally
					{
						if (binaryWriter != null)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									if (1 == 0)
									{
										/*OpCode not supported: LdMemberToken*/;
									}
									((IDisposable)binaryWriter).Dispose();
									goto end_IL_007d;
								}
							}
						}
						end_IL_007d:;
					}
					return memoryStream.ToArray();
				}
				finally
				{
					if (cryptoStream != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								((IDisposable)cryptoStream).Dispose();
								goto end_IL_00a7;
							}
						}
					}
					end_IL_00a7:;
				}
			}
			finally
			{
				if (memoryStream != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							((IDisposable)memoryStream).Dispose();
							goto end_IL_00bf;
						}
					}
				}
				end_IL_00bf:;
			}
		}
		finally
		{
			if (rijndaelManaged != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						((IDisposable)rijndaelManaged).Dispose();
						goto end_IL_00d5;
					}
				}
			}
			end_IL_00d5:;
		}
	}
}
