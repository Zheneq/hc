using System;
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

	public static event Action<string> OnLoading = delegate {};

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
		if (Get() != null && Get().enabled)
		{
			return;
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.VisualSceneLoaded, null);
		ClientGameManager.Get().VisualSceneLoaded = true;
		ClientGameManager.Get().CheckAndSendClientPreparedForGameStartNotification();
	}

	public bool LevelLoaded()
	{
		return m_levelLoaded || !enabled;
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
			ClientGameManager.Get().VisualSceneLoaded = true;
			ClientGameManager.Get().CheckAndSendClientPreparedForGameStartNotification();
		}
		m_eventSent = true;
	}

	private void Start()
	{
		if ((HydrogenConfig.Get().UseTempSceneVisuals
		     || m_visualsSceneName.IsNullOrEmpty()
		     || Application.isEditor && HydrogenConfig.Get().UseTempVisualsInEditor)
		    && m_tempVisuals != null)
		{
			m_tempVisuals.SetActive(true);
			m_levelLoaded = true;
			SendSceneLoaded();
			return;
		}
		if (m_tempVisuals != null)
		{
			m_tempVisuals.SetActive(false);
		}
		DontDestroyOnLoad(gameObject);
		if (m_scriptingGameObjects != null)
		{
			foreach (GameObject obj in m_scriptingGameObjects)
			{
				DontDestroyOnLoad(obj);
			}

			GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameTeardown);
		}
		OnLoading(m_visualsSceneName);
		StartCoroutine(AssetBundleManager.Get().LoadSceneAsync(m_visualsSceneName, LoadSceneMode.Single));
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameFlowDataStarted);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameTeardown);
			s_instance = null;
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
		if (SceneManager.GetActiveScene().name.ToLower() == m_visualsSceneName.ToLower())
		{
			m_levelLoaded = true;
			SendSceneLoaded();
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GameFlowDataStarted)
		{
			m_canSendEvent = true;
			SendSceneLoaded();
		}
		else if (eventType == GameEventManager.EventType.GameTeardown && m_scriptingGameObjects != null)
		{
			foreach (GameObject obj in m_scriptingGameObjects)
			{
				Destroy(obj);
			}

			m_scriptingGameObjects = null;
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameTeardown);
		}
	}
}
