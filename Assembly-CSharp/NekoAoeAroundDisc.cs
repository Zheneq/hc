using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "AoE Around Disc";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		AbilityUtil_Targeter_AoE_Smooth abilityUtil_Targeter_AoE_Smooth = new AbilityUtil_Targeter_AoE_Smooth(this, this.GetAoeRadius(), this.PenetrateLoS(), true, false, this.GetMaxTargets());
		abilityUtil_Targeter_AoE_Smooth.m_customCenterPosDelegate = new AbilityUtil_Targeter_AoE_Smooth.CustomCenterPosDelegate(this.ClampToSquareCenter);
		abilityUtil_Targeter_AoE_Smooth.SetupKnockbackData(this.GetKnockbackDist(), this.GetKnockbackType());
		base.Targeters.Add(abilityUtil_Targeter_AoE_Smooth);
		this.m_syncComp = base.GetComponent<Neko_SyncComponent>();
	}

	private void SetCachedFields()
	{
		this.m_cachedEffectOnEnemies = this.m_effectOnEnemies;
	}

	public float GetAoeRadius()
	{
		return this.m_aoeRadius;
	}

	public bool PenetrateLoS()
	{
		return this.m_penetrateLoS;
	}

	public int GetMaxTargets()
	{
		return this.m_maxTargets;
	}

	public int GetDamageAmount()
	{
		return this.m_damageAmount;
	}

	public StandardEffectInfo GetEffectOnEnemies()
	{
		return (this.m_cachedEffectOnEnemies == null) ? this.m_effectOnEnemies : this.m_cachedEffectOnEnemies;
	}

	public bool RemoveTargetDiscBeforeReturn()
	{
		return this.m_removeTargetDiscBeforeReturn;
	}

	public float GetKnockbackDist()
	{
		return this.m_knockbackDist;
	}

	public KnockbackType GetKnockbackType()
	{
		return this.m_knockbackType;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, this.m_damageAmount, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnEnemies, "EffectOnEnemies", this.m_effectOnEnemies, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.m_damageAmount)
		};
	}

	public override bool GetCheckLoS(int targetIndex)
	{
		if (this.GetTargetData().IsNullOrEmpty<TargetData>())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoAoeAroundDisc.GetCheckLoS(int)).MethodHandle;
			}
			if (!this.m_targetData.IsNullOrEmpty<TargetData>())
			{
				return this.m_targetData[targetIndex].m_checkLineOfSight;
			}
		}
		return base.GetCheckLoS(targetIndex);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_syncComp != null && caster.GetCurrentBoardSquare() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoAoeAroundDisc.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			List<BoardSquare> activeDiscSquares = this.m_syncComp.GetActiveDiscSquares();
			foreach (BoardSquare boardSquare in activeDiscSquares)
			{
				float minRange = this.m_targetData[0].m_minRange;
				float range = this.m_targetData[0].m_range;
				bool flag = caster.GetAbilityData().IsTargetSquareInRangeOfAbilityFromSquare(boardSquare, caster.GetCurrentBoardSquare(), range, minRange);
				bool flag2;
				if (flag)
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
					if (this.m_targetData[0].m_checkLineOfSight)
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
						flag2 = caster.GetCurrentBoardSquare().\u0013(boardSquare.x, boardSquare.y);
					}
					else
					{
						flag2 = true;
					}
				}
				else
				{
					flag2 = false;
				}
				flag = flag2;
				if (flag)
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
					return true;
				}
			}
			return false;
		}
		return false;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (this.m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = this.m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Contains(boardSquareSafe))
			{
				return true;
			}
		}
		return false;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (this.m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = this.m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Count > 1)
			{
				return 1;
			}
		}
		return 0;
	}

	public override TargetData[] GetTargetData()
	{
		if (this.m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = this.m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Count > 1)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(NekoAoeAroundDisc.GetTargetData()).MethodHandle;
				}
				return base.GetTargetData();
			}
		}
		return new TargetData[0];
	}

	public override AbilityTarget CreateAbilityTargetForSimpleAction(ActorData caster)
	{
		AbilityTarget abilityTarget = base.CreateAbilityTargetForSimpleAction(caster);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoAoeAroundDisc.CreateAbilityTargetForSimpleAction(ActorData)).MethodHandle;
			}
			List<BoardSquare> activeDiscSquares = this.m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Count == 1)
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
