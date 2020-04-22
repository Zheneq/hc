using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class DinoDashOrShield : GenericAbility_Container
{
	[Separator("Targeting", true)]
	public Color m_iconColorWhileActive;

	[Separator("[Dash]: Target Select for the second turn if you use the ability again", true)]
	public GenericAbility_TargetSelectBase m_targetSelectForDash;

	[Separator("[Dash]: On Hit Data for second turn, if using ability again to dash", "yellow")]
	public OnHitAuthoredData m_dashOnHitData;

	[Separator("[Dash]: Energy Interactions", true)]
	public TechPointInteraction[] m_dashTechPointInteractions;

	[Separator("[Dash]: Movement Adjust", true)]
	public MovementAdjustment m_dashMovementAdjust = MovementAdjustment.NoMovement;

	[Separator("[Dash]: Shielding per enemy hit", true)]
	public int m_shieldPerEnemyHit;

	public int m_shieldDuration = 1;

	[Separator("For No Dash, applied on end of prep phase", true)]
	public StandardEffectInfo m_shieldEffect;

	public int m_healIfNoDash;

	public int m_cdrIfNoDash;

	[Separator("Cooldown, set on turn after initial cast", true)]
	public int m_delayedCooldown;

	[Separator("Powering up primary", true)]
	public bool m_fullyChargeUpLayerCone;

	[Separator("Animation Index", true)]
	public ActorModelData.ActionAnimationType m_dashAnimIndex;

	public ActorModelData.ActionAnimationType m_noDashShieldAnimIndex;

	[Separator("Sequences", true)]
	public GameObject m_onTriggerSequencePrefab;

	private Dino_SyncComponent m_syncComp;

	private AbilityMod_DinoDashOrShield m_abilityMod;

	private OnHitAuthoredData m_cachedDashOnHitData;

	private StandardEffectInfo m_cachedShieldEffect;

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "\n-- On Hit Data for dash --\n" + m_dashOnHitData.GetInEditorDesc();
	}

	public override List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> relevantTargetSelectCompForEditor = base.GetRelevantTargetSelectCompForEditor();
		if (m_targetSelectForDash != null)
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
			relevantTargetSelectCompForEditor.Add(m_targetSelectForDash);
		}
		return relevantTargetSelectCompForEditor;
	}

	public void ResetTargetersForStanceChange()
	{
		SetupTargetersAndCachedVars();
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Dino_SyncComponent>();
		SetCachedFields();
		base.SetupTargetersAndCachedVars();
		if (!(base.Targeter is AbilityUtil_Targeter_LaserChargeReverseCones))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityUtil_Targeter_LaserChargeReverseCones abilityUtil_Targeter_LaserChargeReverseCones = base.Targeter as AbilityUtil_Targeter_LaserChargeReverseCones;
			GenericAbility_TargetSelectBase targetSelectComp = GetTargetSelectComp();
			abilityUtil_Targeter_LaserChargeReverseCones.SetAffectedGroups(targetSelectComp.IncludeEnemies(), targetSelectComp.IncludeAllies(), true);
			abilityUtil_Targeter_LaserChargeReverseCones.m_includeCasterDelegate = TargeterIncludeCaster;
			return;
		}
	}

	private bool TargeterIncludeCaster(ActorData caster, List<ActorData> actorsSoFar)
	{
		int result;
		if (GetShieldPerEnemyHit() > 0)
		{
			while (true)
			{
				switch (1)
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
			result = ((actorsSoFar.Count > 0) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (!IsInReadyStance())
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GetShieldPerEnemyHit() > 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					SetShieldPerEnemyHitTargetingNumbers(targetActor, caster, GetShieldPerEnemyHit(), actorHitContext, results);
					return;
				}
			}
			return;
		}
	}

	public static void SetShieldPerEnemyHitTargetingNumbers(ActorData targetActor, ActorData caster, int shieldPerEnemyHit, Dictionary<ActorData, ActorHitContext> actorHitContext, TargetingNumberUpdateScratch results)
	{
		if (shieldPerEnemyHit <= 0)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(targetActor == caster))
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				int num = 0;
				using (Dictionary<ActorData, ActorHitContext>.Enumerator enumerator = actorHitContext.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ActorData, ActorHitContext> current = enumerator.Current;
						ActorData key = current.Key;
						if (key.GetTeam() != caster.GetTeam())
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (current.Value._0012)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								num++;
							}
						}
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (num <= 0)
				{
					return;
				}
				int num2 = shieldPerEnemyHit * num;
				if (results.m_absorb >= 0)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							results.m_absorb += num2;
							return;
						}
					}
				}
				results.m_absorb = num2;
				return;
			}
		}
	}

	public override bool ShouldUpdateDrawnTargetersOnQueueChange()
	{
		return FullyChargeUpLayerCone();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedShieldEffect;
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (7)
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
			cachedShieldEffect = m_abilityMod.m_shieldEffectMod.GetModifiedValue(m_shieldEffect);
		}
		else
		{
			cachedShieldEffect = m_shieldEffect;
		}
		m_cachedShieldEffect = cachedShieldEffect;
		if (m_abilityMod != null)
		{
			m_cachedDashOnHitData = m_abilityMod.m_dashOnHitDataMod._001D(m_dashOnHitData);
		}
		else
		{
			m_cachedDashOnHitData = m_dashOnHitData;
		}
	}

	public int GetShieldPerEnemyHit()
	{
		int result;
		if (m_abilityMod != null)
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
			result = m_abilityMod.m_shieldDurationMod.GetModifiedValue(m_shieldDuration);
		}
		else
		{
			result = m_shieldDuration;
		}
		return result;
	}

	public StandardEffectInfo GetShieldEffect()
	{
		StandardEffectInfo result;
		if (m_cachedShieldEffect != null)
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
			result = m_cachedShieldEffect;
		}
		else
		{
			result = m_shieldEffect;
		}
		return result;
	}

	public int GetHealIfNoDash()
	{
		return (!(m_abilityMod != null)) ? m_healIfNoDash : m_abilityMod.m_healIfNoDashMod.GetModifiedValue(m_healIfNoDash);
	}

	public int GetCdrIfNoDash()
	{
		int result;
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (5)
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
			result = m_abilityMod.m_cdrIfNoDashMod.GetModifiedValue(m_cdrIfNoDash);
		}
		else
		{
			result = m_cdrIfNoDash;
		}
		return result;
	}

	public int GetDelayedCooldown()
	{
		int result;
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (4)
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
			result = m_abilityMod.m_delayedCooldownMod.GetModifiedValue(m_delayedCooldown);
		}
		else
		{
			result = m_delayedCooldown;
		}
		return result;
	}

	public bool FullyChargeUpLayerCone()
	{
		return (!(m_abilityMod != null)) ? m_fullyChargeUpLayerCone : m_abilityMod.m_fullyChargeUpLayerConeMod.GetModifiedValue(m_fullyChargeUpLayerCone);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_dashOnHitData.AddTooltipTokens(tokens);
		AbilityMod.AddToken_EffectInfo(tokens, m_shieldEffect, "ShieldEffect", m_shieldEffect);
		AddTokenInt(tokens, "HealIfNoDash", string.Empty, m_healIfNoDash);
		AddTokenInt(tokens, "CdrIfNoDash", string.Empty, m_cdrIfNoDash);
		AddTokenInt(tokens, "DelayedCooldown", string.Empty, m_delayedCooldown);
	}

	public override bool UseCustomAbilityIconColor()
	{
		int result;
		if (m_syncComp != null)
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
			result = (m_syncComp.m_dashOrShieldInReadyStance ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override Color GetCustomAbilityIconColor(ActorData actor)
	{
		if (UseCustomAbilityIconColor())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_iconColorWhileActive;
				}
			}
		}
		return base.GetCustomAbilityIconColor(actor);
	}

	public override GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		if (IsInReadyStance())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_targetSelectForDash;
				}
			}
		}
		return base.GetTargetSelectComp();
	}

	public override AbilityPriority GetRunPriority()
	{
		if (IsInReadyStance())
		{
			return AbilityPriority.Evasion;
		}
		return base.GetRunPriority();
	}

	public override MovementAdjustment GetMovementAdjustment()
	{
		if (IsInReadyStance())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_dashMovementAdjust;
				}
			}
		}
		return base.GetMovementAdjustment();
	}

	public override TechPointInteraction[] GetBaseTechPointInteractions()
	{
		if (IsInReadyStance())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_dashTechPointInteractions;
				}
			}
		}
		return base.GetBaseTechPointInteractions();
	}

	public override OnHitAuthoredData GetOnHitAuthoredData()
	{
		if (IsInReadyStance())
		{
			return m_cachedDashOnHitData;
		}
		return base.GetOnHitAuthoredData();
	}

	public override bool IsFreeAction()
	{
		if (IsInReadyStance())
		{
			return false;
		}
		return base.IsFreeAction();
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (IsInReadyStance())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_dashAnimIndex;
				}
			}
		}
		return base.GetActionAnimType();
	}

	public override int GetCooldownForUIDisplay()
	{
		return GetDelayedCooldown();
	}

	public bool IsInReadyStance()
	{
		return m_syncComp != null && m_syncComp.m_dashOrShieldInReadyStance;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_DinoDashOrShield);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}

	protected override void SetTargetSelectModReference()
	{
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_targetSelectComp.SetTargetSelectMod(m_abilityMod.m_initialCastTargetSelectMod);
					m_targetSelectForDash.SetTargetSelectMod(m_abilityMod.m_dashTargetSelectMod);
					return;
				}
			}
		}
		m_targetSelectComp.ClearTargetSelectMod();
		m_targetSelectForDash.ClearTargetSelectMod();
	}
}
