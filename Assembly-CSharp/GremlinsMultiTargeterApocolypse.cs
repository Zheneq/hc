using System;
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
		return this.m_abilityMod;
	}

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsMultiTargeterApocolypse.Start()).MethodHandle;
			}
			this.m_abilityName = "MultiTargeter Apocolypse";
		}
		this.m_bombInfoComp = base.GetComponent<GremlinsLandMineInfoComponent>();
		this.SetupTargeter();
	}

	public int GetDamage()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_bombDamageAmount) : this.m_bombDamageAmount;
	}

	public int GetSubsequentDamage()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_subsequentDamageMod.GetModifiedValue(this.m_bombSubsequentDamageAmount) : this.m_bombSubsequentDamageAmount;
	}

	public bool ShouldSpawnLandmineAtEmptySquare()
	{
		bool result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsMultiTargeterApocolypse.ShouldSpawnLandmineAtEmptySquare()).MethodHandle;
			}
			result = this.m_leaveLandmineOnEmptySquare;
		}
		else
		{
			result = this.m_abilityMod.m_leaveLandmineOnEmptySquaresMod.GetModifiedValue(this.m_leaveLandmineOnEmptySquare);
		}
		return result;
	}

	public AbilityAreaShape GetBombShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsMultiTargeterApocolypse.GetBombShape()).MethodHandle;
			}
			result = this.m_bombShape;
		}
		else
		{
			result = this.m_abilityMod.m_shapeMod.GetModifiedValue(this.m_bombShape);
		}
		return result;
	}

	public float GetMinDistBetweenBombs()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_minDistanceBetweenBombsMod.GetModifiedValue(this.m_minDistanceBetweenBombs) : this.m_minDistanceBetweenBombs;
	}

	public float GetMaxAngleWithFirstSegment()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsMultiTargeterApocolypse.GetMaxAngleWithFirstSegment()).MethodHandle;
			}
			result = this.m_maxAngleWithFirst;
		}
		else
		{
			result = this.m_abilityMod.m_maxAngleWithFirstMod.GetModifiedValue(this.m_maxAngleWithFirst);
		}
		return result;
	}

	public bool GetPenetrateLos()
	{
		bool result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsMultiTargeterApocolypse.GetPenetrateLos()).MethodHandle;
			}
			result = this.m_penetrateLos;
		}
		else
		{
			result = this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
		}
		return result;
	}

	public int GetEnergyGainPerMiss()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsMultiTargeterApocolypse.GetEnergyGainPerMiss()).MethodHandle;
			}
			result = this.m_abilityMod.m_energyGainPerMissMod.GetModifiedValue(this.m_energyGainPerMiss);
		}
		else
		{
			result = this.m_energyGainPerMiss;
		}
		return result;
	}

	private void SetupTargeter()
	{
		if (this.m_bombInfoComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsMultiTargeterApocolypse.SetupTargeter()).MethodHandle;
			}
			this.m_bombInfoComp = base.GetComponent<GremlinsLandMineInfoComponent>();
		}
		if (this.m_bombSubsequentDamageAmount < 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_bombSubsequentDamageAmount = 0;
		}
		if (this.GetExpectedNumberOfTargeters() > 1)
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
			base.ClearTargeters();
			for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
			{
				AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, this.GetBombShape(), this.GetPenetrateLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
				abilityUtil_Targeter_Shape.SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Primary, AbilityTooltipSubject.None);
				base.Targeters.Add(abilityUtil_Targeter_Shape);
			}
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_Shape(this, this.GetBombShape(), this.GetPenetrateLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Max(1, base.GetNumTargets());
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetDamage());
		if (this.GetSubsequentDamage() != this.GetDamage())
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.GetSubsequentDamage());
		}
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeters[0].LastUpdatingGridPos);
		int i = 0;
		while (i <= currentTargeterIndex)
		{
			if (i <= 0)
			{
				goto IL_98;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsMultiTargeterApocolypse.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(base.Targeters[i].LastUpdatingGridPos);
			if (!(boardSquareSafe2 == null))
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
				if (!(boardSquareSafe2 == boardSquareSafe))
				{
					goto IL_98;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			IL_C1:
			i++;
			continue;
			IL_98:
			Ability.AddNameplateValueForOverlap(ref result, base.Targeters[i], targetActor, currentTargeterIndex, this.GetDamage(), this.GetSubsequentDamage(), AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
			goto IL_C1;
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		return result;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int num = 0;
		if (this.GetEnergyGainPerMiss() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsMultiTargeterApocolypse.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
			}
			if (base.Targeters != null)
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
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeters[0].LastUpdatingGridPos);
				int i = 0;
				while (i <= currentTargeterIndex)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (i >= base.Targeters.Count)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							return num;
						}
					}
					else
					{
						AbilityUtil_Targeter abilityUtil_Targeter = base.Targeters[i];
						if (abilityUtil_Targeter != null)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (i > 0)
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
								BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(abilityUtil_Targeter.LastUpdatingGridPos);
								if (!(boardSquareSafe2 == null))
								{
									if (!(boardSquareSafe2 == boardSquareSafe))
									{
										goto IL_CA;
									}
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								goto IL_EC;
							}
							IL_CA:
							if (abilityUtil_Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy) == 0)
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								num += this.GetEnergyGainPerMiss();
							}
						}
						IL_EC:
						i++;
					}
				}
			}
		}
		return num;
	}

	public override bool StatusAdjustAdditionalTechPointForTargeting()
	{
		return this.m_energyRefundAffectedByBuff;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null) && boardSquareSafe.IsBaselineHeight())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsMultiTargeterApocolypse.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
			{
				if (targetIndex > 0)
				{
					bool flag = true;
					Vector3 from = Board.Get().GetBoardSquareSafe(currentTargets[0].GridPos).ToVector3() - caster.GetTravelBoardSquareWorldPosition();
					Vector3 to = boardSquareSafe.ToVector3() - caster.GetTravelBoardSquareWorldPosition();
					if (Mathf.RoundToInt(Vector3.Angle(from, to)) > (int)this.GetMaxAngleWithFirstSegment())
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
						flag = false;
					}
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
						float num = this.GetMinDistBetweenBombs() * Board.Get().squareSize;
						for (int i = 0; i < targetIndex; i++)
						{
							BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTargets[i].GridPos);
							Vector3 vector = boardSquareSafe.ToVector3() - boardSquareSafe2.ToVector3();
							vector.y = 0f;
							float magnitude = vector.magnitude;
							if (magnitude < num)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								return false;
							}
						}
						for (;;)
						{
							switch (7)
							{
							case 0:
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
		if (abilityMod_GremlinsMultiTargeterApocolypse)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsMultiTargeterApocolypse.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			num = this.GetExpectedNumberOfTargeters();
		}
		else
		{
			num = Mathf.Max(1, this.m_targetData.Length);
		}
		int val = num;
		base.AddTokenInt(tokens, "NumBombs", string.Empty, val, false);
		string name = "Damage";
		string empty = string.Empty;
		int val2;
		if (abilityMod_GremlinsMultiTargeterApocolypse)
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
			val2 = abilityMod_GremlinsMultiTargeterApocolypse.m_damageMod.GetModifiedValue(this.m_bombDamageAmount);
		}
		else
		{
			val2 = this.m_bombDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val2, false);
		string name2 = "Damage_OnOverlap";
		string empty2 = string.Empty;
		int val3;
		if (abilityMod_GremlinsMultiTargeterApocolypse)
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
			val3 = abilityMod_GremlinsMultiTargeterApocolypse.m_subsequentDamageMod.GetModifiedValue(this.m_bombSubsequentDamageAmount);
		}
		else
		{
			val3 = this.m_bombSubsequentDamageAmount;
		}
		base.AddTokenInt(tokens, name2, empty2, val3, false);
		base.AddTokenInt(tokens, "EnergyGainPerMiss", string.Empty, (!abilityMod_GremlinsMultiTargeterApocolypse) ? this.m_energyGainPerMiss : abilityMod_GremlinsMultiTargeterApocolypse.m_energyGainPerMissMod.GetModifiedValue(this.m_energyGainPerMiss), false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_GremlinsMultiTargeterApocolypse))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsMultiTargeterApocolypse.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_GremlinsMultiTargeterApocolypse);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
