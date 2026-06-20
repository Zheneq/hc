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
		Targeter = new AbilityUtil_Targeter_CapsuleAoE(
			this,
			GetRadiusAroundStart(),
			GetRadiusAroundEnd(),
			GetLaserWidth(),
			GetMaxTargets(),
			false,
			false)
		{
			GetDefaultStartSquare = GetCurrentDiscSquare
		};
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
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_directDamage),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, m_returnTripDamage)
		};
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_syncComp != null && m_syncComp.m_superDiscActive
			? 0
			: 1;
	}

	public override bool IsFreeAction()
	{
		return m_syncComp != null && m_syncComp.m_superDiscActive
		       || base.IsFreeAction();
	}

	public override int GetModdedCost()
	{
		return m_syncComp != null && m_syncComp.m_superDiscActive
			? 0
			: base.GetModdedCost();
	}

	public override TargetData[] GetTargetData()
	{
		return m_syncComp != null && m_syncComp.m_superDiscActive
			? new TargetData[0]
			: base.GetTargetData();
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return m_syncComp != null
		       && m_syncComp.m_superDiscActive
		       && animIndex == (int)base.GetActionAnimType();
	}

	public BoardSquare GetCurrentDiscSquare()
	{
		return m_syncComp != null && m_syncComp.m_superDiscActive
			? Board.Get().GetSquareFromIndex(m_syncComp.m_superDiscBoardX, m_syncComp.m_superDiscBoardY)
			: null;
	}
}
