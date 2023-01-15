using System.Collections.Generic;
using UnityEngine;

public class RageBeastBasicAttack : Ability
{
	public float m_coneWidthAngle = 180f;
	public float m_coneBackwardOffset;
	public float m_coneLengthInner = 1.5f;
	public float m_coneLengthOuter = 2.5f;
	public int m_damageAmountInner = 5;
	public int m_damageAmountOuter = 3;
	public StandardEffectInfo m_effectInner;
	public StandardEffectInfo m_effectOuter;
	public int m_tpGainInner;
	public int m_tpGainOuter;
	public bool m_penetrateLineOfSight;

	private AbilityMod_RageBeastBasicAttack m_abilityMod;
	private StandardEffectInfo m_cachedEffectInner;
	private StandardEffectInfo m_cachedEffectOuter;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Flurry";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		float angle = ModdedConeAngle();
		Targeter = new AbilityUtil_Targeter_MultipleCones(
			this, 
			new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>
			{
				new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, ModdedInnerRadius()),
				new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, ModdedOuterRadius())
			},
			m_coneBackwardOffset,
			m_penetrateLineOfSight,
			true);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return ModdedOuterRadius();
	}

	private void SetCachedFields()
	{
		m_cachedEffectInner = m_abilityMod != null
			? m_abilityMod.m_effectInnerMod.GetModifiedValue(m_effectInner)
			: m_effectInner;
		m_cachedEffectOuter = m_abilityMod != null
			? m_abilityMod.m_effectOuterMod.GetModifiedValue(m_effectOuter)
			: m_effectOuter;
	}

	public StandardEffectInfo GetEffectInner()
	{
		return m_cachedEffectInner ?? m_effectInner;
	}

	public StandardEffectInfo GetEffectOuter()
	{
		return m_cachedEffectOuter ?? m_effectOuter;
	}

	private float ModdedConeAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	private float ModdedInnerRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneInnerRadiusMod.GetModifiedValue(m_coneLengthInner)
			: m_coneLengthInner;
	}

	private float ModdedOuterRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneOuterRadiusMod.GetModifiedValue(m_coneLengthOuter)
			: m_coneLengthOuter;
	}

	private int ModdedInnerDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_innerDamageMod.GetModifiedValue(m_damageAmountInner)
			: m_damageAmountInner;
	}

	private int ModdedOuterDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_outerDamageMod.GetModifiedValue(m_damageAmountOuter)
			: m_damageAmountOuter;
	}

	private int ModdedDamagePerAdjacentEnemy()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamagePerAdjacentEnemy
			: 0;
	}

	private int ModdedTechPointsPerAdjacentEnemy()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraTechPointsPerAdjacentEnemy
			: 0;
	}

	private int ModdedInnerTpGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_innerTpGain.GetModifiedValue(m_tpGainInner)
			: m_tpGainInner;
	}

	private int ModdedOuterTpGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_outerTpGain.GetModifiedValue(m_tpGainOuter)
			: m_tpGainOuter;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Near, m_damageAmountInner));
		m_effectInner.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Near);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Far, m_damageAmountOuter));
		m_effectOuter.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Far);
		return numbers;
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = base.Debug_GetExpectedNumbersInTooltip();
		int num = Mathf.Abs(m_damageAmountInner - m_damageAmountOuter);
		if (num != 0)
		{
			list.Add(num);
		}
		return list;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RageBeastBasicAttack abilityMod_RageBeastBasicAttack = modAsBase as AbilityMod_RageBeastBasicAttack;
		AddTokenInt(tokens, "DamageAmountInner", string.Empty, abilityMod_RageBeastBasicAttack != null
			? abilityMod_RageBeastBasicAttack.m_innerDamageMod.GetModifiedValue(m_damageAmountInner)
			: m_damageAmountInner);
		AddTokenInt(tokens, "DamageAmountOuter", string.Empty, abilityMod_RageBeastBasicAttack != null
			? abilityMod_RageBeastBasicAttack.m_outerDamageMod.GetModifiedValue(m_damageAmountOuter)
			: m_damageAmountOuter);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_RageBeastBasicAttack != null
			? abilityMod_RageBeastBasicAttack.m_effectInnerMod.GetModifiedValue(m_effectInner)
			: m_effectInner, "EffectInner", m_effectInner);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_RageBeastBasicAttack != null
			? abilityMod_RageBeastBasicAttack.m_effectOuterMod.GetModifiedValue(m_effectOuter)
			: m_effectOuter, "EffectOuter", m_effectOuter);
		AddTokenInt(tokens, "TpGainInner", string.Empty, abilityMod_RageBeastBasicAttack != null
			? abilityMod_RageBeastBasicAttack.m_innerTpGain.GetModifiedValue(m_tpGainInner)
			: m_tpGainInner);
		AddTokenInt(tokens, "TpGainOuter", string.Empty, abilityMod_RageBeastBasicAttack != null
			? abilityMod_RageBeastBasicAttack.m_outerTpGain.GetModifiedValue(m_tpGainOuter)
			: m_tpGainOuter);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RageBeastBasicAttack))
		{
			m_abilityMod = abilityMod as AbilityMod_RageBeastBasicAttack;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	private void GetExtraDamageAndTPForCurrentLocation(bool visibleActorsOnly, out int damageAmount, out int techPointAmount)
	{
		damageAmount = ModdedDamagePerAdjacentEnemy();
		techPointAmount = ModdedTechPointsPerAdjacentEnemy();
		if (damageAmount == 0 && techPointAmount == 0)
		{
			return;
		}
		int num = 0;
		List<BoardSquare> result = new List<BoardSquare>();
		Board.Get().GetAllAdjacentSquares(ActorData.GetCurrentBoardSquare().x, ActorData.GetCurrentBoardSquare().y, ref result);
		foreach (BoardSquare square in result)
		{
			if (square.OccupantActor != null
			    && square.OccupantActor.GetTeam() != ActorData.GetTeam()
			    && !square.OccupantActor.IgnoreForAbilityHits
			    && (!visibleActorsOnly || square.OccupantActor.IsActorVisibleToClient()))
			{
				num++;
			}
		}
		damageAmount *= num;
		techPointAmount *= num;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			GetExtraDamageAndTPForCurrentLocation(true, out int damageAmount, out _);
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Near))
			{
				dictionary[AbilityTooltipSymbol.Damage] = ModdedInnerDamage() + damageAmount;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Far))
			{
				dictionary[AbilityTooltipSymbol.Damage] = ModdedOuterDamage() + damageAmount;
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		GetExtraDamageAndTPForCurrentLocation(true, out _, out int techPointAmount);
		int num = 0;
		if (ModdedInnerTpGain() > 0 || techPointAmount > 0)
		{
			List<ActorData> nearNum = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Near);
			num += nearNum.Count * (ModdedInnerTpGain() + techPointAmount);
		}
		if (ModdedOuterTpGain() > 0 || techPointAmount > 0)
		{
			List<ActorData> farNum = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Far);
			num += farNum.Count * (ModdedOuterTpGain() + techPointAmount);
		}
		return num;
	}

	public override bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		if (subjectType != AbilityTooltipSubject.Near && subjectType != AbilityTooltipSubject.Far)
		{
			return base.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
		}
		float innerRadiusInWorld = ModdedInnerRadius() * Board.Get().squareSize;
		Vector3 vector = targetActor.GetFreePos() - damageOrigin;
		vector.y = 0f;
		float dist = vector.magnitude;
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			dist -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
		}

		bool isNear = dist <= innerRadiusInWorld;
		return subjectType == AbilityTooltipSubject.Near ? isNear : !isNear;
	}
}
