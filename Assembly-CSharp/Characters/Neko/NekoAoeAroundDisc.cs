using System.Collections.Generic;
using UnityEngine;

public class NekoAoeAroundDisc : Ability
{
	[Separator("Targeting")]
	public float m_aoeRadius;
	public bool m_penetrateLoS;
	public int m_maxTargets;
	[Separator("On Hit Damage/Effect")]
	public int m_damageAmount;
	public StandardEffectInfo m_effectOnEnemies;
	public bool m_removeTargetDiscBeforeReturn;
	public float m_knockbackDist;
	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;
	[Separator("Sequences")]
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
		AbilityUtil_Targeter_AoE_Smooth targeter = new AbilityUtil_Targeter_AoE_Smooth(
			this, GetAoeRadius(), PenetrateLoS(), true, false, GetMaxTargets());
		targeter.m_customCenterPosDelegate = ClampToSquareCenter;
		targeter.SetupKnockbackData(GetKnockbackDist(), GetKnockbackType());
		Targeters.Add(targeter);
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
		return m_cachedEffectOnEnemies ?? m_effectOnEnemies;
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
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damageAmount)
		};
	}

	public override bool GetCheckLoS(int targetIndex)
	{
		return GetTargetData().IsNullOrEmpty() && !m_targetData.IsNullOrEmpty()
			? m_targetData[targetIndex].m_checkLineOfSight
			: base.GetCheckLoS(targetIndex);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_syncComp != null && caster.GetCurrentBoardSquare() != null)
		{
			foreach (BoardSquare item in m_syncComp.GetActiveDiscSquares())
			{
				bool isTargetInRange = caster.GetAbilityData().IsTargetSquareInRangeOfAbilityFromSquare(
					item, caster.GetCurrentBoardSquare(), m_targetData[0].m_range, m_targetData[0].m_minRange);
				if (isTargetInRange
				    && (!m_targetData[0].m_checkLineOfSight || caster.GetCurrentBoardSquare().GetLOS(item.x, item.y)))
				{
					return true;
				}
			}
		}
		return false;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		return m_syncComp != null && m_syncComp.GetActiveDiscSquares().Contains(targetSquare);
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_syncComp != null && m_syncComp.GetActiveDiscSquares().Count > 1
			? 1
			: 0;
	}

	public override TargetData[] GetTargetData()
	{
		return m_syncComp != null && m_syncComp.GetActiveDiscSquares().Count > 1
			? base.GetTargetData()
			: new TargetData[0];
	}

	public override AbilityTarget CreateAbilityTargetForSimpleAction(ActorData caster)
	{
		AbilityTarget abilityTarget = base.CreateAbilityTargetForSimpleAction(caster);
		if (m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Count == 1)
			{
				abilityTarget.SetValuesFromBoardSquare(activeDiscSquares[0], activeDiscSquares[0].GetOccupantLoSPos());
			}
		}
		return abilityTarget;
	}

	public Vector3 ClampToSquareCenter(ActorData caster, AbilityTarget currentTarget)
	{
		return Board.Get().GetSquare(currentTarget.GridPos).GetOccupantLoSPos();
	}
}
