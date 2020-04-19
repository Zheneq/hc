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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDetonateNotifySequence.FinishSetup()).MethodHandle;
			}
			if (Application.isEditor)
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
				Debug.LogError(base.GetType() + " did not find sync component on caster");
			}
		}
	}

	protected override void DoSequenceHits()
	{
		if (this.m_syncComp != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDetonateNotifySequence.DoSequenceHits()).MethodHandle;
			}
			this.m_syncComp.m_clientDetonateNovaUsedTurn = GameFlowData.Get().CurrentTurn;
		}
	}
}
