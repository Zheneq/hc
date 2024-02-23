using System.Collections.Generic;
using UnityEngine;

public class MantaDashThroughWall : Ability
{
	[Header("-- Charge Targeting")]
	public float m_aoeConeWidth = 180f;
	public float m_aoeConeLength = 2f;
	public float m_aoeThroughWallConeWidth = 180f;
	public float m_aoeThroughWallConeLength = 2f;
	public float m_width = 1f;
	public float m_maxRange = 10f;
	public float m_maxWidthOfWall = 1f;
	public Color m_normalHighlightColor = Color.green;
	public Color m_throughWallsHighlightColor = Color.yellow;
	public float m_extraTotalDistanceIfThroughWalls = 1.5f;
	public bool m_clampConeToWall = true;
	public bool m_aoeWithMiss;
	[Tooltip("backward offset not used for through-walls case")]
	public float m_coneBackwardOffset = 0.5f;
	[Header("-- Normal On Hit Damage, Effect, etc")]
	public int m_directHitDamage = 20;
	public StandardEffectInfo m_directEnemyHitEffect;
	public bool m_directHitIgnoreCover = true;
	[Space(10f)]
	public int m_aoeDamage = 10;
	public StandardEffectInfo m_aoeEnemyHitEffect;
	[Space(10f)]
	public int m_aoeThroughWallsDamage = 10;
	public StandardEffectInfo m_aoeThroughWallsEffect;
	[Header("-- Sequences")]
	public GameObject m_aoeHitSequencePrefab;

	private const int c_maxTargetsHit = 1;
	private const bool c_penetrateLoS = false;
	private Manta_SyncComponent m_syncComp;
	private AbilityMod_MantaDashThroughWall m_abilityMod;
	private StandardEffectInfo m_cachedDirectEnemyHitEffect;
	private StandardEffectInfo m_cachedAoeEnemyHitEffect;
	private StandardEffectInfo m_cachedAoeThroughWallsEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Dash Through Wall";
		}
		m_syncComp = GetComponent<Manta_SyncComponent>();
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_DashThroughWall(
			this,
			GetWidth(),
			GetMaxRange(),
			GetMaxWidthOfWall(),
			GetAoeConeWidth(),
			GetAoeThroughWallConeWidth(),
			GetAoeConeLength(),
			GetAoeThroughWallConeLength(),
			m_extraTotalDistanceIfThroughWalls,
			m_coneBackwardOffset,
			DirectHitIgnoreCover(),
			m_clampConeToWall,
			m_aoeWithMiss);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetMaxRange();
	}

	private void SetCachedFields()
	{
		m_cachedDirectEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_directEnemyHitEffectMod.GetModifiedValue(m_directEnemyHitEffect)
			: m_directEnemyHitEffect;
		m_cachedAoeEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_aoeEnemyHitEffectMod.GetModifiedValue(m_aoeEnemyHitEffect)
			: m_aoeEnemyHitEffect;
		m_cachedAoeThroughWallsEffect = m_abilityMod != null
			? m_abilityMod.m_aoeThroughWallsEffectMod.GetModifiedValue(m_aoeThroughWallsEffect)
			: m_aoeThroughWallsEffect;
	}

	public float GetAoeConeWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeConeWidthMod.GetModifiedValue(m_aoeConeWidth)
			: m_aoeConeWidth;
	}

	public float GetAoeConeLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeConeLengthMod.GetModifiedValue(m_aoeConeLength)
			: m_aoeConeLength;
	}

	public float GetAoeThroughWallConeWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeThroughWallConeWidthMod.GetModifiedValue(m_aoeThroughWallConeWidth)
			: m_aoeThroughWallConeWidth;
	}

	public float GetAoeThroughWallConeLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeThroughWallConeLengthMod.GetModifiedValue(m_aoeThroughWallConeLength)
			: m_aoeThroughWallConeLength;
	}

	public float GetWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_widthMod.GetModifiedValue(m_width)
			: m_width;
	}

	public float GetMaxRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxRangeMod.GetModifiedValue(m_maxRange)
			: m_maxRange;
	}

	public float GetMaxWidthOfWall()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxWidthOfWallMod.GetModifiedValue(m_maxWidthOfWall)
			: m_maxWidthOfWall;
	}

	public float GetExtraTotalDistanceIfThroughWalls()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraTotalDistanceIfThroughWallsMod.GetModifiedValue(m_extraTotalDistanceIfThroughWalls)
			: m_extraTotalDistanceIfThroughWalls;
	}

	public bool ClampConeToWall()
	{
		return m_abilityMod != null
			? m_abilityMod.m_clampConeToWallMod.GetModifiedValue(m_clampConeToWall)
			: m_clampConeToWall;
	}

	public bool AoeWithMiss()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeWithMissMod.GetModifiedValue(m_aoeWithMiss)
			: m_aoeWithMiss;
	}

	public float GetConeBackwardOffset()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset)
			: m_coneBackwardOffset;
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

	public bool DirectHitIgnoreCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_directHitIgnoreCoverMod.GetModifiedValue(m_directHitIgnoreCover)
			: m_directHitIgnoreCover;
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

	public int GetAoeThroughWallsDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeThroughWallsDamageMod.GetModifiedValue(m_aoeThroughWallsDamage)
			: m_aoeThroughWallsDamage;
	}

	public StandardEffectInfo GetAoeThroughWallsEffect()
	{
		return m_cachedAoeThroughWallsEffect ?? m_aoeThroughWallsEffect;
	}

	public StandardEffectInfo GetAdditionalDirtyFightingExplosionEffect()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_additionalDirtyFightingExplosionEffect.operation == AbilityModPropertyEffectInfo.ModOp.Override
				? m_abilityMod.m_additionalDirtyFightingExplosionEffect.effectInfo
				: null;
	}

	public int GetMaxTargets()
	{
		return 1;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MantaDashThroughWall))
		{
			m_abilityMod = abilityMod as AbilityMod_MantaDashThroughWall;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DirectHitDamage", string.Empty, m_directHitDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_directEnemyHitEffect, "DirectEnemyHitEffect", m_directEnemyHitEffect);
		AddTokenInt(tokens, "AoeDamage", string.Empty, m_aoeDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_aoeEnemyHitEffect, "AoeEnemyHitEffect", m_aoeEnemyHitEffect);
		AddTokenInt(tokens, "AoeThroughWallsDamage", string.Empty, m_aoeThroughWallsDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_aoeThroughWallsEffect, "AoeThroughWallsEffect", m_aoeThroughWallsEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDirectHitDamage());
		GetDirectEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetAoeDamage());
		GetAoeEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Tertiary, GetAoeThroughWallsDamage());
		GetAoeThroughWallsEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Tertiary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			int damage = 0;
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				damage += GetDirectHitDamage();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				damage += GetAoeDamage();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Tertiary))
			{
				damage += GetAoeThroughWallsDamage();
			}
			dictionary[AbilityTooltipSymbol.Damage] = damage;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (m_syncComp == null)
		{
			return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
		}
		
		int energy = 0;
		foreach (AbilityUtil_Targeter.ActorTarget actorTarget in Targeters[currentTargeterIndex].GetActorsInRange())
		{
			energy += m_syncComp.GetDirtyFightingExtraTP(actorTarget.m_actor);
		}
		return energy;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return symbolType == AbilityTooltipSymbol.Damage
		       && m_syncComp != null
			? m_syncComp.GetAccessoryStringForDamage(targetActor, ActorData, this)
			: null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	internal static BoardSquare GetSquareBeyondWall(
		Vector3 startPos,
		Vector3 endPos,
		ActorData targetingActor,
		float penetrationDistance,
		ref Vector3 coneStartPos,
		ref Vector3 perpendicularFromWall)
	{
		float quarter = 0.25f * Board.Get().squareSize;
		int steps = Mathf.CeilToInt(penetrationDistance / quarter);
		Vector3 vector = endPos - startPos;
		float dist = vector.magnitude;
		vector.Normalize();
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(startPos, vector, dist, false, targetingActor);
		Vector3 vector2D = laserEndPoint;
		BoardSquare boardSquare = null;
		int i = 0;
		while (boardSquare == null)
		{
			vector2D += quarter * vector;
			boardSquare = Board.Get().GetSquareFromVec3(vector2D);
			if (boardSquare != null && !boardSquare.IsValidForGameplay())
			{
				boardSquare = null;
			}
			if (++i > steps)
			{
				break;
			}
		}
		if (boardSquare == null || !boardSquare.IsValidForGameplay())
		{
			boardSquare = Board.Get().GetSquareFromVec3(laserEndPoint);
			coneStartPos = endPos;
		}

		if (boardSquare != null)
		{
			Vector3 occupantLoSPos = boardSquare.GetOccupantLoSPos();
			Vector3 direction = (occupantLoSPos - startPos).normalized;
			direction.y = 0f;
			if (Mathf.Abs(direction.x) > 0.3f && Mathf.Abs(direction.z) > 0.3f)
			{
				float x = direction.x;
				direction.x = 0f;
				RaycastHit foo;
				if (!VectorUtils.RaycastInDirection(occupantLoSPos, -1f * direction.normalized, Board.Get().squareSize, out foo))
				{
					direction.z = 0f;
					direction.x = x;
					RaycastHit foo1;
					VectorUtils.RaycastInDirection(occupantLoSPos, -1f * direction.normalized, Board.Get().squareSize, out foo1);
				}
			}
			int angleWithHorizontal = Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(direction));
			perpendicularFromWall = VectorUtils.HorizontalAngleToClosestCardinalDirection(angleWithHorizontal);
			coneStartPos = occupantLoSPos - perpendicularFromWall * 0.5f;
		}
		return boardSquare;
	}
}
