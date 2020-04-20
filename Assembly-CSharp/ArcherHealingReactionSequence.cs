using System;
using UnityEngine;

public class ArcherHealingReactionSequence : SimpleAttachedVFXOnTargetSequence
{
	[Header("-- Archer Specific Fields --")]
	public GameObject m_usedUpFxPrefab;

	private bool m_switchedToUsedUpFx;

	private Archer_SyncComponent m_syncComp;

	private ActorData m_activeClientActor;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		if (base.Caster != null)
		{
			this.m_syncComp = base.Caster.GetComponent<Archer_SyncComponent>();
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.m_initialized)
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
			{
				if (this.m_syncComp != null)
				{
					if (!this.m_switchedToUsedUpFx && this.m_syncComp.ActorShouldSwapVfxForHealReaction(activeOwnedActorData))
					{
						base.SwitchFxTo(this.m_usedUpFxPrefab);
						this.m_switchedToUsedUpFx = true;
					}
					else if (this.m_switchedToUsedUpFx)
					{
						if (activeOwnedActorData != this.m_activeClientActor && !this.m_syncComp.ActorShouldSwapVfxForHealReaction(activeOwnedActorData))
						{
							base.SwitchFxTo(this.m_fxPrefab);
							this.m_switchedToUsedUpFx = false;
						}
					}
					this.m_activeClientActor = activeOwnedActorData;
				}
			}
		}
	}
}
