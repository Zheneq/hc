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
		if (m_fpsDisplay != null)
		{
			m_fpsDisplay.gameObject.SetActive(false);
		}
		ClearDebugDisplay();
	}

	private void Update()
	{
		if (!(m_fpsDisplay != null))
		{
			return;
		}
		while (true)
		{
			if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleFPS))
			{
				m_fpsDisplay.gameObject.SetActive(!m_fpsDisplay.gameObject.activeSelf);
			}
			if (m_fpsDisplay.gameObject.activeSelf)
			{
				if (m_fpsTimer == null)
				{
					m_fpsTimer = new FPS(OnFPSTimerChange);
				}
				m_fpsTimer.SampleFrame();
			}
			return;
		}
	}

	public void SetEntry(string key, string displayStr)
	{
		displayStr += "\n\n";
		if (!(m_debugDisplay.text != displayStr))
		{
			return;
		}
		while (true)
		{
			m_debugDisplay.gameObject.SetActive(true);
			m_debugDisplay.text = displayStr;
			return;
		}
	}

	public void ClearDebugDisplay()
	{
		if (!(m_debugDisplay != null))
		{
			return;
		}
		while (true)
		{
			m_debugDisplay.gameObject.SetActive(false);
			return;
		}
	}

	private void OnFPSTimerChange(float fps)
	{
		string text = $"{fps:F2} FPS";
		if (NetworkClient.active)
		{
			if (!NetworkServer.active && NetworkClient.allClients != null)
			{
				if (NetworkClient.allClients[0] != null)
				{
					if (NetworkClient.allClients[0].connection != null)
					{
						if (NetworkClient.allClients[0].isConnected)
						{
							if (!NetworkClient.allClients[0].isExternalNetworkConnection)
							{
								byte error;
								int currentRTT = NetworkTransport.GetCurrentRTT(NetworkClient.allClients[0].connection.hostId, NetworkClient.allClients[0].connection.connectionId, out error);
								string str = $"\n\n {currentRTT} ms RTT";
								text += str;
							}
						}
					}
				}
			}
		}
		m_fpsDisplay.text = text;
		if (fps < 10f)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_fpsDisplay.color = Color.red;
					return;
				}
			}
		}
		if (fps < 30f)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_fpsDisplay.color = Color.yellow;
					return;
				}
			}
		}
		m_fpsDisplay.color = Color.green;
	}
}
