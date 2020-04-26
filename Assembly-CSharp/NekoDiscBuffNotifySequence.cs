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
		m_syncComp = base.Caster.GetComponent<Neko_SyncComponent>();
		if (!(m_syncComp == null))
		{
			return;
		}
		while (true)
		{
			if (Application.isEditor)
			{
				Debug.LogError(string.Concat(GetType(), " did not find sync component on caster"));
			}
			return;
		}
	}

	protected override void DoSequenceHits()
	{
		if (m_syncComp != null)
		{
			if (GameFlowData.Get() != null)
			{
				BoardSquare boardSquare = Board.Get().GetBoardSquare(base.TargetPos);
				if (boardSquare != null)
				{
					m_syncComp.m_clientLastDiscBuffTurn = GameFlowData.Get().CurrentTurn;
					m_syncComp.m_clientDiscBuffTargetSquare = boardSquare;
				}
			}
		}
		if (string.IsNullOrEmpty(m_audioEventOnNotify))
		{
			return;
		}
		while (true)
		{
			AudioManager.PostEvent(m_audioEventOnNotify, base.Caster.gameObject);
			return;
		}
	}
}
