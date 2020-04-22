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
		return (!(m_abilityMod == null)) ? m_abilityMod.m_damageMod.GetModifiedValue(m_bombDamageAmount) : m_bombDamageAmount;
	}

	public int GetSubsequentDamage()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_subsequentDamageMod.GetModifiedValue(m_bombSubsequentDamageAmount) : m_bombSubsequentDamageAmount;
	}

	public bool ShouldSpawnLandmineAtEmptySquare()
	{
		bool result;
		if (m_abilityMod == null)
		{
			result = m_leaveLandmineOnEmptySquare;
		}
		else
		{
			result = m_abilityMod.m_leaveLandmineOnEmptySquaresMod.GetModifiedValue(m_leaveLandmineOnEmptySquare);
		}
		return result;
	}

	public AbilityAreaShape GetBombShape()
	{
		AbilityAreaShape result;
		if (m_abilityMod == null)
		{
			result = m_bombShape;
		}
		else
		{
			result = m_abilityMod.m_shapeMod.GetModifiedValue(m_bombShape);
		}
		return result;
	}

	public float GetMinDistBetweenBombs()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_minDistanceBetweenBombsMod.GetModifiedValue(m_minDistanceBetweenBombs) : m_minDistanceBetweenBombs;
	}

	public float GetMaxAngleWithFirstSegment()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_maxAngleWithFirst;
		}
		else
		{
			result = m_abilityMod.m_maxAngleWithFirstMod.GetModifiedValue(m_maxAngleWithFirst);
		}
		return result;
	}

	public bool GetPenetrateLos()
	{
		bool result;
		if (m_abilityMod == null)
		{
			result = m_penetrateLos;
		}
		else
		{
			result = m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos);
		}
		return result;
	}

	public int GetEnergyGainPerMiss()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyGainPerMissMod.GetModifiedValue(m_energyGainPerMiss);
		}
		else
		{
			result = m_energyGainPerMiss;
		}
		return result;
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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					ClearTargeters();
					for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
					{
						AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, GetBombShape(), GetPenetrateLos());
						abilityUtil_Targeter_Shape.SetTooltipSubjectTypes();
						base.Targeters.Add(abilityUtil_Targeter_Shape);
					}
					return;
				}
				}
			}
		}
		base.Targeter = new AbilityUtil_Targeter_Shape(this, GetBombShape(), GetPenetrateLos());
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
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeters[0].LastUpdatingGridPos);
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			if (i > 0)
			{
				BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(base.Targeters[i].LastUpdatingGridPos);
				if (boardSquareSafe2 == null)
				{
					continue;
				}
				if (boardSquareSafe2 == boardSquareSafe)
				{
					continue;
				}
			}
			Ability.AddNameplateValueForOverlap(ref symbolToValue, base.Targeters[i], targetActor, currentTargeterIndex, GetDamage(), GetSubsequentDamage());
		}
		while (true)
		{
			return symbolToValue;
		}
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int num = 0;
		if (GetEnergyGainPerMiss() > 0)
		{
			if (base.Targeters != null)
			{
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeters[0].LastUpdatingGridPos);
				for (int i = 0; i <= currentTargeterIndex; i++)
				{
					if (i < base.Targeters.Count)
					{
						AbilityUtil_Targeter abilityUtil_Targeter = base.Targeters[i];
						if (abilityUtil_Targeter == null)
						{
							continue;
						}
						if (i > 0)
						{
							BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(abilityUtil_Targeter.LastUpdatingGridPos);
							if (boardSquareSafe2 == null)
							{
								continue;
							}
							if (boardSquareSafe2 == boardSquareSafe)
							{
								continue;
							}
						}
						if (abilityUtil_Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy) == 0)
						{
							num += GetEnergyGainPerMiss();
						}
						continue;
					}
					break;
				}
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
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null) && boardSquareSafe.IsBaselineHeight())
		{
			if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
			{
				if (targetIndex > 0)
				{
					bool flag = true;
					Vector3 from = Board.Get().GetBoardSquareSafe(currentTargets[0].GridPos).ToVector3() - caster.GetTravelBoardSquareWorldPosition();
					Vector3 to = boardSquareSafe.ToVector3() - caster.GetTravelBoardSquareWorldPosition();
					if (Mathf.RoundToInt(Vector3.Angle(from, to)) > (int)GetMaxAngleWithFirstSegment())
					{
						flag = false;
					}
					if (flag)
					{
						float num = GetMinDistBetweenBombs() * Board.Get().squareSize;
						int num2 = 0;
						while (true)
						{
							if (num2 < targetIndex)
							{
								BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTargets[num2].GridPos);
								Vector3 vector = boardSquareSafe.ToVector3() - boardSquareSafe2.ToVector3();
								vector.y = 0f;
								float magnitude = vector.magnitude;
								if (magnitude < num)
								{
									flag = false;
									break;
								}
								num2++;
								continue;
							}
							break;
						}
					}
					return flag;
				}
				return true;
			}
		}
		return false;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_GremlinsMultiTargeterApocolypse abilityMod_GremlinsMultiTargeterApocolypse = modAsBase as AbilityMod_GremlinsMultiTargeterApocolypse;
		int num;
		if ((bool)abilityMod_GremlinsMultiTargeterApocolypse)
		{
			num = GetExpectedNumberOfTargeters();
		}
		else
		{
			num = Mathf.Max(1, m_targetData.Length);
		}
		int val = num;
		AddTokenInt(tokens, "NumBombs", string.Empty, val);
		string empty = string.Empty;
		int val2;
		if ((bool)abilityMod_GremlinsMultiTargeterApocolypse)
		{
			val2 = abilityMod_GremlinsMultiTargeterApocolypse.m_damageMod.GetModifiedValue(m_bombDamageAmount);
		}
		else
		{
			val2 = m_bombDamageAmount;
		}
		AddTokenInt(tokens, "Damage", empty, val2);
		string empty2 = string.Empty;
		int val3;
		if ((bool)abilityMod_GremlinsMultiTargeterApocolypse)
		{
			val3 = abilityMod_GremlinsMultiTargeterApocolypse.m_subsequentDamageMod.GetModifiedValue(m_bombSubsequentDamageAmount);
		}
		else
		{
			val3 = m_bombSubsequentDamageAmount;
		}
		AddTokenInt(tokens, "Damage_OnOverlap", empty2, val3);
		AddTokenInt(tokens, "EnergyGainPerMiss", string.Empty, (!abilityMod_GremlinsMultiTargeterApocolypse) ? m_energyGainPerMiss : abilityMod_GremlinsMultiTargeterApocolypse.m_energyGainPerMissMod.GetModifiedValue(m_energyGainPerMiss));
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_GremlinsMultiTargeterApocolypse))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_GremlinsMultiTargeterApocolypse);
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
