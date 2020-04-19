using System;
using System.Collections.Generic;
using AbilityContextNamespace;

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
		if (this.m_targetSelectComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_Container.GetUsageForEditor()).MethodHandle;
			}
			return this.m_targetSelectComp.GetUsageForEditor();
		}
		return string.Empty;
	}

	public virtual string GetOnHitDataDesc()
	{
		if (this.m_onHitData != null)
		{
			return this.m_onHitData.GetInEditorDesc();
		}
		return string.Empty;
	}

	public virtual List<string> GetContextNamesForEditor()
	{
		List<string> list = new List<string>();
		if (this.m_targetSelectComp != null)
		{
			this.m_targetSelectComp.ListContextNamesForEditor(list);
		}
		return list;
	}

	public virtual List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> list = new List<GenericAbility_TargetSelectBase>();
		if (this.m_targetSelectComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_Container.GetRelevantTargetSelectCompForEditor()).MethodHandle;
			}
			list.Add(this.m_targetSelectComp);
		}
		return list;
	}

	private void Start()
	{
		this.SetupTargetersAndCachedVars();
	}

	protected virtual void SetupTargetersAndCachedVars()
	{
		base.ClearTargeters();
		if (this.GetTargetSelectComp() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_Container.SetupTargetersAndCachedVars()).MethodHandle;
			}
			this.GetTargetSelectComp().Initialize();
			List<AbilityUtil_Targeter> list = this.GetTargetSelectComp().CreateTargeters(this);
			using (List<AbilityUtil_Targeter>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityUtil_Targeter item = enumerator.Current;
					base.Targeters.Add(item);
				}
				for (;;)
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_cachedOnHitData = base.CurrentAbilityMod.GenModImpl_GetModdedOnHitData(this.m_onHitData);
		}
		else
		{
			this.m_cachedOnHitData = this.m_onHitData;
		}
	}

	public virtual OnHitAuthoredData GetOnHitAuthoredData()
	{
		return (this.m_cachedOnHitData == null) ? this.m_onHitData : this.m_cachedOnHitData;
	}

	public virtual GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		return this.m_targetSelectComp;
	}

	public override TargetData[] GetBaseTargetData()
	{
		GenericAbility_TargetSelectBase targetSelectComp = this.GetTargetSelectComp();
		if (targetSelectComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_Container.GetBaseTargetData()).MethodHandle;
			}
			if (targetSelectComp.m_useTargetDataOverride)
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
				if (targetSelectComp.GetTargetDataOverride() != null)
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
					return targetSelectComp.GetTargetDataOverride();
				}
			}
		}
		return base.GetBaseTargetData();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		this.m_onHitData.AddTooltipTokens(tokens);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		if (this.GetTargetSelectComp() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_Container.CanShowTargetableRadiusPreview()).MethodHandle;
			}
			return this.GetTargetSelectComp().CanShowTargeterRangePreview(this.GetTargetData());
		}
		return base.CanShowTargetableRadiusPreview();
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		if (this.GetTargetSelectComp() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_Container.GetTargetableRadiusInSquares(ActorData)).MethodHandle;
			}
			return this.GetTargetSelectComp().GetTargeterRangePreviewRadius(this, caster);
		}
		return base.GetTargetableRadiusInSquares(caster);
	}

	protected virtual void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		Log.Error("Please implement GenModImpl_SetModRef in derived class " + base.GetType(), new object[0]);
	}

	protected virtual void GenModImpl_ClearModRef()
	{
		Log.Error("Please implement GenModImpl_ClearModRef in derived class " + base.GetType(), new object[0]);
	}

	protected virtual void SetTargetSelectModReference()
	{
		if (this.m_targetSelectComp != null)
		{
			if (base.CurrentAbilityMod != null)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_Container.SetTargetSelectModReference()).MethodHandle;
				}
				base.CurrentAbilityMod.GenModImpl_SetTargetSelectMod(this.m_targetSelectComp);
			}
			else
			{
				this.m_targetSelectComp.ClearTargetSelectMod();
			}
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		this.GenModImpl_SetModRef(abilityMod);
		this.SetTargetSelectModReference();
		this.SetupTargetersAndCachedVars();
	}

	protected override void OnRemoveAbilityMod()
	{
		this.GenModImpl_ClearModRef();
		this.SetTargetSelectModReference();
		this.SetupTargetersAndCachedVars();
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.GetTargetSelectComp() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_Container.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			return this.GetTargetSelectComp().HandleCanCastValidation(this, caster);
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return !(this.GetTargetSelectComp() != null) || this.GetTargetSelectComp().HandleCustomTargetValidation(this, caster, target, targetIndex, currentTargets);
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		ActorData actorData = base.ActorData;
		if (actorData != null)
		{
			Dictionary<ActorData, ActorHitContext> dictionary;
			ContextVars abilityContext;
			this.GetHitContextForTargetingNumbers(currentTargeterIndex, out dictionary, out abilityContext);
			if (dictionary.ContainsKey(targetActor))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_Container.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
				}
				this.PreProcessTargetingNumbers(targetActor, currentTargeterIndex, dictionary, abilityContext);
				this.m_calculatedValuesForTargeter.Reset();
				if (actorData.\u000E() == targetActor.\u000E())
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
					GenericAbility_Container.CalcIntFieldValues(targetActor, actorData, dictionary[targetActor], abilityContext, this.GetOnHitAuthoredData().m_allyHitIntFields, this.m_calculatedValuesForTargeter);
					results.m_absorb = GenericAbility_Container.CalcAbsorbFromEffectFields(targetActor, actorData, dictionary[targetActor], abilityContext, this.GetOnHitAuthoredData().m_allyHitEffectFields);
				}
				else
				{
					GenericAbility_Container.CalcIntFieldValues(targetActor, actorData, dictionary[targetActor], abilityContext, this.GetOnHitAuthoredData().m_enemyHitIntFields, this.m_calculatedValuesForTargeter);
					results.m_absorb = GenericAbility_Container.CalcAbsorbFromEffectFields(targetActor, actorData, dictionary[targetActor], abilityContext, this.GetOnHitAuthoredData().m_enemyHitEffectFields);
				}
				results.m_damage = this.m_calculatedValuesForTargeter.m_damage;
				results.m_healing = this.m_calculatedValuesForTargeter.m_healing;
				results.m_energy = this.m_calculatedValuesForTargeter.m_energyGain;
				this.PostProcessTargetingNumbers(targetActor, currentTargeterIndex, dictionary, abilityContext, actorData, results);
				return true;
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
		if (this.GetTargetSelectComp() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_Container.GetMovementType()).MethodHandle;
			}
			return this.GetTargetSelectComp().GetMovementType();
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
				OnHitIntField onHitIntField = enumerator.Current;
				if (TargetFilterHelper.\u001D(onHitIntField.m_conditions, targetActor, caster, actorContext, abilityContext))
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_Container.CalcIntFieldValues(ActorData, ActorData, ActorHitContext, ContextVars, List<OnHitIntField>, NumericHitResultScratch)).MethodHandle;
					}
					int num = onHitIntField.CalcValue(actorContext, abilityContext);
					if (onHitIntField.m_hitType == OnHitIntField.HitType.Damage)
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
						if (result.m_damage == 0)
						{
							result.m_damage = num;
						}
					}
					else if (onHitIntField.m_hitType == OnHitIntField.HitType.Healing)
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
						if (result.m_healing == 0)
						{
							result.m_healing = num;
						}
					}
					else if (onHitIntField.m_hitType == OnHitIntField.HitType.EnergyChange)
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
						if (num > 0)
						{
							if (result.m_energyGain == 0)
							{
								result.m_energyGain = num;
							}
						}
						else if (num < 0 && result.m_energyLoss == 0)
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
							result.m_energyLoss = -1 * num;
						}
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
	}

	protected static int CalcAbsorbFromEffectFields(ActorData targetActor, ActorData caster, ActorHitContext actorContext, ContextVars abilityContext, List<OnHitEffecField> effectFields)
	{
		int num = 0;
		foreach (OnHitEffecField onHitEffecField in effectFields)
		{
			if (TargetFilterHelper.\u001D(onHitEffecField.m_conditions, targetActor, caster, actorContext, abilityContext))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_Container.CalcAbsorbFromEffectFields(ActorData, ActorData, ActorHitContext, ContextVars, List<OnHitEffecField>)).MethodHandle;
				}
				if (onHitEffecField.m_effect.m_applyEffect)
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
					if (onHitEffecField.m_effect.m_effectData.m_absorbAmount > 0)
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
						num += onHitEffecField.m_effect.m_effectData.m_absorbAmount;
					}
				}
			}
		}
		return num;
	}
}
