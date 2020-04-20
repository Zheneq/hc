using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGGBoostPurchaseScreen : UIScene
{
	public RectTransform m_container;

	public _ButtonSwapSprite m_cancelButton;

	public HorizontalLayoutGroup m_ggBoostGrid;

	public UIGGBoostPurchaseButton m_ggButtonPrefab;

	private static UIGGBoostPurchaseScreen s_instance;

	public override SceneType GetSceneType()
	{
		return SceneType.GGBoostPurchase;
	}

	public static UIGGBoostPurchaseScreen Get()
	{
		return UIGGBoostPurchaseScreen.s_instance;
	}

	public override void Awake()
	{
		UIGGBoostPurchaseScreen.s_instance = this;
		this.m_cancelButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseScreen);
		base.Awake();
	}

	private void OnDestroy()
	{
		if (UIGGBoostPurchaseScreen.s_instance == this)
		{
			UIGGBoostPurchaseScreen.s_instance = null;
		}
	}

	private void Start()
	{
		foreach (UIGGBoostPurchaseButton uiggboostPurchaseButton in this.m_ggBoostGrid.GetComponentsInChildren<UIGGBoostPurchaseButton>())
		{
			UnityEngine.Object.Destroy(uiggboostPurchaseButton.gameObject);
		}
		foreach (GGPack pack in GameWideData.Get().m_ggPackData.m_ggPacks)
		{
			UIGGBoostPurchaseButton uiggboostPurchaseButton2 = UnityEngine.Object.Instantiate<UIGGBoostPurchaseButton>(this.m_ggButtonPrefab);
			uiggboostPurchaseButton2.transform.SetParent(this.m_ggBoostGrid.transform);
			uiggboostPurchaseButton2.transform.localPosition = Vector3.zero;
			uiggboostPurchaseButton2.transform.localScale = Vector3.one;
			uiggboostPurchaseButton2.Setup(pack);
		}
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_container, visible, null);
		if (visible)
		{
			UIRAFProgramScreen.Get().SetVisible(false);
			UIPlayerProgressPanel.Get().SetVisible(false, true);
		}
	}

	private void CloseScreen(BaseEventData data)
	{
		this.SetVisible(false);
	}
}
