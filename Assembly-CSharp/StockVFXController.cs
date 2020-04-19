using System;
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
		this.Initialize();
	}

	private void Update()
	{
		this.UpdateForStocks();
	}

	protected virtual void Initialize()
	{
		this.m_actorModelData = base.GetComponent<ActorModelData>();
	}

	protected virtual void UpdateForStocks()
	{
		if (this.m_actorModelData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(StockVFXController.UpdateForStocks()).MethodHandle;
			}
			if (this.m_actorModelData.m_parentActorData != null)
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
				int stockAmount = this.GetStockAmount(this.m_actorModelData.m_parentActorData);
				if (stockAmount != this.m_prevStockAmount)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_prevStockAmount = stockAmount;
					if (this.m_vfxInstance != null)
					{
						UnityEngine.Object.Destroy(this.m_vfxInstance);
					}
					if (stockAmount > 0)
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
						if (this.m_vfxs.Length > stockAmount - 1)
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
							this.m_vfxInstance = UnityEngine.Object.Instantiate<GameObject>(this.m_vfxs[stockAmount - 1], Vector3.zero, Quaternion.identity);
							this.m_vfxInstance.transform.parent = base.transform;
						}
					}
				}
				goto IL_16E;
			}
		}
		if (this.m_actorModelData == null && this.m_vfxInstance == null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_vfxs.Length > 0)
			{
				this.m_vfxInstance = UnityEngine.Object.Instantiate<GameObject>(this.m_vfxs[this.m_vfxs.Length - 1], Vector3.zero, Quaternion.identity);
				this.m_vfxInstance.transform.parent = base.transform;
			}
		}
		IL_16E:
		if (this.m_vfxInstance != null)
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
			bool flag = true;
			bool flag2 = true;
			if (this.m_actorModelData != null)
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
				flag = this.m_actorModelData.IsVisibleToClient();
				flag2 = (this.m_actorModelData.m_parentActorData == null || !this.m_actorModelData.m_parentActorData.\u000E());
			}
			this.m_vfxInstance.SetActive(flag && flag2);
			this.m_vfxInstance.transform.position = base.transform.position;
			this.m_vfxInstance.transform.rotation = Quaternion.identity;
		}
	}

	protected virtual int GetStockAmount(ActorData actorData)
	{
		AbilityData component = actorData.GetComponent<AbilityData>();
		return (!(component != null)) ? 0 : component.GetStocksRemaining(this.m_abilityType);
	}
}
