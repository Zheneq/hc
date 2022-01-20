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
		List<AbilityUtil_Targeter_MultipleCones.ConeDimensions> list = new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>();
		list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, ModdedInnerRadius()));
		list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, ModdedOuterRadius()));
		base.Targeter = new AbilityUtil_Targeter_MultipleCones(this, list, m_coneBackwardOffset, m_penetrateLineOfSight, true);
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
		m_cachedEffectInner = ((!m_abilityMod) ? m_effectInner : m_abilityMod.m_effectInnerMod.GetModifiedValue(m_effectInner));
		StandardEffectInfo cachedEffectOuter;
		if ((bool)m_abilityMod)
		{
			cachedEffectOuter = m_abilityMod.m_effectOuterMod.GetModifiedValue(m_effectOuter);
		}
		else
		{
			cachedEffectOuter = m_effectOuter;
		}
		m_cachedEffectOuter = cachedEffectOuter;
	}

	public StandardEffectInfo GetEffectInner()
	{
		StandardEffectInfo result;
		if (m_cachedEffectInner != null)
		{
			result = m_cachedEffectInner;
		}
		else
		{
			result = m_effectInner;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOuter()
	{
		return (m_cachedEffectOuter == null) ? m_effectOuter : m_cachedEffectOuter;
	}

	private float ModdedConeAngle()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_coneWidthAngle;
		}
		else
		{
			result = m_abilityMod.m_coneAngleMod.GetModifiedValue(m_coneWidthAngle);
		}
		return result;
	}

	private float ModdedInnerRadius()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_coneLengthInner;
		}
		else
		{
			result = m_abilityMod.m_coneInnerRadiusMod.GetModifiedValue(m_coneLengthInner);
		}
		return result;
	}

	private float ModdedOuterRadius()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_coneLengthOuter;
		}
		else
		{
			result = m_abilityMod.m_coneOuterRadiusMod.GetModifiedValue(m_coneLengthOuter);
		}
		return result;
	}

	private int ModdedInnerDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_damageAmountInner;
		}
		else
		{
			result = m_abilityMod.m_innerDamageMod.GetModifiedValue(m_damageAmountInner);
		}
		return result;
	}

	private int ModdedOuterDamage()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_outerDamageMod.GetModifiedValue(m_damageAmountOuter) : m_damageAmountOuter;
	}

	private int ModdedDamagePerAdjacentEnemy()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_extraDamagePerAdjacentEnemy;
		}
		return result;
	}

	private int ModdedTechPointsPerAdjacentEnemy()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_extraTechPointsPerAdjacentEnemy;
		}
		return result;
	}

	private int ModdedInnerTpGain()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_tpGainInner;
		}
		else
		{
			result = m_abilityMod.m_innerTpGain.GetModifiedValue(m_tpGainInner);
		}
		return result;
	}

	private int ModdedOuterTpGain()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_outerTpGain.GetModifiedValue(m_tpGainOuter) : m_tpGainOuter;
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
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_RageBeastBasicAttack)
		{
			val = abilityMod_RageBeastBasicAttack.m_innerDamageMod.GetModifiedValue(m_damageAmountInner);
		}
		else
		{
			val = m_damageAmountInner;
		}
		AddTokenInt(tokens, "DamageAmountInner", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_RageBeastBasicAttack)
		{
			val2 = abilityMod_RageBeastBasicAttack.m_outerDamageMod.GetModifiedValue(m_damageAmountOuter);
		}
		else
		{
			val2 = m_damageAmountOuter;
		}
		AddTokenInt(tokens, "DamageAmountOuter", empty2, val2);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_RageBeastBasicAttack)
		{
			effectInfo = abilityMod_RageBeastBasicAttack.m_effectInnerMod.GetModifiedValue(m_effectInner);
		}
		else
		{
			effectInfo = m_effectInner;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectInner", m_effectInner);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_RageBeastBasicAttack)
		{
			effectInfo2 = abilityMod_RageBeastBasicAttack.m_effectOuterMod.GetModifiedValue(m_effectOuter);
		}
		else
		{
			effectInfo2 = m_effectOuter;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOuter", m_effectOuter);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_RageBeastBasicAttack)
		{
			val3 = abilityMod_RageBeastBasicAttack.m_innerTpGain.GetModifiedValue(m_tpGainInner);
		}
		else
		{
			val3 = m_tpGainInner;
		}
		AddTokenInt(tokens, "TpGainInner", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_RageBeastBasicAttack)
		{
			val4 = abilityMod_RageBeastBasicAttack.m_outerTpGain.GetModifiedValue(m_tpGainOuter);
		}
		else
		{
			val4 = m_tpGainOuter;
		}
		AddTokenInt(tokens, "TpGainOuter", empty4, val4);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_RageBeastBasicAttack))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_RageBeastBasicAttack);
			Setup();
			return;
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
		if (damageAmount == 0)
		{
			if (techPointAmount == 0)
			{
				return;
			}
		}
		int num = 0;
		List<BoardSquare> result = new List<BoardSquare>();
		Board.Get().GetAllAdjacentSquares(base.ActorData.GetCurrentBoardSquare().x, base.ActorData.GetCurrentBoardSquare().y, ref result);
		using (List<BoardSquare>.Enumerator enumerator = result.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				if (current.OccupantActor != null)
				{
					if (current.OccupantActor.GetTeam() != base.ActorData.GetTeam())
					{
						if (!current.OccupantActor.IgnoreForAbilityHits)
						{
							if (visibleActorsOnly)
							{
								if (!current.OccupantActor.IsVisibleToClient())
								{
									continue;
								}
							}
							num++;
						}
					}
				}
			}
		}
		damageAmount *= num;
		techPointAmount *= num;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			int damageAmount = 0;
			int techPointAmount = 0;
			GetExtraDamageAndTPForCurrentLocation(true, out damageAmount, out techPointAmount);
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
		int damageAmount = 0;
		int techPointAmount = 0;
		GetExtraDamageAndTPForCurrentLocation(true, out damageAmount, out techPointAmount);
		int num = 0;
		if (ModdedInnerTpGain() > 0 || techPointAmount > 0)
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Near);
			num += visibleActorsInRangeByTooltipSubject.Count * (ModdedInnerTpGain() + techPointAmount);
		}
		if (ModdedOuterTpGain() > 0 || techPointAmount > 0)
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject2 = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Far);
			num += visibleActorsInRangeByTooltipSubject2.Count * (ModdedOuterTpGain() + techPointAmount);
		}
		return num;
	}

	public override bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		if (subjectType != AbilityTooltipSubject.Near)
		{
			if (subjectType != AbilityTooltipSubject.Far)
			{
				return base.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
			}
		}
		float num = ModdedInnerRadius() * Board.Get().squareSize;
		Vector3 vector = targetActor.GetTravelBoardSquareWorldPosition() - damageOrigin;
		vector.y = 0f;
		float num2 = vector.magnitude;
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			num2 -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
		}
		bool flag;
		if (num2 > num)
		{
			flag = false;
		}
		else
		{
			flag = true;
		}
		if (subjectType == AbilityTooltipSubject.Near)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return flag;
				}
			}
		}
		return !flag;
	}
}
