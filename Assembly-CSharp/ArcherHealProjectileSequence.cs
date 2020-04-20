using System;

public class ArcherHealProjectileSequence : ArcingProjectileSequence
{
	private Archer_SyncComponent m_syncComp;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (extraSequenceParams is Sequence.ActorIndexExtraParam)
			{
				Sequence.ActorIndexExtraParam actorIndexExtraParam = extraSequenceParams as Sequence.ActorIndexExtraParam;
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex((int)actorIndexExtraParam.m_actorIndex);
				if (actorData != null)
				{
					this.m_syncComp = actorData.GetComponent<Archer_SyncComponent>();
				}
				break;
			}
		}
	}

	protected override void SpawnFX()
	{
		base.SpawnFX();
		if (base.Target != null && this.m_syncComp != null)
		{
			if (this.m_syncComp.ActorHasExpendedHealReaction(base.Target))
			{
				this.m_syncComp.ChangeVfxForHealReaction(base.Target);
			}
		}
	}

	protected override Team GetFoFObservingTeam()
	{
		return base.Caster.GetOpposingTeam();
	}
}
