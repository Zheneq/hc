using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStoreAccountPanel : UIStoreBasePanel
{
	public StorePanelData[] m_panels;

	public UIStoreSideNavButton m_defaultPanelBtn;

	public TextMeshProUGUI m_totalTotalText;

	public TextMeshProUGUI m_totalOwnedText;

	public ImageFilledSloped m_ownedBar;

	public RectTransform m_ownedCompleteContainer;

	public Image m_banner;

	public Image m_emblem;

	private bool m_disableInitialSelectPanel;

	private void Start()
	{
		StorePanelData[] panels = m_panels;
		foreach (StorePanelData storePanelData in panels)
		{
			storePanelData.Button.m_button.spriteController.callback = NavButtonClicked;
			storePanelData.Panel.OnCountsRefreshed += CountChanged;
			storePanelData.Panel.Initialize();
			CountChanged(storePanelData.Panel, storePanelData.Panel.GetNumOwned(), storePanelData.Panel.GetNumTotal());
			storePanelData.Panel.SetParentContainer(base.gameObject);
		}
		if (!m_disableInitialSelectPanel)
		{
			SelectPanel(m_defaultPanelBtn);
		}
		ClientGameManager.Get().OnPlayerBannerChange += UpdateBanner;
		if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			return;
		}
		while (true)
		{
			AccountComponent accountComponent = ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(accountComponent.SelectedForegroundBannerID);
			GameBalanceVars.PlayerBanner banner2 = GameBalanceVars.Get().GetBanner(accountComponent.SelectedBackgroundBannerID);
			UpdateBanner(banner, banner2);
			return;
		}
	}

	private void OnDestroy()
	{
		StorePanelData[] panels = m_panels;
		foreach (StorePanelData storePanelData in panels)
		{
			storePanelData.Panel.OnCountsRefreshed -= CountChanged;
		}
		while (true)
		{
			if (ClientGameManager.Get() != null)
			{
				while (true)
				{
					ClientGameManager.Get().OnPlayerBannerChange -= UpdateBanner;
					return;
				}
			}
			return;
		}
	}

	private void OnEnable()
	{
		for (int i = 0; i < m_panels.Length; i++)
		{
			m_panels[i].Panel.HandlePendingUpdates();
		}
		while (true)
		{
			return;
		}
	}

	private void UpdateBanner(GameBalanceVars.PlayerBanner emblem, GameBalanceVars.PlayerBanner banner)
	{
		string path = "Banners/Emblems/Chest01";
		string path2 = "Banners/Background/02_blue";
		if (emblem != null)
		{
			path = emblem.m_resourceString;
		}
		if (banner != null)
		{
			path2 = banner.m_resourceString;
		}
		m_emblem.sprite = Resources.Load<Sprite>(path);
		m_banner.sprite = Resources.Load<Sprite>(path2);
	}

	private void NavButtonClicked(BaseEventData data)
	{
		if (data.selectedObject == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		_ButtonSwapSprite component = data.selectedObject.GetComponent<_ButtonSwapSprite>();
		UIStoreSideNavButton component2 = component.selectableButton.GetComponent<UIStoreSideNavButton>();
		SelectPanel(component2);
	}

	public void SelectPanel(UIStoreSideNavButton btn)
	{
		StorePanelData[] panels = m_panels;
		foreach (StorePanelData storePanelData in panels)
		{
			storePanelData.Button.m_button.SetSelected(storePanelData.Button == btn, false, string.Empty, string.Empty);
			storePanelData.Panel.SetVisible(storePanelData.Button == btn);
			if (storePanelData.Button == btn)
			{
				storePanelData.Panel.RefreshPage();
			}
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void DisableInitialSelectPanel()
	{
		m_disableInitialSelectPanel = true;
	}

	private void CountChanged(UIStoreBasePanel panel, int ownedCount, int totalCount)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < m_panels.Length; i++)
		{
			if (m_panels[i].Panel == panel)
			{
				m_panels[i].Button.m_ownedCount.text = ownedCount.ToString();
				m_panels[i].Button.m_totalCount.text = "/" + totalCount;
			}
			num += m_panels[i].Panel.GetNumTotal();
			num2 += m_panels[i].Panel.GetNumOwned();
		}
		while (true)
		{
			m_totalOwnedText.text = num2.ToString();
			m_totalTotalText.text = "/" + num;
			if (num == 0)
			{
				m_ownedBar.fillAmount = 0f;
				UIManager.SetGameObjectActive(m_ownedCompleteContainer, false);
			}
			else
			{
				m_ownedBar.fillAmount = (float)num2 / (float)num;
				UIManager.SetGameObjectActive(m_ownedCompleteContainer, num2 == num);
			}
			return;
		}
	}
}
