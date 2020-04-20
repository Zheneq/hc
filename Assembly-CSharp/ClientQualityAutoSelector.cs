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
			return;
		}
		if (Options_UI.Get() != null)
		{
			if (Options_UI.Get().GetGraphicsQualityEverSetManually())
			{
				base.enabled = false;
				return;
			}
		}
		bool flag = current == AppState_GameLoading.Get();
		if (!(current == AppState_CharacterSelect.Get()))
		{
			if (current == AppState_GroupCharacterSelect.Get())
			{
			}
			else
			{
				if (this.m_fps != null && this.m_fps.NumSampledFrames > 0xF)
				{
					float num = this.m_fps.CalcForSampledFrames();
					if (num <= this.m_fpsToLowerQuality)
					{
						this.m_newQuality = GraphicsQuality.Low;
					}
					else if (num >= this.m_fpsToRaiseQualityCharSelect)
					{
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
			GraphicsQuality currentGraphicsQuality = Options_UI.Get().GetCurrentGraphicsQuality();
			if (this.m_newQuality != currentGraphicsQuality)
			{
				if (currentGraphicsQuality == GraphicsQuality.Medium)
				{
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
			string text = this.FormatPrefKey();
			if (this.m_newQuality < GraphicsQuality.Medium)
			{
				if (!(Options_UI.Get() == null))
				{
					if (Options_UI.Get().GetGraphicsQualityEverSetManually())
					{
						return;
					}
				}
				if (text != null)
				{
					if (PlayerPrefs.GetInt(text) != 0)
					{
						return;
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
			if (HydrogenConfig.Get().Ticket != null)
			{
				return string.Format("{0}:ClosedAutoLowQualNotification", HydrogenConfig.Get().Ticket.AccountId);
			}
		}
		return null;
	}
}
