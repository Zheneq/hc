using UnityEngine;
using UnityEngine.EventSystems;

public class UITakeoverManager : UIScene
{
	public RectTransform m_purchaseGameContainer;

	public _ButtonSwapSprite m_purchaseGameCloseButton;

	private static UITakeoverManager s_instance;

	private float TakeOverTimeStart = -1f;

	public static UITakeoverManager Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.ThankYouContainer;
	}

	public override void Awake()
	{
		m_purchaseGameCloseButton.callback = ClosePurchaseGameClicked;
		s_instance = this;
		UIManager.SetGameObjectActive(m_purchaseGameContainer, false);
		base.Awake();
	}

	private void Start()
	{
		ClientGameManager.Get().OnLobbyServerClientAccessLevelChange += HandleAccessLevelChange;
	}

	private void OnDestroy()
	{
		s_instance = null;
		if (!(ClientGameManager.Get() != null))
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
			ClientGameManager.Get().OnLobbyServerClientAccessLevelChange -= HandleAccessLevelChange;
			return;
		}
	}

	public void ShowPurchaseGameTakeover()
	{
		UIManager.SetGameObjectActive(m_purchaseGameCloseButton.selectableButton, ClientGameManager.Get().ClientAccessLevel > ClientAccessLevel.Free);
		UIManager.SetGameObjectActive(m_purchaseGameContainer, true);
		TakeOverTimeStart = Time.time + 10f;
	}

	public void ClosePurchaseGameClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_purchaseGameContainer, false);
	}

	private void HandleAccessLevelChange(ClientAccessLevel oldLevel, ClientAccessLevel newLevel)
	{
		if (newLevel > ClientAccessLevel.Free)
		{
			UIManager.SetGameObjectActive(m_purchaseGameCloseButton.selectableButton, true);
		}
	}

	private void Update()
	{
		if (!(TakeOverTimeStart > 0f))
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
			if (!(Time.time >= TakeOverTimeStart))
			{
				if (ClientGameManager.Get().ClientAccessLevel <= ClientAccessLevel.Free)
				{
					return;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			TakeOverTimeStart = -1f;
			UIManager.SetGameObjectActive(m_purchaseGameCloseButton.selectableButton, true);
			return;
		}
	}
}
