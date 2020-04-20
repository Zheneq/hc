using System;
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
		return this.m_charLinkRef;
	}

	public void Setup(CharacterResourceLink charLink, int currentProgress, int totalProgress)
	{
		this.m_charLinkRef = charLink;
		if (charLink != null)
		{
			this.m_hitBox.SetClickable(true);
			this.m_hitBox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.FreelancerSelected);
			this.m_nameText.text = charLink.GetDisplayName();
			if (totalProgress > 0)
			{
				this.m_collectionProgressBar.fillAmount = (float)currentProgress / (float)totalProgress;
				this.m_collectionProgressText.text = currentProgress + "/" + totalProgress;
				UIManager.SetGameObjectActive(this.m_ownedCompleteContainer, currentProgress == totalProgress, null);
			}
			else
			{
				this.m_collectionProgressBar.fillAmount = 0f;
				this.m_collectionProgressText.text = string.Empty;
				UIManager.SetGameObjectActive(this.m_ownedCompleteContainer, false, null);
			}
			this.m_icon.sprite = Resources.Load<Sprite>(charLink.m_characterIconResourceString);
			UIManager.SetGameObjectActive(this.m_nameText, true, null);
			UIManager.SetGameObjectActive(this.m_collectionProgressBar, true, null);
			UIManager.SetGameObjectActive(this.m_collectionProgressText, true, null);
			UIManager.SetGameObjectActive(this.m_icon, true, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_nameText, false, null);
			UIManager.SetGameObjectActive(this.m_collectionProgressBar, false, null);
			UIManager.SetGameObjectActive(this.m_collectionProgressText, false, null);
			UIManager.SetGameObjectActive(this.m_ownedCompleteContainer, false, null);
			UIManager.SetGameObjectActive(this.m_icon, false, null);
			this.m_hitBox.SetClickable(false);
		}
	}

	public void FreelancerSelected(BaseEventData data)
	{
		this.m_hitBox.ResetMouseState();
		if (this.m_charLinkRef != null)
		{
			UIStorePanel.Get().m_freelancerPanel.FreeLancerClicked(this);
		}
	}
}
