using UnityEngine;

public class IceborgNovaCoreSequence : SimpleAttachedVFXOnTargetSequence
{
	[Separator("Active Version of Nova Core Vfx", true)]
	public GameObject m_activeNovaFxPrefab;

	[Separator("Empowered Vfx (when triggered by Detonate ability)", true)]
	public GameObject m_empoweredFxPrefab;

	private Iceborg_SyncComponent m_syncComp;

	private bool m_switchedToEmpowered;

	private bool m_switchedToActive;

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
			if (Application.isEditor)
			{
				Debug.LogError(string.Concat(GetType(), " did not find sync component on caster"));
			}
			return;
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			if (!(m_syncComp != null))
			{
				return;
			}
			while (true)
			{
				if (!m_switchedToEmpowered)
				{
					if (m_empoweredFxPrefab != null)
					{
						ActorData target = base.Target;
						AbilityPriority currentAbilityPhase = ServerClientUtils.GetCurrentAbilityPhase();
						if (GameFlowData.Get().CurrentTurn == m_syncComp.m_clientDetonateNovaUsedTurn)
						{
							if (currentAbilityPhase < AbilityPriority.Evasion)
							{
								if (target != null && m_syncComp.HasNovaCore(target))
								{
									SwitchFxTo(m_empoweredFxPrefab);
									m_switchedToEmpowered = true;
								}
							}
						}
					}
				}
				if (m_switchedToActive)
				{
					return;
				}
				while (true)
				{
					if (m_switchedToEmpowered)
					{
						return;
					}
					while (true)
					{
						if (!(m_activeNovaFxPrefab != null))
						{
							return;
						}
						while (true)
						{
							if (base.AgeInTurns > 0)
							{
								SwitchFxTo(m_activeNovaFxPrefab);
								m_switchedToActive = true;
							}
							return;
						}
					}
				}
			}
		}
	}
}
