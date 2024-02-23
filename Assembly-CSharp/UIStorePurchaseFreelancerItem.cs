using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStorePurchaseFreelancerItem : MonoBehaviour
{
	public TextMeshProUGUI m_nameText;

	public TextMeshProUGUI m_collectionProgressText;

	public Image m_collectionProgressBar;

	public RectTransform m_ownedCompleteContainer;

	public Image m_icon;

	public _ButtonSwapSprite m_hitBox;

	private CharacterResourceLink m_charLinkRef;

	public CharacterResourceLink GetCharLink()
	{
		return m_charLinkRef;
	}

	public void Setup(CharacterResourceLink charLink, int currentProgress, int totalProgress)
	{
		m_charLinkRef = charLink;
		if (charLink != null)
		{
			m_hitBox.SetClickable(true);
			m_hitBox.callback = FreelancerSelected;
			m_nameText.text = charLink.GetDisplayName();
			if (totalProgress > 0)
			{
				m_collectionProgressBar.fillAmount = (float)currentProgress / (float)totalProgress;
				m_collectionProgressText.text = new StringBuilder().Append(currentProgress).Append("/").Append(totalProgress).ToString();
				UIManager.SetGameObjectActive(m_ownedCompleteContainer, currentProgress == totalProgress);
			}
			else
			{
				m_collectionProgressBar.fillAmount = 0f;
				m_collectionProgressText.text = string.Empty;
				UIManager.SetGameObjectActive(m_ownedCompleteContainer, false);
			}
			m_icon.sprite = Resources.Load<Sprite>(charLink.m_characterIconResourceString);
			UIManager.SetGameObjectActive(m_nameText, true);
			UIManager.SetGameObjectActive(m_collectionProgressBar, true);
			UIManager.SetGameObjectActive(m_collectionProgressText, true);
			UIManager.SetGameObjectActive(m_icon, true);
		}
		else
		{
			UIManager.SetGameObjectActive(m_nameText, false);
			UIManager.SetGameObjectActive(m_collectionProgressBar, false);
			UIManager.SetGameObjectActive(m_collectionProgressText, false);
			UIManager.SetGameObjectActive(m_ownedCompleteContainer, false);
			UIManager.SetGameObjectActive(m_icon, false);
			m_hitBox.SetClickable(false);
		}
	}

	public void FreelancerSelected(BaseEventData data)
	{
		m_hitBox.ResetMouseState();
		if (!(m_charLinkRef != null))
		{
			return;
		}
		while (true)
		{
			UIStorePanel.Get().m_freelancerPanel.FreeLancerClicked(this);
			return;
		}
	}
}
