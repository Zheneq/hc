using UnityEngine;
using UnityEngine.UI;

public class UIReplayRestartingScreen : MonoBehaviour, IGameEventListener
{
	private static UIReplayRestartingScreen s_instance;

	private int m_disableOnFrame;

	private Texture2D m_lastTexture;

	private void Start()
	{
		s_instance = this;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReplayRestart);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReplaySeekFinished);
		UIManager.SetGameObjectActive(base.gameObject, false);
	}

	private void OnDestroy()
	{
		s_instance = null;
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReplayRestart);
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReplaySeekFinished);
		if ((bool)m_lastTexture)
		{
			Object.Destroy(m_lastTexture);
		}
	}

	public static UIReplayRestartingScreen Get()
	{
		return s_instance;
	}

	public void Update()
	{
		if (Time.frameCount != m_disableOnFrame)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIManager.SetGameObjectActive(base.gameObject, false);
			return;
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		switch (eventType)
		{
		case GameEventManager.EventType.ReplayRestart:
		{
			if ((bool)m_lastTexture)
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
				Object.Destroy(m_lastTexture);
			}
			UIManager.SetGameObjectActive(base.gameObject, true);
			m_disableOnFrame = -1;
			m_lastTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			Image component = GetComponent<Image>();
			Debug.Log("---- THE FOLLOWING ERROR WORKS ANYWAY");
			m_lastTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
			Debug.Log("---- THAT'S THE END OF THE WORKING ERROR");
			m_lastTexture.Apply();
			component.sprite = Sprite.Create(m_lastTexture, new Rect(0f, 0f, m_lastTexture.width, m_lastTexture.height), Vector2.one * 0.5f);
			component.enabled = true;
			break;
		}
		case GameEventManager.EventType.ReplaySeekFinished:
			m_disableOnFrame = Time.frameCount + 2;
			break;
		}
	}
}
