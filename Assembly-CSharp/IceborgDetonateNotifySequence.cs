using UnityEngine;

public class IceborgDetonateNotifySequence : SimpleTimingSequence
{
	private Iceborg_SyncComponent m_syncComp;

	public override void FinishSetup()
	{
		base.FinishSetup();
		m_syncComp = base.Caster.GetComponent<Iceborg_SyncComponent>();
		if (!(m_syncComp == null))
		{
			return;
		}
		while (true)
		{
			if (Application.isEditor)
			{
				while (true)
				{
					Debug.LogError(string.Concat(GetType(), " did not find sync component on caster"));
					return;
				}
			}
			return;
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
			m_syncComp.m_clientDetonateNovaUsedTurn = GameFlowData.Get().CurrentTurn;
			return;
		}
	}
}
