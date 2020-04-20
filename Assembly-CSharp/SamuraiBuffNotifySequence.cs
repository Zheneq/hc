using System;
using UnityEngine;

public class SamuraiBuffNotifySequence : SimpleTimingSequence
{
	public bool m_finalTurnVfx;

	private Samurai_SyncComponent m_syncComp;

	public override void FinishSetup()
	{
		base.FinishSetup();
		this.m_syncComp = base.Caster.GetComponent<Samurai_SyncComponent>();
		if (this.m_syncComp == null && Application.isEditor)
		{
			Debug.LogError(base.GetType() + " did not find sync component on caster");
		}
	}

	protected override void DoSequenceHits()
	{
		if (this.m_syncComp != null)
		{
			if (this.m_finalTurnVfx)
			{
				this.m_syncComp.m_swordBuffFinalTurnVfxPending = true;
			}
			else
			{
				this.m_syncComp.m_swordBuffVfxPending = true;
			}
		}
	}
}
