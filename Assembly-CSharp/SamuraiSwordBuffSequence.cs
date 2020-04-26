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
		m_syncComp = base.Caster.GetComponent<Samurai_SyncComponent>();
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

	protected override void OnUpdate()
	{
		if (m_initialized)
		{
			if (m_syncComp != null)
			{
				int damageIncrease = 0;
				if (m_syncComp.m_swordBuffVfxPending)
				{
					if (m_fx == null && base.AgeInTurns <= 0)
					{
						SpawnFX();
					}
					m_syncComp.m_swordBuffVfxPending = false;
				}
				else
				{
					if (!m_switchedToActiveBuffFx)
					{
						if (m_syncComp.IsSelfBuffActive(ref damageIncrease) && GameFlowData.Get().IsInDecisionState())
						{
							if (m_fxPrefabForActiveBuff != null)
							{
								StopFX();
								SpawnFX(m_fxPrefabForActiveBuff);
								if (!string.IsNullOrEmpty(m_onSwordActivateAudioEvent))
								{
									GameObject gameObject = null;
									if (base.Caster != null)
									{
										gameObject = base.Caster.gameObject;
									}
									if (gameObject != null)
									{
										AudioManager.PostEvent(m_onSwordActivateAudioEvent, gameObject);
									}
								}
								m_switchedToActiveBuffFx = true;
								goto IL_01c5;
							}
						}
					}
					if (m_syncComp.m_swordBuffFinalTurnVfxPending)
					{
						if (m_switchedToActiveBuffFx)
						{
							if (!m_syncComp.IsSelfBuffActive(ref damageIncrease))
							{
								if (base.AgeInTurns > 0)
								{
									StopFX();
								}
								m_syncComp.m_swordBuffFinalTurnVfxPending = false;
							}
						}
					}
				}
			}
		}
		goto IL_01c5;
		IL_01c5:
		base.OnUpdate();
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
	}
}
