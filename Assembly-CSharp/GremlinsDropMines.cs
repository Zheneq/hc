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
		return m_abilityMod;
	}

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Drop Mines";
		}
		m_bombInfoComp = GetComponent<GremlinsLandMineInfoComponent>();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, m_minePlaceShape, m_ignoreLos, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Never);
		base.Targeter.ShowArcToShape = false;
		ResetTooltipAndTargetingNumbers();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_bombInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_bombInfoComp.m_damageAmount);
			m_bombInfoComp.m_enemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		}
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_bombInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_bombInfoComp.GetDamageOnMovedOver());
			GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		}
		return numbers;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int result = 0;
		if (m_bombInfoComp != null && m_bombInfoComp.GetEnergyOnExplosion() > 0)
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
			result = m_bombInfoComp.GetEnergyOnExplosion() * visibleActorsInRangeByTooltipSubject.Count;
		}
		return result;
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = new List<int>();
		GremlinsLandMineInfoComponent component = GetComponent<GremlinsLandMineInfoComponent>();
		if (component != null)
		{
			list.Add(component.m_damageAmount);
		}
		return list;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		GremlinsLandMineInfoComponent component = GetComponent<GremlinsLandMineInfoComponent>();
		if (component != null)
		{
			AddTokenInt(tokens, "Damage", string.Empty, component.m_damageAmount);
			AddTokenInt(tokens, "MineDuration", string.Empty, component.m_mineDuration);
			AddTokenInt(tokens, "EnergyGainOnMineHit", "energy gain on mine explosion", component.m_energyGainOnExplosion);
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_GremlinsDropMines))
		{
			return;
		}
		m_abilityMod = (abilityMod as AbilityMod_GremlinsDropMines);
		if (m_bombInfoComp == null)
		{
			m_bombInfoComp = GetComponent<GremlinsLandMineInfoComponent>();
		}
		if (!(m_bombInfoComp != null))
		{
			return;
		}
		while (true)
		{
			m_cachedEnemyHitEffectInfo = m_bombInfoComp.GetEnemyHitEffectOnMovedOver();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
		{
			result = m_bombInfoComp.m_enemyHitEffect;
		}
		else
		{
			result = m_cachedEnemyHitEffectInfo;
		}
		return result;
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		caster.TurnToDirection(new Vector3(0f, 0f, 1f));
	}
}
