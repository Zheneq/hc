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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiBuffNotifySequence.DoSequenceHits()).MethodHandle;
			}
			if (this.m_finalTurnVfx)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_syncComp.m_swordBuffFinalTurnVfxPending = true;
			}
			else
			{
				this.m_syncComp.m_swordBuffVfxPending = true;
			}
		}
	}
}
