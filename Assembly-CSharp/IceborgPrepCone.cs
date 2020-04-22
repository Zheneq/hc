using AbilityContextNamespace;
using System.Collections.Generic;

public class IceborgPrepCone : GenericAbility_Container
{
	[Separator("Shielding per enemy hit on cast", true)]
	public int m_shieldPerEnemyHit;

	public int m_shieldDuration = 1;

	[Separator("Apply Nova effect?", true)]
	public bool m_applyDelayedAoeEffect = true;

	private Iceborg_SyncComponent m_syncComp;

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		if (m_syncComp == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_syncComp = GetComponent<Iceborg_SyncComponent>();
		}
		if (!(m_syncComp != null))
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
			m_syncComp.AddTooltipTokens(tokens);
			return;
		}
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		IceborgConeOrLaser.SetShieldPerEnemyHitTargetingNumbers(targetActor, caster, m_shieldPerEnemyHit, actorHitContext, results);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
	}

	protected override void GenModImpl_ClearModRef()
	{
	}
}
