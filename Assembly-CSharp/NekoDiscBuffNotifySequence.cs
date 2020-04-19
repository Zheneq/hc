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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoDiscBuffNotifySequence.FinishSetup()).MethodHandle;
			}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoDiscBuffNotifySequence.DoSequenceHits()).MethodHandle;
			}
			if (GameFlowData.Get() != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				BoardSquare boardSquare = Board.\u000E().\u000E(base.TargetPos);
				if (boardSquare != null)
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
					this.m_syncComp.m_clientLastDiscBuffTurn = GameFlowData.Get().CurrentTurn;
					this.m_syncComp.m_clientDiscBuffTargetSquare = boardSquare;
				}
			}
		}
		if (!string.IsNullOrEmpty(this.m_audioEventOnNotify))
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			AudioManager.PostEvent(this.m_audioEventOnNotify, base.Caster.gameObject);
		}
	}
}
