using System;
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
			this.m_newQuality = GraphicsQuality.Low;
		}
	}

	private void Update()
	{
		AppState current = AppState.GetCurrent();
		if (current == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientQualityAutoSelector.Update()).MethodHandle;
			}
			return;
		}
		if (Options_UI.Get() != null)
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
			if (Options_UI.Get().GetGraphicsQualityEverSetManually())
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
				base.enabled = false;
				return;
			}
		}
		bool flag = current == AppState_GameLoading.Get();
		if (!(current == AppState_CharacterSelect.Get()))
		{
			if (current == AppState_GroupCharacterSelect.Get())
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
			}
			else
			{
				if (this.m_fps != null && this.m_fps.NumSampledFrames > 0xF)
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
					float num = this.m_fps.CalcForSampledFrames();
					if (num <= this.m_fpsToLowerQuality)
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
						this.m_newQuality = GraphicsQuality.Low;
					}
					else if (num >= this.m_fpsToRaiseQualityCharSelect)
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
						this.m_newQuality = GraphicsQuality.High;
					}
					this.m_fps = null;
					goto IL_143;
				}
				goto IL_143;
			}
		}
		if ((float)Application.targetFrameRate < this.m_fpsToRaiseQualityCharSelect)
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
			return;
		}
		if (this.m_fps == null)
		{
			this.m_fps = new FPS();
		}
		this.m_fps.SampleFrame();
		IL_143:
		if (flag && this.m_newQuality != GraphicsQuality.Medium)
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
			GraphicsQuality currentGraphicsQuality = Options_UI.Get().GetCurrentGraphicsQuality();
			if (this.m_newQuality != currentGraphicsQuality)
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
				if (currentGraphicsQuality == GraphicsQuality.Medium)
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
					if (Options_UI.Get() != null)
					{
						Options_UI.Get().SetPendingGraphicsQuality(this.m_newQuality);
						Options_UI.Get().ApplyCurrentSettings();
					}
				}
			}
		}
		else if (AppState.IsInGame())
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
			string text = this.FormatPrefKey();
			if (this.m_newQuality < GraphicsQuality.Medium)
			{
				if (!(Options_UI.Get() == null))
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
					if (Options_UI.Get().GetGraphicsQualityEverSetManually())
					{
						return;
					}
					for (;;)
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
					for (;;)
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
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				UIGraphicSettingsNotification.SetVisible(true, new UIGraphicSettingsNotification.CloseCallback(this.NotificationCloseCallback));
			}
		}
	}

	private void NotificationCloseCallback()
	{
		UIGraphicSettingsNotification.SetVisible(false, new UIGraphicSettingsNotification.CloseCallback(this.NotificationCloseCallback));
		string text = this.FormatPrefKey();
		if (text != null)
		{
			PlayerPrefs.SetInt(text, 1);
		}
	}

	private string FormatPrefKey()
	{
		if (HydrogenConfig.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientQualityAutoSelector.FormatPrefKey()).MethodHandle;
			}
			if (HydrogenConfig.Get().Ticket != null)
			{
				return string.Format("{0}:ClosedAutoLowQualNotification", HydrogenConfig.Get().Ticket.AccountId);
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return null;
	}
}
