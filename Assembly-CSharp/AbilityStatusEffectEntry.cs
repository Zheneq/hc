using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityStatusEffectEntry : MonoBehaviour
{
	public Image m_iconImage;

	public TextMeshProUGUI m_statusText;

	public void Setup(StatusType type)
	{
		UIManager.SetGameObjectActive(base.gameObject, true, null);
		HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType(type);
		this.m_iconImage.sprite = iconForStatusType.icon;
		this.m_statusText.text = string.Format("{2}{0}{3} - {1}", new object[]
		{
			iconForStatusType.buffName,
			iconForStatusType.buffDescription,
			"<color=#FFC000>",
			"</color>"
		});
	}
}
