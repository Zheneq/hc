using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class UIDebugDisplayPanel : MonoBehaviour
{
	public TextMeshProUGUI m_debugDisplay;

	public TextMeshProUGUI m_fpsDisplay;

	private FPS m_fpsTimer;

	private void Start()
	{
		if (this.m_fpsDisplay != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDebugDisplayPanel.Start()).MethodHandle;
			}
			this.m_fpsDisplay.gameObject.SetActive(false);
		}
		this.ClearDebugDisplay();
	}

	private void Update()
	{
		if (this.m_fpsDisplay != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDebugDisplayPanel.Update()).MethodHandle;
			}
			if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleFPS))
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
				this.m_fpsDisplay.gameObject.SetActive(!this.m_fpsDisplay.gameObject.activeSelf);
			}
			if (this.m_fpsDisplay.gameObject.activeSelf)
			{
				if (this.m_fpsTimer == null)
				{
					this.m_fpsTimer = new FPS(new Action<float>(this.OnFPSTimerChange));
				}
				this.m_fpsTimer.SampleFrame();
			}
		}
	}

	public void SetEntry(string key, string displayStr)
	{
		displayStr += "\n\n";
		if (this.m_debugDisplay.text != displayStr)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDebugDisplayPanel.SetEntry(string, string)).MethodHandle;
			}
			this.m_debugDisplay.gameObject.SetActive(true);
			this.m_debugDisplay.text = displayStr;
		}
	}

	public void ClearDebugDisplay()
	{
		if (this.m_debugDisplay != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDebugDisplayPanel.ClearDebugDisplay()).MethodHandle;
			}
			this.m_debugDisplay.gameObject.SetActive(false);
		}
	}

	private void OnFPSTimerChange(float fps)
	{
		string text = string.Format("{0:F2} FPS", fps);
		if (NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDebugDisplayPanel.OnFPSTimerChange(float)).MethodHandle;
			}
			if (!NetworkServer.active && NetworkClient.allClients != null)
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
				if (NetworkClient.allClients[0] != null)
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
					if (NetworkClient.allClients[0].connection != null)
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
						if (NetworkClient.allClients[0].isConnected)
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
							if (!NetworkClient.allClients[0].isExternalNetworkConnection)
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
								byte b;
								int currentRTT = NetworkTransport.GetCurrentRTT(NetworkClient.allClients[0].connection.hostId, NetworkClient.allClients[0].connection.connectionId, out b);
								string str = string.Format("\n\n {0} ms RTT", currentRTT);
								text += str;
							}
						}
					}
				}
			}
		}
		this.m_fpsDisplay.text = text;
		if (fps < 10f)
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
			this.m_fpsDisplay.color = Color.red;
		}
		else if (fps < 30f)
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
			this.m_fpsDisplay.color = Color.yellow;
		}
		else
		{
			this.m_fpsDisplay.color = Color.green;
		}
	}
}
