using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIVoiceListPlayerEntry : MonoBehaviour
{
	public TextMeshProUGUI[] m_playerNames;

	public Image m_banner;

	public Image m_emblem;

	public RectTransform m_speakingIndicator;

	private ulong m_discordUserId;

	public void Setup(DiscordUserInfo userInfo)
	{
		this.m_discordUserId = userInfo.UserId;
		this.SetSpeaking(userInfo.IsSpeaking);
		for (int i = 0; i < this.m_playerNames.Length; i++)
		{
			this.m_playerNames[i].text = userInfo.UserName + "#" + userInfo.Discriminator;
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIVoiceListPlayerEntry.Setup(DiscordUserInfo)).MethodHandle;
		}
	}

	public bool IsUser(DiscordUserInfo userInfo)
	{
		return this.m_discordUserId == userInfo.UserId;
	}

	public void SetSpeaking(bool isSpeaking)
	{
		UIManager.SetGameObjectActive(this.m_speakingIndicator, isSpeaking, null);
	}
}
