using System.Collections.Generic;
using UnityEngine;

public class GremlinsMultiTargeterApocolypse : Ability
{
	[Header("-- Targeting")]
	public AbilityAreaShape m_bombShape = AbilityAreaShape.Three_x_Three;
	public bool m_penetrateLos;
	public float m_minDistanceBetweenBombs = 1f;
	public float m_maxAngleWithFirst = 90f;
	[Header("-- Damage")]
	public int m_bombDamageAmount = 5;
	public int m_bombSubsequentDamageAmount = 3;
	[Header("-- Leave Mine on Empty Square")]
	public bool m_leaveLandmineOnEmptySquare;
	[Header("-- Energy Gain per Miss (no enemy hit)--")]
	public int m_energyGainPerMiss;
	public bool m_energyRefundAffectedByBuff;
	[Header("-- Sequences")]
	public GameObject m_bombSequencePrefab;

	private GremlinsLandMineInfoComponent m_bombInfoComp;
	private AbilityMod_GremlinsMultiTargeterApocolypse m_abilityMod;

	public AbilityMod_GremlinsMultiTargeterApocolypse GetMod()
	{
		return m_abilityMod;
	}

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "MultiTargeter Apocolypse";
		}
		m_bombInfoComp = GetComponent<GremlinsLandMineInfoComponent>();
		SetupTargeter();
	}

	public int GetDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_bombDamageAmount)
			: m_bombDamageAmount;
	}

	public int GetSubsequentDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_subsequentDamageMod.GetModifiedValue(m_bombSubsequentDamageAmount)
			: m_bombSubsequentDamageAmount;
	}

	public bool ShouldSpawnLandmineAtEmptySquare()
	{
		return m_abilityMod != null
			? m_abilityMod.m_leaveLandmineOnEmptySquaresMod.GetModifiedValue(m_leaveLandmineOnEmptySquare)
			: m_leaveLandmineOnEmptySquare;
	}

	public AbilityAreaShape GetBombShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shapeMod.GetModifiedValue(m_bombShape)
			: m_bombShape;
	}

	public float GetMinDistBetweenBombs()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minDistanceBetweenBombsMod.GetModifiedValue(m_minDistanceBetweenBombs)
			: m_minDistanceBetweenBombs;
	}

	public float GetMaxAngleWithFirstSegment()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxAngleWithFirstMod.GetModifiedValue(m_maxAngleWithFirst)
			: m_maxAngleWithFirst;
	}

	public bool GetPenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos)
			: m_penetrateLos;
	}

	public int GetEnergyGainPerMiss()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyGainPerMissMod.GetModifiedValue(m_energyGainPerMiss)
			: m_energyGainPerMiss;
	}

	private void SetupTargeter()
	{
		if (m_bombInfoComp == null)
		{
			m_bombInfoComp = GetComponent<GremlinsLandMineInfoComponent>();
		}
		if (m_bombSubsequentDamageAmount < 0)
		{
			m_bombSubsequentDamageAmount = 0;
		}
		if (GetExpectedNumberOfTargeters() > 1)
		{
			ClearTargeters();
			for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
			{
				AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, GetBombShape(), GetPenetrateLos());
				abilityUtil_Targeter_Shape.SetTooltipSubjectTypes();
				Targeters.Add(abilityUtil_Targeter_Shape);
			}
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_Shape(this, GetBombShape(), GetPenetrateLos());
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Max(1, GetNumTargets());
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamage());
		if (GetSubsequentDamage() != GetDamage())
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetSubsequentDamage());
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		BoardSquare firstSquare = Board.Get().GetSquare(Targeters[0].LastUpdatingGridPos);
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			if (i > 0)
			{
				BoardSquare curSquare = Board.Get().GetSquare(Targeters[i].LastUpdatingGridPos);
				if (curSquare == null || curSquare == firstSquare)
				{
					continue;
				}
			}
			AddNameplateValueForOverlap(
				ref symbolToValue,
				Targeters[i],
				targetActor,
				currentTargeterIndex,
				GetDamage(),
				GetSubsequentDamage());
		}
		return symbolToValue;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int num = 0;
		if (GetEnergyGainPerMiss() <= 0 || Targeters == null)
		{
			return num;
		}
		BoardSquare firstSquare = Board.Get().GetSquare(Targeters[0].LastUpdatingGridPos);
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			if (i >= Targeters.Count)
			{
				break;
			}
			AbilityUtil_Targeter abilityUtil_Targeter = Targeters[i];
			if (abilityUtil_Targeter == null)
			{
				continue;
			}
			if (i > 0)
			{
				BoardSquare curSquare = Board.Get().GetSquare(abilityUtil_Targeter.LastUpdatingGridPos);
				if (curSquare == null || curSquare == firstSquare)
				{
					continue;
				}
			}
			if (abilityUtil_Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy) == 0)
			{
				num += GetEnergyGainPerMiss();
			}
		}
		return num;
	}

	public override bool StatusAdjustAdditionalTechPointForTargeting()
	{
		return m_energyRefundAffectedByBuff;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null
		    || !targetSquare.IsValidForGameplay()
		    || targetSquare == caster.GetCurrentBoardSquare())
		{
			return false;
		}

		if (targetIndex <= 0)
		{
			return true;
		}
		Vector3 from = Board.Get().GetSquare(currentTargets[0].GridPos).ToVector3() - caster.GetFreePos();
		Vector3 to = targetSquare.ToVector3() - caster.GetFreePos();
		if (Mathf.RoundToInt(Vector3.Angle(from, to)) > (int)GetMaxAngleWithFirstSegment())
		{
			return false;
		}
		float minDist = GetMinDistBetweenBombs() * Board.Get().squareSize;
		for (int i = 0; i < targetIndex; i++)
		{
			BoardSquare prevTargetSquare = Board.Get().GetSquare(currentTargets[i].GridPos);
			Vector3 vector = targetSquare.ToVector3() - prevTargetSquare.ToVector3();
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (magnitude < minDist)
			{
				return false;
			}
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_GremlinsMultiTargeterApocolypse abilityMod_GremlinsMultiTargeterApocolypse = modAsBase as AbilityMod_GremlinsMultiTargeterApocolypse;
		AddTokenInt(tokens, "NumBombs", string.Empty, abilityMod_GremlinsMultiTargeterApocolypse != null
			? GetExpectedNumberOfTargeters()
			: Mathf.Max(1, m_targetData.Length));
		AddTokenInt(tokens, "Damage", string.Empty, abilityMod_GremlinsMultiTargeterApocolypse != null
			? abilityMod_GremlinsMultiTargeterApocolypse.m_damageMod.GetModifiedValue(m_bombDamageAmount)
			: m_bombDamageAmount);
		AddTokenInt(tokens, "Damage_OnOverlap", string.Empty, abilityMod_GremlinsMultiTargeterApocolypse != null
			? abilityMod_GremlinsMultiTargeterApocolypse.m_subsequentDamageMod.GetModifiedValue(m_bombSubsequentDamageAmount)
			: m_bombSubsequentDamageAmount);
		AddTokenInt(tokens, "EnergyGainPerMiss", string.Empty, abilityMod_GremlinsMultiTargeterApocolypse != null
			? abilityMod_GremlinsMultiTargeterApocolypse.m_energyGainPerMissMod.GetModifiedValue(m_energyGainPerMiss)
			: m_energyGainPerMiss);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_GremlinsMultiTargeterApocolypse))
		{
			m_abilityMod = abilityMod as AbilityMod_GremlinsMultiTargeterApocolypse;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
