using System;
using System.Linq;
using System.Text;
using Steamworks;
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
			SteamManager result;
			if ((result = SteamManager.s_instance) == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SteamManager.get_Instance()).MethodHandle;
				}
				result = new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return result;
		}
	}

	public static bool UsingSteam
	{
		get
		{
			bool result;
			if (SteamManager.Instance.m_usingSteam && SteamManager.s_instance != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SteamManager.get_UsingSteam()).MethodHandle;
				}
				result = SteamManager.s_instance.m_bInitialized;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}

	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	private void Awake()
	{
		if (SteamManager.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SteamManager.Awake()).MethodHandle;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		this.m_usingSteam = false;
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Count<string>(); i++)
		{
			if (commandLineArgs[i].EqualsIgnoreCase("-Steam"))
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
				this.m_usingSteam = true;
				break;
			}
		}
		if (SteamManager.s_EverInialized)
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
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
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
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
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
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		if (this.m_usingSteam)
		{
			try
			{
				this.m_bInitialized = SteamAPI.Init();
			}
			catch (DllNotFoundException arg)
			{
				Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, this);
			}
			if (!this.m_bInitialized)
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
				Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
				return;
			}
			SteamManager.s_EverInialized = true;
		}
	}

	private void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SteamManager.OnEnable()).MethodHandle;
			}
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	private void OnDestroy()
	{
		if (SteamManager.s_instance != this)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SteamManager.OnDestroy()).MethodHandle;
			}
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
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
			return;
		}
		SteamAPI.Shutdown();
	}

	private void Update()
	{
		if (!this.m_bInitialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SteamManager.Update()).MethodHandle;
			}
			return;
		}
		SteamAPI.RunCallbacks();
	}
}
