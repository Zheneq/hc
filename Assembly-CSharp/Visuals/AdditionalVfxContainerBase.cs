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
		if (!(m_fx == null) || !(m_fxPrefab != null))
		{
			return;
		}
		while (true)
		{
			m_fx = Object.Instantiate(m_fxPrefab, position, orientation, parentSequence.transform);
			if (m_fx != null)
			{
				while (true)
				{
					m_fofSelector = m_fx.GetComponent<FriendlyEnemyVFXSelector>();
					return;
				}
			}
			return;
		}
	}

	public void SetAsInactive()
	{
		if (!(m_fx != null))
		{
			return;
		}
		while (true)
		{
			m_fx.SetActive(false);
			return;
		}
	}

	public void DestroyFX()
	{
		if (!(m_fx != null))
		{
			return;
		}
		while (true)
		{
			Object.Destroy(m_fx);
			m_fx = null;
			return;
		}
	}

	public void OnUpdate(bool parentSeqVisible, ActorData caster)
	{
		if (!(m_fx != null))
		{
			return;
		}
		if (CanBeVisible(parentSeqVisible))
		{
			m_fx.SetActive(true);
		}
		else
		{
			m_fx.SetActive(false);
		}
		if (!(m_fofSelector != null))
		{
			return;
		}
		while (true)
		{
			if (caster != null)
			{
				m_fofSelector.Setup(caster.GetTeam());
			}
			return;
		}
	}

	public virtual bool CanBeVisible(bool parentSeqVisible)
	{
		return parentSeqVisible;
	}
}
