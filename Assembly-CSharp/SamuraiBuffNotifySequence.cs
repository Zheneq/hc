using UnityEngine;

public class SamuraiBuffNotifySequence : SimpleTimingSequence
{
	public bool m_finalTurnVfx;

	private Samurai_SyncComponent m_syncComp;

	public override void FinishSetup()
	{
		base.FinishSetup();
		m_syncComp = base.Caster.GetComponent<Samurai_SyncComponent>();
		if (m_syncComp == null && Application.isEditor)
		{
			Debug.LogError(string.Concat(GetType(), " did not find sync component on caster"));
		}
	}

	protected override void DoSequenceHits()
	{
		if (!(m_syncComp != null))
		{
			return;
		}
		while (true)
		{
			if (m_finalTurnVfx)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						m_syncComp.m_swordBuffFinalTurnVfxPending = true;
						return;
					}
				}
			}
			m_syncComp.m_swordBuffVfxPending = true;
			return;
		}
	}
}
