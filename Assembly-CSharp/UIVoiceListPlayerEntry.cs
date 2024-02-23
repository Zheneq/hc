using System.Text;
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
		m_discordUserId = userInfo.UserId;
		SetSpeaking(userInfo.IsSpeaking);
		for (int i = 0; i < m_playerNames.Length; i++)
		{
			m_playerNames[i].text = new StringBuilder().Append(userInfo.UserName).Append("#").Append(userInfo.Discriminator).ToString();
		}
		while (true)
		{
			return;
		}
	}

	public bool IsUser(DiscordUserInfo userInfo)
	{
		return m_discordUserId == userInfo.UserId;
	}

	public void SetSpeaking(bool isSpeaking)
	{
		UIManager.SetGameObjectActive(m_speakingIndicator, isSpeaking);
	}
}
