using System.Collections.Generic;
using UnityEngine;

public class ValkyriePullToLaserCenter : Ability
{
	[Header("-- Targeting")]
	public float m_laserWidth = 5f;

	public float m_laserRangeInSquares = 6.5f;

	public int m_maxTargets = 5;

	public bool m_lengthIgnoreLos = true;

	[Header("-- Damage & effects")]
	public int m_damage = 40;

	public StandardEffectInfo m_effectToEnemies;

	public int m_extraDamageForCenterHits;

	public float m_centerHitWidth = 0.1f;

	[Header("-- Knockback on Cast")]
	public float m_maxKnockbackDist = 3f;

	public KnockbackType m_knockbackType = KnockbackType.PerpendicularPullToAimDir;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ValkyriePullToLaserCenter m_abilityMod;

	private StandardEffectInfo m_cachedEffectToEnemies;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Valkyrie Pull Beam";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		AbilityUtil_Targeter_KnockbackLaser abilityUtil_Targeter_KnockbackLaser = new AbilityUtil_Targeter_KnockbackLaser(this, GetLaserWidth(), GetLaserRangeInSquares(), false, m_maxTargets, GetMaxKnockbackDist(), GetMaxKnockbackDist(), m_knockbackType, false);
		abilityUtil_Targeter_KnockbackLaser.LengthIgnoreWorldGeo = m_lengthIgnoreLos;
		base.Targeter = abilityUtil_Targeter_KnockbackLaser;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRangeInSquares();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectToEnemies;
		if ((bool)m_abilityMod)
		{
			cachedEffectToEnemies = m_abilityMod.m_effectToEnemiesMod.GetModifiedValue(m_effectToEnemies);
		}
		else
		{
			cachedEffectToEnemies = m_effectToEnemies;
		}
		m_cachedEffectToEnemies = cachedEffectToEnemies;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "Damage", string.Empty, m_damage);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectToEnemies, "EffectToEnemies", m_effectToEnemies);
		AddTokenInt(tokens, "ExtraDamageForCenterHits", string.Empty, m_extraDamageForCenterHits);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int num = GetDamage();
		int extraDamageIfKnockedInPlace = GetExtraDamageIfKnockedInPlace();
		if (extraDamageIfKnockedInPlace != 0 && !targetActor.GetActorStatus().IsMovementDebuffImmune())
		{
			List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeter.GetActorsInRange();
			using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
			{
				while (true)
				{
					if (!enumerator.MoveNext())
					{
						break;
					}
					AbilityUtil_Targeter.ActorTarget current = enumerator.Current;
					if (current.m_actor == targetActor)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								if (current.m_subjectTypes.Contains(AbilityTooltipSubject.HighHP))
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											num += extraDamageIfKnockedInPlace;
											goto end_IL_0049;
										}
									}
								}
								goto end_IL_0049;
							}
						}
					}
				}
				end_IL_0049:;
			}
		}
		int extraDamageForCenterHits = GetExtraDamageForCenterHits();
		if (extraDamageForCenterHits > 0 && base.Targeter is AbilityUtil_Targeter_KnockbackLaser)
		{
			AbilityUtil_Targeter_KnockbackLaser abilityUtil_Targeter_KnockbackLaser = base.Targeter as AbilityUtil_Targeter_KnockbackLaser;
			if (AreaEffectUtils.IsSquareInBoxByActorRadius(targetActor.GetCurrentBoardSquare(), base.ActorData.GetTravelBoardSquareWorldPositionForLos(), abilityUtil_Targeter_KnockbackLaser.GetLastLaserEndPos(), GetCenterHitWidth()))
			{
				num += extraDamageForCenterHits;
			}
		}
		dictionary[AbilityTooltipSymbol.Damage] = num;
		return dictionary;
	}

	public float GetLaserWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
		}
		else
		{
			result = m_laserWidth;
		}
		return result;
	}

	public float GetLaserRangeInSquares()
	{
		return (!m_abilityMod) ? m_laserRangeInSquares : m_abilityMod.m_laserRangeInSquaresMod.GetModifiedValue(m_laserRangeInSquares);
	}

	public int GetMaxTargets()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
		}
		else
		{
			result = m_maxTargets;
		}
		return result;
	}

	public bool LengthIgnoreLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_lengthIgnoreLosMod.GetModifiedValue(m_lengthIgnoreLos);
		}
		else
		{
			result = m_lengthIgnoreLos;
		}
		return result;
	}

	public int GetDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damage);
		}
		else
		{
			result = m_damage;
		}
		return result;
	}

	public int GetExtraDamageIfKnockedInPlace()
	{
		return m_abilityMod ? m_abilityMod.m_extraDamageIfKnockedInPlaceMod.GetModifiedValue(0) : 0;
	}

	public StandardEffectInfo GetEffectToEnemies()
	{
		StandardEffectInfo result;
		if (m_cachedEffectToEnemies != null)
		{
			result = m_cachedEffectToEnemies;
		}
		else
		{
			result = m_effectToEnemies;
		}
		return result;
	}

	public int GetExtraDamageForCenterHits()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_extraDamageForCenterHitsMod.GetModifiedValue(m_extraDamageForCenterHits);
		}
		else
		{
			result = m_extraDamageForCenterHits;
		}
		return result;
	}

	public float GetCenterHitWidth()
	{
		float result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_centerHitWidthMod.GetModifiedValue(m_centerHitWidth);
		}
		else
		{
			result = m_centerHitWidth;
		}
		return result;
	}

	public float GetMaxKnockbackDist()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxKnockbackDistMod.GetModifiedValue(m_maxKnockbackDist);
		}
		else
		{
			result = m_maxKnockbackDist;
		}
		return result;
	}

	public KnockbackType GetKnockbackType()
	{
		return (!m_abilityMod) ? m_knockbackType : m_abilityMod.m_knockbackTypeMod.GetModifiedValue(m_knockbackType);
	}

	public bool ShouldSkipDamageReductionOnNextTurnStab()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = (m_abilityMod.m_nextTurnStabSkipsDamageReduction.GetModifiedValue(false) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ValkyriePullToLaserCenter))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ValkyriePullToLaserCenter);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
