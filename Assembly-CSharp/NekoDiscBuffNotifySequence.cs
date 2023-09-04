using UnityEngine;

public class NekoDiscBuffNotifySequence : SimpleTimingSequence
{
	[AudioEvent(false, order = 0)]
	[Separator("Audio Event (for when powered up disc vfx shows up)", "orange", order = 1)]
	public string m_audioEventOnNotify;

	private Neko_SyncComponent m_syncComp;

	public override void FinishSetup()
	{
		base.FinishSetup();
		m_syncComp = Caster.GetComponent<Neko_SyncComponent>();
		if (m_syncComp == null && Application.isEditor)
		{
			Debug.LogError(string.Concat(GetType(), " did not find sync component on caster"));
		}
	}

	protected override void DoSequenceHits()
	{
		if (m_syncComp != null && GameFlowData.Get() != null)
		{
			BoardSquare targetSquare = Board.Get().GetSquareFromVec3(TargetPos);
			if (targetSquare != null)
			{
				m_syncComp.m_clientLastDiscBuffTurn = GameFlowData.Get().CurrentTurn;
				m_syncComp.m_clientDiscBuffTargetSquare = targetSquare;
			}
		}
		if (!string.IsNullOrEmpty(m_audioEventOnNotify))
		{
			AudioManager.PostEvent(m_audioEventOnNotify, Caster.gameObject);
		}
	}
}
