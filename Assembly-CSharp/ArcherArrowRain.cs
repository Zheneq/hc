using System.Collections.Generic;
using UnityEngine;

public class ArcherArrowRain : Ability
{
	[Separator("Targeting Info", true)]
	public float m_startRadius = 3f;

	public float m_endRadius = 3f;

	public float m_lineRadius = 3f;

	public float m_minRangeBetween = 1f;

	public float m_maxRangeBetween = 4f;

	[Header("-- Whether require LoS to end square of line")]
	public bool m_linePenetrateLoS;

	[Header("-- Whether check LoS for gameplay hits")]
	public bool m_aoePenetrateLoS;

	public int m_maxTargets = 5;

	[Separator("Enemy Hit", true)]
	public int m_damage = 40;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	public GameObject m_hitAreaSequencePrefab;

	private AbilityMod_ArcherArrowRain m_abilityMod;

	private ArcherHealingDebuffArrow m_healArrowAbility;

	private AbilityData.ActionType m_healArrowActionType = AbilityData.ActionType.INVALID_ACTION;

	private AbilityData m_abilityData;

	private ActorTargeting m_actorTargeting;

	private Archer_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedAdditionalEnemyHitEffect;

	private StandardEffectInfo m_cachedSingleEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Arrow Rain";
		}
		m_abilityData = GetComponent<AbilityData>();
		if (m_abilityData != null)
		{
			m_healArrowAbility = (GetAbilityOfType(typeof(ArcherHealingDebuffArrow)) as ArcherHealingDebuffArrow);
			if (m_healArrowAbility != null)
			{
				m_healArrowActionType = m_abilityData.GetActionTypeOfAbility(m_healArrowAbility);
			}
		}
		m_actorTargeting = GetComponent<ActorTargeting>();
		m_syncComp = GetComponent<Archer_SyncComponent>();
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		base.Targeters.Clear();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_CapsuleAoE abilityUtil_Targeter_CapsuleAoE = new AbilityUtil_Targeter_CapsuleAoE(this, GetStartRadius(), GetEndRadius(), GetLineRadius(), GetMaxTargets(), false, AoePenetrateLoS());
			abilityUtil_Targeter_CapsuleAoE.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_CapsuleAoE.ShowArcToShape = false;
			base.Targeters.Add(abilityUtil_Targeter_CapsuleAoE);
		}
		while (true)
		{
			return;
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return GetTargetData().Length;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex > 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					{
						BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
						BoardSquare boardSquareSafe2 = Board.Get().GetSquare(target.GridPos);
						if (boardSquareSafe != null)
						{
							if (boardSquareSafe2 != null)
							{
								float num = Vector3.Distance(boardSquareSafe.ToVector3(), boardSquareSafe2.ToVector3());
								if (num <= GetMaxRangeBetween() * Board.Get().squareSize)
								{
									if (num >= GetMinRangeBetween() * Board.Get().squareSize)
									{
										if (!LinePenetrateLoS())
										{
											if (!boardSquareSafe._0013(boardSquareSafe2.x, boardSquareSafe2.y))
											{
												goto IL_0117;
											}
										}
										return true;
									}
								}
							}
						}
						goto IL_0117;
					}
					IL_0117:
					return false;
				}
			}
		}
		return base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "Damage", string.Empty, m_damage);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ArcherArrowRain))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ArcherArrowRain);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		object cachedAdditionalEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedAdditionalEnemyHitEffect = m_abilityMod.m_additionalEnemyHitEffect.GetModifiedValue(null);
		}
		else
		{
			cachedAdditionalEnemyHitEffect = null;
		}
		m_cachedAdditionalEnemyHitEffect = (StandardEffectInfo)cachedAdditionalEnemyHitEffect;
		m_cachedSingleEnemyHitEffect = ((!m_abilityMod) ? null : m_abilityMod.m_singleEnemyHitEffectMod.GetModifiedValue(null));
	}

	public float GetStartRadius()
	{
		return (!m_abilityMod) ? m_startRadius : m_abilityMod.m_startRadiusMod.GetModifiedValue(m_startRadius);
	}

	public float GetEndRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_endRadiusMod.GetModifiedValue(m_endRadius);
		}
		else
		{
			result = m_endRadius;
		}
		return result;
	}

	public float GetLineRadius()
	{
		return (!m_abilityMod) ? m_lineRadius : m_abilityMod.m_lineRadiusMod.GetModifiedValue(m_lineRadius);
	}

	public float GetMinRangeBetween()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_minRangeBetweenMod.GetModifiedValue(m_minRangeBetween);
		}
		else
		{
			result = m_minRangeBetween;
		}
		return result;
	}

	public float GetMaxRangeBetween()
	{
		return (!m_abilityMod) ? m_maxRangeBetween : m_abilityMod.m_maxRangeBetweenMod.GetModifiedValue(m_maxRangeBetween);
	}

	public bool LinePenetrateLoS()
	{
		return (!m_abilityMod) ? m_linePenetrateLoS : m_abilityMod.m_linePenetrateLoSMod.GetModifiedValue(m_linePenetrateLoS);
	}

	public bool AoePenetrateLoS()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_aoePenetrateLoSMod.GetModifiedValue(m_aoePenetrateLoS);
		}
		else
		{
			result = m_aoePenetrateLoS;
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

	public int GetDamageBelowHealthThreshold()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageBelowHealthThresholdMod.GetModifiedValue(GetDamage());
		}
		else
		{
			result = GetDamage();
		}
		return result;
	}

	public float GetHealthThresholdForBonusDamage()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healthThresholdForDamageMod.GetModifiedValue(0f);
		}
		else
		{
			result = 0f;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
		{
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAdditionalEnemyHitEffect()
	{
		return m_cachedAdditionalEnemyHitEffect;
	}

	public StandardEffectInfo GetSingleEnemyHitEffect()
	{
		return m_cachedSingleEnemyHitEffect;
	}

	public int GetTechPointRefundNoHits()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_techPointRefundNoHits.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int num = GetDamage();
		if (targetActor.GetHitPointShareOfMax() <= GetHealthThresholdForBonusDamage())
		{
			num = GetDamageBelowHealthThreshold();
		}
		if (IsReactionHealTarget(targetActor))
		{
			num += m_healArrowAbility.GetExtraDamageToThisTargetFromCaster();
		}
		dictionary[AbilityTooltipSymbol.Damage] = num;
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[currentTargeterIndex].GetActorsInRange();
		using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityUtil_Targeter.ActorTarget current = enumerator.Current;
				if (IsReactionHealTarget(current.m_actor))
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							return m_healArrowAbility.GetTechPointsPerHeal();
						}
					}
				}
			}
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	private bool IsReactionHealTarget(ActorData targetActor)
	{
		if (m_syncComp.m_healReactionTargetActor == targetActor.ActorIndex)
		{
			if (!m_syncComp.ActorHasUsedHealReaction(base.ActorData))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		if (m_healArrowActionType != AbilityData.ActionType.INVALID_ACTION)
		{
			if (m_actorTargeting != null)
			{
				List<AbilityTarget> abilityTargetsInRequest = m_actorTargeting.GetAbilityTargetsInRequest(m_healArrowActionType);
				if (abilityTargetsInRequest != null)
				{
					if (abilityTargetsInRequest.Count > 0)
					{
						BoardSquare boardSquareSafe = Board.Get().GetSquare(abilityTargetsInRequest[0].GridPos);
						ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(boardSquareSafe, true, false, base.ActorData);
						if (targetableActorOnSquare == targetActor)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									return true;
								}
							}
						}
					}
				}
			}
		}
		return false;
	}
}
