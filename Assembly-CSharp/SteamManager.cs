using Steamworks;
using System;
using System.Linq;
using System.Text;
using UnityEngine;

[DisallowMultipleComponent]
internal class SteamManager : MonoBehaviour
{
	private static SteamManager s_instance;

	private bool m_usingSteam;

	private static bool s_EverInialized;

	private bool m_bInitialized;

	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	private static SteamManager Instance
	{
		get
		{
			SteamManager steamManager = s_instance;
			if ((object)steamManager == null)
			{
				while (true)
				{
					switch (3)
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
				steamManager = new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return steamManager;
		}
	}

	public static bool UsingSteam
	{
		get
		{
			int result;
			if (Instance.m_usingSteam && s_instance != null)
			{
				while (true)
				{
					switch (1)
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
				result = (s_instance.m_bInitialized ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}

	public static bool Initialized => Instance.m_bInitialized;

	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	private void Awake()
	{
		if (s_instance != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					UnityEngine.Object.Destroy(base.gameObject);
					return;
				}
			}
		}
		s_instance = this;
		m_usingSteam = false;
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Count(); i++)
		{
			if (commandLineArgs[i].EqualsIgnoreCase("-Steam"))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				m_usingSteam = true;
				break;
			}
		}
		if (s_EverInialized)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
				}
			}
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		if (!m_usingSteam)
		{
			return;
		}
		try
		{
			m_bInitialized = SteamAPI.Init();
		}
		catch (DllNotFoundException arg)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, this);
		}
		if (!m_bInitialized)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
					return;
				}
			}
		}
		s_EverInialized = true;
	}

	private void OnEnable()
	{
		if (s_instance == null)
		{
			s_instance = this;
		}
		if (!m_bInitialized || m_SteamAPIWarningMessageHook != null)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
			SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
			return;
		}
	}

	private void OnDestroy()
	{
		if (s_instance != this)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		s_instance = null;
		if (!m_bInitialized)
		{
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		SteamAPI.Shutdown();
	}

	private void Update()
	{
		if (!m_bInitialized)
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
					return;
				}
			}
		}
		SteamAPI.RunCallbacks();
	}
}
