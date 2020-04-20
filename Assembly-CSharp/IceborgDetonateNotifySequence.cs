using System;
using UnityEngine;

public class IceborgDetonateNotifySequence : SimpleTimingSequence
{
	private Iceborg_SyncComponent m_syncComp;

	public override void FinishSetup()
	{
		base.FinishSetup();
		this.m_syncComp = base.Caster.GetComponent<Iceborg_SyncComponent>();
		if (this.m_syncComp == null)
		{
			if (Application.isEditor)
			{
				Debug.LogError(base.GetType() + " did not find sync component on caster");
			}
		}
	}

	protected override void DoSequenceHits()
	{
		if (this.m_syncComp != null)
		{
			this.m_syncComp.m_clientDetonateNovaUsedTurn = GameFlowData.Get().CurrentTurn;
		}
	}
}
