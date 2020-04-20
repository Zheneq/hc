using System;
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
	public int m_directHitDamage = 0x14;

	public StandardEffectInfo m_directEnemyHitEffect;

	public bool m_directHitIgnoreCover = true;

	[Space(10f)]
	public int m_aoeDamage = 0xA;

	public StandardEffectInfo m_aoeEnemyHitEffect;

	[Space(10f)]
	public int m_aoeThroughWallsDamage = 0xA;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Dash Through Wall";
		}
		this.m_syncComp = base.GetComponent<Manta_SyncComponent>();
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_DashThroughWall(this, this.GetWidth(), this.GetMaxRange(), this.GetMaxWidthOfWall(), this.GetAoeConeWidth(), this.GetAoeThroughWallConeWidth(), this.GetAoeConeLength(), this.GetAoeThroughWallConeLength(), this.m_extraTotalDistanceIfThroughWalls, this.m_coneBackwardOffset, this.DirectHitIgnoreCover(), this.m_clampConeToWall, this.m_aoeWithMiss);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetMaxRange();
	}

	private void SetCachedFields()
	{
		this.m_cachedDirectEnemyHitEffect = ((!this.m_abilityMod) ? this.m_directEnemyHitEffect : this.m_abilityMod.m_directEnemyHitEffectMod.GetModifiedValue(this.m_directEnemyHitEffect));
		StandardEffectInfo cachedAoeEnemyHitEffect;
		if (this.m_abilityMod)
		{
			cachedAoeEnemyHitEffect = this.m_abilityMod.m_aoeEnemyHitEffectMod.GetModifiedValue(this.m_aoeEnemyHitEffect);
		}
		else
		{
			cachedAoeEnemyHitEffect = this.m_aoeEnemyHitEffect;
		}
		this.m_cachedAoeEnemyHitEffect = cachedAoeEnemyHitEffect;
		this.m_cachedAoeThroughWallsEffect = ((!this.m_abilityMod) ? this.m_aoeThroughWallsEffect : this.m_abilityMod.m_aoeThroughWallsEffectMod.GetModifiedValue(this.m_aoeThroughWallsEffect));
	}

	public float GetAoeConeWidth()
	{
		return (!this.m_abilityMod) ? this.m_aoeConeWidth : this.m_abilityMod.m_aoeConeWidthMod.GetModifiedValue(this.m_aoeConeWidth);
	}

	public float GetAoeConeLength()
	{
		return (!this.m_abilityMod) ? this.m_aoeConeLength : this.m_abilityMod.m_aoeConeLengthMod.GetModifiedValue(this.m_aoeConeLength);
	}

	public float GetAoeThroughWallConeWidth()
	{
		return (!this.m_abilityMod) ? this.m_aoeThroughWallConeWidth : this.m_abilityMod.m_aoeThroughWallConeWidthMod.GetModifiedValue(this.m_aoeThroughWallConeWidth);
	}

	public float GetAoeThroughWallConeLength()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_aoeThroughWallConeLengthMod.GetModifiedValue(this.m_aoeThroughWallConeLength);
		}
		else
		{
			result = this.m_aoeThroughWallConeLength;
		}
		return result;
	}

	public float GetWidth()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_widthMod.GetModifiedValue(this.m_width);
		}
		else
		{
			result = this.m_width;
		}
		return result;
	}

	public float GetMaxRange()
	{
		return (!this.m_abilityMod) ? this.m_maxRange : this.m_abilityMod.m_maxRangeMod.GetModifiedValue(this.m_maxRange);
	}

	public float GetMaxWidthOfWall()
	{
		return (!this.m_abilityMod) ? this.m_maxWidthOfWall : this.m_abilityMod.m_maxWidthOfWallMod.GetModifiedValue(this.m_maxWidthOfWall);
	}

	public float GetExtraTotalDistanceIfThroughWalls()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraTotalDistanceIfThroughWallsMod.GetModifiedValue(this.m_extraTotalDistanceIfThroughWalls);
		}
		else
		{
			result = this.m_extraTotalDistanceIfThroughWalls;
		}
		return result;
	}

	public bool ClampConeToWall()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_clampConeToWallMod.GetModifiedValue(this.m_clampConeToWall);
		}
		else
		{
			result = this.m_clampConeToWall;
		}
		return result;
	}

	public bool AoeWithMiss()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_aoeWithMissMod.GetModifiedValue(this.m_aoeWithMiss);
		}
		else
		{
			result = this.m_aoeWithMiss;
		}
		return result;
	}

	public float GetConeBackwardOffset()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(this.m_coneBackwardOffset);
		}
		else
		{
			result = this.m_coneBackwardOffset;
		}
		return result;
	}

	public int GetDirectHitDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_directHitDamageMod.GetModifiedValue(this.m_directHitDamage);
		}
		else
		{
			result = this.m_directHitDamage;
		}
		return result;
	}

	public StandardEffectInfo GetDirectEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedDirectEnemyHitEffect != null)
		{
			result = this.m_cachedDirectEnemyHitEffect;
		}
		else
		{
			result = this.m_directEnemyHitEffect;
		}
		return result;
	}

	public bool DirectHitIgnoreCover()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_directHitIgnoreCoverMod.GetModifiedValue(this.m_directHitIgnoreCover);
		}
		else
		{
			result = this.m_directHitIgnoreCover;
		}
		return result;
	}

	public int GetAoeDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_aoeDamageMod.GetModifiedValue(this.m_aoeDamage);
		}
		else
		{
			result = this.m_aoeDamage;
		}
		return result;
	}

	public StandardEffectInfo GetAoeEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAoeEnemyHitEffect != null)
		{
			result = this.m_cachedAoeEnemyHitEffect;
		}
		else
		{
			result = this.m_aoeEnemyHitEffect;
		}
		return result;
	}

	public int GetAoeThroughWallsDamage()
	{
		return (!this.m_abilityMod) ? this.m_aoeThroughWallsDamage : this.m_abilityMod.m_aoeThroughWallsDamageMod.GetModifiedValue(this.m_aoeThroughWallsDamage);
	}

	public StandardEffectInfo GetAoeThroughWallsEffect()
	{
		return (this.m_cachedAoeThroughWallsEffect == null) ? this.m_aoeThroughWallsEffect : this.m_cachedAoeThroughWallsEffect;
	}

	public StandardEffectInfo GetAdditionalDirtyFightingExplosionEffect()
	{
		if (this.m_abilityMod && this.m_abilityMod.m_additionalDirtyFightingExplosionEffect.operation == AbilityModPropertyEffectInfo.ModOp.Override)
		{
			return this.m_abilityMod.m_additionalDirtyFightingExplosionEffect.effectInfo;
		}
		return null;
	}

	public int GetMaxTargets()
	{
		return 1;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MantaDashThroughWall))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_MantaDashThroughWall);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "DirectHitDamage", string.Empty, this.m_directHitDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_directEnemyHitEffect, "DirectEnemyHitEffect", this.m_directEnemyHitEffect, true);
		base.AddTokenInt(tokens, "AoeDamage", string.Empty, this.m_aoeDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_aoeEnemyHitEffect, "AoeEnemyHitEffect", this.m_aoeEnemyHitEffect, true);
		base.AddTokenInt(tokens, "AoeThroughWallsDamage", string.Empty, this.m_aoeThroughWallsDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_aoeThroughWallsEffect, "AoeThroughWallsEffect", this.m_aoeThroughWallsEffect, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetDirectHitDamage());
		this.GetDirectEnemyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.GetAoeDamage());
		this.GetAoeEnemyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Tertiary, this.GetAoeThroughWallsDamage());
		this.GetAoeThroughWallsEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Tertiary);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			int num = 0;
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				num += this.GetDirectHitDamage();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				num += this.GetAoeDamage();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Tertiary))
			{
				num += this.GetAoeThroughWallsDamage();
			}
			dictionary[AbilityTooltipSymbol.Damage] = num;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (this.m_syncComp != null)
		{
			int num = 0;
			List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[currentTargeterIndex].GetActorsInRange();
			using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityUtil_Targeter.ActorTarget actorTarget = enumerator.Current;
					num += this.m_syncComp.GetDirtyFightingExtraTP(actorTarget.m_actor);
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
			if (this.m_syncComp != null)
			{
				return this.m_syncComp.GetAccessoryStringForDamage(targetActor, base.ActorData, this);
			}
		}
		return null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	internal unsafe static BoardSquare GetSquareBeyondWall(Vector3 startPos, Vector3 endPos, ActorData targetingActor, float penetrationDistance, ref Vector3 coneStartPos, ref Vector3 perpendicularFromWall)
	{
		float num = 0.25f * Board.Get().squareSize;
		int num2 = Mathf.CeilToInt(penetrationDistance / num);
		Vector3 vector = endPos - startPos;
		float magnitude = vector.magnitude;
		vector.Normalize();
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(startPos, vector, magnitude, false, targetingActor, null, true);
		Vector3 vector2 = laserEndPoint;
		BoardSquare boardSquare = null;
		int num3 = 0;
		while (boardSquare == null)
		{
			vector2 += num * vector;
			boardSquare = Board.Get().GetBoardSquare(vector2);
			if (boardSquare != null)
			{
				if (!boardSquare.IsBaselineHeight())
				{
					boardSquare = null;
				}
			}
			if (++num3 > num2)
			{
				break;
			}
		}
		if (boardSquare == null || !boardSquare.IsBaselineHeight())
		{
			boardSquare = Board.Get().GetBoardSquare(laserEndPoint);
			coneStartPos = endPos;
		}
		if (boardSquare != null)
		{
			Vector3 worldPositionForLoS = boardSquare.GetWorldPositionForLoS();
			Vector3 normalized = (worldPositionForLoS - startPos).normalized;
			normalized.y = 0f;
			if (Mathf.Abs(normalized.x) > 0.3f && Mathf.Abs(normalized.z) > 0.3f)
			{
				float x = normalized.x;
				normalized.x = 0f;
				RaycastHit raycastHit;
				if (!VectorUtils.RaycastInDirection(worldPositionForLoS, -1f * normalized.normalized, Board.Get().squareSize, out raycastHit))
				{
					normalized.z = 0f;
					normalized.x = x;
					bool flag = VectorUtils.RaycastInDirection(worldPositionForLoS, -1f * normalized.normalized, Board.Get().squareSize, out raycastHit);
				}
			}
			int angleWithHorizontal = Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(normalized));
			perpendicularFromWall = VectorUtils.HorizontalAngleToClosestCardinalDirection(angleWithHorizontal);
			coneStartPos = worldPositionForLoS - perpendicularFromWall * 0.5f;
		}
		return boardSquare;
	}
}
