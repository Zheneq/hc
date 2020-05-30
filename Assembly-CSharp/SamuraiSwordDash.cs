using System.Collections.Generic;
using UnityEngine;

public class SamuraiSwordDash : Ability
{
	[Header("-- Targeting")]
	public float m_damageRadius = 2f;

	public float m_damageRadiusAtStart = 2f;

	public float m_damageRadiusAtEnd;

	public bool m_penetrateLineOfSight;

	[Space(5f)]
	public bool m_canMoveAfterEvade;

	[Header("-- MaxTargets -> How many targets can be hit total | MaxDamageTargets-> how many targets can be damaged")]
	public int m_maxTargets;

	public int m_maxDamageTargets = 1;

	[Header("-- Enemy Hits, Dash Phase")]
	public int m_dashDamage = 30;

	public int m_dashLessDamagePerTarget = 5;

	public StandardEffectInfo m_dashEnemyHitEffect;

	[Header("-- Effect on Self")]
	public StandardEffectInfo m_dashSelfHitEffect;

	[Header("-- Mark data")]
	public StandardEffectInfo m_markEffectInfo;

	[Header("-- Energy Refund if target dashed away")]
	public int m_energyRefundIfTargetDashedAway;

	public bool m_energyRefundIgnoreBuff = true;

	[Separator("For Chain Ability (Knockback phase)", true)]
	public int m_knockbackDamage = 30;

	public int m_knockbackLessDamagePerTarget = 5;

	public float m_knockbackExtraDamageFromDamageTakenMult;

	[Space(10f)]
	public int m_knockbackExtraDamageByDist;

	public int m_knockbackExtraDamageChangePerDist;

	[Header("-- Knockback")]
	public float m_knockbackDist = 2f;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	[Separator("Sequences - Dash Phase (for knockback sequences, see chain ability)", true)]
	public GameObject m_castSequencePrefab;

	public GameObject m_afterimageSequencePrefab;

	private SamuraiAfterimageStrike m_chainedStrike;

	private AbilityMod_SamuraiSwordDash m_abilityMod;

	private Samurai_SyncComponent m_syncComponent;

	private StandardEffectInfo m_cachedDashEnemyHitEffect;

	private StandardEffectInfo m_cachedDashSelfHitEffect;

	private StandardEffectInfo m_cachedMarkEffectInfo;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Sword Dash";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_chainedStrike == null)
		{
			Ability[] chainAbilities = m_chainAbilities;
			int num = 0;
			while (true)
			{
				if (num < chainAbilities.Length)
				{
					Ability ability = chainAbilities[num];
					m_chainedStrike = (ability as SamuraiAfterimageStrike);
					if (m_chainedStrike != null)
					{
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}
		m_syncComponent = base.ActorData.GetComponent<Samurai_SyncComponent>();
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_SamuraiShowdown(this, GetDamageRadiusAtStart(), GetDamageRadiusAtEnd(), GetDamageRadius(), GetMaxTargets(), false, PenetrateLineOfSight(), m_knockbackDist, m_knockbackType);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedDashEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedDashEnemyHitEffect = m_abilityMod.m_dashEnemyHitEffectMod.GetModifiedValue(m_dashEnemyHitEffect);
		}
		else
		{
			cachedDashEnemyHitEffect = m_dashEnemyHitEffect;
		}
		m_cachedDashEnemyHitEffect = cachedDashEnemyHitEffect;
		StandardEffectInfo cachedDashSelfHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedDashSelfHitEffect = m_abilityMod.m_dashSelfHitEffectMod.GetModifiedValue(m_dashSelfHitEffect);
		}
		else
		{
			cachedDashSelfHitEffect = m_dashSelfHitEffect;
		}
		m_cachedDashSelfHitEffect = cachedDashSelfHitEffect;
		StandardEffectInfo cachedMarkEffectInfo;
		if ((bool)m_abilityMod)
		{
			cachedMarkEffectInfo = m_abilityMod.m_markEffectInfoMod.GetModifiedValue(m_markEffectInfo);
		}
		else
		{
			cachedMarkEffectInfo = m_markEffectInfo;
		}
		m_cachedMarkEffectInfo = cachedMarkEffectInfo;
	}

	public float GetDamageRadius()
	{
		return (!m_abilityMod) ? m_damageRadius : m_abilityMod.m_damageRadiusMod.GetModifiedValue(m_damageRadius);
	}

	public float GetDamageRadiusAtStart()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageRadiusAtStartMod.GetModifiedValue(m_damageRadiusAtStart);
		}
		else
		{
			result = m_damageRadiusAtStart;
		}
		return result;
	}

	public float GetDamageRadiusAtEnd()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageRadiusAtEndMod.GetModifiedValue(m_damageRadiusAtEnd);
		}
		else
		{
			result = m_damageRadiusAtEnd;
		}
		return result;
	}

	public bool PenetrateLineOfSight()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight);
		}
		else
		{
			result = m_penetrateLineOfSight;
		}
		return result;
	}

	public bool CanMoveAfterEvade()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_canMoveAfterEvadeMod.GetModifiedValue(m_canMoveAfterEvade);
		}
		else
		{
			result = m_canMoveAfterEvade;
		}
		return result;
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

	public int GetMaxDamageTargets()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxDamageTargetsMod.GetModifiedValue(m_maxDamageTargets);
		}
		else
		{
			result = m_maxDamageTargets;
		}
		return result;
	}

	public int GetDashDamage()
	{
		return (!m_abilityMod) ? m_dashDamage : m_abilityMod.m_dashDamageMod.GetModifiedValue(m_dashDamage);
	}

	public int GetDashLessDamagePerTarget()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_dashLessDamagePerTargetMod.GetModifiedValue(m_dashLessDamagePerTarget);
		}
		else
		{
			result = m_dashLessDamagePerTarget;
		}
		return result;
	}

	public StandardEffectInfo GetDashEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedDashEnemyHitEffect != null)
		{
			result = m_cachedDashEnemyHitEffect;
		}
		else
		{
			result = m_dashEnemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetDashSelfHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedDashSelfHitEffect != null)
		{
			result = m_cachedDashSelfHitEffect;
		}
		else
		{
			result = m_dashSelfHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetMarkEffectInfo()
	{
		StandardEffectInfo result;
		if (m_cachedMarkEffectInfo != null)
		{
			result = m_cachedMarkEffectInfo;
		}
		else
		{
			result = m_markEffectInfo;
		}
		return result;
	}

	public int GetEnergyRefundIfTargetDashedAway()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyRefundIfTargetDashedAwayMod.GetModifiedValue(m_energyRefundIfTargetDashedAway);
		}
		else
		{
			result = m_energyRefundIfTargetDashedAway;
		}
		return result;
	}

	public int GetKnockbackDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_knockbackDamageMod.GetModifiedValue(m_knockbackDamage);
		}
		else
		{
			result = m_knockbackDamage;
		}
		return result;
	}

	public int GetKnockbackLessDamagePerTarget()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_knockbackLessDamagePerTargetMod.GetModifiedValue(m_knockbackLessDamagePerTarget);
		}
		else
		{
			result = m_knockbackLessDamagePerTarget;
		}
		return result;
	}

	public float GetKnockbackExtraDamageFromDamageTakenMult()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_knockbackExtraDamageFromDamageTakenMultMod.GetModifiedValue(m_knockbackExtraDamageFromDamageTakenMult);
		}
		else
		{
			result = m_knockbackExtraDamageFromDamageTakenMult;
		}
		return result;
	}

	public int GetKnockbackExtraDamageByDist()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_knockbackExtraDamageByDistMod.GetModifiedValue(m_knockbackExtraDamageByDist);
		}
		else
		{
			result = m_knockbackExtraDamageByDist;
		}
		return result;
	}

	public int GetKnockbackExtraDamageChangePerDist()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_knockbackExtraDamageChangePerDistMod.GetModifiedValue(m_knockbackExtraDamageChangePerDist);
		}
		else
		{
			result = m_knockbackExtraDamageChangePerDist;
		}
		return result;
	}

	public float GetKnockbackDist()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_knockbackDistMod.GetModifiedValue(m_knockbackDist);
		}
		else
		{
			result = m_knockbackDist;
		}
		return result;
	}

	public KnockbackType GetKnockbackType()
	{
		return (!m_abilityMod) ? m_knockbackType : m_abilityMod.m_knockbackTypeMod.GetModifiedValue(m_knockbackType);
	}

	public int CalcExtraDamageForDashDist(BoardSquare startSquare, BoardSquare endSquare)
	{
		int num = 0;
		if (GetKnockbackExtraDamageByDist() <= 0)
		{
			if (GetKnockbackExtraDamageChangePerDist() == 0)
			{
				goto IL_00b9;
			}
		}
		num = GetKnockbackExtraDamageByDist();
		if (endSquare != null)
		{
			if (startSquare != null)
			{
				if (GetKnockbackExtraDamageChangePerDist() != 0)
				{
					if (KnockbackUtils.CanBuildStraightLineChargePath(base.ActorData, endSquare, startSquare, false, out int numSquaresInPath))
					{
						int num2 = numSquaresInPath - 2;
						if (num2 > 0)
						{
							num += num2 * GetKnockbackExtraDamageChangePerDist();
						}
					}
				}
			}
		}
		goto IL_00b9;
		IL_00b9:
		return num;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		int num = GetDashDamage();
		if (m_chainedStrike != null)
		{
			num += GetKnockbackDamage();
		}
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, num));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		AbilityUtil_Targeter_SamuraiShowdown abilityUtil_Targeter_SamuraiShowdown = base.Targeter as AbilityUtil_Targeter_SamuraiShowdown;
		if (abilityUtil_Targeter_SamuraiShowdown != null && abilityUtil_Targeter_SamuraiShowdown.OrderedHitActors.Contains(targetActor))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
					bool flag = true;
					if (GetMaxDamageTargets() > 0)
					{
						int num = abilityUtil_Targeter_SamuraiShowdown.OrderedHitActors.IndexOf(targetActor);
						if (num >= GetMaxDamageTargets())
						{
							flag = false;
						}
					}
					if (flag)
					{
						int num2 = GetDashDamage();
						int num3 = GetDashLessDamagePerTarget();
						if (m_chainedStrike != null)
						{
							num2 += GetKnockbackDamage();
							num3 += m_knockbackLessDamagePerTarget;
						}
						int b = num2 - num3 * (base.Targeter.GetNumActorsInRange() - 1);
						b = Mathf.Max(0, b);
						Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, b);
						if (m_syncComponent != null)
						{
							symbolToValue[AbilityTooltipSymbol.Damage] += m_syncComponent.CalcExtraDamageFromSelfBuffAbility();
						}
						BoardSquare boardSquareSafe = Board.Get().GetSquare(base.Targeter.LastUpdatingGridPos);
						BoardSquare currentBoardSquare = base.ActorData.CurrentBoardSquare;
						symbolToValue[AbilityTooltipSymbol.Damage] += CalcExtraDamageForDashDist(currentBoardSquare, boardSquareSafe);
					}
					else
					{
						symbolToValue[AbilityTooltipSymbol.Damage] = 0;
					}
					return symbolToValue;
				}
				}
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		SamuraiAfterimageStrike x = null;
		Ability[] chainAbilities = m_chainAbilities;
		foreach (Ability ability in chainAbilities)
		{
			x = (ability as SamuraiAfterimageStrike);
			if (x != null)
			{
				break;
			}
		}
		int num = m_dashDamage;
		int num2 = m_dashLessDamagePerTarget;
		if (x != null)
		{
			num += m_knockbackDamage;
			num2 += m_knockbackLessDamagePerTarget;
		}
		AddTokenInt(tokens, "DamageAmount", "includes chained AfterimageStrike if present", num);
		AddTokenInt(tokens, "LessDamagePerTarget", "includes chained AfterimageStrike if present", num2);
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "MaxDamageTargets", string.Empty, m_maxDamageTargets);
		AbilityMod.AddToken_EffectInfo(tokens, m_dashEnemyHitEffect, "DashEnemyHitEffect", m_dashEnemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_dashSelfHitEffect, "DashSelfHitEffect", m_dashSelfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_markEffectInfo, "MarkEffectInfo", m_markEffectInfo);
		AddTokenInt(tokens, "KnockbackDamage", string.Empty, m_knockbackDamage);
		AddTokenInt(tokens, "KnockbackLessDamagePerTarget", string.Empty, m_knockbackLessDamagePerTarget);
		AddTokenInt(tokens, "KnockbackExtraDamageByDist", string.Empty, m_knockbackExtraDamageByDist);
		AddTokenInt(tokens, "KnockbackExtraDamageChangePerDist", string.Empty, m_knockbackExtraDamageChangePerDist);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
				{
					return KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false) != null;
				}
			}
		}
		return false;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return CanMoveAfterEvade();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SamuraiSwordDash))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_SamuraiSwordDash);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
