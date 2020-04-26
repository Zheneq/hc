using System.Collections.Generic;
using UnityEngine;

public class StoreWideData : MonoBehaviour
{
	private static StoreWideData s_instance;

	public List<Sale> m_sales;

	public List<CashShopFeaturedItem> m_featuredItems;

	public int m_featuredItemsVersion;

	private void Awake()
	{
		s_instance = this;
	}

	public static StoreWideData Get()
	{
		return s_instance;
	}
}
