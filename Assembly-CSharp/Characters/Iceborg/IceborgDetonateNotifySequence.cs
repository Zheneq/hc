using UnityEngine;

public class IceborgDetonateNotifySequence : SimpleTimingSequence
{
	private Iceborg_SyncComponent m_syncComp;

	public override void FinishSetup()
	{
		base.FinishSetup();
		m_syncComp = Caster.GetComponent<Iceborg_SyncComponent>();
		if (m_syncComp == null && Application.isEditor)
		{
			Debug.LogError(string.Concat(GetType(), " did not find sync component on caster"));
		}
	}

	protected override void DoSequenceHits()
	{
		if (m_syncComp != null)
		{
			m_syncComp.m_clientDetonateNovaUsedTurn = GameFlowData.Get().CurrentTurn;
		}
	}
}
