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
		this.ChannelId = 0x96;
		this.ChannelName = "REACTOR";
		this.AllowFakeTickets = false;
		this.AllowRequestTickets = false;
		this.RequiredAdminEntitlement = "ADMIN_ACCESS";
		this.RequiredAgentEntitlement = "AGENT_ACCESS";
		this.RequiredLoadTestEntitlement = "LOADTEST_ACCESS";
		this.RequiredVIPEntitlement = "VIP_ACCESS";
		this.RequiredTrionEntitlement = "TRION_ACCESS";
		this.RequiredGamePurchaseEntitlement = "GAME_OWNERSHIP";
		this.AccessEntitlements = new string[]
		{
			"*",
			"TRION_ACCESS",
			"FNF_ACCESS",
			"ALPHA_ACCESS",
			"BETA_ACCESS",
			"GAME_ACCESS",
			"VIP_ACCESS"
		};
		this.FullAccessEntitlements = new string[0];
		this.Played10GamesProductCode = "AR_PLAYED_10_GAMES_CONVEYANCE";
		this.Played10GamesEntitlement = "AR_PLAYED_10_GAMES";
	}

	private static int GetHexVal(char hex)
	{
		int num;
		if (hex < ':')
		{
			num = 0x30;
		}
		else if (hex < 'a')
		{
			num = 0x37;
		}
		else
		{
			num = 0x57;
		}
		return (int)hex - num;
	}

	private static byte[] StringToByteArray(string hex)
	{
		if (hex.Length % 2 == 1)
		{
			throw new Exception("The binary key cannot have an odd number of digits");
		}
		byte[] array = new byte[hex.Length >> 1];
		for (int i = 0; i < hex.Length >> 1; i++)
		{
			array[i] = (byte)((PlatformConfig.GetHexVal(hex[i << 1]) << 4) + PlatformConfig.GetHexVal(hex[(i << 1) + 1]));
		}
		return array;
	}

	public byte[] Encrypt(byte[] plainText)
	{
		byte[] iv = new byte[0x10];
		return this.Encrypt(plainText, this.GameDataServerKey, iv);
	}

	public byte[] Encrypt(byte[] plainText, string encryptionKey, byte[] iv)
	{
		byte[] result = null;
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		try
		{
			rijndaelManaged.KeySize = 0x100;
			rijndaelManaged.BlockSize = 0x80;
			rijndaelManaged.Mode = CipherMode.CBC;
			rijndaelManaged.Padding = PaddingMode.PKCS7;
			rijndaelManaged.Key = PlatformConfig.StringToByteArray(encryptionKey);
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
							((IDisposable)binaryWriter).Dispose();
						}
					}
					result = memoryStream.ToArray();
				}
				finally
				{
					if (cryptoStream != null)
					{
						((IDisposable)cryptoStream).Dispose();
					}
				}
			}
			finally
			{
				if (memoryStream != null)
				{
					((IDisposable)memoryStream).Dispose();
				}
			}
		}
		finally
		{
			if (rijndaelManaged != null)
			{
				((IDisposable)rijndaelManaged).Dispose();
			}
		}
		return result;
	}
}
