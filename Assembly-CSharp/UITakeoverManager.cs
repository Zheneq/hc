using System;
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
		return UITakeoverManager.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.ThankYouContainer;
	}

	public override void Awake()
	{
		this.m_purchaseGameCloseButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClosePurchaseGameClicked);
		UITakeoverManager.s_instance = this;
		UIManager.SetGameObjectActive(this.m_purchaseGameContainer, false, null);
		base.Awake();
	}

	private void Start()
	{
		ClientGameManager.Get().OnLobbyServerClientAccessLevelChange += this.HandleAccessLevelChange;
	}

	private void OnDestroy()
	{
		UITakeoverManager.s_instance = null;
		if (ClientGameManager.Get() != null)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITakeoverManager.OnDestroy()).MethodHandle;
			}
			ClientGameManager.Get().OnLobbyServerClientAccessLevelChange -= this.HandleAccessLevelChange;
		}
	}

	public void ShowPurchaseGameTakeover()
	{
		UIManager.SetGameObjectActive(this.m_purchaseGameCloseButton.selectableButton, ClientGameManager.Get().ClientAccessLevel > ClientAccessLevel.Free, null);
		UIManager.SetGameObjectActive(this.m_purchaseGameContainer, true, null);
		this.TakeOverTimeStart = Time.time + 10f;
	}

	public void ClosePurchaseGameClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_purchaseGameContainer, false, null);
	}

	private void HandleAccessLevelChange(ClientAccessLevel oldLevel, ClientAccessLevel newLevel)
	{
		if (newLevel > ClientAccessLevel.Free)
		{
			UIManager.SetGameObjectActive(this.m_purchaseGameCloseButton.selectableButton, true, null);
		}
	}

	private void Update()
	{
		if (this.TakeOverTimeStart > 0f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITakeoverManager.Update()).MethodHandle;
			}
			if (Time.time < this.TakeOverTimeStart)
			{
				if (ClientGameManager.Get().ClientAccessLevel <= ClientAccessLevel.Free)
				{
					return;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.TakeOverTimeStart = -1f;
			UIManager.SetGameObjectActive(this.m_purchaseGameCloseButton.selectableButton, true, null);
		}
	}
}
