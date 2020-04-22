using AbilityContextNamespace;
using System.Collections.Generic;

public class GenericAbility_Container : Ability
{
	[Separator("Target Select Component", true)]
	public GenericAbility_TargetSelectBase m_targetSelectComp;

	[Separator("On Hit Authored Data", "yellow")]
	public OnHitAuthoredData m_onHitData;

	protected NumericHitResultScratch m_calculatedValuesForTargeter = new NumericHitResultScratch();

	protected OnHitAuthoredData m_cachedOnHitData;

	public virtual string GetUsageForEditor()
	{
		if (m_targetSelectComp != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_targetSelectComp.GetUsageForEditor();
				}
			}
		}
		return string.Empty;
	}

	public virtual string GetOnHitDataDesc()
	{
		if (m_onHitData != null)
		{
			return m_onHitData.GetInEditorDesc();
		}
		return string.Empty;
	}

	public virtual List<string> GetContextNamesForEditor()
	{
		List<string> list = new List<string>();
		if (m_targetSelectComp != null)
		{
			m_targetSelectComp.ListContextNamesForEditor(list);
		}
		return list;
	}

	public virtual List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> list = new List<GenericAbility_TargetSelectBase>();
		if (m_targetSelectComp != null)
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
			list.Add(m_targetSelectComp);
		}
		return list;
	}

	private void Start()
	{
		SetupTargetersAndCachedVars();
	}

	protected virtual void SetupTargetersAndCachedVars()
	{
		ClearTargeters();
		if (GetTargetSelectComp() != null)
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
			GetTargetSelectComp().Initialize();
			List<AbilityUtil_Targeter> list = GetTargetSelectComp().CreateTargeters(this);
			using (List<AbilityUtil_Targeter>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityUtil_Targeter current = enumerator.Current;
					base.Targeters.Add(current);
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (base.CurrentAbilityMod != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_cachedOnHitData = base.CurrentAbilityMod.GenModImpl_GetModdedOnHitData(m_onHitData);
					return;
				}
			}
		}
		m_cachedOnHitData = m_onHitData;
	}

	public virtual OnHitAuthoredData GetOnHitAuthoredData()
	{
		return (m_cachedOnHitData == null) ? m_onHitData : m_cachedOnHitData;
	}

	public virtual GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		return m_targetSelectComp;
	}

	public override TargetData[] GetBaseTargetData()
	{
		GenericAbility_TargetSelectBase targetSelectComp = GetTargetSelectComp();
		if (targetSelectComp != null)
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
			if (targetSelectComp.m_useTargetDataOverride)
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
				if (targetSelectComp.GetTargetDataOverride() != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return targetSelectComp.GetTargetDataOverride();
						}
					}
				}
			}
		}
		return base.GetBaseTargetData();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		m_onHitData.AddTooltipTokens(tokens);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		if (GetTargetSelectComp() != null)
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
					return GetTargetSelectComp().CanShowTargeterRangePreview(GetTargetData());
				}
			}
		}
		return base.CanShowTargetableRadiusPreview();
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		if (GetTargetSelectComp() != null)
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
					return GetTargetSelectComp().GetTargeterRangePreviewRadius(this, caster);
				}
			}
		}
		return base.GetTargetableRadiusInSquares(caster);
	}

	protected virtual void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		Log.Error("Please implement GenModImpl_SetModRef in derived class " + GetType());
	}

	protected virtual void GenModImpl_ClearModRef()
	{
		Log.Error("Please implement GenModImpl_ClearModRef in derived class " + GetType());
	}

	protected virtual void SetTargetSelectModReference()
	{
		if (!(m_targetSelectComp != null))
		{
			return;
		}
		if (base.CurrentAbilityMod != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					base.CurrentAbilityMod.GenModImpl_SetTargetSelectMod(m_targetSelectComp);
					return;
				}
			}
		}
		m_targetSelectComp.ClearTargetSelectMod();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		GenModImpl_SetModRef(abilityMod);
		SetTargetSelectModReference();
		SetupTargetersAndCachedVars();
	}

	protected override void OnRemoveAbilityMod()
	{
		GenModImpl_ClearModRef();
		SetTargetSelectModReference();
		SetupTargetersAndCachedVars();
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (GetTargetSelectComp() != null)
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
					return GetTargetSelectComp().HandleCanCastValidation(this, caster);
				}
			}
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (GetTargetSelectComp() != null)
		{
			return GetTargetSelectComp().HandleCustomTargetValidation(this, caster, target, targetIndex, currentTargets);
		}
		return true;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		ActorData actorData = base.ActorData;
		if (actorData != null)
		{
			GetHitContextForTargetingNumbers(currentTargeterIndex, out Dictionary<ActorData, ActorHitContext> actorHitContext, out ContextVars abilityContext);
			if (actorHitContext.ContainsKey(targetActor))
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
						PreProcessTargetingNumbers(targetActor, currentTargeterIndex, actorHitContext, abilityContext);
						m_calculatedValuesForTargeter.Reset();
						if (actorData.GetTeam() == targetActor.GetTeam())
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
							CalcIntFieldValues(targetActor, actorData, actorHitContext[targetActor], abilityContext, GetOnHitAuthoredData().m_allyHitIntFields, m_calculatedValuesForTargeter);
							results.m_absorb = CalcAbsorbFromEffectFields(targetActor, actorData, actorHitContext[targetActor], abilityContext, GetOnHitAuthoredData().m_allyHitEffectFields);
						}
						else
						{
							CalcIntFieldValues(targetActor, actorData, actorHitContext[targetActor], abilityContext, GetOnHitAuthoredData().m_enemyHitIntFields, m_calculatedValuesForTargeter);
							results.m_absorb = CalcAbsorbFromEffectFields(targetActor, actorData, actorHitContext[targetActor], abilityContext, GetOnHitAuthoredData().m_enemyHitEffectFields);
						}
						results.m_damage = m_calculatedValuesForTargeter.m_damage;
						results.m_healing = m_calculatedValuesForTargeter.m_healing;
						results.m_energy = m_calculatedValuesForTargeter.m_energyGain;
						PostProcessTargetingNumbers(targetActor, currentTargeterIndex, actorHitContext, abilityContext, actorData, results);
						return true;
					}
				}
			}
		}
		return false;
	}

	public virtual void GetHitContextForTargetingNumbers(int currentTargeterIndex, out Dictionary<ActorData, ActorHitContext> actorHitContext, out ContextVars abilityContext)
	{
		actorHitContext = base.Targeter.GetActorContextVars();
		abilityContext = base.Targeter.GetNonActorSpecificContext();
	}

	public virtual void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
	{
	}

	public virtual void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
	}

	internal override ActorData.MovementType GetMovementType()
	{
		if (GetTargetSelectComp() != null)
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
					return GetTargetSelectComp().GetMovementType();
				}
			}
		}
		return base.GetMovementType();
	}

	public static void CalcIntFieldValues(ActorData targetActor, ActorData caster, ActorHitContext actorContext, ContextVars abilityContext, List<OnHitIntField> intFields, NumericHitResultScratch result)
	{
		result.Reset();
		using (List<OnHitIntField>.Enumerator enumerator = intFields.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				OnHitIntField current = enumerator.Current;
				if (TargetFilterHelper._001D(current.m_conditions, targetActor, caster, actorContext, abilityContext))
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					int num = current.CalcValue(actorContext, abilityContext);
					if (current.m_hitType == OnHitIntField.HitType.Damage)
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
						if (result.m_damage == 0)
						{
							result.m_damage = num;
						}
					}
					else if (current.m_hitType == OnHitIntField.HitType.Healing)
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
						if (result.m_healing == 0)
						{
							result.m_healing = num;
						}
					}
					else if (current.m_hitType == OnHitIntField.HitType.EnergyChange)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (num > 0)
						{
							if (result.m_energyGain == 0)
							{
								result.m_energyGain = num;
							}
						}
						else if (num < 0 && result.m_energyLoss == 0)
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
							result.m_energyLoss = -1 * num;
						}
					}
				}
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	protected static int CalcAbsorbFromEffectFields(ActorData targetActor, ActorData caster, ActorHitContext actorContext, ContextVars abilityContext, List<OnHitEffecField> effectFields)
	{
		int num = 0;
		foreach (OnHitEffecField effectField in effectFields)
		{
			if (TargetFilterHelper._001D(effectField.m_conditions, targetActor, caster, actorContext, abilityContext))
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
				if (effectField.m_effect.m_applyEffect)
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
					if (effectField.m_effect.m_effectData.m_absorbAmount > 0)
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
						num += effectField.m_effect.m_effectData.m_absorbAmount;
					}
				}
			}
		}
		return num;
	}
}
