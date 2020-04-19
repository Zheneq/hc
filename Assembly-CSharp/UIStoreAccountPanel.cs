using System;
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
		foreach (StorePanelData storePanelData in this.m_panels)
		{
			storePanelData.Button.m_button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.NavButtonClicked);
			storePanelData.Panel.OnCountsRefreshed += new Action<UIStoreBaseInventoryPanel, int, int>(this.CountChanged);
			storePanelData.Panel.Initialize();
			this.CountChanged(storePanelData.Panel, storePanelData.Panel.GetNumOwned(), storePanelData.Panel.GetNumTotal());
			storePanelData.Panel.SetParentContainer(base.gameObject);
		}
		if (!this.m_disableInitialSelectPanel)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountPanel.Start()).MethodHandle;
			}
			this.SelectPanel(this.m_defaultPanelBtn);
		}
		ClientGameManager.Get().OnPlayerBannerChange += this.UpdateBanner;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
			AccountComponent accountComponent = ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(accountComponent.SelectedForegroundBannerID);
			GameBalanceVars.PlayerBanner banner2 = GameBalanceVars.Get().GetBanner(accountComponent.SelectedBackgroundBannerID);
			this.UpdateBanner(banner, banner2);
		}
	}

	private void OnDestroy()
	{
		foreach (StorePanelData storePanelData in this.m_panels)
		{
			storePanelData.Panel.OnCountsRefreshed -= new Action<UIStoreBaseInventoryPanel, int, int>(this.CountChanged);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountPanel.OnDestroy()).MethodHandle;
		}
		if (ClientGameManager.Get() != null)
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
			ClientGameManager.Get().OnPlayerBannerChange -= this.UpdateBanner;
		}
	}

	private void OnEnable()
	{
		for (int i = 0; i < this.m_panels.Length; i++)
		{
			this.m_panels[i].Panel.HandlePendingUpdates();
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountPanel.OnEnable()).MethodHandle;
		}
	}

	private void UpdateBanner(GameBalanceVars.PlayerBanner emblem, GameBalanceVars.PlayerBanner banner)
	{
		string path = "Banners/Emblems/Chest01";
		string path2 = "Banners/Background/02_blue";
		if (emblem != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountPanel.UpdateBanner(GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner)).MethodHandle;
			}
			path = emblem.m_resourceString;
		}
		if (banner != null)
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
			path2 = banner.m_resourceString;
		}
		this.m_emblem.sprite = Resources.Load<Sprite>(path);
		this.m_banner.sprite = Resources.Load<Sprite>(path2);
	}

	private void NavButtonClicked(BaseEventData data)
	{
		if (data.selectedObject == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountPanel.NavButtonClicked(BaseEventData)).MethodHandle;
			}
			return;
		}
		_ButtonSwapSprite component = data.selectedObject.GetComponent<_ButtonSwapSprite>();
		UIStoreSideNavButton component2 = component.selectableButton.GetComponent<UIStoreSideNavButton>();
		this.SelectPanel(component2);
	}

	public void SelectPanel(UIStoreSideNavButton btn)
	{
		foreach (StorePanelData storePanelData in this.m_panels)
		{
			storePanelData.Button.m_button.SetSelected(storePanelData.Button == btn, false, string.Empty, string.Empty);
			storePanelData.Panel.SetVisible(storePanelData.Button == btn);
			if (storePanelData.Button == btn)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountPanel.SelectPanel(UIStoreSideNavButton)).MethodHandle;
				}
				storePanelData.Panel.RefreshPage();
			}
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

	public void DisableInitialSelectPanel()
	{
		this.m_disableInitialSelectPanel = true;
	}

	private void CountChanged(UIStoreBasePanel panel, int ownedCount, int totalCount)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.m_panels.Length; i++)
		{
			if (this.m_panels[i].Panel == panel)
			{
				this.m_panels[i].Button.m_ownedCount.text = ownedCount.ToString();
				this.m_panels[i].Button.m_totalCount.text = "/" + totalCount.ToString();
			}
			num += this.m_panels[i].Panel.GetNumTotal();
			num2 += this.m_panels[i].Panel.GetNumOwned();
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreAccountPanel.CountChanged(UIStoreBasePanel, int, int)).MethodHandle;
		}
		this.m_totalOwnedText.text = num2.ToString();
		this.m_totalTotalText.text = "/" + num.ToString();
		if (num == 0)
		{
			this.m_ownedBar.fillAmount = 0f;
			UIManager.SetGameObjectActive(this.m_ownedCompleteContainer, false, null);
		}
		else
		{
			this.m_ownedBar.fillAmount = (float)num2 / (float)num;
			UIManager.SetGameObjectActive(this.m_ownedCompleteContainer, num2 == num, null);
		}
	}
}
