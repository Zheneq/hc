using System.Collections.Generic;
using UnityEngine;

public class NekoAoeAroundDisc : Ability
{
	[Separator("Targeting", true)]
	public float m_aoeRadius;

	public bool m_penetrateLoS;

	public int m_maxTargets;

	[Separator("On Hit Damage/Effect", true)]
	public int m_damageAmount;

	public StandardEffectInfo m_effectOnEnemies;

	public bool m_removeTargetDiscBeforeReturn;

	public float m_knockbackDist;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private Neko_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedEffectOnEnemies;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "AoE Around Disc";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		AbilityUtil_Targeter_AoE_Smooth abilityUtil_Targeter_AoE_Smooth = new AbilityUtil_Targeter_AoE_Smooth(this, GetAoeRadius(), PenetrateLoS(), true, false, GetMaxTargets());
		abilityUtil_Targeter_AoE_Smooth.m_customCenterPosDelegate = ClampToSquareCenter;
		abilityUtil_Targeter_AoE_Smooth.SetupKnockbackData(GetKnockbackDist(), GetKnockbackType());
		base.Targeters.Add(abilityUtil_Targeter_AoE_Smooth);
		m_syncComp = GetComponent<Neko_SyncComponent>();
	}

	private void SetCachedFields()
	{
		m_cachedEffectOnEnemies = m_effectOnEnemies;
	}

	public float GetAoeRadius()
	{
		return m_aoeRadius;
	}

	public bool PenetrateLoS()
	{
		return m_penetrateLoS;
	}

	public int GetMaxTargets()
	{
		return m_maxTargets;
	}

	public int GetDamageAmount()
	{
		return m_damageAmount;
	}

	public StandardEffectInfo GetEffectOnEnemies()
	{
		return (m_cachedEffectOnEnemies == null) ? m_effectOnEnemies : m_cachedEffectOnEnemies;
	}

	public bool RemoveTargetDiscBeforeReturn()
	{
		return m_removeTargetDiscBeforeReturn;
	}

	public float GetKnockbackDist()
	{
		return m_knockbackDist;
	}

	public KnockbackType GetKnockbackType()
	{
		return m_knockbackType;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnEnemies, "EffectOnEnemies", m_effectOnEnemies);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damageAmount));
		return list;
	}

	public override bool GetCheckLoS(int targetIndex)
	{
		if (GetTargetData().IsNullOrEmpty())
		{
			if (!m_targetData.IsNullOrEmpty())
			{
				return m_targetData[targetIndex].m_checkLineOfSight;
			}
		}
		return base.GetCheckLoS(targetIndex);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_syncComp != null && caster.GetCurrentBoardSquare() != null)
		{
			List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
			foreach (BoardSquare item in activeDiscSquares)
			{
				float minRange = m_targetData[0].m_minRange;
				float range = m_targetData[0].m_range;
				int num;
				if (caster.GetAbilityData().IsTargetSquareInRangeOfAbilityFromSquare(item, caster.GetCurrentBoardSquare(), range, minRange))
				{
					if (m_targetData[0].m_checkLineOfSight)
					{
						num = (caster.GetCurrentBoardSquare()._0013(item.x, item.y) ? 1 : 0);
					}
					else
					{
						num = 1;
					}
				}
				else
				{
					num = 0;
				}
				if (num != 0)
				{
					while (true)
					{
						switch (6)
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
		return false;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Contains(boardSquareSafe))
			{
				return true;
			}
		}
		return false;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Count > 1)
			{
				return 1;
			}
		}
		return 0;
	}

	public override TargetData[] GetTargetData()
	{
		if (m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Count > 1)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return base.GetTargetData();
					}
				}
			}
		}
		return new TargetData[0];
	}

	public override AbilityTarget CreateAbilityTargetForSimpleAction(ActorData caster)
	{
		AbilityTarget abilityTarget = base.CreateAbilityTargetForSimpleAction(caster);
		if (m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Count == 1)
			{
				abilityTarget.SetValuesFromBoardSquare(activeDiscSquares[0], activeDiscSquares[0].GetWorldPositionForLoS());
			}
		}
		return abilityTarget;
	}

	public Vector3 ClampToSquareCenter(ActorData caster, AbilityTarget currentTarget)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		return boardSquareSafe.GetWorldPositionForLoS();
	}
}
