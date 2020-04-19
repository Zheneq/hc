using System;
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
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.Start()).MethodHandle;
			}
			this.m_abilityName = "NinjaShurikenOrDash";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		if (this.m_syncComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.Setup()).MethodHandle;
			}
			this.m_syncComp = base.GetComponent<Ninja_SyncComponent>();
		}
		base.ClearTargeters();
		AbilityUtil_Targeter_Shape item = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeters.Add(item);
		AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, 0f, 0f, 0f, -1, false, false);
		abilityUtil_Targeter_ChargeAoE.SetUseMultiTargetUpdate(true);
		abilityUtil_Targeter_ChargeAoE.ShowTeleportLines = this.GetIsTeleport();
		base.Targeters.Add(abilityUtil_Targeter_ChargeAoE);
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Design --</color>\nPlease edit [Deathmark] info on Ninja sync component.";
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Min(2, this.GetTargetData().Length);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return (!this.GetIsTeleport()) ? ActorData.MovementType.Charge : ActorData.MovementType.Teleport;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return this.CanQueueMoveAfterEvade();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetDashRangeDefault() - 0.5f;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "DashDamage", string.Empty, this.m_dashDamage, false);
		base.AddTokenInt(tokens, "ExtraDamageOnMarked", string.Empty, this.m_extraDamageOnMarked, false);
		base.AddTokenInt(tokens, "ExtraDamageIfNotMarked", string.Empty, this.m_extraDamageIfNotMarked, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_dashEnemyHitEffect, "DashEnemyHitEffect", this.m_dashEnemyHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_extraEnemyEffectOnMarked, "ExtraEnemyEffectOnMarked", this.m_extraEnemyEffectOnMarked, true);
		base.AddTokenInt(tokens, "DashHealing", string.Empty, this.m_dashHealing, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_dashAllyHitEffect, "DashAllyHitEffect", this.m_dashAllyHitEffect, true);
	}

	private void SetCachedFields()
	{
		this.m_cachedDashEnemyHitEffect = ((!this.m_abilityMod) ? this.m_dashEnemyHitEffect : this.m_abilityMod.m_dashEnemyHitEffectMod.GetModifiedValue(this.m_dashEnemyHitEffect));
		this.m_cachedExtraEnemyEffectOnMarked = ((!this.m_abilityMod) ? this.m_extraEnemyEffectOnMarked : this.m_abilityMod.m_extraEnemyEffectOnMarkedMod.GetModifiedValue(this.m_extraEnemyEffectOnMarked));
		this.m_cachedDashAllyHitEffect = ((!this.m_abilityMod) ? this.m_dashAllyHitEffect : this.m_abilityMod.m_dashAllyHitEffectMod.GetModifiedValue(this.m_dashAllyHitEffect));
	}

	public bool GetIsTeleport()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.GetIsTeleport()).MethodHandle;
			}
			result = this.m_abilityMod.m_isTeleportMod.GetModifiedValue(this.m_isTeleport);
		}
		else
		{
			result = this.m_isTeleport;
		}
		return result;
	}

	public float GetDashRangeDefault()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.GetDashRangeDefault()).MethodHandle;
			}
			result = this.m_abilityMod.m_dashRangeDefaultMod.GetModifiedValue(this.m_dashRangeDefault);
		}
		else
		{
			result = this.m_dashRangeDefault;
		}
		return result;
	}

	public float GetDashRangeMarked()
	{
		return (!this.m_abilityMod) ? this.m_dashRangeMarked : this.m_abilityMod.m_dashRangeMarkedMod.GetModifiedValue(this.m_dashRangeMarked);
	}

	public bool DashRequireDeathmark()
	{
		return (!this.m_abilityMod) ? this.m_dashRequireDeathmark : this.m_abilityMod.m_dashRequireDeathmarkMod.GetModifiedValue(this.m_dashRequireDeathmark);
	}

	public float GetDashToUnmarkedRange()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.GetDashToUnmarkedRange()).MethodHandle;
			}
			result = this.m_abilityMod.m_dashToUnmarkedRangeMod.GetModifiedValue(this.m_dashToUnmarkedRange);
		}
		else
		{
			result = this.m_dashToUnmarkedRange;
		}
		return result;
	}

	public bool CanDashToAlly()
	{
		return (!this.m_abilityMod) ? this.m_canDashToAlly : this.m_abilityMod.m_canDashToAllyMod.GetModifiedValue(this.m_canDashToAlly);
	}

	public bool CanDashToEnemy()
	{
		return (!this.m_abilityMod) ? this.m_canDashToEnemy : this.m_abilityMod.m_canDashToEnemyMod.GetModifiedValue(this.m_canDashToEnemy);
	}

	public bool DashIgnoreLos()
	{
		bool result;
		if (this.m_abilityMod)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.DashIgnoreLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_dashIgnoreLosMod.GetModifiedValue(this.m_dashIgnoreLos);
		}
		else
		{
			result = this.m_dashIgnoreLos;
		}
		return result;
	}

	public AbilityAreaShape GetDashDestShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.GetDashDestShape()).MethodHandle;
			}
			result = this.m_abilityMod.m_dashDestShapeMod.GetModifiedValue(this.m_dashDestShape);
		}
		else
		{
			result = this.m_dashDestShape;
		}
		return result;
	}

	public int GetDashDamage()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.GetDashDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_dashDamageMod.GetModifiedValue(this.m_dashDamage);
		}
		else
		{
			result = this.m_dashDamage;
		}
		return result;
	}

	public int GetExtraDamageOnMarked()
	{
		int result;
		if (this.m_abilityMod)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.GetExtraDamageOnMarked()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageOnMarkedMod.GetModifiedValue(this.m_extraDamageOnMarked);
		}
		else
		{
			result = this.m_extraDamageOnMarked;
		}
		return result;
	}

	public int GetExtraDamageIfNotMarked()
	{
		return (!this.m_abilityMod) ? this.m_extraDamageIfNotMarked : this.m_abilityMod.m_extraDamageIfNotMarkedMod.GetModifiedValue(this.m_extraDamageIfNotMarked);
	}

	public StandardEffectInfo GetDashEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedDashEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.GetDashEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedDashEnemyHitEffect;
		}
		else
		{
			result = this.m_dashEnemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetExtraEnemyEffectOnMarked()
	{
		StandardEffectInfo result;
		if (this.m_cachedExtraEnemyEffectOnMarked != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.GetExtraEnemyEffectOnMarked()).MethodHandle;
			}
			result = this.m_cachedExtraEnemyEffectOnMarked;
		}
		else
		{
			result = this.m_extraEnemyEffectOnMarked;
		}
		return result;
	}

	public int GetDashHealing()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.GetDashHealing()).MethodHandle;
			}
			result = this.m_abilityMod.m_dashHealingMod.GetModifiedValue(this.m_dashHealing);
		}
		else
		{
			result = this.m_dashHealing;
		}
		return result;
	}

	public StandardEffectInfo GetDashAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedDashAllyHitEffect != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.GetDashAllyHitEffect()).MethodHandle;
			}
			result = this.m_cachedDashAllyHitEffect;
		}
		else
		{
			result = this.m_dashAllyHitEffect;
		}
		return result;
	}

	public bool DashApplyDeathmark()
	{
		return (!this.m_abilityMod) ? this.m_dashApplyDeathmark : this.m_abilityMod.m_dashApplyDeathmarkMod.GetModifiedValue(this.m_dashApplyDeathmark);
	}

	public bool CanTriggerDeathmark()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.CanTriggerDeathmark()).MethodHandle;
			}
			result = this.m_abilityMod.m_canTriggerDeathmarkMod.GetModifiedValue(this.m_canTriggerDeathmark);
		}
		else
		{
			result = this.m_canTriggerDeathmark;
		}
		return result;
	}

	public bool CanQueueMoveAfterEvade()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.CanQueueMoveAfterEvade()).MethodHandle;
			}
			result = this.m_abilityMod.m_canQueueMoveAfterEvadeMod.GetModifiedValue(this.m_canQueueMoveAfterEvade);
		}
		else
		{
			result = this.m_canQueueMoveAfterEvade;
		}
		return result;
	}

	public int CalcDamageOnActor(ActorData target, ActorData caster)
	{
		int num = 0;
		if (target.\u000E() != caster.\u000E())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.CalcDamageOnActor(ActorData, ActorData)).MethodHandle;
			}
			num = this.GetDashDamage();
			if (this.IsActorMarked(target))
			{
				if (this.GetExtraDamageOnMarked() > 0)
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
					num += this.GetExtraDamageOnMarked();
				}
			}
			else if (this.GetExtraDamageIfNotMarked() > 0)
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
				num += this.GetExtraDamageIfNotMarked();
			}
		}
		return num;
	}

	public bool IsActorMarked(ActorData actor)
	{
		bool result;
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.IsActorMarked(ActorData)).MethodHandle;
			}
			result = this.m_syncComp.ActorHasDeathmark(actor);
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDashDamage());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetDashHealing());
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		results.m_damage = 0;
		results.m_healing = 0;
		BoardSquare boardSquare = Board.\u000E().\u000E(base.Targeter.LastUpdatingGridPos);
		bool flag;
		if (boardSquare)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
			}
			flag = (boardSquare == targetActor.\u0012());
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
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
			int damage = 0;
			if (flag2)
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
				damage = this.CalcDamageOnActor(targetActor, base.ActorData);
			}
			results.m_damage = damage;
		}
		else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Ally) > 0)
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
			int healing;
			if (flag2)
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
				healing = this.GetDashHealing();
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.GetAccessoryTargeterNumberString(ActorData, AbilityTooltipSymbol, int)).MethodHandle;
			}
			if (this.m_syncComp != null && this.m_syncComp.m_deathmarkOnTriggerDamage > 0)
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
				if (this.IsActorMarked(targetActor))
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
					return "\n+ " + AbilityUtils.CalculateDamageForTargeter(base.ActorData, targetActor, this, this.m_syncComp.m_deathmarkOnTriggerDamage, false).ToString();
				}
			}
		}
		return null;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool result = true;
		Ability.TargetingParadigm targetingParadigm = base.GetTargetingParadigm(0);
		if (targetingParadigm != Ability.TargetingParadigm.BoardSquare)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			if (targetingParadigm != Ability.TargetingParadigm.Position)
			{
				return result;
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
		result = false;
		List<ActorData> actorsVisibleToActor;
		if (NetworkServer.active)
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
			actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(caster, true);
		}
		else
		{
			actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData, true);
		}
		List<ActorData> list = actorsVisibleToActor;
		list.Remove(caster);
		if (list != null)
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
			float num = this.GetDashToUnmarkedRange() * Board.\u000E().squareSize;
			using (List<ActorData>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					Vector3 vector = actorData.\u0016() - caster.\u0016();
					vector.y = 0f;
					float magnitude = vector.magnitude;
					bool flag = this.IsActorMarked(actorData);
					float squareSize = Board.\u000E().squareSize;
					float num2;
					if (flag)
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
						num2 = this.GetDashRangeMarked();
					}
					else
					{
						num2 = this.GetDashRangeDefault();
					}
					float num3 = squareSize * num2;
					if (magnitude > num3)
					{
						if (num3 > 0f)
						{
							continue;
						}
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					bool flag2;
					if (this.DashRequireDeathmark())
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
						flag2 = flag;
					}
					else
					{
						flag2 = true;
					}
					bool flag3 = flag2;
					if (!flag3 && num > 0f)
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
						if (magnitude <= num)
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
							flag3 = true;
						}
					}
					Ability.ValidateCheckPath checkPath = (!this.GetIsTeleport()) ? Ability.ValidateCheckPath.CanBuildPath : Ability.ValidateCheckPath.Ignore;
					if (flag3)
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
						if (base.CanTargetActorInDecision(caster, actorData, this.CanDashToEnemy(), this.CanDashToAlly(), false, checkPath, this.DashIgnoreLos(), false, false))
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
							return true;
						}
					}
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
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (!(boardSquare == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (boardSquare.\u0016())
			{
				bool flag = false;
				bool flag2 = false;
				if (targetIndex == 0)
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
					ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(boardSquare, this.CanDashToEnemy(), this.CanDashToAlly(), caster);
					if (targetableActorOnSquare != null)
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
						if (targetableActorOnSquare != caster)
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
							if (AreaEffectUtils.IsActorTargetable(targetableActorOnSquare, null))
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
								Vector3 vector = targetableActorOnSquare.\u0016() - caster.\u0016();
								vector.y = 0f;
								float magnitude = vector.magnitude;
								bool flag3 = this.IsActorMarked(targetableActorOnSquare);
								float squareSize = Board.\u000E().squareSize;
								float num;
								if (flag3)
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
									num = this.GetDashRangeMarked();
								}
								else
								{
									num = this.GetDashRangeDefault();
								}
								float num2 = squareSize * num;
								if (magnitude > num2)
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
									if (num2 > 0f)
									{
										goto IL_221;
									}
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								float num3 = this.GetDashToUnmarkedRange() * Board.\u000E().squareSize;
								bool flag4;
								if (this.DashRequireDeathmark())
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
										flag4 = this.m_syncComp.ActorHasDeathmark(targetableActorOnSquare);
									}
									else
									{
										flag4 = false;
									}
								}
								else
								{
									flag4 = true;
								}
								bool flag5 = flag4;
								if (!flag5 && num3 > 0f)
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
									if (magnitude <= num3)
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
										flag5 = true;
									}
								}
								Ability.ValidateCheckPath checkPath = (!this.GetIsTeleport()) ? Ability.ValidateCheckPath.CanBuildPath : Ability.ValidateCheckPath.Ignore;
								if (flag5)
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
									if (base.CanTargetActorInDecision(caster, targetableActorOnSquare, this.CanDashToEnemy(), this.CanDashToAlly(), false, checkPath, this.DashIgnoreLos(), false, false))
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
										flag = true;
										flag2 = true;
									}
								}
							}
						}
					}
					IL_221:;
				}
				else
				{
					flag = true;
					BoardSquare boardSquare2 = Board.\u000E().\u000E(currentTargets[targetIndex - 1].GridPos);
					BoardSquare boardSquare3 = Board.\u000E().\u000E(target.GridPos);
					if (boardSquare3 != null)
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
						if (boardSquare3.\u0016())
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
							if (boardSquare3 != boardSquare2)
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
								if (boardSquare3 != caster.\u0012())
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
									bool flag6 = false;
									if (targetIndex == 1)
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
										flag6 = AreaEffectUtils.IsSquareInShape(boardSquare3, this.GetDashDestShape(), target.FreePos, boardSquare2, false, caster);
									}
									if (flag6)
									{
										bool flag7;
										if (!this.GetIsTeleport())
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
											int num4;
											flag7 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquare3, boardSquare2, false, out num4);
										}
										else
										{
											flag7 = true;
										}
										flag2 = flag7;
									}
								}
							}
						}
					}
				}
				bool result;
				if (flag2)
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
					result = flag;
				}
				else
				{
					result = false;
				}
				return result;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NinjaShurikenOrDash))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaShurikenOrDash.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_NinjaShurikenOrDash);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
