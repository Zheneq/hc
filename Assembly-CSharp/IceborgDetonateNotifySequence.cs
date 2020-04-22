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
			switch (6)
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
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
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
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_syncComp.m_clientDetonateNovaUsedTurn = GameFlowData.Get().CurrentTurn;
			return;
		}
	}
}
