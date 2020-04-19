using System;
using System.Collections.Generic;
using UnityEngine;

public class MantaCreateBarriers : Ability
{
	[Header("-- Whether require Manta to be inside target area --")]
	public bool m_requireCasterInShape = true;

	public AbilityAreaShape m_targetAreaShape = AbilityAreaShape.Five_x_Five;

	[Header("-- Barriers")]
	[Separator("NOTE: you can also use MantaCreateBarriersChainFinal for damage stuff!", true)]
	public bool m_delayBarriersUntilStartOfNextTurn;

	public int m_prisonSides = 8;

	public float m_prisonRadius = 3.5f;

	public StandardBarrierData m_prisonBarrierData;

	public AbilityAreaShape m_shapeForTargeter = AbilityAreaShape.Seven_x_Seven;

	[Tooltip("WARNING: don't do this if it's a Blast phase ability unless the walls don't block abilities")]
	public bool m_createBarriersImmediately;

	[Header("-- Ground effect")]
	public StandardGroundEffectInfo m_groundEffectInfo;

	public int m_damageOnCast = 0x1E;

	[Header("-- On Cast Ally Hit (applies to caster as well)")]
	public int m_allyHealOnCast;

	public StandardEffectInfo m_effectOnAlliesOnCast;

	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private Manta_SyncComponent m_syncComp;

	private AbilityMod_MantaCreateBarriers m_abilityMod;

	private MantaCreateBarriersChainFinal m_finalDamageChain;

	private StandardBarrierData m_cachedPrisonBarrierData;

	private StandardEffectInfo m_cachedEffectOnAlliesOnCast;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Lair";
		}
		if (this.m_prisonSides < 3)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.Start()).MethodHandle;
			}
			this.m_prisonSides = 4;
		}
		Ability[] chainAbilities = base.GetChainAbilities();
		foreach (Ability ability in chainAbilities)
		{
			if (ability != null && ability is MantaCreateBarriersChainFinal)
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
				this.m_finalDamageChain = (ability as MantaCreateBarriersChainFinal);
				IL_90:
				this.m_syncComp = base.GetComponent<Manta_SyncComponent>();
				this.Setup();
				base.ResetTooltipAndTargetingNumbers();
				return;
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			goto IL_90;
		}
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_TeslaPrison(this, TrackerTeslaPrison.PrisonWallSegmentType.RegularPolygon, 0, 0, this.GetPrisonSides(), this.GetPrisonRadius(), this.GetShapeForTargeter(), true);
		base.Targeter.SetAffectedGroups(true, this.IncludeAllies(), false);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetPrisonRadius();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		this.m_prisonBarrierData.AddTooltipTokens(tokens, "PrisonBarrierData", false, null);
		base.AddTokenInt(tokens, "DamageOnCast", string.Empty, this.m_damageOnCast, false);
		base.AddTokenInt(tokens, "AllyHealOnCast", string.Empty, this.m_allyHealOnCast, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnAlliesOnCast, "EffectOnAlliesOnCast", this.m_effectOnAlliesOnCast, true);
	}

	private void SetCachedFields()
	{
		StandardBarrierData cachedPrisonBarrierData;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.SetCachedFields()).MethodHandle;
			}
			cachedPrisonBarrierData = this.m_abilityMod.m_prisonBarrierDataMod.GetModifiedValue(this.m_prisonBarrierData);
		}
		else
		{
			cachedPrisonBarrierData = this.m_prisonBarrierData;
		}
		this.m_cachedPrisonBarrierData = cachedPrisonBarrierData;
		this.m_cachedEffectOnAlliesOnCast = ((!this.m_abilityMod) ? this.m_effectOnAlliesOnCast : this.m_abilityMod.m_effectOnAlliesOnCastMod.GetModifiedValue(this.m_effectOnAlliesOnCast));
	}

	public bool RequireCasterInShape()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.RequireCasterInShape()).MethodHandle;
			}
			result = this.m_abilityMod.m_requireCasterInShapeMod.GetModifiedValue(this.m_requireCasterInShape);
		}
		else
		{
			result = this.m_requireCasterInShape;
		}
		return result;
	}

	public AbilityAreaShape GetTargetAreaShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.GetTargetAreaShape()).MethodHandle;
			}
			result = this.m_abilityMod.m_targetAreaShapeMod.GetModifiedValue(this.m_targetAreaShape);
		}
		else
		{
			result = this.m_targetAreaShape;
		}
		return result;
	}

	public bool DelayBarriersUntilStartOfNextTurn()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.DelayBarriersUntilStartOfNextTurn()).MethodHandle;
			}
			result = this.m_abilityMod.m_delayBarriersUntilStartOfNextTurnMod.GetModifiedValue(this.m_delayBarriersUntilStartOfNextTurn);
		}
		else
		{
			result = this.m_delayBarriersUntilStartOfNextTurn;
		}
		return result;
	}

	public int GetPrisonSides()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.GetPrisonSides()).MethodHandle;
			}
			result = this.m_abilityMod.m_prisonSidesMod.GetModifiedValue(this.m_prisonSides);
		}
		else
		{
			result = this.m_prisonSides;
		}
		return result;
	}

	public float GetPrisonRadius()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.GetPrisonRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_prisonRadiusMod.GetModifiedValue(this.m_prisonRadius);
		}
		else
		{
			result = this.m_prisonRadius;
		}
		return result;
	}

	public AbilityAreaShape GetShapeForTargeter()
	{
		return (!this.m_abilityMod) ? this.m_shapeForTargeter : this.m_abilityMod.m_shapeForTargeterMod.GetModifiedValue(this.m_shapeForTargeter);
	}

	public bool CreateBarriersImmediately()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.CreateBarriersImmediately()).MethodHandle;
			}
			result = this.m_abilityMod.m_createBarriersImmediatelyMod.GetModifiedValue(this.m_createBarriersImmediately);
		}
		else
		{
			result = this.m_createBarriersImmediately;
		}
		return result;
	}

	public StandardGroundEffectInfo GetGroundEffectInfo()
	{
		StandardGroundEffectInfo result;
		if (this.m_abilityMod && this.m_abilityMod.m_groundEffectInfoMod.m_applyGroundEffect)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.GetGroundEffectInfo()).MethodHandle;
			}
			result = this.m_abilityMod.m_groundEffectInfoMod;
		}
		else
		{
			result = this.m_groundEffectInfo;
		}
		return result;
	}

	public int GetDamageOnCast()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.GetDamageOnCast()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageOnCastMod.GetModifiedValue(this.m_damageOnCast);
		}
		else
		{
			result = this.m_damageOnCast;
		}
		return result;
	}

	public int GetAllyHealOnCast()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.GetAllyHealOnCast()).MethodHandle;
			}
			result = this.m_abilityMod.m_allyHealOnCastMod.GetModifiedValue(this.m_allyHealOnCast);
		}
		else
		{
			result = this.m_allyHealOnCast;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnAlliesOnCast()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnAlliesOnCast != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.GetEffectOnAlliesOnCast()).MethodHandle;
			}
			result = this.m_cachedEffectOnAlliesOnCast;
		}
		else
		{
			result = this.m_effectOnAlliesOnCast;
		}
		return result;
	}

	private StandardBarrierData GetPrisonBarrierData()
	{
		StandardBarrierData result;
		if (this.m_cachedPrisonBarrierData == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.GetPrisonBarrierData()).MethodHandle;
			}
			result = this.m_prisonBarrierData;
		}
		else
		{
			result = this.m_cachedPrisonBarrierData;
		}
		return result;
	}

	private bool ShouldAddVisionProvider()
	{
		return this.m_abilityMod && this.m_abilityMod.m_addVisionProviderInsideBarriers.GetModifiedValue(false);
	}

	public bool IncludeAllies()
	{
		return this.GetAllyHealOnCast() > 0 || this.GetEffectOnAlliesOnCast().m_applyEffect;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MantaCreateBarriers))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_MantaCreateBarriers);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		if (this.m_finalDamageChain != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.m_finalDamageChain.GetDamageOnCast()));
		}
		else
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.GetDamageOnCast()));
			this.m_groundEffectInfo.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
			this.GetEffectOnAlliesOnCast().ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Ally);
			this.GetEffectOnAlliesOnCast().ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Self);
			AbilityTooltipHelper.ReportHealing(ref list, AbilityTooltipSubject.Ally, this.GetAllyHealOnCast());
			AbilityTooltipHelper.ReportHealing(ref list, AbilityTooltipSubject.Self, this.GetAllyHealOnCast());
		}
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int num = (!(this.m_finalDamageChain != null)) ? this.GetDamageOnCast() : this.m_finalDamageChain.GetDamageOnCast();
		if (this.GetGroundEffectInfo().m_applyGroundEffect && this.GetGroundEffectInfo().m_groundEffectData.damageAmount > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			num += this.GetGroundEffectInfo().m_groundEffectData.damageAmount;
		}
		dictionary[AbilityTooltipSymbol.Damage] = num;
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
			}
			int num = 0;
			List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[currentTargeterIndex].GetActorsInRange();
			using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityUtil_Targeter.ActorTarget actorTarget = enumerator.Current;
					num += this.m_syncComp.GetDirtyFightingExtraTP(actorTarget.m_actor);
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
			return num;
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (symbolType == AbilityTooltipSymbol.Damage)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaCreateBarriers.GetAccessoryTargeterNumberString(ActorData, AbilityTooltipSymbol, int)).MethodHandle;
			}
			if (this.m_syncComp != null)
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
				return this.m_syncComp.GetAccessoryStringForDamage(targetActor, base.ActorData, this);
			}
		}
		return null;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (this.m_requireCasterInShape && caster.\u0012() != null)
		{
			BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
			return boardSquare != null && AreaEffectUtils.IsSquareInShape(caster.\u0012(), this.GetTargetAreaShape(), target.FreePos, boardSquare, true, caster);
		}
		return true;
	}

	public override bool AllowInvalidSquareForSquareBasedTarget()
	{
		return true;
	}
}
