using System;
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
		this.m_syncComp = base.Caster.GetComponent<Iceborg_SyncComponent>();
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
		base.OnUpdate();
		if (this.m_initialized)
		{
			if (this.m_syncComp != null)
			{
				if (!this.m_switchedToEmpowered)
				{
					if (this.m_empoweredFxPrefab != null)
					{
						ActorData target = base.Target;
						AbilityPriority currentAbilityPhase = ServerClientUtils.GetCurrentAbilityPhase();
						if (GameFlowData.Get().CurrentTurn == this.m_syncComp.m_clientDetonateNovaUsedTurn)
						{
							if (currentAbilityPhase < AbilityPriority.Evasion)
							{
								if (target != null && this.m_syncComp.HasNovaCore(target))
								{
									base.SwitchFxTo(this.m_empoweredFxPrefab);
									this.m_switchedToEmpowered = true;
								}
							}
						}
					}
				}
				if (!this.m_switchedToActive)
				{
					if (!this.m_switchedToEmpowered)
					{
						if (this.m_activeNovaFxPrefab != null)
						{
							if (base.AgeInTurns > 0)
							{
								base.SwitchFxTo(this.m_activeNovaFxPrefab);
								this.m_switchedToActive = true;
							}
						}
					}
				}
			}
		}
	}
}
