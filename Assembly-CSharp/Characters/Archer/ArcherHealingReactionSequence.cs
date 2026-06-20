using UnityEngine;

public class ArcherHealingReactionSequence : SimpleAttachedVFXOnTargetSequence
{
	[Header("-- Archer Specific Fields --")]
	public GameObject m_usedUpFxPrefab;

	private bool m_switchedToUsedUpFx;

	private Archer_SyncComponent m_syncComp;

	private ActorData m_activeClientActor;

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		if (!(base.Caster != null))
		{
			return;
		}
		while (true)
		{
			m_syncComp = base.Caster.GetComponent<Archer_SyncComponent>();
			return;
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (!(activeOwnedActorData != null))
			{
				return;
			}
			while (true)
			{
				if (!(m_syncComp != null))
				{
					return;
				}
				if (!m_switchedToUsedUpFx && m_syncComp.ActorShouldSwapVfxForHealReaction(activeOwnedActorData))
				{
					SwitchFxTo(m_usedUpFxPrefab);
					m_switchedToUsedUpFx = true;
				}
				else if (m_switchedToUsedUpFx)
				{
					if (activeOwnedActorData != m_activeClientActor && !m_syncComp.ActorShouldSwapVfxForHealReaction(activeOwnedActorData))
					{
						SwitchFxTo(m_fxPrefab);
						m_switchedToUsedUpFx = false;
					}
				}
				m_activeClientActor = activeOwnedActorData;
				return;
			}
		}
	}
}
