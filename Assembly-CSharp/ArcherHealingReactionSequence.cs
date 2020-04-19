using System;
using UnityEngine;

public class ArcherHealingReactionSequence : SimpleAttachedVFXOnTargetSequence
{
	[Header("-- Archer Specific Fields --")]
	public GameObject m_usedUpFxPrefab;

	private bool m_switchedToUsedUpFx;

	private Archer_SyncComponent m_syncComp;

	private ActorData m_activeClientActor;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		if (base.Caster != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherHealingReactionSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
			}
			this.m_syncComp = base.Caster.GetComponent<Archer_SyncComponent>();
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.m_initialized)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherHealingReactionSequence.OnUpdate()).MethodHandle;
			}
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
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
				if (this.m_syncComp != null)
				{
					if (!this.m_switchedToUsedUpFx && this.m_syncComp.ActorShouldSwapVfxForHealReaction(activeOwnedActorData))
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
						base.SwitchFxTo(this.m_usedUpFxPrefab);
						this.m_switchedToUsedUpFx = true;
					}
					else if (this.m_switchedToUsedUpFx)
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
						if (activeOwnedActorData != this.m_activeClientActor && !this.m_syncComp.ActorShouldSwapVfxForHealReaction(activeOwnedActorData))
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
							base.SwitchFxTo(this.m_fxPrefab);
							this.m_switchedToUsedUpFx = false;
						}
					}
					this.m_activeClientActor = activeOwnedActorData;
				}
			}
		}
	}
}
