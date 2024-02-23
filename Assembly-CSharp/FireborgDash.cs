using AbilityContextNamespace;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FireborgDash : GenericAbility_Container
{
	[Separator("Whether to add ground fire effect", true)]
	public bool m_addGroundFire = true;

	public int m_groundFireDuration = 1;

	public int m_groundFireDurationIfSuperheated = 1;

	public bool m_igniteIfNormal;

	public bool m_igniteIfSuperheated = true;

	[Separator("Shield per Enemy Hit", true)]
	public int m_shieldPerEnemyHit;

	public int m_shieldDuration = 1;

	[Separator("Cooldown Reduction", true)]
	public int m_cdrPerTurnIfLowHealth;

	public int m_lowHealthThresh;

	[Separator("Sequence", true)]
	public GameObject m_superheatedCastSeqPrefab;

	private Fireborg_SyncComponent m_syncComp;

	private AbilityMod_FireborgDash m_abilityMod;

	public override string GetUsageForEditor()
	{
		string usageForEditor = base.GetUsageForEditor();
		return new StringBuilder().Append(usageForEditor).Append(Fireborg_SyncComponent.GetSuperheatedCvarUsage()).ToString();
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(Fireborg_SyncComponent.s_cvarSuperheated.GetName());
		return contextNamesForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Fireborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "GroundFireDuration", string.Empty, m_groundFireDuration);
		AddTokenInt(tokens, "GroundFireDurationIfSuperheated", string.Empty, m_groundFireDurationIfSuperheated);
		AddTokenInt(tokens, "ShieldPerEnemyHit", string.Empty, m_shieldPerEnemyHit);
		AddTokenInt(tokens, "ShieldDuration", string.Empty, m_shieldDuration);
		AddTokenInt(tokens, "CdrPerTurnIfLowHealth", string.Empty, m_cdrPerTurnIfLowHealth);
		AddTokenInt(tokens, "LowHealthThresh", string.Empty, m_lowHealthThresh);
	}

	public bool AddGroundFire()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_addGroundFireMod.GetModifiedValue(m_addGroundFire);
		}
		else
		{
			result = m_addGroundFire;
		}
		return result;
	}

	public int GetGroundFireDuration()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_groundFireDurationMod.GetModifiedValue(m_groundFireDuration);
		}
		else
		{
			result = m_groundFireDuration;
		}
		return result;
	}

	public int GetGroundFireDurationIfSuperheated()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_groundFireDurationIfSuperheatedMod.GetModifiedValue(m_groundFireDurationIfSuperheated);
		}
		else
		{
			result = m_groundFireDurationIfSuperheated;
		}
		return result;
	}

	public bool IgniteIfNormal()
	{
		return (!(m_abilityMod != null)) ? m_igniteIfNormal : m_abilityMod.m_igniteIfNormalMod.GetModifiedValue(m_igniteIfNormal);
	}

	public bool IgniteIfSuperheated()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_igniteIfSuperheatedMod.GetModifiedValue(m_igniteIfSuperheated);
		}
		else
		{
			result = m_igniteIfSuperheated;
		}
		return result;
	}

	public int GetShieldPerEnemyHit()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_shieldPerEnemyHitMod.GetModifiedValue(m_shieldPerEnemyHit);
		}
		else
		{
			result = m_shieldPerEnemyHit;
		}
		return result;
	}

	public int GetShieldDuration()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_shieldDurationMod.GetModifiedValue(m_shieldDuration);
		}
		else
		{
			result = m_shieldDuration;
		}
		return result;
	}

	public int GetCdrPerTurnIfLowHealth()
	{
		return (!(m_abilityMod != null)) ? m_cdrPerTurnIfLowHealth : m_abilityMod.m_cdrPerTurnIfLowHealthMod.GetModifiedValue(m_cdrPerTurnIfLowHealth);
	}

	public int GetLowHealthThresh()
	{
		return (!(m_abilityMod != null)) ? m_lowHealthThresh : m_abilityMod.m_lowHealthThreshMod.GetModifiedValue(m_lowHealthThresh);
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
	{
		m_syncComp.SetSuperheatedContextVar(abilityContext);
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		IceborgConeOrLaser.SetShieldPerEnemyHitTargetingNumbers(targetActor, caster, GetShieldPerEnemyHit(), actorHitContext, results);
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (AddGroundFire())
		{
			if (!m_syncComp.m_actorsInGroundFireOnTurnStart.Contains((uint)targetActor.ActorIndex))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
					}
				}
			}
		}
		return null;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_FireborgDash);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
