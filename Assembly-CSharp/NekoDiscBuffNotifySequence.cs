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
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameFlowData.Get() != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				BoardSquare boardSquare = Board.Get().GetBoardSquare(base.TargetPos);
				if (boardSquare != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
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
			switch (1)
			{
			case 0:
				continue;
			}
			AudioManager.PostEvent(m_audioEventOnNotify, base.Caster.gameObject);
			return;
		}
	}
}
