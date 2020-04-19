using System;
using UnityEngine;

public class AdditionalVfxContainerBase : MonoBehaviour
{
	public GameObject m_fxPrefab;

	protected GameObject m_fx;

	protected FriendlyEnemyVFXSelector m_fofSelector;

	public virtual void Initialize(Sequence parentSequence)
	{
	}

	public void SpawnFX(Vector3 position, Quaternion orientation, Sequence parentSequence)
	{
		if (this.m_fx == null && this.m_fxPrefab != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AdditionalVfxContainerBase.SpawnFX(Vector3, Quaternion, Sequence)).MethodHandle;
			}
			this.m_fx = UnityEngine.Object.Instantiate<GameObject>(this.m_fxPrefab, position, orientation, parentSequence.transform);
			if (this.m_fx != null)
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
				this.m_fofSelector = this.m_fx.GetComponent<FriendlyEnemyVFXSelector>();
			}
		}
	}

	public void SetAsInactive()
	{
		if (this.m_fx != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AdditionalVfxContainerBase.SetAsInactive()).MethodHandle;
			}
			this.m_fx.SetActive(false);
		}
	}

	public void DestroyFX()
	{
		if (this.m_fx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AdditionalVfxContainerBase.DestroyFX()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_fx);
			this.m_fx = null;
		}
	}

	public void OnUpdate(bool parentSeqVisible, ActorData caster)
	{
		if (this.m_fx != null)
		{
			if (this.CanBeVisible(parentSeqVisible))
			{
				this.m_fx.SetActive(true);
			}
			else
			{
				this.m_fx.SetActive(false);
			}
			if (this.m_fofSelector != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AdditionalVfxContainerBase.OnUpdate(bool, ActorData)).MethodHandle;
				}
				if (caster != null)
				{
					this.m_fofSelector.Setup(caster.\u000E());
				}
			}
		}
	}

	public virtual bool CanBeVisible(bool parentSeqVisible)
	{
		return parentSeqVisible;
	}
}
