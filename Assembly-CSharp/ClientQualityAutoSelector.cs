using UnityEngine;

public class ClientQualityAutoSelector : MonoBehaviour
{
	[Tooltip("Target highest quality that can support 30 FPS, testing at medium quality.")]
	public float m_fpsToLowerQuality = 27f;

	public float m_fpsToRaiseQualityCharSelect = 59f;

	private FPS m_fps;

	private GraphicsQuality m_newQuality = GraphicsQuality.Medium;

	private void Start()
	{
		if (ClientQualityComponentEnabler.OptimizeForMemory())
		{
			m_newQuality = GraphicsQuality.Low;
		}
	}

	private void Update()
	{
		AppState current = AppState.GetCurrent();
		if (current == null)
		{
			while (true)
			{
				switch (1)
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
		if (Options_UI.Get() != null)
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
			if (Options_UI.Get().GetGraphicsQualityEverSetManually())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						base.enabled = false;
						return;
					}
				}
			}
		}
		bool flag = current == AppState_GameLoading.Get();
		if (!(current == AppState_CharacterSelect.Get()))
		{
			if (!(current == AppState_GroupCharacterSelect.Get()))
			{
				if (m_fps != null && m_fps.NumSampledFrames > 15)
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
					float num = m_fps.CalcForSampledFrames();
					if (num <= m_fpsToLowerQuality)
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
						m_newQuality = GraphicsQuality.Low;
					}
					else if (num >= m_fpsToRaiseQualityCharSelect)
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
						m_newQuality = GraphicsQuality.High;
					}
					m_fps = null;
				}
				goto IL_0143;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if ((float)Application.targetFrameRate < m_fpsToRaiseQualityCharSelect)
		{
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		if (m_fps == null)
		{
			m_fps = new FPS();
		}
		m_fps.SampleFrame();
		goto IL_0143;
		IL_0143:
		if (flag && m_newQuality != GraphicsQuality.Medium)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					GraphicsQuality currentGraphicsQuality = Options_UI.Get().GetCurrentGraphicsQuality();
					if (m_newQuality != currentGraphicsQuality)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								if (currentGraphicsQuality == GraphicsQuality.Medium)
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											break;
										default:
											if (Options_UI.Get() != null)
											{
												Options_UI.Get().SetPendingGraphicsQuality(m_newQuality);
												Options_UI.Get().ApplyCurrentSettings();
											}
											return;
										}
									}
								}
								return;
							}
						}
					}
					return;
				}
				}
			}
		}
		if (!AppState.IsInGame())
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
			string text = FormatPrefKey();
			if (m_newQuality >= GraphicsQuality.Medium)
			{
				return;
			}
			if (!(Options_UI.Get() == null))
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
				if (Options_UI.Get().GetGraphicsQualityEverSetManually())
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
					break;
				}
			}
			if (text != null)
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
				if (PlayerPrefs.GetInt(text) != 0)
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
					break;
				}
			}
			UIGraphicSettingsNotification.SetVisible(true, NotificationCloseCallback);
			return;
		}
	}

	private void NotificationCloseCallback()
	{
		UIGraphicSettingsNotification.SetVisible(false, NotificationCloseCallback);
		string text = FormatPrefKey();
		if (text != null)
		{
			PlayerPrefs.SetInt(text, 1);
		}
	}

	private string FormatPrefKey()
	{
		object result;
		if (HydrogenConfig.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (HydrogenConfig.Get().Ticket != null)
			{
				result = $"{HydrogenConfig.Get().Ticket.AccountId}:ClosedAutoLowQualNotification";
				goto IL_005b;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		result = null;
		goto IL_005b;
		IL_005b:
		return (string)result;
	}
}
