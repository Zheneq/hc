using UnityEngine;

public class SamuraiBuffNotifySequence : SimpleTimingSequence
{
	public bool m_finalTurnVfx;

	private Samurai_SyncComponent m_syncComp;

	public override void FinishSetup()
	{
		base.FinishSetup();
		m_syncComp = Caster.GetComponent<Samurai_SyncComponent>();
		if (m_syncComp == null && Application.isEditor)
		{
			Debug.LogError(GetType() + " did not find sync component on caster");
		}
	}

	protected override void DoSequenceHits()
	{
		if (m_syncComp != null)
		{
			if (m_finalTurnVfx)
			{
				m_syncComp.m_swordBuffFinalTurnVfxPending = true;
			}
			else
			{
				m_syncComp.m_swordBuffVfxPending = true;
			}
		}
	}
}
