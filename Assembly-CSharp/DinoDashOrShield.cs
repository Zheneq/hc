using System;
using System.Collections.Generic;
using AbilityContextNamespace;
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
	public Ability.MovementAdjustment m_dashMovementAdjust = Ability.MovementAdjustment.NoMovement;

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
		return base.GetOnHitDataDesc() + "\n-- On Hit Data for dash --\n" + this.m_dashOnHitData.GetInEditorDesc();
	}

	public override List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> relevantTargetSelectCompForEditor = base.GetRelevantTargetSelectCompForEditor();
		if (this.m_targetSelectForDash != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.GetRelevantTargetSelectCompForEditor()).MethodHandle;
			}
			relevantTargetSelectCompForEditor.Add(this.m_targetSelectForDash);
		}
		return relevantTargetSelectCompForEditor;
	}

	public void ResetTargetersForStanceChange()
	{
		this.SetupTargetersAndCachedVars();
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Dino_SyncComponent>();
		this.SetCachedFields();
		base.SetupTargetersAndCachedVars();
		if (base.Targeter is AbilityUtil_Targeter_LaserChargeReverseCones)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.SetupTargetersAndCachedVars()).MethodHandle;
			}
			AbilityUtil_Targeter_LaserChargeReverseCones abilityUtil_Targeter_LaserChargeReverseCones = base.Targeter as AbilityUtil_Targeter_LaserChargeReverseCones;
			GenericAbility_TargetSelectBase targetSelectComp = this.GetTargetSelectComp();
			abilityUtil_Targeter_LaserChargeReverseCones.SetAffectedGroups(targetSelectComp.IncludeEnemies(), targetSelectComp.IncludeAllies(), true);
			abilityUtil_Targeter_LaserChargeReverseCones.m_includeCasterDelegate = new AbilityUtil_Targeter_LaserChargeReverseCones.IncludeCasterDelegate(this.TargeterIncludeCaster);
		}
	}

	private bool TargeterIncludeCaster(ActorData caster, List<ActorData> actorsSoFar)
	{
		bool result;
		if (this.GetShieldPerEnemyHit() > 0)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.TargeterIncludeCaster(ActorData, List<ActorData>)).MethodHandle;
			}
			result = (actorsSoFar.Count > 0);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (this.IsInReadyStance())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.PostProcessTargetingNumbers(ActorData, int, Dictionary<ActorData, ActorHitContext>, ContextVars, ActorData, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (this.GetShieldPerEnemyHit() > 0)
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
				DinoDashOrShield.SetShieldPerEnemyHitTargetingNumbers(targetActor, caster, this.GetShieldPerEnemyHit(), actorHitContext, results);
			}
		}
	}

	public static void SetShieldPerEnemyHitTargetingNumbers(ActorData targetActor, ActorData caster, int shieldPerEnemyHit, Dictionary<ActorData, ActorHitContext> actorHitContext, TargetingNumberUpdateScratch results)
	{
		if (shieldPerEnemyHit > 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.SetShieldPerEnemyHitTargetingNumbers(ActorData, ActorData, int, Dictionary<ActorData, ActorHitContext>, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (targetActor == caster)
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
				int num = 0;
				using (Dictionary<ActorData, ActorHitContext>.Enumerator enumerator = actorHitContext.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ActorData, ActorHitContext> keyValuePair = enumerator.Current;
						ActorData key = keyValuePair.Key;
						if (key.\u000E() != caster.\u000E())
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
							if (keyValuePair.Value.\u0012)
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
								num++;
							}
						}
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (num > 0)
				{
					int num2 = shieldPerEnemyHit * num;
					if (results.m_absorb >= 0)
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
						results.m_absorb += num2;
					}
					else
					{
						results.m_absorb = num2;
					}
				}
			}
		}
	}

	public override bool ShouldUpdateDrawnTargetersOnQueueChange()
	{
		return this.FullyChargeUpLayerCone();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedShieldEffect;
		if (this.m_abilityMod != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.SetCachedFields()).MethodHandle;
			}
			cachedShieldEffect = this.m_abilityMod.m_shieldEffectMod.GetModifiedValue(this.m_shieldEffect);
		}
		else
		{
			cachedShieldEffect = this.m_shieldEffect;
		}
		this.m_cachedShieldEffect = cachedShieldEffect;
		if (this.m_abilityMod != null)
		{
			this.m_cachedDashOnHitData = this.m_abilityMod.m_dashOnHitDataMod.\u001D(this.m_dashOnHitData);
		}
		else
		{
			this.m_cachedDashOnHitData = this.m_dashOnHitData;
		}
	}

	public int GetShieldPerEnemyHit()
	{
		int result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.GetShieldPerEnemyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_shieldPerEnemyHitMod.GetModifiedValue(this.m_shieldPerEnemyHit);
		}
		else
		{
			result = this.m_shieldPerEnemyHit;
		}
		return result;
	}

	public int GetShieldDuration()
	{
		int result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.GetShieldDuration()).MethodHandle;
			}
			result = this.m_abilityMod.m_shieldDurationMod.GetModifiedValue(this.m_shieldDuration);
		}
		else
		{
			result = this.m_shieldDuration;
		}
		return result;
	}

	public StandardEffectInfo GetShieldEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedShieldEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.GetShieldEffect()).MethodHandle;
			}
			result = this.m_cachedShieldEffect;
		}
		else
		{
			result = this.m_shieldEffect;
		}
		return result;
	}

	public int GetHealIfNoDash()
	{
		return (!(this.m_abilityMod != null)) ? this.m_healIfNoDash : this.m_abilityMod.m_healIfNoDashMod.GetModifiedValue(this.m_healIfNoDash);
	}

	public int GetCdrIfNoDash()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.GetCdrIfNoDash()).MethodHandle;
			}
			result = this.m_abilityMod.m_cdrIfNoDashMod.GetModifiedValue(this.m_cdrIfNoDash);
		}
		else
		{
			result = this.m_cdrIfNoDash;
		}
		return result;
	}

	public int GetDelayedCooldown()
	{
		int result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.GetDelayedCooldown()).MethodHandle;
			}
			result = this.m_abilityMod.m_delayedCooldownMod.GetModifiedValue(this.m_delayedCooldown);
		}
		else
		{
			result = this.m_delayedCooldown;
		}
		return result;
	}

	public bool FullyChargeUpLayerCone()
	{
		return (!(this.m_abilityMod != null)) ? this.m_fullyChargeUpLayerCone : this.m_abilityMod.m_fullyChargeUpLayerConeMod.GetModifiedValue(this.m_fullyChargeUpLayerCone);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		this.m_dashOnHitData.AddTooltipTokens(tokens);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_shieldEffect, "ShieldEffect", this.m_shieldEffect, true);
		base.AddTokenInt(tokens, "HealIfNoDash", string.Empty, this.m_healIfNoDash, false);
		base.AddTokenInt(tokens, "CdrIfNoDash", string.Empty, this.m_cdrIfNoDash, false);
		base.AddTokenInt(tokens, "DelayedCooldown", string.Empty, this.m_delayedCooldown, false);
	}

	public override bool UseCustomAbilityIconColor()
	{
		bool result;
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.UseCustomAbilityIconColor()).MethodHandle;
			}
			result = this.m_syncComp.m_dashOrShieldInReadyStance;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override Color GetCustomAbilityIconColor(ActorData actor)
	{
		if (this.UseCustomAbilityIconColor())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.GetCustomAbilityIconColor(ActorData)).MethodHandle;
			}
			return this.m_iconColorWhileActive;
		}
		return base.GetCustomAbilityIconColor(actor);
	}

	public override GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		if (this.IsInReadyStance())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.GetTargetSelectComp()).MethodHandle;
			}
			return this.m_targetSelectForDash;
		}
		return base.GetTargetSelectComp();
	}

	public override AbilityPriority GetRunPriority()
	{
		if (this.IsInReadyStance())
		{
			return AbilityPriority.Evasion;
		}
		return base.GetRunPriority();
	}

	public override Ability.MovementAdjustment GetMovementAdjustment()
	{
		if (this.IsInReadyStance())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.GetMovementAdjustment()).MethodHandle;
			}
			return this.m_dashMovementAdjust;
		}
		return base.GetMovementAdjustment();
	}

	public override TechPointInteraction[] GetBaseTechPointInteractions()
	{
		if (this.IsInReadyStance())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.GetBaseTechPointInteractions()).MethodHandle;
			}
			return this.m_dashTechPointInteractions;
		}
		return base.GetBaseTechPointInteractions();
	}

	public override OnHitAuthoredData GetOnHitAuthoredData()
	{
		if (this.IsInReadyStance())
		{
			return this.m_cachedDashOnHitData;
		}
		return base.GetOnHitAuthoredData();
	}

	public override bool IsFreeAction()
	{
		return !this.IsInReadyStance() && base.IsFreeAction();
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (this.IsInReadyStance())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.GetActionAnimType()).MethodHandle;
			}
			return this.m_dashAnimIndex;
		}
		return base.GetActionAnimType();
	}

	public override int GetCooldownForUIDisplay()
	{
		return this.GetDelayedCooldown();
	}

	public bool IsInReadyStance()
	{
		return this.m_syncComp != null && this.m_syncComp.m_dashOrShieldInReadyStance;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_DinoDashOrShield);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}

	protected override void SetTargetSelectModReference()
	{
		if (this.m_abilityMod != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoDashOrShield.SetTargetSelectModReference()).MethodHandle;
			}
			this.m_targetSelectComp.SetTargetSelectMod(this.m_abilityMod.m_initialCastTargetSelectMod);
			this.m_targetSelectForDash.SetTargetSelectMod(this.m_abilityMod.m_dashTargetSelectMod);
		}
		else
		{
			this.m_targetSelectComp.ClearTargetSelectMod();
			this.m_targetSelectForDash.ClearTargetSelectMod();
		}
	}
}
