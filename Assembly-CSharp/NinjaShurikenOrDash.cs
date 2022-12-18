using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NinjaShurikenOrDash : Ability
{
	[Separator("Dash - Type, Targeting Info")]
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
	[Separator("Dash - On Hit Stuff")]
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
	[Separator("Dash - Allow move after evade?")]
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
		Targeters.Add(new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true));
		AbilityUtil_Targeter_ChargeAoE targeter = new AbilityUtil_Targeter_ChargeAoE(
			this, 0f, 0f, 0f, -1, false, false);
		targeter.SetUseMultiTargetUpdate(true);
		targeter.ShowTeleportLines = GetIsTeleport();
		Targeters.Add(targeter);
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
		m_cachedDashEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_dashEnemyHitEffectMod.GetModifiedValue(m_dashEnemyHitEffect)
			: m_dashEnemyHitEffect;
		m_cachedExtraEnemyEffectOnMarked = m_abilityMod != null
			? m_abilityMod.m_extraEnemyEffectOnMarkedMod.GetModifiedValue(m_extraEnemyEffectOnMarked)
			: m_extraEnemyEffectOnMarked;
		m_cachedDashAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_dashAllyHitEffectMod.GetModifiedValue(m_dashAllyHitEffect)
			: m_dashAllyHitEffect;
	}

	public bool GetIsTeleport()
	{
		return m_abilityMod != null
			? m_abilityMod.m_isTeleportMod.GetModifiedValue(m_isTeleport)
			: m_isTeleport;
	}

	public float GetDashRangeDefault()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashRangeDefaultMod.GetModifiedValue(m_dashRangeDefault)
			: m_dashRangeDefault;
	}

	public float GetDashRangeMarked()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashRangeMarkedMod.GetModifiedValue(m_dashRangeMarked)
			: m_dashRangeMarked;
	}

	public bool DashRequireDeathmark()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashRequireDeathmarkMod.GetModifiedValue(m_dashRequireDeathmark)
			: m_dashRequireDeathmark;
	}

	public float GetDashToUnmarkedRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashToUnmarkedRangeMod.GetModifiedValue(m_dashToUnmarkedRange)
			: m_dashToUnmarkedRange;
	}

	public bool CanDashToAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canDashToAllyMod.GetModifiedValue(m_canDashToAlly)
			: m_canDashToAlly;
	}

	public bool CanDashToEnemy()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canDashToEnemyMod.GetModifiedValue(m_canDashToEnemy)
			: m_canDashToEnemy;
	}

	public bool DashIgnoreLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashIgnoreLosMod.GetModifiedValue(m_dashIgnoreLos)
			: m_dashIgnoreLos;
	}

	public AbilityAreaShape GetDashDestShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashDestShapeMod.GetModifiedValue(m_dashDestShape)
			: m_dashDestShape;
	}

	public int GetDashDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashDamageMod.GetModifiedValue(m_dashDamage)
			: m_dashDamage;
	}

	public int GetExtraDamageOnMarked()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageOnMarkedMod.GetModifiedValue(m_extraDamageOnMarked)
			: m_extraDamageOnMarked;
	}

	public int GetExtraDamageIfNotMarked()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageIfNotMarkedMod.GetModifiedValue(m_extraDamageIfNotMarked)
			: m_extraDamageIfNotMarked;
	}

	public StandardEffectInfo GetDashEnemyHitEffect()
	{
		return m_cachedDashEnemyHitEffect ?? m_dashEnemyHitEffect;
	}

	public StandardEffectInfo GetExtraEnemyEffectOnMarked()
	{
		return m_cachedExtraEnemyEffectOnMarked ?? m_extraEnemyEffectOnMarked;
	}

	public int GetDashHealing()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashHealingMod.GetModifiedValue(m_dashHealing)
			: m_dashHealing;
	}

	public StandardEffectInfo GetDashAllyHitEffect()
	{
		return m_cachedDashAllyHitEffect ?? m_dashAllyHitEffect;
	}

	public bool DashApplyDeathmark()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashApplyDeathmarkMod.GetModifiedValue(m_dashApplyDeathmark)
			: m_dashApplyDeathmark;
	}

	public bool CanTriggerDeathmark()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canTriggerDeathmarkMod.GetModifiedValue(m_canTriggerDeathmark)
			: m_canTriggerDeathmark;
	}

	public bool CanQueueMoveAfterEvade()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canQueueMoveAfterEvadeMod.GetModifiedValue(m_canQueueMoveAfterEvade)
			: m_canQueueMoveAfterEvade;
	}

	public int CalcDamageOnActor(ActorData target, ActorData caster)
	{
		int damage = 0;
		if (target.GetTeam() != caster.GetTeam())
		{
			damage = GetDashDamage();
			if (IsActorMarked(target))
			{
				if (GetExtraDamageOnMarked() > 0)
				{
					damage += GetExtraDamageOnMarked();
				}
			}
			else if (GetExtraDamageIfNotMarked() > 0)
			{
				damage += GetExtraDamageIfNotMarked();
			}
		}
		return damage;
	}

	public bool IsActorMarked(ActorData actor)
	{
		return m_syncComp != null && m_syncComp.ActorHasDeathmark(actor);
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
		BoardSquare square = Board.Get().GetSquare(Targeter.LastUpdatingGridPos);
		bool isTarget = square != null && square == targetActor.GetCurrentBoardSquare();
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			results.m_damage = isTarget ? CalcDamageOnActor(targetActor, ActorData) : 0;
		}
		else if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Ally) > 0)
		{
			results.m_healing = isTarget ? GetDashHealing() : 0;
		}
		return true;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (symbolType == AbilityTooltipSymbol.Damage
		    && m_syncComp != null
		    && m_syncComp.m_deathmarkOnTriggerDamage > 0
		    && IsActorMarked(targetActor))
		{
			return "\n+ " + AbilityUtils.CalculateDamageForTargeter(
				ActorData, targetActor, this, m_syncComp.m_deathmarkOnTriggerDamage, false);
		}
		return null;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		TargetingParadigm targetingParadigm = GetTargetingParadigm(0);
		if (targetingParadigm != TargetingParadigm.BoardSquare && targetingParadigm != TargetingParadigm.Position)
		{
			return true;
		}
		List<ActorData> visibleActors = GameFlowData.Get().GetActorsVisibleToActor(
			NetworkServer.active
				? caster
				: GameFlowData.Get().activeOwnedActorData);
		visibleActors.Remove(caster);
		if (visibleActors != null)
		{
			float num = GetDashToUnmarkedRange() * Board.Get().squareSize;
			foreach (ActorData current in visibleActors)
			{
				Vector3 vector = current.GetFreePos() - caster.GetFreePos();
				vector.y = 0f;
				float dist = vector.magnitude;
				bool isActorMarked = IsActorMarked(current);
				float squareSize = Board.Get().squareSize;
				float dashRange = isActorMarked ? GetDashRangeMarked() : GetDashRangeDefault();
				float dashRangeInWorld = squareSize * dashRange;
				if (dist <= dashRangeInWorld || dashRangeInWorld <= 0f)
				{
					bool isValid = !DashRequireDeathmark() || isActorMarked;
					if (!isValid && num > 0f && dist <= num)
					{
						isValid = true;
					}
					ValidateCheckPath checkPath = GetIsTeleport()
						? ValidateCheckPath.Ignore
						: ValidateCheckPath.CanBuildPath;
					if (isValid)
					{
						bool canTarget = CanTargetActorInDecision(
							caster,
							current,
							CanDashToEnemy(),
							CanDashToAlly(),
							false,
							checkPath,
							DashIgnoreLos(),
							false);
						if (canTarget)
						{
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
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null || !targetSquare.IsValidForGameplay())
		{
			return false;
		}
		bool flag = false;
		bool flag2 = false;
		if (targetIndex == 0)
		{
			ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(targetSquare, CanDashToEnemy(), CanDashToAlly(), caster);
			if (targetableActorOnSquare != null
			    && targetableActorOnSquare != caster
			    && AreaEffectUtils.IsActorTargetable(targetableActorOnSquare))
			{
				Vector3 vector = targetableActorOnSquare.GetFreePos() - caster.GetFreePos();
				vector.y = 0f;
				float dist = vector.magnitude;
				bool isActorMarked = IsActorMarked(targetableActorOnSquare);
				float squareSize = Board.Get().squareSize;
				float dashRangeInWorld = squareSize * (isActorMarked ? GetDashRangeMarked() : GetDashRangeDefault());
				if (!(dist <= dashRangeInWorld) && !(dashRangeInWorld <= 0f))
				{
					return flag2 && flag;
				}
				float unmarkedRange = GetDashToUnmarkedRange() * Board.Get().squareSize;
				bool isValid = !DashRequireDeathmark()
				               || (m_syncComp != null && m_syncComp.ActorHasDeathmark(targetableActorOnSquare));
				if (!isValid && unmarkedRange > 0f && dist <= unmarkedRange)
				{
					isValid = true;
				}
				ValidateCheckPath checkPath = GetIsTeleport()
					? ValidateCheckPath.Ignore
					: ValidateCheckPath.CanBuildPath;
				if (isValid)
				{
					bool canTarget = CanTargetActorInDecision(
						caster,
						targetableActorOnSquare,
						CanDashToEnemy(),
						CanDashToAlly(),
						false,
						checkPath,
						DashIgnoreLos(),
						false);
					if (canTarget)
					{
						flag = true;
						flag2 = true;
					}
				}
			}
		}
		else
		{
			flag = true;
			BoardSquare prevTargetSquare = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
			BoardSquare curTargetSquare = Board.Get().GetSquare(target.GridPos);
			if (curTargetSquare != null
			    && curTargetSquare.IsValidForGameplay()
			    && curTargetSquare != prevTargetSquare
			    && curTargetSquare != caster.GetCurrentBoardSquare())
			{
				bool flag5 = false;
				if (targetIndex == 1)
				{
					flag5 = AreaEffectUtils.IsSquareInShape(curTargetSquare, GetDashDestShape(), target.FreePos, prevTargetSquare, false, caster);
				}
				if (flag5)
				{
					flag2 = GetIsTeleport()
					        || KnockbackUtils.CanBuildStraightLineChargePath(caster, curTargetSquare, prevTargetSquare, false, out _);
				}
			}
		}
		return flag2 && flag;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NinjaShurikenOrDash))
		{
			m_abilityMod = abilityMod as AbilityMod_NinjaShurikenOrDash;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
