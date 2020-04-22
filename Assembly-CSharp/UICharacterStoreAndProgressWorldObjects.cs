using UnityEngine;

public class UICharacterStoreAndProgressWorldObjects : UICharacterWorldObjects
{
	public Transform m_slotsContainer;

	public Transform m_collectionsTransform;

	public Transform m_cashShopTransform;

	private static UICharacterStoreAndProgressWorldObjects s_instance;

	private void Awake()
	{
		s_instance = this;
		Initialize();
	}

	private void OnDestroy()
	{
		if (s_instance == this)
		{
			Log.Info(string.Concat(GetType(), " OnDestroy, clearing singleton reference"));
			s_instance = null;
		}
	}

	public static UICharacterStoreAndProgressWorldObjects Get()
	{
		return s_instance;
	}

	protected override void OnVisibleChange()
	{
		UIManager.SetGameObjectActive(base.gameObject, IsVisible());
		if (!IsVisible())
		{
			return;
		}
		while (true)
		{
			if (UICashShopPanel.Get().IsVisible())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						m_slotsContainer.position = m_cashShopTransform.position;
						m_slotsContainer.rotation = m_cashShopTransform.rotation;
						m_slotsContainer.localScale = m_cashShopTransform.localScale;
						return;
					}
				}
			}
			m_slotsContainer.position = m_collectionsTransform.position;
			m_slotsContainer.rotation = m_collectionsTransform.rotation;
			m_slotsContainer.localScale = m_collectionsTransform.localScale;
			return;
		}
	}
}
