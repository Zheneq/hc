using System.Collections.Generic;
using UnityEngine;

public class ClaymoreCharge : Ability
{
	[Header("-- Charge Targeting")]
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;
	public float m_width = 1f;
	public float m_maxRange = 10f;
	[Header("-- Normal On Hit Damage, Effect, etc")]
	public int m_directHitDamage = 20;
	public StandardEffectInfo m_directEnemyHitEffect;
	public bool m_directHitIgnoreCover = true;
	[Space(10f)]
	public int m_aoeDamage = 10;
	public StandardEffectInfo m_aoeEnemyHitEffect;
	[Header("-- Extra Damage from Charge Path Length")]
	public int m_extraDirectHitDamagePerSquare;
	[Header("-- Heal On Self")]
	public int m_healOnSelfPerTargetHit;
	[Header("-- Other On Hit Config")]
	public int m_cooldownOnHit = -1;
	public bool m_chaseHitActor;
	[Header("-- Charge Anim")]
	[Tooltip("Whether to set up charge like battlemonk charge with pivots and recovery")]
	public bool m_chargeWithPivotAndRecovery;
	[Tooltip("Only relevant if using pivot and recovery charge setup")]
	public float m_recoveryTime = 0.5f;
	[Header("-- Sequences")]
	public GameObject m_chargeSequencePrefab;
	public GameObject m_aoeHitSequencePrefab;

	private const int c_maxBounces = 0;
	private const int c_maxTargetsHit = 1;
	private const bool c_penetrateLoS = false;

	private AbilityMod_ClaymoreCharge m_abilityMod;
	private Claymore_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedDirectEnemyHitEffect;
	private StandardEffectInfo m_cachedAoeEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Berserker Charge";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_syncComp = GetComponent<Claymore_SyncComponent>();
		SetCachedFields();
		AbilityUtil_Targeter_ClaymoreCharge targeter = new AbilityUtil_Targeter_ClaymoreCharge(
			this,
			GetChargeWidth(),
			GetChargeRange(),
			GetAoeShape(),
			DirectHitIgnoreCover());
		if (GetHealOnSelfPerTargetHit() > 0)
		{
			targeter.m_affectCasterDelegate = (ActorData caster, List<ActorData> actorsSoFar) => actorsSoFar.Count > 0;
		}
		Targeter = targeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetChargeRange();
	}

	private void SetCachedFields()
	{
		m_cachedDirectEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_directEnemyHitEffectMod.GetModifiedValue(m_directEnemyHitEffect)
			: m_directEnemyHitEffect;
		m_cachedAoeEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_aoeEnemyHitEffectMod.GetModifiedValue(m_aoeEnemyHitEffect)
			: m_aoeEnemyHitEffect;
	}

	public AbilityAreaShape GetAoeShape()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_aoeShapeMod.GetModifiedValue(m_aoeShape) 
			: m_aoeShape;
	}

	public float GetChargeWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_widthMod.GetModifiedValue(m_width)
			: m_width;
	}

	public float GetChargeRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxRangeMod.GetModifiedValue(m_maxRange)
			: m_maxRange;
	}

	public bool DirectHitIgnoreCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_directHitIgnoreCoverMod.GetModifiedValue(m_directHitIgnoreCover)
			: m_directHitIgnoreCover;
	}

	public int GetDirectHitDamage()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_directHitDamageMod.GetModifiedValue(m_directHitDamage) 
			: m_directHitDamage;
	}

	public StandardEffectInfo GetDirectEnemyHitEffect()
	{
		return m_cachedDirectEnemyHitEffect ?? m_directEnemyHitEffect;
	}

	public int GetAoeDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeDamageMod.GetModifiedValue(m_aoeDamage)
			: m_aoeDamage;
	}

	public StandardEffectInfo GetAoeEnemyHitEffect()
	{
		return m_cachedAoeEnemyHitEffect ?? m_aoeEnemyHitEffect;
	}

	public int GetExtraDirectHitDamagePerSquare()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDirectHitDamagePerSquareMod.GetModifiedValue(m_extraDirectHitDamagePerSquare)
			: m_extraDirectHitDamagePerSquare;
	}

	public int GetHealOnSelfPerTargetHit()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_healOnSelfPerTargetHitMod.GetModifiedValue(m_healOnSelfPerTargetHit) 
			: m_healOnSelfPerTargetHit;
	}

	public int GetCooldownOnHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownOnHitMod.GetModifiedValue(m_cooldownOnHit)
			: m_cooldownOnHit;
	}

	public bool GetChaseHitActor()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_chaseHitActorMod.GetModifiedValue(m_chaseHitActor) 
			: m_chaseHitActor;
	}

	public int GetMaxTargets()
	{
		return 1;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreCharge abilityMod_ClaymoreCharge = modAsBase as AbilityMod_ClaymoreCharge;
		AddTokenInt(tokens, "DirectHitDamage", string.Empty, abilityMod_ClaymoreCharge != null
			? abilityMod_ClaymoreCharge.m_directHitDamageMod.GetModifiedValue(m_directHitDamage)
			: m_directHitDamage);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ClaymoreCharge != null
			? abilityMod_ClaymoreCharge.m_directEnemyHitEffectMod.GetModifiedValue(m_directEnemyHitEffect)
			: m_directEnemyHitEffect, "DirectEnemyHitEffect", m_directEnemyHitEffect);
		AddTokenInt(tokens, "AoeDamage", string.Empty, abilityMod_ClaymoreCharge != null
			? abilityMod_ClaymoreCharge.m_aoeDamageMod.GetModifiedValue(m_aoeDamage)
			: m_aoeDamage);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ClaymoreCharge != null
			? abilityMod_ClaymoreCharge.m_aoeEnemyHitEffectMod.GetModifiedValue(m_aoeEnemyHitEffect)
			: m_aoeEnemyHitEffect, "AoeEnemyHitEffect", m_aoeEnemyHitEffect);
		AddTokenInt(tokens, "ExtraDirectHitDamagePerSquare", string.Empty, abilityMod_ClaymoreCharge != null
			? abilityMod_ClaymoreCharge.m_extraDirectHitDamagePerSquareMod.GetModifiedValue(m_extraDirectHitDamagePerSquare)
			: m_extraDirectHitDamagePerSquare);
		AddTokenInt(tokens, "HealOnSelfPerTargetHit", string.Empty, abilityMod_ClaymoreCharge != null
			? abilityMod_ClaymoreCharge.m_healOnSelfPerTargetHitMod.GetModifiedValue(m_healOnSelfPerTargetHit)
			: m_healOnSelfPerTargetHit);
		AddTokenInt(tokens, "CooldownOnHit", string.Empty, abilityMod_ClaymoreCharge != null
			? abilityMod_ClaymoreCharge.m_cooldownOnHitMod.GetModifiedValue(m_cooldownOnHit)
			: m_cooldownOnHit);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDirectHitDamage());
		GetDirectEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetAoeDamage());
		GetAoeEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetHealOnSelfPerTargetHit());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				int damage = GetDirectHitDamage();
				if (GetExtraDirectHitDamagePerSquare() > 0
				    && Targeter is AbilityUtil_Targeter_ClaymoreCharge)
				{
					AbilityUtil_Targeter_ClaymoreCharge targeter = Targeter as AbilityUtil_Targeter_ClaymoreCharge;
					int chargeDist = Mathf.Max(0, targeter.LastUpdatePathSquareCount - 1);
					damage += chargeDist * GetExtraDirectHitDamagePerSquare();
				}
				dictionary[AbilityTooltipSymbol.Damage] = damage;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				dictionary[AbilityTooltipSymbol.Damage] = GetAoeDamage();
			}
			else if (GetHealOnSelfPerTargetHit() > 0 && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				dictionary[AbilityTooltipSymbol.Healing] = GetHealOnSelfPerTargetHit() * Mathf.Max(0, Targeter.GetActorsInRange().Count - 1);
			}
		}
		return dictionary;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return m_syncComp != null
			? m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, ActorData)
			: null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public static List<ActorData> GetActorsOnPath(BoardSquarePathInfo path, List<Team> relevantTeams, ActorData caster)
	{
		List<ActorData> list = new List<ActorData>();
		if (path == null)
		{
			return list;
		}
		for (BoardSquarePathInfo step = path; step != null; step = step.next)
		{
			ActorData occupantActor = step.square.OccupantActor;
			if (occupantActor != null
			    && AreaEffectUtils.IsActorTargetable(occupantActor, relevantTeams)
			    && (relevantTeams == null || relevantTeams.Contains(occupantActor.GetTeam())))
			{
				list.Add(occupantActor);
			}
		}
		return list;
	}

	public static float GetMaxPotentialChargeDistance(
		Vector3 startPos,
		Vector3 endPos,
		Vector3 aimDir,
		float laserMaxDistInWorld,
		ActorData mover,
		out BoardSquare pathEndSquare)
	{
		float result = laserMaxDistInWorld;
		pathEndSquare = KnockbackUtils.GetLastValidBoardSquareInLine(
			startPos,
			endPos,
			false,
			false,
			laserMaxDistInWorld + 0.5f);
		BoardSquare boardSquare = Board.Get().GetSquareFromVec3(startPos);
		if (pathEndSquare != null && pathEndSquare != boardSquare)
		{
			Vector3 pointToProject = pathEndSquare.ToVector3();
			Vector3 projectionPoint = VectorUtils.GetProjectionPoint(aimDir, startPos, pointToProject);
			float num = (projectionPoint - startPos).magnitude + 0.5f;
			if (num < laserMaxDistInWorld)
			{
				result = num;
			}
		}
		else
		{
			result = 0.5f;
		}
		return result;
	}

	public static BoardSquare GetTrimmedDestinationInPath(BoardSquarePathInfo chargePath, out bool differentFromInputDest)
	{
		differentFromInputDest = false;
		if (chargePath == null)
		{
			return null;
		}
		BoardSquarePathInfo boardSquarePathInfo = chargePath;
		BoardSquare result = boardSquarePathInfo.square;
		int num = 0;
		while (boardSquarePathInfo.next != null)
		{
			BoardSquare square = boardSquarePathInfo.next.square;
			if (boardSquarePathInfo.square.IsValidForKnockbackAndCharge()
			    && !square.IsValidForKnockbackAndCharge()
			    && num > 0)
			{
				result = boardSquarePathInfo.square;
				differentFromInputDest = true;
				break;
			}
			result = square;
			boardSquarePathInfo = boardSquarePathInfo.next;
			num++;
		}
		return result;
	}

	public static BoardSquare GetChargeDestinationSquare(
		Vector3 startPos,
		Vector3 chargeDestPos,
		ActorData lastChargeHitActor,
		BoardSquare initialPathEndSquare,
		ActorData caster,
		bool trimBeforeFirstInvalid)
	{
		BoardSquare destination = null;
		if (lastChargeHitActor != null)
		{
			destination = lastChargeHitActor.GetCurrentBoardSquare();
		}
		else
		{
			if (initialPathEndSquare != null)
			{
				destination = initialPathEndSquare;
			}
			else
			{
				destination = KnockbackUtils.GetLastValidBoardSquareInLine(startPos, chargeDestPos, true);
			}
			BoardSquare start = Board.Get().GetSquareFromVec3(startPos);
			BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(
				caster,
				destination,
				start,
				true);
			if (boardSquarePathInfo != null && trimBeforeFirstInvalid)
			{
				bool foo;
				destination = GetTrimmedDestinationInPath(boardSquarePathInfo, out foo);
			}
		}
		return destination;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClaymoreCharge))
		{
			m_abilityMod = abilityMod as AbilityMod_ClaymoreCharge;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
