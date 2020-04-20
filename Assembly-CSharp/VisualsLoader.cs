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

	// Note: this type is marked as 'beforefieldinit'.
	static VisualsLoader()
	{
		VisualsLoader.OnLoading = delegate(string A_0)
		{
		};
	}

	public static VisualsLoader Get()
	{
		return VisualsLoader.s_instance;
	}

	public static event Action<string> OnLoading
	{
		add
		{
			Action<string> action = VisualsLoader.OnLoading;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<string>>(ref VisualsLoader.OnLoading, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = VisualsLoader.OnLoading;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<string>>(ref VisualsLoader.OnLoading, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Awake()
	{
		this.m_canSendEvent = false;
		if (VisualsLoader.s_instance == null)
		{
			VisualsLoader.s_instance = this;
		}
		this.m_levelLoaded = false;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameFlowDataStarted);
	}

	public static void FireSceneLoadedEventIfNoVisualLoader()
	{
		if (!(VisualsLoader.Get() == null))
		{
			if (VisualsLoader.Get().enabled)
			{
				return;
			}
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.VisualSceneLoaded, null);
		ClientGameManager.Get().VisualSceneLoaded = true;
		ClientGameManager.Get().CheckAndSendClientPreparedForGameStartNotification();
	}

	public bool LevelLoaded()
	{
		bool result;
		if (!this.m_levelLoaded)
		{
			result = !base.enabled;
		}
		else
		{
			result = true;
		}
		return result;
	}

	private void SendSceneLoaded()
	{
		if (this.m_canSendEvent && this.m_levelLoaded && !this.m_eventSent)
		{
			GameEventManager.Get().FireEvent(GameEventManager.EventType.VisualSceneLoaded, null);
			if (ClientGameManager.Get() != null)
			{
				ClientGameManager.Get().VisualSceneLoaded = true;
				ClientGameManager.Get().CheckAndSendClientPreparedForGameStartNotification();
			}
			this.m_eventSent = true;
		}
	}

	private void Start()
	{
		bool flag;
		if (!HydrogenConfig.Get().UseTempSceneVisuals)
		{
			if (!this.m_visualsSceneName.IsNullOrEmpty())
			{
				if (Application.isEditor)
				{
					flag = HydrogenConfig.Get().UseTempVisualsInEditor;
				}
				else
				{
					flag = false;
				}
				goto IL_53;
			}
		}
		flag = true;
		IL_53:
		bool flag2 = flag;
		if (flag2)
		{
			if (this.m_tempVisuals != null)
			{
				this.m_tempVisuals.SetActive(true);
				this.m_levelLoaded = true;
				this.SendSceneLoaded();
				return;
			}
		}
		if (this.m_tempVisuals != null)
		{
			this.m_tempVisuals.SetActive(false);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (this.m_scriptingGameObjects != null)
		{
			for (int i = 0; i < this.m_scriptingGameObjects.Length; i++)
			{
				UnityEngine.Object.DontDestroyOnLoad(this.m_scriptingGameObjects[i]);
			}
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameTeardown);
		}
		VisualsLoader.OnLoading(this.m_visualsSceneName);
		base.StartCoroutine(AssetBundleManager.Get().LoadSceneAsync(this.m_visualsSceneName, LoadSceneMode.Single));
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameFlowDataStarted);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameTeardown);
			VisualsLoader.s_instance = null;
		}
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (SceneManager.GetActiveScene().name.ToLower() == this.m_visualsSceneName.ToLower())
		{
			this.m_levelLoaded = true;
			this.SendSceneLoaded();
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GameFlowDataStarted)
		{
			this.m_canSendEvent = true;
			this.SendSceneLoaded();
		}
		else if (eventType == GameEventManager.EventType.GameTeardown)
		{
			if (this.m_scriptingGameObjects != null)
			{
				for (int i = 0; i < this.m_scriptingGameObjects.Length; i++)
				{
					UnityEngine.Object.Destroy(this.m_scriptingGameObjects[i]);
				}
				this.m_scriptingGameObjects = null;
				GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameTeardown);
			}
		}
	}
}
