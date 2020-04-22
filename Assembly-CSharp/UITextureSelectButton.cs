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
		m_hitbox.callback = OnTextureClicked;
	}

	public void OnTextureClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectOptionsChoice);
	}

	public void Setup(UIPatternData patternData)
	{
		m_textureInfo = patternData;
		m_colorIcon.color = patternData.m_buttonColor;
		UIManager.SetGameObjectActive(m_lockedIcon, !patternData.m_isAvailable);
		if (patternData.m_isAvailable)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (patternData.m_unlockCharacterLevel > 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					m_unlockTooltipText = string.Format(StringUtil.TR("UnlockedAtCharacterLevel", "Global"), patternData.m_unlockCharacterLevel);
					return;
				}
			}
			return;
		}
	}
}
