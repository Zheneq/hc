using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisualsLoader : MonoBehaviour, IGameEventListener
{
	public GameObject m_tempVisuals;

	public string m_visualsSceneName;

	public GameObject[] m_scriptingGameObjects;

	private static VisualsLoader s_instance;

	private bool m_levelLoaded;

	private bool m_canSendEvent;

	private bool m_eventSent;

	public static event Action<string> OnLoading
	{
		add
		{
			Action<string> action = VisualsLoader.OnLoading;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref VisualsLoader.OnLoading, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		remove
		{
			Action<string> action = VisualsLoader.OnLoading;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref VisualsLoader.OnLoading, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
	}

	static VisualsLoader()
	{
		VisualsLoader.OnLoading = delegate
		{
		};
	}

	public static VisualsLoader Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		m_canSendEvent = false;
		if (s_instance == null)
		{
			s_instance = this;
		}
		m_levelLoaded = false;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameFlowDataStarted);
	}

	public static void FireSceneLoadedEventIfNoVisualLoader()
	{
		if (!(Get() == null))
		{
			while (true)
			{
				switch (6)
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
			if (Get().enabled)
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.VisualSceneLoaded, null);
		ClientGameManager.Get().VisualSceneLoaded = true;
		ClientGameManager.Get().CheckAndSendClientPreparedForGameStartNotification();
	}

	public bool LevelLoaded()
	{
		int result;
		if (!m_levelLoaded)
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
			result = ((!base.enabled) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private void SendSceneLoaded()
	{
		if (!m_canSendEvent || !m_levelLoaded || m_eventSent)
		{
			return;
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.VisualSceneLoaded, null);
		if (ClientGameManager.Get() != null)
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
			ClientGameManager.Get().VisualSceneLoaded = true;
			ClientGameManager.Get().CheckAndSendClientPreparedForGameStartNotification();
		}
		m_eventSent = true;
	}

	private void Start()
	{
		int num;
		if (!HydrogenConfig.Get().UseTempSceneVisuals)
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
			if (!m_visualsSceneName.IsNullOrEmpty())
			{
				if (Application.isEditor)
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
					num = (HydrogenConfig.Get().UseTempVisualsInEditor ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				goto IL_0053;
			}
		}
		num = 1;
		goto IL_0053;
		IL_0053:
		if (num != 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_tempVisuals != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						m_tempVisuals.SetActive(true);
						m_levelLoaded = true;
						SendSceneLoaded();
						return;
					}
				}
			}
		}
		if (m_tempVisuals != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			m_tempVisuals.SetActive(false);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (m_scriptingGameObjects != null)
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
			for (int i = 0; i < m_scriptingGameObjects.Length; i++)
			{
				UnityEngine.Object.DontDestroyOnLoad(m_scriptingGameObjects[i]);
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameTeardown);
		}
		VisualsLoader.OnLoading(m_visualsSceneName);
		StartCoroutine(AssetBundleManager.Get().LoadSceneAsync(m_visualsSceneName, LoadSceneMode.Single));
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() == null)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameFlowDataStarted);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameTeardown);
			s_instance = null;
			return;
		}
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (!(SceneManager.GetActiveScene().name.ToLower() == m_visualsSceneName.ToLower()))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_levelLoaded = true;
			SendSceneLoaded();
			return;
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GameFlowDataStarted)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_canSendEvent = true;
					SendSceneLoaded();
					return;
				}
			}
		}
		if (eventType != GameEventManager.EventType.GameTeardown)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (m_scriptingGameObjects == null)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				for (int i = 0; i < m_scriptingGameObjects.Length; i++)
				{
					UnityEngine.Object.Destroy(m_scriptingGameObjects[i]);
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					m_scriptingGameObjects = null;
					GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameTeardown);
					return;
				}
			}
		}
	}
}
