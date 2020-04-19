using System;
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
	public int m_directDamage = 0x23;

	public int m_returnTripDamage = 0x14;

	public StandardGroundEffectInfo m_stationaryTrap;

	[Header("Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_returnTripSequencePrefab;

	private Neko_SyncComponent m_syncComp;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Super Disc";
		}
		this.m_syncComp = base.GetComponent<Neko_SyncComponent>();
		this.Setup();
	}

	private void Setup()
	{
		base.Targeter = new AbilityUtil_Targeter_CapsuleAoE(this, this.GetRadiusAroundStart(), this.GetRadiusAroundEnd(), this.GetLaserWidth(), this.GetMaxTargets(), false, false)
		{
			GetDefaultStartSquare = new AbilityUtil_Targeter_CapsuleAoE.StartSquareDelegate(this.GetCurrentDiscSquare)
		};
	}

	public float GetLaserWidth()
	{
		return this.m_laserWidth;
	}

	public float GetRadiusAroundStart()
	{
		return this.m_radiusAroundStart;
	}

	public float GetRadiusAroundEnd()
	{
		return this.m_radiusAroundEnd;
	}

	public int GetMaxTargets()
	{
		return this.m_maxTargets;
	}

	public int GetDirectDamage()
	{
		return this.m_directDamage;
	}

	public int GetReturnTripDamage()
	{
		return this.m_returnTripDamage;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "DirectDamage", string.Empty, this.m_directDamage, false);
		base.AddTokenInt(tokens, "ReturnTripDamage", string.Empty, this.m_returnTripDamage, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.m_directDamage),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, this.m_returnTripDamage)
		};
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (this.m_syncComp != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoSuperDisc.GetExpectedNumberOfTargeters()).MethodHandle;
			}
			if (this.m_syncComp.m_superDiscActive)
			{
				return 0;
			}
		}
		return 1;
	}

	public override bool IsFreeAction()
	{
		if (this.m_syncComp != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoSuperDisc.IsFreeAction()).MethodHandle;
			}
			if (this.m_syncComp.m_superDiscActive)
			{
				return true;
			}
		}
		return base.IsFreeAction();
	}

	public override int GetModdedCost()
	{
		if (this.m_syncComp != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoSuperDisc.GetModdedCost()).MethodHandle;
			}
			if (this.m_syncComp.m_superDiscActive)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				return 0;
			}
		}
		return base.GetModdedCost();
	}

	public override TargetData[] GetTargetData()
	{
		if (this.m_syncComp != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoSuperDisc.GetTargetData()).MethodHandle;
			}
			if (this.m_syncComp.m_superDiscActive)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				return new TargetData[0];
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
		if (this.m_syncComp != null && this.m_syncComp.m_superDiscActive)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoSuperDisc.CanTriggerAnimAtIndexForTaunt(int)).MethodHandle;
			}
			if (animIndex == (int)base.GetActionAnimType())
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				return true;
			}
		}
		return false;
	}

	public BoardSquare GetCurrentDiscSquare()
	{
		if (this.m_syncComp != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoSuperDisc.GetCurrentDiscSquare()).MethodHandle;
			}
			if (this.m_syncComp.m_superDiscActive)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				return Board.\u000E().\u0016(this.m_syncComp.m_superDiscBoardX, this.m_syncComp.m_superDiscBoardY);
			}
		}
		return null;
	}
}
