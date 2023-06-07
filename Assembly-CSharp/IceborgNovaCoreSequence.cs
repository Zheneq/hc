using UnityEngine;

public class IceborgNovaCoreSequence : SimpleAttachedVFXOnTargetSequence
{
	[Separator("Active Version of Nova Core Vfx")]
	public GameObject m_activeNovaFxPrefab;
	[Separator("Empowered Vfx (when triggered by Detonate ability)")]
	public GameObject m_empoweredFxPrefab;

	private Iceborg_SyncComponent m_syncComp;
	private bool m_switchedToEmpowered;
	private bool m_switchedToActive;

	public override void FinishSetup()
	{
		base.FinishSetup();
		m_syncComp = Caster.GetComponent<Iceborg_SyncComponent>();
		if (m_syncComp == null && Application.isEditor)
		{
			Debug.LogError(string.Concat(GetType(), " did not find sync component on caster"));
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (!m_initialized || m_syncComp == null)
		{
			return;
		}
		if (!m_switchedToEmpowered
		    && m_empoweredFxPrefab != null
		    && GameFlowData.Get().CurrentTurn == m_syncComp.m_clientDetonateNovaUsedTurn 
		    && ServerClientUtils.GetCurrentAbilityPhase() < AbilityPriority.Evasion 
		    && Target != null
		    && m_syncComp.HasNovaCore(Target))
		{
			SwitchFxTo(m_empoweredFxPrefab);
			m_switchedToEmpowered = true;
		}
		if (!m_switchedToActive
		    && !m_switchedToEmpowered
		    && m_activeNovaFxPrefab != null
		    && AgeInTurns > 0)
		{
			SwitchFxTo(m_activeNovaFxPrefab);
			m_switchedToActive = true;
		}
	}
}
