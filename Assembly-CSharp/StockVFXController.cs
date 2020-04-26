using UnityEngine;

public class StockVFXController : CopyableVfxControllerComponent
{
	public AbilityData.ActionType m_abilityType;

	public GameObject[] m_vfxs;

	protected ActorModelData m_actorModelData;

	private int m_prevStockAmount;

	private GameObject m_vfxInstance;

	private void Start()
	{
		Initialize();
	}

	private void Update()
	{
		UpdateForStocks();
	}

	protected virtual void Initialize()
	{
		m_actorModelData = GetComponent<ActorModelData>();
	}

	protected virtual void UpdateForStocks()
	{
		if (m_actorModelData != null)
		{
			if (m_actorModelData.m_parentActorData != null)
			{
				int stockAmount = GetStockAmount(m_actorModelData.m_parentActorData);
				if (stockAmount != m_prevStockAmount)
				{
					m_prevStockAmount = stockAmount;
					if (m_vfxInstance != null)
					{
						Object.Destroy(m_vfxInstance);
					}
					if (stockAmount > 0)
					{
						if (m_vfxs.Length > stockAmount - 1)
						{
							m_vfxInstance = Object.Instantiate(m_vfxs[stockAmount - 1], Vector3.zero, Quaternion.identity);
							m_vfxInstance.transform.parent = base.transform;
						}
					}
				}
				goto IL_016e;
			}
		}
		if (m_actorModelData == null && m_vfxInstance == null)
		{
			if (m_vfxs.Length > 0)
			{
				m_vfxInstance = Object.Instantiate(m_vfxs[m_vfxs.Length - 1], Vector3.zero, Quaternion.identity);
				m_vfxInstance.transform.parent = base.transform;
			}
		}
		goto IL_016e;
		IL_016e:
		if (!(m_vfxInstance != null))
		{
			return;
		}
		while (true)
		{
			bool flag = true;
			bool flag2 = true;
			if (m_actorModelData != null)
			{
				flag = m_actorModelData.IsVisibleToClient();
				flag2 = (m_actorModelData.m_parentActorData == null || !m_actorModelData.m_parentActorData.IsDead());
			}
			m_vfxInstance.SetActive(flag && flag2);
			m_vfxInstance.transform.position = base.transform.position;
			m_vfxInstance.transform.rotation = Quaternion.identity;
			return;
		}
	}

	protected virtual int GetStockAmount(ActorData actorData)
	{
		AbilityData component = actorData.GetComponent<AbilityData>();
		return (component != null) ? component.GetStocksRemaining(m_abilityType) : 0;
	}
}
