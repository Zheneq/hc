using System;
using UnityEngine;

public class UICharacterStoreAndProgressWorldObjects : UICharacterWorldObjects
{
	public Transform m_slotsContainer;

	public Transform m_collectionsTransform;

	public Transform m_cashShopTransform;

	private static UICharacterStoreAndProgressWorldObjects s_instance;

	private void Awake()
	{
		UICharacterStoreAndProgressWorldObjects.s_instance = this;
		base.Initialize();
	}

	private void OnDestroy()
	{
		if (UICharacterStoreAndProgressWorldObjects.s_instance == this)
		{
			Log.Info(base.GetType() + " OnDestroy, clearing singleton reference", new object[0]);
			UICharacterStoreAndProgressWorldObjects.s_instance = null;
		}
	}

	public static UICharacterStoreAndProgressWorldObjects Get()
	{
		return UICharacterStoreAndProgressWorldObjects.s_instance;
	}

	protected override void OnVisibleChange()
	{
		UIManager.SetGameObjectActive(base.gameObject, base.IsVisible(), null);
		if (base.IsVisible())
		{
			if (UICashShopPanel.Get().IsVisible())
			{
				this.m_slotsContainer.position = this.m_cashShopTransform.position;
				this.m_slotsContainer.rotation = this.m_cashShopTransform.rotation;
				this.m_slotsContainer.localScale = this.m_cashShopTransform.localScale;
			}
			else
			{
				this.m_slotsContainer.position = this.m_collectionsTransform.position;
				this.m_slotsContainer.rotation = this.m_collectionsTransform.rotation;
				this.m_slotsContainer.localScale = this.m_collectionsTransform.localScale;
			}
		}
	}
}
