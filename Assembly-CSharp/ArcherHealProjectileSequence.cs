public class ArcherHealProjectileSequence : ArcingProjectileSequence
{
	private Archer_SyncComponent m_syncComp;

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		int num = 0;
		IExtraSequenceParams extraSequenceParams;
		while (true)
		{
			if (num < extraParams.Length)
			{
				extraSequenceParams = extraParams[num];
				if (extraSequenceParams is ActorIndexExtraParam)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ActorIndexExtraParam actorIndexExtraParam = extraSequenceParams as ActorIndexExtraParam;
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndexExtraParam.m_actorIndex);
			if (actorData != null)
			{
				m_syncComp = actorData.GetComponent<Archer_SyncComponent>();
			}
			return;
		}
	}

	protected override void SpawnFX()
	{
		base.SpawnFX();
		if (!(base.Target != null) || !(m_syncComp != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_syncComp.ActorHasExpendedHealReaction(base.Target))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					m_syncComp.ChangeVfxForHealReaction(base.Target);
					return;
				}
			}
			return;
		}
	}

	protected override Team GetFoFObservingTeam()
	{
		return base.Caster.GetOpposingTeam();
	}
}
