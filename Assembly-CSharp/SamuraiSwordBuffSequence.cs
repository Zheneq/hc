// ROGUES
// SERVER
using UnityEngine;

// identical in reactor and rogues
public class SamuraiSwordBuffSequence : SimpleAttachedVFXSequence
{
	[Separator("Samurai-specific FX Prefab for Decision of the turn buff becomes active")]
	public GameObject m_fxPrefabForActiveBuff;
	[AudioEvent(false)]
	public string m_onSwordActivateAudioEvent = string.Empty;

	private Samurai_SyncComponent m_syncComp;
	private bool m_switchedToActiveBuffFx;

	public override void FinishSetup()
	{
		m_syncComp = Caster.GetComponent<Samurai_SyncComponent>();
		if (m_syncComp == null && Application.isEditor)
		{
			Debug.LogError(GetType() + " did not find sync component on caster");
		}
	}

	protected override void OnUpdate()
	{
		if (m_initialized && m_syncComp != null)
		{
			int damageIncrease = 0;
			if (m_syncComp.m_swordBuffVfxPending)
			{
				if (m_fx == null && AgeInTurns <= 0)
				{
					SpawnFX();
				}
				m_syncComp.m_swordBuffVfxPending = false;
			}
			else if (!m_switchedToActiveBuffFx
			         && m_syncComp.IsSelfBuffActive(ref damageIncrease)
			         && GameFlowData.Get().IsInDecisionState()
			         && m_fxPrefabForActiveBuff != null)
			{
				StopFX();
				SpawnFX(m_fxPrefabForActiveBuff);
				if (!string.IsNullOrEmpty(m_onSwordActivateAudioEvent))
				{
					GameObject casterGameObject = null;
					if (Caster != null)
					{
						casterGameObject = Caster.gameObject;
					}
					if (casterGameObject != null)
					{
						AudioManager.PostEvent(m_onSwordActivateAudioEvent, casterGameObject);
					}
				}
				m_switchedToActiveBuffFx = true;
			}
			else if (m_syncComp.m_swordBuffFinalTurnVfxPending
			         && m_switchedToActiveBuffFx
			         && !m_syncComp.IsSelfBuffActive(ref damageIncrease))
			{
				if (AgeInTurns > 0)
				{
					StopFX();
				}
				m_syncComp.m_swordBuffFinalTurnVfxPending = false;
			}
		}
		base.OnUpdate();
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
	}
}
