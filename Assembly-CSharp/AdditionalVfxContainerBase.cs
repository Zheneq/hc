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
			this.m_fx = UnityEngine.Object.Instantiate<GameObject>(this.m_fxPrefab, position, orientation, parentSequence.transform);
			if (this.m_fx != null)
			{
				this.m_fofSelector = this.m_fx.GetComponent<FriendlyEnemyVFXSelector>();
			}
		}
	}

	public void SetAsInactive()
	{
		if (this.m_fx != null)
		{
			this.m_fx.SetActive(false);
		}
	}

	public void DestroyFX()
	{
		if (this.m_fx != null)
		{
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
				if (caster != null)
				{
					this.m_fofSelector.Setup(caster.GetTeam());
				}
			}
		}
	}

	public virtual bool CanBeVisible(bool parentSeqVisible)
	{
		return parentSeqVisible;
	}
}
