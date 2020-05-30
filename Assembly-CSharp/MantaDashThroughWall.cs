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
		base.Targeter = new AbilityUtil_Targeter_DashThroughWall(this, GetWidth(), GetMaxRange(), GetMaxWidthOfWall(), GetAoeConeWidth(), GetAoeThroughWallConeWidth(), GetAoeConeLength(), GetAoeThroughWallConeLength(), m_extraTotalDistanceIfThroughWalls, m_coneBackwardOffset, DirectHitIgnoreCover(), m_clampConeToWall, m_aoeWithMiss);
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
		m_cachedDirectEnemyHitEffect = ((!m_abilityMod) ? m_directEnemyHitEffect : m_abilityMod.m_directEnemyHitEffectMod.GetModifiedValue(m_directEnemyHitEffect));
		StandardEffectInfo cachedAoeEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedAoeEnemyHitEffect = m_abilityMod.m_aoeEnemyHitEffectMod.GetModifiedValue(m_aoeEnemyHitEffect);
		}
		else
		{
			cachedAoeEnemyHitEffect = m_aoeEnemyHitEffect;
		}
		m_cachedAoeEnemyHitEffect = cachedAoeEnemyHitEffect;
		m_cachedAoeThroughWallsEffect = ((!m_abilityMod) ? m_aoeThroughWallsEffect : m_abilityMod.m_aoeThroughWallsEffectMod.GetModifiedValue(m_aoeThroughWallsEffect));
	}

	public float GetAoeConeWidth()
	{
		return (!m_abilityMod) ? m_aoeConeWidth : m_abilityMod.m_aoeConeWidthMod.GetModifiedValue(m_aoeConeWidth);
	}

	public float GetAoeConeLength()
	{
		return (!m_abilityMod) ? m_aoeConeLength : m_abilityMod.m_aoeConeLengthMod.GetModifiedValue(m_aoeConeLength);
	}

	public float GetAoeThroughWallConeWidth()
	{
		return (!m_abilityMod) ? m_aoeThroughWallConeWidth : m_abilityMod.m_aoeThroughWallConeWidthMod.GetModifiedValue(m_aoeThroughWallConeWidth);
	}

	public float GetAoeThroughWallConeLength()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_aoeThroughWallConeLengthMod.GetModifiedValue(m_aoeThroughWallConeLength);
		}
		else
		{
			result = m_aoeThroughWallConeLength;
		}
		return result;
	}

	public float GetWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_widthMod.GetModifiedValue(m_width);
		}
		else
		{
			result = m_width;
		}
		return result;
	}

	public float GetMaxRange()
	{
		return (!m_abilityMod) ? m_maxRange : m_abilityMod.m_maxRangeMod.GetModifiedValue(m_maxRange);
	}

	public float GetMaxWidthOfWall()
	{
		return (!m_abilityMod) ? m_maxWidthOfWall : m_abilityMod.m_maxWidthOfWallMod.GetModifiedValue(m_maxWidthOfWall);
	}

	public float GetExtraTotalDistanceIfThroughWalls()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraTotalDistanceIfThroughWallsMod.GetModifiedValue(m_extraTotalDistanceIfThroughWalls);
		}
		else
		{
			result = m_extraTotalDistanceIfThroughWalls;
		}
		return result;
	}

	public bool ClampConeToWall()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_clampConeToWallMod.GetModifiedValue(m_clampConeToWall);
		}
		else
		{
			result = m_clampConeToWall;
		}
		return result;
	}

	public bool AoeWithMiss()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_aoeWithMissMod.GetModifiedValue(m_aoeWithMiss);
		}
		else
		{
			result = m_aoeWithMiss;
		}
		return result;
	}

	public float GetConeBackwardOffset()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset);
		}
		else
		{
			result = m_coneBackwardOffset;
		}
		return result;
	}

	public int GetDirectHitDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_directHitDamageMod.GetModifiedValue(m_directHitDamage);
		}
		else
		{
			result = m_directHitDamage;
		}
		return result;
	}

	public StandardEffectInfo GetDirectEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedDirectEnemyHitEffect != null)
		{
			result = m_cachedDirectEnemyHitEffect;
		}
		else
		{
			result = m_directEnemyHitEffect;
		}
		return result;
	}

	public bool DirectHitIgnoreCover()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_directHitIgnoreCoverMod.GetModifiedValue(m_directHitIgnoreCover);
		}
		else
		{
			result = m_directHitIgnoreCover;
		}
		return result;
	}

	public int GetAoeDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_aoeDamageMod.GetModifiedValue(m_aoeDamage);
		}
		else
		{
			result = m_aoeDamage;
		}
		return result;
	}

	public StandardEffectInfo GetAoeEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAoeEnemyHitEffect != null)
		{
			result = m_cachedAoeEnemyHitEffect;
		}
		else
		{
			result = m_aoeEnemyHitEffect;
		}
		return result;
	}

	public int GetAoeThroughWallsDamage()
	{
		return (!m_abilityMod) ? m_aoeThroughWallsDamage : m_abilityMod.m_aoeThroughWallsDamageMod.GetModifiedValue(m_aoeThroughWallsDamage);
	}

	public StandardEffectInfo GetAoeThroughWallsEffect()
	{
		return (m_cachedAoeThroughWallsEffect == null) ? m_aoeThroughWallsEffect : m_cachedAoeThroughWallsEffect;
	}

	public StandardEffectInfo GetAdditionalDirtyFightingExplosionEffect()
	{
		if ((bool)m_abilityMod && m_abilityMod.m_additionalDirtyFightingExplosionEffect.operation == AbilityModPropertyEffectInfo.ModOp.Override)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_abilityMod.m_additionalDirtyFightingExplosionEffect.effectInfo;
				}
			}
		}
		return null;
	}

	public int GetMaxTargets()
	{
		return 1;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_MantaDashThroughWall))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_MantaDashThroughWall);
			SetupTargeter();
			return;
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
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			int num = 0;
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				num += GetDirectHitDamage();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				num += GetAoeDamage();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Tertiary))
			{
				num += GetAoeThroughWallsDamage();
			}
			dictionary[AbilityTooltipSymbol.Damage] = num;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					int num = 0;
					List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[currentTargeterIndex].GetActorsInRange();
					using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							AbilityUtil_Targeter.ActorTarget current = enumerator.Current;
							num += m_syncComp.GetDirtyFightingExtraTP(current.m_actor);
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return num;
							}
						}
					}
				}
				}
			}
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (symbolType == AbilityTooltipSymbol.Damage)
		{
			if (m_syncComp != null)
			{
				return m_syncComp.GetAccessoryStringForDamage(targetActor, base.ActorData, this);
			}
		}
		return null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	internal static BoardSquare GetSquareBeyondWall(Vector3 startPos, Vector3 endPos, ActorData targetingActor, float penetrationDistance, ref Vector3 coneStartPos, ref Vector3 perpendicularFromWall)
	{
		float num = 0.25f * Board.Get().squareSize;
		int num2 = Mathf.CeilToInt(penetrationDistance / num);
		Vector3 vector = endPos - startPos;
		float magnitude = vector.magnitude;
		vector.Normalize();
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(startPos, vector, magnitude, false, targetingActor);
		Vector3 vector2D = laserEndPoint;
		BoardSquare boardSquare = null;
		int num3 = 0;
		do
		{
			if (boardSquare == null)
			{
				vector2D += num * vector;
				boardSquare = Board.Get().GetSquare(vector2D);
				if (!(boardSquare != null))
				{
					continue;
				}
				if (!boardSquare.IsBaselineHeight())
				{
					boardSquare = null;
				}
				continue;
			}
			break;
		}
		while (++num3 <= num2);
		if (!(boardSquare == null))
		{
			if (boardSquare.IsBaselineHeight())
			{
				goto IL_0113;
			}
		}
		boardSquare = Board.Get().GetSquare(laserEndPoint);
		coneStartPos = endPos;
		goto IL_0113;
		IL_0113:
		if (boardSquare != null)
		{
			Vector3 worldPositionForLoS = boardSquare.GetWorldPositionForLoS();
			Vector3 normalized = (worldPositionForLoS - startPos).normalized;
			normalized.y = 0f;
			if (Mathf.Abs(normalized.x) > 0.3f)
			{
				if (Mathf.Abs(normalized.z) > 0.3f)
				{
					float x = normalized.x;
					normalized.x = 0f;
					if (!VectorUtils.RaycastInDirection(worldPositionForLoS, -1f * normalized.normalized, Board.Get().squareSize, out RaycastHit hit))
					{
						normalized.z = 0f;
						normalized.x = x;
						bool flag = VectorUtils.RaycastInDirection(worldPositionForLoS, -1f * normalized.normalized, Board.Get().squareSize, out hit);
					}
				}
			}
			int angleWithHorizontal = Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(normalized));
			perpendicularFromWall = VectorUtils.HorizontalAngleToClosestCardinalDirection(angleWithHorizontal);
			coneStartPos = worldPositionForLoS - perpendicularFromWall * 0.5f;
		}
		return boardSquare;
	}
}
