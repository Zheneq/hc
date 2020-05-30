using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NinjaShurikenOrDash : Ability
{
	[Separator("Dash - Type, Targeting Info", true)]
	public bool m_isTeleport = true;

	public float m_dashRangeDefault = 7.5f;

	public float m_dashRangeMarked = 7.5f;

	[Header("-- Who can be dash targets --")]
	public bool m_dashRequireDeathmark = true;

	public float m_dashToUnmarkedRange;

	[Space(5f)]
	public bool m_canDashToAlly;

	public bool m_canDashToEnemy = true;

	public bool m_dashIgnoreLos = true;

	public AbilityAreaShape m_dashDestShape = AbilityAreaShape.Three_x_Three;

	[Separator("Dash - On Hit Stuff", true)]
	public int m_dashDamage;

	public int m_extraDamageOnMarked;

	public int m_extraDamageIfNotMarked;

	public StandardEffectInfo m_dashEnemyHitEffect;

	public StandardEffectInfo m_extraEnemyEffectOnMarked;

	public bool m_delayExtraMarkedEffectToTurnStart = true;

	[Header("-- For All Hit --")]
	public int m_dashHealing;

	public StandardEffectInfo m_dashAllyHitEffect;

	[Separator("Dash - [Deathmark]", "magenta")]
	public bool m_dashApplyDeathmark = true;

	public bool m_canTriggerDeathmark = true;

	[Separator("Dash - Allow move after evade?", true)]
	public bool m_canQueueMoveAfterEvade = true;

	[Header("-- Sequences --")]
	public GameObject m_dashSequencePrefab;

	private AbilityMod_NinjaShurikenOrDash m_abilityMod;

	private Ninja_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedDashEnemyHitEffect;

	private StandardEffectInfo m_cachedExtraEnemyEffectOnMarked;

	private StandardEffectInfo m_cachedDashAllyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "NinjaShurikenOrDash";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Ninja_SyncComponent>();
		}
		ClearTargeters();
		AbilityUtil_Targeter_Shape item = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true);
		base.Targeters.Add(item);
		AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, 0f, 0f, 0f, -1, false, false);
		abilityUtil_Targeter_ChargeAoE.SetUseMultiTargetUpdate(true);
		abilityUtil_Targeter_ChargeAoE.ShowTeleportLines = GetIsTeleport();
		base.Targeters.Add(abilityUtil_Targeter_ChargeAoE);
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Design --</color>\nPlease edit [Deathmark] info on Ninja sync component.";
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Min(2, GetTargetData().Length);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return GetIsTeleport() ? ActorData.MovementType.Teleport : ActorData.MovementType.Charge;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return CanQueueMoveAfterEvade();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetDashRangeDefault() - 0.5f;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DashDamage", string.Empty, m_dashDamage);
		AddTokenInt(tokens, "ExtraDamageOnMarked", string.Empty, m_extraDamageOnMarked);
		AddTokenInt(tokens, "ExtraDamageIfNotMarked", string.Empty, m_extraDamageIfNotMarked);
		AbilityMod.AddToken_EffectInfo(tokens, m_dashEnemyHitEffect, "DashEnemyHitEffect", m_dashEnemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_extraEnemyEffectOnMarked, "ExtraEnemyEffectOnMarked", m_extraEnemyEffectOnMarked);
		AddTokenInt(tokens, "DashHealing", string.Empty, m_dashHealing);
		AbilityMod.AddToken_EffectInfo(tokens, m_dashAllyHitEffect, "DashAllyHitEffect", m_dashAllyHitEffect);
	}

	private void SetCachedFields()
	{
		m_cachedDashEnemyHitEffect = ((!m_abilityMod) ? m_dashEnemyHitEffect : m_abilityMod.m_dashEnemyHitEffectMod.GetModifiedValue(m_dashEnemyHitEffect));
		m_cachedExtraEnemyEffectOnMarked = ((!m_abilityMod) ? m_extraEnemyEffectOnMarked : m_abilityMod.m_extraEnemyEffectOnMarkedMod.GetModifiedValue(m_extraEnemyEffectOnMarked));
		m_cachedDashAllyHitEffect = ((!m_abilityMod) ? m_dashAllyHitEffect : m_abilityMod.m_dashAllyHitEffectMod.GetModifiedValue(m_dashAllyHitEffect));
	}

	public bool GetIsTeleport()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_isTeleportMod.GetModifiedValue(m_isTeleport);
		}
		else
		{
			result = m_isTeleport;
		}
		return result;
	}

	public float GetDashRangeDefault()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_dashRangeDefaultMod.GetModifiedValue(m_dashRangeDefault);
		}
		else
		{
			result = m_dashRangeDefault;
		}
		return result;
	}

	public float GetDashRangeMarked()
	{
		return (!m_abilityMod) ? m_dashRangeMarked : m_abilityMod.m_dashRangeMarkedMod.GetModifiedValue(m_dashRangeMarked);
	}

	public bool DashRequireDeathmark()
	{
		return (!m_abilityMod) ? m_dashRequireDeathmark : m_abilityMod.m_dashRequireDeathmarkMod.GetModifiedValue(m_dashRequireDeathmark);
	}

	public float GetDashToUnmarkedRange()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_dashToUnmarkedRangeMod.GetModifiedValue(m_dashToUnmarkedRange);
		}
		else
		{
			result = m_dashToUnmarkedRange;
		}
		return result;
	}

	public bool CanDashToAlly()
	{
		return (!m_abilityMod) ? m_canDashToAlly : m_abilityMod.m_canDashToAllyMod.GetModifiedValue(m_canDashToAlly);
	}

	public bool CanDashToEnemy()
	{
		return (!m_abilityMod) ? m_canDashToEnemy : m_abilityMod.m_canDashToEnemyMod.GetModifiedValue(m_canDashToEnemy);
	}

	public bool DashIgnoreLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_dashIgnoreLosMod.GetModifiedValue(m_dashIgnoreLos);
		}
		else
		{
			result = m_dashIgnoreLos;
		}
		return result;
	}

	public AbilityAreaShape GetDashDestShape()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_dashDestShapeMod.GetModifiedValue(m_dashDestShape);
		}
		else
		{
			result = m_dashDestShape;
		}
		return result;
	}

	public int GetDashDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_dashDamageMod.GetModifiedValue(m_dashDamage);
		}
		else
		{
			result = m_dashDamage;
		}
		return result;
	}

	public int GetExtraDamageOnMarked()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageOnMarkedMod.GetModifiedValue(m_extraDamageOnMarked);
		}
		else
		{
			result = m_extraDamageOnMarked;
		}
		return result;
	}

	public int GetExtraDamageIfNotMarked()
	{
		return (!m_abilityMod) ? m_extraDamageIfNotMarked : m_abilityMod.m_extraDamageIfNotMarkedMod.GetModifiedValue(m_extraDamageIfNotMarked);
	}

	public StandardEffectInfo GetDashEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedDashEnemyHitEffect != null)
		{
			result = m_cachedDashEnemyHitEffect;
		}
		else
		{
			result = m_dashEnemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetExtraEnemyEffectOnMarked()
	{
		StandardEffectInfo result;
		if (m_cachedExtraEnemyEffectOnMarked != null)
		{
			result = m_cachedExtraEnemyEffectOnMarked;
		}
		else
		{
			result = m_extraEnemyEffectOnMarked;
		}
		return result;
	}

	public int GetDashHealing()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_dashHealingMod.GetModifiedValue(m_dashHealing);
		}
		else
		{
			result = m_dashHealing;
		}
		return result;
	}

	public StandardEffectInfo GetDashAllyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedDashAllyHitEffect != null)
		{
			result = m_cachedDashAllyHitEffect;
		}
		else
		{
			result = m_dashAllyHitEffect;
		}
		return result;
	}

	public bool DashApplyDeathmark()
	{
		return (!m_abilityMod) ? m_dashApplyDeathmark : m_abilityMod.m_dashApplyDeathmarkMod.GetModifiedValue(m_dashApplyDeathmark);
	}

	public bool CanTriggerDeathmark()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_canTriggerDeathmarkMod.GetModifiedValue(m_canTriggerDeathmark);
		}
		else
		{
			result = m_canTriggerDeathmark;
		}
		return result;
	}

	public bool CanQueueMoveAfterEvade()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_canQueueMoveAfterEvadeMod.GetModifiedValue(m_canQueueMoveAfterEvade);
		}
		else
		{
			result = m_canQueueMoveAfterEvade;
		}
		return result;
	}

	public int CalcDamageOnActor(ActorData target, ActorData caster)
	{
		int num = 0;
		if (target.GetTeam() != caster.GetTeam())
		{
			num = GetDashDamage();
			if (IsActorMarked(target))
			{
				if (GetExtraDamageOnMarked() > 0)
				{
					num += GetExtraDamageOnMarked();
				}
			}
			else if (GetExtraDamageIfNotMarked() > 0)
			{
				num += GetExtraDamageIfNotMarked();
			}
		}
		return num;
	}

	public bool IsActorMarked(ActorData actor)
	{
		int result;
		if (m_syncComp != null)
		{
			result = (m_syncComp.ActorHasDeathmark(actor) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDashDamage());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetDashHealing());
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		results.m_damage = 0;
		results.m_healing = 0;
		BoardSquare boardSquareSafe = Board.Get().GetSquare(base.Targeter.LastUpdatingGridPos);
		int num;
		if ((bool)boardSquareSafe)
		{
			num = ((boardSquareSafe == targetActor.GetCurrentBoardSquare()) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			int damage = 0;
			if (flag)
			{
				damage = CalcDamageOnActor(targetActor, base.ActorData);
			}
			results.m_damage = damage;
		}
		else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Ally) > 0)
		{
			int healing;
			if (flag)
			{
				healing = GetDashHealing();
			}
			else
			{
				healing = 0;
			}
			results.m_healing = healing;
		}
		return true;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (symbolType == AbilityTooltipSymbol.Damage)
		{
			if (m_syncComp != null && m_syncComp.m_deathmarkOnTriggerDamage > 0)
			{
				if (IsActorMarked(targetActor))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return "\n+ " + AbilityUtils.CalculateDamageForTargeter(base.ActorData, targetActor, this, m_syncComp.m_deathmarkOnTriggerDamage, false);
						}
					}
				}
			}
		}
		return null;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool result = true;
		TargetingParadigm targetingParadigm = GetTargetingParadigm(0);
		if (targetingParadigm != TargetingParadigm.BoardSquare)
		{
			if (targetingParadigm != TargetingParadigm.Position)
			{
				goto IL_0200;
			}
		}
		result = false;
		List<ActorData> actorsVisibleToActor;
		if (NetworkServer.active)
		{
			actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(caster);
		}
		else
		{
			actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData);
		}
		List<ActorData> list = actorsVisibleToActor;
		list.Remove(caster);
		if (list != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					float num = GetDashToUnmarkedRange() * Board.Get().squareSize;
					using (List<ActorData>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData current = enumerator.Current;
							Vector3 vector = current.GetTravelBoardSquareWorldPosition() - caster.GetTravelBoardSquareWorldPosition();
							vector.y = 0f;
							float magnitude = vector.magnitude;
							bool flag = IsActorMarked(current);
							float squareSize = Board.Get().squareSize;
							float num2;
							if (flag)
							{
								num2 = GetDashRangeMarked();
							}
							else
							{
								num2 = GetDashRangeDefault();
							}
							float num3 = squareSize * num2;
							if (!(magnitude <= num3))
							{
								if (!(num3 <= 0f))
								{
									continue;
								}
							}
							int num4;
							if (DashRequireDeathmark())
							{
								num4 = (flag ? 1 : 0);
							}
							else
							{
								num4 = 1;
							}
							bool flag2 = (byte)num4 != 0;
							if (!flag2 && num > 0f)
							{
								if (magnitude <= num)
								{
									flag2 = true;
								}
							}
							ValidateCheckPath checkPath = (!GetIsTeleport()) ? ValidateCheckPath.CanBuildPath : ValidateCheckPath.Ignore;
							if (flag2)
							{
								if (CanTargetActorInDecision(caster, current, CanDashToEnemy(), CanDashToAlly(), false, checkPath, DashIgnoreLos(), false))
								{
									while (true)
									{
										switch (5)
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
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return result;
							}
						}
					}
				}
				}
			}
		}
		goto IL_0200;
		IL_0200:
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		bool flag;
		bool flag2;
		if (!(boardSquareSafe == null))
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				flag = false;
				flag2 = false;
				if (targetIndex == 0)
				{
					ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(boardSquareSafe, CanDashToEnemy(), CanDashToAlly(), caster);
					if (targetableActorOnSquare != null)
					{
						if (targetableActorOnSquare != caster)
						{
							if (AreaEffectUtils.IsActorTargetable(targetableActorOnSquare))
							{
								Vector3 vector = targetableActorOnSquare.GetTravelBoardSquareWorldPosition() - caster.GetTravelBoardSquareWorldPosition();
								vector.y = 0f;
								float magnitude = vector.magnitude;
								bool flag3 = IsActorMarked(targetableActorOnSquare);
								float squareSize = Board.Get().squareSize;
								float num;
								if (flag3)
								{
									num = GetDashRangeMarked();
								}
								else
								{
									num = GetDashRangeDefault();
								}
								float num2 = squareSize * num;
								if (!(magnitude <= num2))
								{
									if (!(num2 <= 0f))
									{
										goto IL_031e;
									}
								}
								float num3 = GetDashToUnmarkedRange() * Board.Get().squareSize;
								int num4;
								if (DashRequireDeathmark())
								{
									if (m_syncComp != null)
									{
										num4 = (m_syncComp.ActorHasDeathmark(targetableActorOnSquare) ? 1 : 0);
									}
									else
									{
										num4 = 0;
									}
								}
								else
								{
									num4 = 1;
								}
								bool flag4 = (byte)num4 != 0;
								if (!flag4 && num3 > 0f)
								{
									if (magnitude <= num3)
									{
										flag4 = true;
									}
								}
								ValidateCheckPath checkPath = (!GetIsTeleport()) ? ValidateCheckPath.CanBuildPath : ValidateCheckPath.Ignore;
								if (flag4)
								{
									if (CanTargetActorInDecision(caster, targetableActorOnSquare, CanDashToEnemy(), CanDashToAlly(), false, checkPath, DashIgnoreLos(), false))
									{
										flag = true;
										flag2 = true;
									}
								}
							}
						}
					}
				}
				else
				{
					flag = true;
					BoardSquare boardSquareSafe2 = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
					BoardSquare boardSquareSafe3 = Board.Get().GetSquare(target.GridPos);
					if (boardSquareSafe3 != null)
					{
						if (boardSquareSafe3.IsBaselineHeight())
						{
							if (boardSquareSafe3 != boardSquareSafe2)
							{
								if (boardSquareSafe3 != caster.GetCurrentBoardSquare())
								{
									bool flag5 = false;
									if (targetIndex == 1)
									{
										flag5 = AreaEffectUtils.IsSquareInShape(boardSquareSafe3, GetDashDestShape(), target.FreePos, boardSquareSafe2, false, caster);
									}
									if (flag5)
									{
										int num5;
										if (!GetIsTeleport())
										{
											num5 = (KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe3, boardSquareSafe2, false, out int _) ? 1 : 0);
										}
										else
										{
											num5 = 1;
										}
										flag2 = ((byte)num5 != 0);
									}
								}
							}
						}
					}
				}
				goto IL_031e;
			}
		}
		return false;
		IL_031e:
		int result;
		if (flag2)
		{
			result = (flag ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_NinjaShurikenOrDash))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_NinjaShurikenOrDash);
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
