using System.Collections.Generic;
using UnityEngine;

public class BazookaGirlDelayedBombDrops : Ability
{
	public enum TargetingType
	{
		Shape,
		Cone
	}

	[Header("-- Targeting")]
	public TargetingType m_targetingType;
	public bool m_penetrateLos;
	[Header("-- Targeting All?")]
	public bool m_targetAll;
	[Header("-- If using Shape")]
	public AbilityAreaShape m_targetingShape = AbilityAreaShape.Five_x_Five_NoCorners;
	[Header("-- If Using Cone")]
	public float m_coneWidthAngle = 270f;
	public float m_coneLength = 1.5f;
	public float m_coneBackwardOffset;
	[Header("-- Dropped Bomb Info")]
	public int m_bombDropDelay = 1;
	public AbilityPriority m_bombDropPhase = AbilityPriority.Combat_Damage;
	public int m_bombDropAnimIndexInEffect = 11;
	public BazookaGirlDroppedBombInfo m_bombInfo;
	[Header("-- Additional Damage from fewer hit areas, = extraDmgPeArea * Max(0, (maxNumAreas - numAreas))")]
	public int m_maxNumOfAreasForExtraDamage;
	public int m_extraDamagePerFewerArea;
	[Header("-- On Ability Hit Effect")]
	public StandardEffectInfo m_enemyOnAbilityHitEffect;
	[Header("-- Sequences ----------------------------------------")]
	public GameObject m_castSequencePrefab;
	public GameObject m_dropSiteMarkerSequencePrefab;
	public GameObject m_bombDropSequencePrefab;
	public GameObject m_targetMarkedSequencePrefab;
	[TextArea(1, 5)]
	public string m_notes;

	private AbilityMod_BazookaGirlDelayedBombDrops m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Delayed Bomb Drops";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (TargetAllEnemies())
		{
			Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, 30f, true);
		}
		else if (m_targetingType == TargetingType.Shape)
		{
			Targeter = new AbilityUtil_Targeter_Shape(this, m_targetingShape, PenetrateLos());
		}
		else if (m_targetingType == TargetingType.Cone)
		{
			Targeter = new AbilityUtil_Targeter_DirectionCone(this, GetConeAngle(), GetConeLength(), m_coneBackwardOffset, PenetrateLos(), true);
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeLength();
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_bombInfo.m_damageAmount)
			: m_bombInfo.m_damageAmount;
	}

	public float GetConeLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength)
			: m_coneLength;
	}

	public float GetConeAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	public bool TargetAllEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targetAllMod.GetModifiedValue(m_targetAll)
			: m_targetAll;
	}

	public bool PenetrateLos()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos) 
			: m_penetrateLos;
	}

	public int GetMaxNumOfAreasForExtraDamage()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_maxNumOfAreasForExtraDamageMod.GetModifiedValue(m_maxNumOfAreasForExtraDamage) 
			: m_maxNumOfAreasForExtraDamage;
	}

	public int GetExtraDamagePerFewerArea()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_extraDamagePerFewerAreaMod.GetModifiedValue(m_extraDamagePerFewerArea) 
			: m_extraDamagePerFewerArea;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_enemyOnAbilityHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipSubject subject = m_bombDropDelay <= 0
			? AbilityTooltipSubject.Primary
			: AbilityTooltipSubject.Tertiary;
		AbilityTooltipHelper.ReportDamage(ref numbers, subject, m_bombInfo.m_damageAmount);
		if (m_bombInfo.m_damageAmount != m_bombInfo.m_subsequentDamageAmount)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Quaternary, m_bombInfo.m_subsequentDamageAmount);
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (m_bombDropDelay > 0)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<ActorData> visibleActorsInRangeByTooltipSubject = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
		List<BoardSquare> targetSquares = new List<BoardSquare>();
		foreach (ActorData target in visibleActorsInRangeByTooltipSubject)
		{
			targetSquares.Add(target.GetCurrentBoardSquare());
		}
		int extraDamage = 0;
		if (GetExtraDamagePerFewerArea() > 0 && GetMaxNumOfAreasForExtraDamage() > 0)
		{
			int num = GetMaxNumOfAreasForExtraDamage() - visibleActorsInRangeByTooltipSubject.Count;
			if (num > 0)
			{
				extraDamage = num * GetExtraDamagePerFewerArea();
			}
		}
		int baseDamage = 0;
		bool subsequentHit = false;
		foreach (BoardSquare item in targetSquares)
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_bombInfo.m_shape, item.ToVector3(), item);
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(m_bombInfo.m_shape, centerOfShape, item, PenetrateLos(), ActorData, ActorData.GetEnemyTeam(), null);
			foreach (ActorData target in actorsInShape)
			{
				if (target != targetActor)
				{
					continue;
				}
				if (subsequentHit)
				{
					baseDamage += m_bombInfo.m_subsequentDamageAmount;
				}
				else
				{
					baseDamage += GetDamageAmount();
					subsequentHit = true;
				}
			}
		}
		dictionary[AbilityTooltipSymbol.Damage] = baseDamage + extraDamage;
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlDelayedBombDrops abilityMod_BazookaGirlDelayedBombDrops = modAsBase as AbilityMod_BazookaGirlDelayedBombDrops;
		AddTokenInt(tokens, "Damage", string.Empty, m_bombInfo.m_damageAmount);
		AddTokenInt(tokens, "MaxNumOfAreasForExtraDamage", string.Empty, abilityMod_BazookaGirlDelayedBombDrops != null
			? abilityMod_BazookaGirlDelayedBombDrops.m_maxNumOfAreasForExtraDamageMod.GetModifiedValue(m_maxNumOfAreasForExtraDamage)
			: m_maxNumOfAreasForExtraDamage);
		AddTokenInt(tokens, "ExtraDamagePerFewerArea", string.Empty, abilityMod_BazookaGirlDelayedBombDrops != null
			? abilityMod_BazookaGirlDelayedBombDrops.m_extraDamagePerFewerAreaMod.GetModifiedValue(m_extraDamagePerFewerArea)
			: m_extraDamagePerFewerArea);
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return animIndex == m_bombDropAnimIndexInEffect || base.CanTriggerAnimAtIndexForTaunt(animIndex);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BazookaGirlDelayedBombDrops))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_BazookaGirlDelayedBombDrops;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> points = new List<Vector3>();
		Vector3 casterPos = caster.GetFreePos();
		if (TargetAllEnemies())
		{
			AreaEffectUtils.AddRadiusExtremaToList(ref points, casterPos, 5f);
		}
		else if (m_targetingType == TargetingType.Shape)
		{
			AreaEffectUtils.AddShapeCornersToList(ref points, m_targetingShape, targets[0]);
		}
		else if (m_targetingType == TargetingType.Cone)
		{
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection);
			AreaEffectUtils.AddConeExtremaToList(ref points, casterPos, coneCenterAngleDegrees, GetConeAngle(), GetConeLength(), m_coneBackwardOffset);
		}
		return points;
	}
}
