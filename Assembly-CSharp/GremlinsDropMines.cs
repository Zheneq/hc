using System;
using System.Collections.Generic;
using UnityEngine;

public class GremlinsDropMines : Ability
{
	[Header("-- Mine Placement")]
	public AbilityAreaShape m_minePlaceShape = AbilityAreaShape.Three_x_Three;

	public bool m_placeBombOnCasterSquare;

	public bool m_ignoreLos;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefabNorth;

	public GameObject m_castSequencePrefabSouth;

	public GameObject m_castSequencePrefabEast;

	public GameObject m_castSequencePrefabWest;

	public GameObject m_castSequencePrefabDiag;

	private GremlinsLandMineInfoComponent m_bombInfoComp;

	private AbilityMod_GremlinsDropMines m_abilityMod;

	private StandardEffectInfo m_cachedEnemyHitEffectInfo;

	public AbilityMod_GremlinsDropMines GetMod()
	{
		return this.m_abilityMod;
	}

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Drop Mines";
		}
		this.m_bombInfoComp = base.GetComponent<GremlinsLandMineInfoComponent>();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, this.m_minePlaceShape, this.m_ignoreLos, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Never, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
		base.ResetTooltipAndTargetingNumbers();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.m_bombInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.m_bombInfoComp.m_damageAmount);
			this.m_bombInfoComp.m_enemyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.m_bombInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.m_bombInfoComp.GetDamageOnMovedOver());
			this.GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		}
		return result;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int result = 0;
		if (this.m_bombInfoComp != null && this.m_bombInfoComp.GetEnergyOnExplosion() > 0)
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
			result = this.m_bombInfoComp.GetEnergyOnExplosion() * visibleActorsInRangeByTooltipSubject.Count;
		}
		return result;
	}

	public override List<int> symbol_001D()
	{
		List<int> list = new List<int>();
		GremlinsLandMineInfoComponent component = base.GetComponent<GremlinsLandMineInfoComponent>();
		if (component != null)
		{
			list.Add(component.m_damageAmount);
		}
		return list;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		GremlinsLandMineInfoComponent component = base.GetComponent<GremlinsLandMineInfoComponent>();
		if (component != null)
		{
			base.AddTokenInt(tokens, "Damage", string.Empty, component.m_damageAmount, false);
			base.AddTokenInt(tokens, "MineDuration", string.Empty, component.m_mineDuration, false);
			base.AddTokenInt(tokens, "EnergyGainOnMineHit", "energy gain on mine explosion", component.m_energyGainOnExplosion, false);
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_GremlinsDropMines))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_GremlinsDropMines);
			if (this.m_bombInfoComp == null)
			{
				this.m_bombInfoComp = base.GetComponent<GremlinsLandMineInfoComponent>();
			}
			if (this.m_bombInfoComp != null)
			{
				this.m_cachedEnemyHitEffectInfo = this.m_bombInfoComp.GetEnemyHitEffectOnMovedOver();
			}
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
		{
			result = this.m_bombInfoComp.m_enemyHitEffect;
		}
		else
		{
			result = this.m_cachedEnemyHitEffectInfo;
		}
		return result;
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		caster.TurnToDirection(new Vector3(0f, 0f, 1f));
	}
}
