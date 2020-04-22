using System.Collections.Generic;
using UnityEngine;

public class SoldierCardinalLine : Ability
{
	[Header("-- Targeting (shape for position targeter, line width for strafe hit area --")]
	public bool m_useBothCardinalDir;

	public AbilityAreaShape m_positionShape = AbilityAreaShape.Two_x_Two;

	public float m_lineWidth = 2f;

	public bool m_penetrateLos = true;

	[Header("-- On Hit Stuff --")]
	public int m_damageAmount = 10;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Extra Damage for near center")]
	public float m_nearCenterDistThreshold;

	public int m_extraDamageForNearCenterTargets;

	[Header("-- AoE around targets --")]
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Three_x_Three;

	public int m_aoeDamage;

	[Header("-- Subsequent Turn Hits --")]
	public int m_numSubsequentTurns;

	public int m_damageOnSubsequentTurns;

	public StandardEffectInfo m_enemyEffectOnSubsequentTurns;

	[Header("-- Sequences --")]
	public GameObject m_projectileSequencePrefab;

	private AbilityMod_SoldierCardinalLine m_abilityMod;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedEnemyEffectOnSubsequentTurns;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "Cardinal Line";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		ClearTargeters();
		AbilityUtil_Targeter_Shape item = new AbilityUtil_Targeter_Shape(this, GetPositionShape(), true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Never);
		base.Targeters.Add(item);
		AbilityUtil_Targeter_SoldierCardinalLines abilityUtil_Targeter_SoldierCardinalLines = new AbilityUtil_Targeter_SoldierCardinalLines(this, GetPositionShape(), GetLineWidth(), PenetrateLos(), UseBothCardinalDir(), GetAoeDamage() > 0, GetAoeShape());
		abilityUtil_Targeter_SoldierCardinalLines.SetUseMultiTargetUpdate(true);
		abilityUtil_Targeter_SoldierCardinalLines.SetAffectedGroups(true, false, false);
		base.Targeters.Add(abilityUtil_Targeter_SoldierCardinalLines);
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	public override TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		if (targetIndex == 1)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return TargetingParadigm.Direction;
				}
			}
		}
		return base.GetControlpadTargetingParadigm(targetIndex);
	}

	public override bool HasAimingOriginOverride(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out Vector3 overridePos)
	{
		if (targetIndex == 1)
		{
			overridePos = AreaEffectUtils.GetCenterOfShape(GetPositionShape(), targetsSoFar[0]);
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					min = 1f;
					max = 1f;
					return true;
				}
			}
		}
		return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		m_cachedEnemyEffectOnSubsequentTurns = ((!m_abilityMod) ? m_enemyEffectOnSubsequentTurns : m_abilityMod.m_enemyEffectOnSubsequentTurnsMod.GetModifiedValue(m_enemyEffectOnSubsequentTurns));
	}

	public bool UseBothCardinalDir()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_useBothCardinalDirMod.GetModifiedValue(m_useBothCardinalDir);
		}
		else
		{
			result = m_useBothCardinalDir;
		}
		return result;
	}

	public AbilityAreaShape GetPositionShape()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_positionShapeMod.GetModifiedValue(m_positionShape);
		}
		else
		{
			result = m_positionShape;
		}
		return result;
	}

	public float GetLineWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_lineWidthMod.GetModifiedValue(m_lineWidth);
		}
		else
		{
			result = m_lineWidth;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		return (!m_abilityMod) ? m_penetrateLos : m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos);
	}

	public int GetDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			result = m_damageAmount;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return (m_cachedEnemyHitEffect == null) ? m_enemyHitEffect : m_cachedEnemyHitEffect;
	}

	public float GetNearCenterDistThreshold()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_nearCenterDistThresholdMod.GetModifiedValue(m_nearCenterDistThreshold);
		}
		else
		{
			result = m_nearCenterDistThreshold;
		}
		return result;
	}

	public int GetExtraDamageForNearCenterTargets()
	{
		return (!m_abilityMod) ? m_extraDamageForNearCenterTargets : m_abilityMod.m_extraDamageForNearCenterTargetsMod.GetModifiedValue(m_extraDamageForNearCenterTargets);
	}

	public AbilityAreaShape GetAoeShape()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_aoeShapeMod.GetModifiedValue(m_aoeShape);
		}
		else
		{
			result = m_aoeShape;
		}
		return result;
	}

	public int GetAoeDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_aoeDamageMod.GetModifiedValue(m_aoeDamage);
		}
		else
		{
			result = m_aoeDamage;
		}
		return result;
	}

	public int GetNumSubsequentTurns()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_numSubsequentTurnsMod.GetModifiedValue(m_numSubsequentTurns);
		}
		else
		{
			result = m_numSubsequentTurns;
		}
		return result;
	}

	public int GetDamageOnSubsequentTurns()
	{
		return (!m_abilityMod) ? m_damageOnSubsequentTurns : m_abilityMod.m_damageOnSubsequentTurnsMod.GetModifiedValue(m_damageOnSubsequentTurns);
	}

	public StandardEffectInfo GetEnemyEffectOnSubsequentTurns()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyEffectOnSubsequentTurns != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedEnemyEffectOnSubsequentTurns;
		}
		else
		{
			result = m_enemyEffectOnSubsequentTurns;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageAmount());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (currentTargeterIndex > 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (currentTargeterIndex < base.Targeters.Count)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				AbilityUtil_Targeter_SoldierCardinalLines abilityUtil_Targeter_SoldierCardinalLines = base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_SoldierCardinalLines;
				if (abilityUtil_Targeter_SoldierCardinalLines != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					List<AbilityTooltipSubject> tooltipSubjectTypes = abilityUtil_Targeter_SoldierCardinalLines.GetTooltipSubjectTypes(targetActor);
					if (tooltipSubjectTypes != null && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						dictionary = new Dictionary<AbilityTooltipSymbol, int>();
						int num = 0;
						if (abilityUtil_Targeter_SoldierCardinalLines.m_directHitActorToCenterDist.ContainsKey(targetActor))
						{
							num += GetDamageAmount();
							if (GetExtraDamageForNearCenterTargets() > 0)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (abilityUtil_Targeter_SoldierCardinalLines.m_directHitActorToCenterDist[targetActor] <= GetNearCenterDistThreshold() * Board.Get().squareSize)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									num += GetExtraDamageForNearCenterTargets();
								}
							}
						}
						if (abilityUtil_Targeter_SoldierCardinalLines.m_aoeHitActors.Contains(targetActor))
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							num += GetAoeDamage();
						}
						dictionary[AbilityTooltipSymbol.Damage] = num;
					}
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SoldierCardinalLine abilityMod_SoldierCardinalLine = modAsBase as AbilityMod_SoldierCardinalLine;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SoldierCardinalLine)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			val = abilityMod_SoldierCardinalLine.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			val = m_damageAmount;
		}
		AddTokenInt(tokens, "DamageAmount", empty, val);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_SoldierCardinalLine) ? m_enemyHitEffect : abilityMod_SoldierCardinalLine.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect), "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "AoeDamage", string.Empty, (!abilityMod_SoldierCardinalLine) ? m_aoeDamage : abilityMod_SoldierCardinalLine.m_aoeDamageMod.GetModifiedValue(m_aoeDamage));
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_SoldierCardinalLine)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			val2 = abilityMod_SoldierCardinalLine.m_extraDamageForNearCenterTargetsMod.GetModifiedValue(m_extraDamageForNearCenterTargets);
		}
		else
		{
			val2 = m_extraDamageForNearCenterTargets;
		}
		AddTokenInt(tokens, "ExtraDamageForNearCenterTargets", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_SoldierCardinalLine)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			val3 = abilityMod_SoldierCardinalLine.m_numSubsequentTurnsMod.GetModifiedValue(m_numSubsequentTurns);
		}
		else
		{
			val3 = m_numSubsequentTurns;
		}
		AddTokenInt(tokens, "NumSubsequentTurns", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_SoldierCardinalLine)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			val4 = abilityMod_SoldierCardinalLine.m_damageOnSubsequentTurnsMod.GetModifiedValue(m_damageOnSubsequentTurns);
		}
		else
		{
			val4 = m_damageOnSubsequentTurns;
		}
		AddTokenInt(tokens, "DamageOnSubsequentTurns", empty4, val4);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_SoldierCardinalLine) ? m_enemyEffectOnSubsequentTurns : abilityMod_SoldierCardinalLine.m_enemyEffectOnSubsequentTurnsMod.GetModifiedValue(m_enemyEffectOnSubsequentTurns), "EnemyEffectOnSubsequentTurns", m_enemyEffectOnSubsequentTurns);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SoldierCardinalLine))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_SoldierCardinalLine);
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
