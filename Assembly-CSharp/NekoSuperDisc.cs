using System.Collections.Generic;
using UnityEngine;

public class NekoSuperDisc : Ability
{
	[Header("Targeting")]
	public float m_laserWidth = 2f;

	public float m_radiusAroundStart = 2f;

	public float m_radiusAroundEnd = 2f;

	public int m_maxTargets;

	[Header("Damage stuff")]
	public int m_directDamage = 35;

	public int m_returnTripDamage = 20;

	public StandardGroundEffectInfo m_stationaryTrap;

	[Header("Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_returnTripSequencePrefab;

	private Neko_SyncComponent m_syncComp;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Super Disc";
		}
		m_syncComp = GetComponent<Neko_SyncComponent>();
		Setup();
	}

	private void Setup()
	{
		AbilityUtil_Targeter_CapsuleAoE abilityUtil_Targeter_CapsuleAoE = new AbilityUtil_Targeter_CapsuleAoE(this, GetRadiusAroundStart(), GetRadiusAroundEnd(), GetLaserWidth(), GetMaxTargets(), false, false);
		abilityUtil_Targeter_CapsuleAoE.GetDefaultStartSquare = GetCurrentDiscSquare;
		base.Targeter = abilityUtil_Targeter_CapsuleAoE;
	}

	public float GetLaserWidth()
	{
		return m_laserWidth;
	}

	public float GetRadiusAroundStart()
	{
		return m_radiusAroundStart;
	}

	public float GetRadiusAroundEnd()
	{
		return m_radiusAroundEnd;
	}

	public int GetMaxTargets()
	{
		return m_maxTargets;
	}

	public int GetDirectDamage()
	{
		return m_directDamage;
	}

	public int GetReturnTripDamage()
	{
		return m_returnTripDamage;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "DirectDamage", string.Empty, m_directDamage);
		AddTokenInt(tokens, "ReturnTripDamage", string.Empty, m_returnTripDamage);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_directDamage));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, m_returnTripDamage));
		return list;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (m_syncComp != null)
		{
			if (m_syncComp.m_superDiscActive)
			{
				return 0;
			}
		}
		return 1;
	}

	public override bool IsFreeAction()
	{
		if (m_syncComp != null)
		{
			if (m_syncComp.m_superDiscActive)
			{
				return true;
			}
		}
		return base.IsFreeAction();
	}

	public override int GetModdedCost()
	{
		if (m_syncComp != null)
		{
			if (m_syncComp.m_superDiscActive)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return 0;
					}
				}
			}
		}
		return base.GetModdedCost();
	}

	public override TargetData[] GetTargetData()
	{
		if (m_syncComp != null)
		{
			if (m_syncComp.m_superDiscActive)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return new TargetData[0];
					}
				}
			}
		}
		return base.GetTargetData();
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		return base.GetActionAnimType();
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		if (m_syncComp != null && m_syncComp.m_superDiscActive)
		{
			if (animIndex == (int)base.GetActionAnimType())
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
		return false;
	}

	public BoardSquare GetCurrentDiscSquare()
	{
		if (m_syncComp != null)
		{
			if (m_syncComp.m_superDiscActive)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return Board.Get().GetSquareFromIndex(m_syncComp.m_superDiscBoardX, m_syncComp.m_superDiscBoardY);
					}
				}
			}
		}
		return null;
	}
}
