using System;
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
		this.m_syncComp = base.Caster.GetComponent<Neko_SyncComponent>();
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
			if (GameFlowData.Get() != null)
			{
				BoardSquare boardSquare = Board.Get().GetBoardSquare(base.TargetPos);
				if (boardSquare != null)
				{
					this.m_syncComp.m_clientLastDiscBuffTurn = GameFlowData.Get().CurrentTurn;
					this.m_syncComp.m_clientDiscBuffTargetSquare = boardSquare;
				}
			}
		}
		if (!string.IsNullOrEmpty(this.m_audioEventOnNotify))
		{
			AudioManager.PostEvent(this.m_audioEventOnNotify, base.Caster.gameObject);
		}
	}
}
