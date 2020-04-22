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
		return s_instance;
	}

	public override void Awake()
	{
		s_instance = this;
		m_cancelButton.callback = CloseScreen;
		base.Awake();
	}

	private void OnDestroy()
	{
		if (!(s_instance == this))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			s_instance = null;
			return;
		}
	}

	private void Start()
	{
		UIGGBoostPurchaseButton[] componentsInChildren = m_ggBoostGrid.GetComponentsInChildren<UIGGBoostPurchaseButton>();
		foreach (UIGGBoostPurchaseButton uIGGBoostPurchaseButton in componentsInChildren)
		{
			Object.Destroy(uIGGBoostPurchaseButton.gameObject);
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GGPack[] ggPacks = GameWideData.Get().m_ggPackData.m_ggPacks;
			foreach (GGPack pack in ggPacks)
			{
				UIGGBoostPurchaseButton uIGGBoostPurchaseButton2 = Object.Instantiate(m_ggButtonPrefab);
				uIGGBoostPurchaseButton2.transform.SetParent(m_ggBoostGrid.transform);
				uIGGBoostPurchaseButton2.transform.localPosition = Vector3.zero;
				uIGGBoostPurchaseButton2.transform.localScale = Vector3.one;
				uIGGBoostPurchaseButton2.Setup(pack);
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
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_container, visible);
		if (!visible)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIRAFProgramScreen.Get().SetVisible(false);
			UIPlayerProgressPanel.Get().SetVisible(false);
			return;
		}
	}

	private void CloseScreen(BaseEventData data)
	{
		SetVisible(false);
	}
}
