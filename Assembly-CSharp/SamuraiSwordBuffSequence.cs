using System;
using UnityEngine;

public class SamuraiSwordBuffSequence : SimpleAttachedVFXSequence
{
	[Separator("Samurai-specific FX Prefab for Decision of the turn buff becomes active", true)]
	public GameObject m_fxPrefabForActiveBuff;

	[AudioEvent(false)]
	public string m_onSwordActivateAudioEvent = string.Empty;

	private Samurai_SyncComponent m_syncComp;

	private bool m_switchedToActiveBuffFx;

	public override void FinishSetup()
	{
		this.m_syncComp = base.Caster.GetComponent<Samurai_SyncComponent>();
		if (this.m_syncComp == null)
		{
			if (Application.isEditor)
			{
				Debug.LogError(base.GetType() + " did not find sync component on caster");
			}
		}
	}

	protected override void OnUpdate()
	{
		if (this.m_initialized)
		{
			if (this.m_syncComp != null)
			{
				int num = 0;
				if (this.m_syncComp.m_swordBuffVfxPending)
				{
					if (this.m_fx == null && base.AgeInTurns <= 0)
					{
						base.SpawnFX(null);
					}
					this.m_syncComp.m_swordBuffVfxPending = false;
				}
				else
				{
					if (!this.m_switchedToActiveBuffFx)
					{
						if (this.m_syncComp.IsSelfBuffActive(ref num) && GameFlowData.Get().IsInDecisionState())
						{
							if (this.m_fxPrefabForActiveBuff != null)
							{
								base.StopFX();
								base.SpawnFX(this.m_fxPrefabForActiveBuff);
								if (!string.IsNullOrEmpty(this.m_onSwordActivateAudioEvent))
								{
									GameObject gameObject = null;
									if (base.Caster != null)
									{
										gameObject = base.Caster.gameObject;
									}
									if (gameObject != null)
									{
										AudioManager.PostEvent(this.m_onSwordActivateAudioEvent, gameObject);
									}
								}
								this.m_switchedToActiveBuffFx = true;
								goto IL_1C5;
							}
						}
					}
					if (this.m_syncComp.m_swordBuffFinalTurnVfxPending)
					{
						if (this.m_switchedToActiveBuffFx)
						{
							if (!this.m_syncComp.IsSelfBuffActive(ref num))
							{
								if (base.AgeInTurns > 0)
								{
									base.StopFX();
								}
								this.m_syncComp.m_swordBuffFinalTurnVfxPending = false;
							}
						}
					}
				}
			}
		}
		IL_1C5:
		base.OnUpdate();
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
	}
}
