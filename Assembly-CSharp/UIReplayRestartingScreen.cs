using System;
using UnityEngine;
using UnityEngine.UI;

public class UIReplayRestartingScreen : MonoBehaviour, IGameEventListener
{
	private static UIReplayRestartingScreen s_instance;

	private int m_disableOnFrame;

	private Texture2D m_lastTexture;

	private void Start()
	{
		UIReplayRestartingScreen.s_instance = this;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReplayRestart);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReplaySeekFinished);
		UIManager.SetGameObjectActive(base.gameObject, false, null);
	}

	private void OnDestroy()
	{
		UIReplayRestartingScreen.s_instance = null;
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReplayRestart);
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReplaySeekFinished);
		if (this.m_lastTexture)
		{
			UnityEngine.Object.Destroy(this.m_lastTexture);
		}
	}

	public static UIReplayRestartingScreen Get()
	{
		return UIReplayRestartingScreen.s_instance;
	}

	public void Update()
	{
		if (Time.frameCount == this.m_disableOnFrame)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIReplayRestartingScreen.Update()).MethodHandle;
			}
			UIManager.SetGameObjectActive(base.gameObject, false, null);
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.ReplayRestart)
		{
			if (this.m_lastTexture)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIReplayRestartingScreen.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
				}
				UnityEngine.Object.Destroy(this.m_lastTexture);
			}
			UIManager.SetGameObjectActive(base.gameObject, true, null);
			this.m_disableOnFrame = -1;
			this.m_lastTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			Image component = base.GetComponent<Image>();
			Debug.Log("---- THE FOLLOWING ERROR WORKS ANYWAY");
			this.m_lastTexture.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0);
			Debug.Log("---- THAT'S THE END OF THE WORKING ERROR");
			this.m_lastTexture.Apply();
			component.sprite = Sprite.Create(this.m_lastTexture, new Rect(0f, 0f, (float)this.m_lastTexture.width, (float)this.m_lastTexture.height), Vector2.one * 0.5f);
			component.enabled = true;
		}
		else if (eventType == GameEventManager.EventType.ReplaySeekFinished)
		{
			this.m_disableOnFrame = Time.frameCount + 2;
		}
	}
}
