using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITextureSelectButton : UICharacterVisualsSelectButton
{
	public Image m_colorIcon;

	public _ButtonSwapSprite m_hitbox;

	public UIPatternData m_textureInfo;

	protected override void Start()
	{
		base.Start();
		this.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnTextureClicked);
	}

	public void OnTextureClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectOptionsChoice);
	}

	public void Setup(UIPatternData patternData)
	{
		this.m_textureInfo = patternData;
		this.m_colorIcon.color = patternData.m_buttonColor;
		UIManager.SetGameObjectActive(this.m_lockedIcon, !patternData.m_isAvailable, null);
		if (!patternData.m_isAvailable)
		{
			if (patternData.m_unlockCharacterLevel > 0)
			{
				this.m_unlockTooltipText = string.Format(StringUtil.TR("UnlockedAtCharacterLevel", "Global"), patternData.m_unlockCharacterLevel);
			}
		}
	}
}
