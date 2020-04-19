using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TricksterCatchMeIfYouCan : Ability
{
	[Header("-- Hit actors in path")]
	public bool m_hitActorsInPath = true;

	public float m_pathRadius = 0.5f;

	public float m_pathStartRadius;

	public float m_pathEndRadius;

	public bool m_penetrateLos;

	public bool m_targeterAllowOccupiedSquares;

	public bool m_chargeThroughInvalidSquares;

	[Header("-- Enemy Hit")]
	public int m_damageAmount = 0xA;

	public int m_subsequentDamageAmount = 0xA;

	public StandardEffectInfo m_enemyHitEffect;

	[Space(10f)]
	public bool m_useEnemyMultiHitEffect;

	public StandardEffectInfo m_enemyMultipleHitEffect;

	[Header("-- Ally Hit")]
	public int m_allyHealingAmount;

	public int m_subsequentHealingAmount;

	public int m_allyEnergyGain;

	public StandardEffectInfo m_allyHitEffect;

	[Space(10f)]
	public bool m_useAllyMultiHitEffect;

	public StandardEffectInfo m_allyMultipleHitEffect;

	[Space(10f)]
	public int m_selfHealingAmount;

	public StandardEffectInfo m_selfHitEffect;

	[Header("-- Sequences, use On Cast Sequence Prefab for hits")]
	public GameObject m_castSequencePrefab;

	[Header("  (assuming Simple Attached VFX Sequence, applied to caster and clones)")]
	public GameObject m_vfxOnCasterAndCloneSequencePrefab;

	public float m_hitImpactDelay = 0.35f;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;

	private AbilityMod_TricksterCatchMeIfYouCan m_abilityMod;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedEnemyMultipleHitEffect;

	private StandardEffectInfo m_cachedAllyHitEffect;

	private StandardEffectInfo m_cachedAllyMultipleHitEffect;

	private StandardEffectInfo m_cachedSelfHitEffect;

	private static readonly int animDistToGoal = Animator.StringToHash("DistToGoal");

	private static readonly int animStartDamageReaction = Animator.StringToHash("StartDamageReaction");

	private static readonly int animAttack = Animator.StringToHash("Attack");

	private static readonly int animCinematicCam = Animator.StringToHash("CinematicCam");

	private static readonly int animStartAttack = Animator.StringToHash("StartAttack");

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Catch Me If You Can";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_afterImageSyncComp = base.GetComponent<TricksterAfterImageNetworkBehaviour>();
		if (this.m_afterImageSyncComp == null)
		{
			Debug.LogError("TricksterAfterImageNetworkBehavior not found");
		}
		this.SetCachedFields();
		int expectedNumberOfTargeters = this.GetExpectedNumberOfTargeters();
		if (expectedNumberOfTargeters < 2)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.Setup()).MethodHandle;
			}
			if (this.HitActorsInPath())
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
				AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, this.GetPathStartRadius(), this.GetPathEndRadius(), this.GetPathRadius(), -1, true, this.PenetrateLos());
				abilityUtil_Targeter_ChargeAoE.SetAffectedGroups(this.IncludeEnemies(), this.IncludeAllies(), this.IncludeSelf());
				abilityUtil_Targeter_ChargeAoE.AllowChargeThroughInvalidSquares = this.m_chargeThroughInvalidSquares;
				base.Targeter = abilityUtil_Targeter_ChargeAoE;
			}
			else
			{
				base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
			}
		}
		else
		{
			base.ClearTargeters();
			for (int i = 0; i < expectedNumberOfTargeters; i++)
			{
				if (this.HitActorsInPath())
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
					AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE2 = new AbilityUtil_Targeter_ChargeAoE(this, this.GetPathStartRadius(), this.GetPathEndRadius(), this.GetPathRadius(), -1, true, this.PenetrateLos());
					abilityUtil_Targeter_ChargeAoE2.SetAffectedGroups(this.IncludeEnemies(), this.IncludeAllies(), this.IncludeSelf());
					abilityUtil_Targeter_ChargeAoE2.AllowChargeThroughInvalidSquares = this.m_chargeThroughInvalidSquares;
					if (i > 0)
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
						abilityUtil_Targeter_ChargeAoE2.SkipEvadeMovementLines = true;
					}
					base.Targeters.Add(abilityUtil_Targeter_ChargeAoE2);
				}
				else
				{
					base.Targeters.Add(new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible));
				}
				base.Targeters[i].SetUseMultiTargetUpdate(false);
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
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Clamp(base.GetNumTargets(), 1, this.m_afterImageSyncComp.GetMaxAfterImageCount() + 1);
	}

	private void SetCachedFields()
	{
		this.m_cachedEnemyHitEffect = ((!this.m_abilityMod) ? this.m_enemyHitEffect : this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect));
		this.m_cachedEnemyMultipleHitEffect = ((!this.m_abilityMod) ? this.m_enemyMultipleHitEffect : this.m_abilityMod.m_enemyMultipleHitEffectMod.GetModifiedValue(this.m_enemyMultipleHitEffect));
		StandardEffectInfo cachedAllyHitEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.SetCachedFields()).MethodHandle;
			}
			cachedAllyHitEffect = this.m_abilityMod.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = this.m_allyHitEffect;
		}
		this.m_cachedAllyHitEffect = cachedAllyHitEffect;
		this.m_cachedAllyMultipleHitEffect = ((!this.m_abilityMod) ? this.m_allyMultipleHitEffect : this.m_abilityMod.m_allyMultipleHitEffectMod.GetModifiedValue(this.m_allyMultipleHitEffect));
		StandardEffectInfo cachedSelfHitEffect;
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
			cachedSelfHitEffect = this.m_abilityMod.m_selfHitEffectMod.GetModifiedValue(this.m_selfHitEffect);
		}
		else
		{
			cachedSelfHitEffect = this.m_selfHitEffect;
		}
		this.m_cachedSelfHitEffect = cachedSelfHitEffect;
	}

	public bool HitActorsInPath()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.HitActorsInPath()).MethodHandle;
			}
			result = this.m_abilityMod.m_hitActorsInPathMod.GetModifiedValue(this.m_hitActorsInPath);
		}
		else
		{
			result = this.m_hitActorsInPath;
		}
		return result;
	}

	public float GetPathRadius()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetPathRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_pathRadiusMod.GetModifiedValue(this.m_pathRadius);
		}
		else
		{
			result = this.m_pathRadius;
		}
		return result;
	}

	public float GetPathStartRadius()
	{
		return (!this.m_abilityMod) ? this.m_pathStartRadius : this.m_abilityMod.m_pathStartRadiusMod.GetModifiedValue(this.m_pathStartRadius);
	}

	public float GetPathEndRadius()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetPathEndRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_pathEndRadiusMod.GetModifiedValue(this.m_pathEndRadius);
		}
		else
		{
			result = this.m_pathEndRadius;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.PenetrateLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
		}
		else
		{
			result = this.m_penetrateLos;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			result = this.m_damageAmount;
		}
		return result;
	}

	public int GetSubsequentDamageAmount()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetSubsequentDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_subsequentDamageAmountMod.GetModifiedValue(this.m_subsequentDamageAmount);
		}
		else
		{
			result = this.m_subsequentDamageAmount;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public bool UseEnemyMultiHitEffect()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.UseEnemyMultiHitEffect()).MethodHandle;
			}
			result = this.m_abilityMod.m_useEnemyMultiHitEffectMod.GetModifiedValue(this.m_useEnemyMultiHitEffect);
		}
		else
		{
			result = this.m_useEnemyMultiHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyMultipleHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyMultipleHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetEnemyMultipleHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyMultipleHitEffect;
		}
		else
		{
			result = this.m_enemyMultipleHitEffect;
		}
		return result;
	}

	public int GetAllyHealingAmount()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetAllyHealingAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_allyHealingAmountMod.GetModifiedValue(this.m_allyHealingAmount);
		}
		else
		{
			result = this.m_allyHealingAmount;
		}
		return result;
	}

	public int GetSubsequentHealingAmount()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetSubsequentHealingAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_subsequentHealingAmountMod.GetModifiedValue(this.m_subsequentHealingAmount);
		}
		else
		{
			result = this.m_subsequentHealingAmount;
		}
		return result;
	}

	public int GetAllyEnergyGain()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetAllyEnergyGain()).MethodHandle;
			}
			result = this.m_abilityMod.m_allyEnergyGainMod.GetModifiedValue(this.m_allyEnergyGain);
		}
		else
		{
			result = this.m_allyEnergyGain;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetAllyHitEffect()).MethodHandle;
			}
			result = this.m_cachedAllyHitEffect;
		}
		else
		{
			result = this.m_allyHitEffect;
		}
		return result;
	}

	public bool UseAllyMultiHitEffect()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.UseAllyMultiHitEffect()).MethodHandle;
			}
			result = this.m_abilityMod.m_useAllyMultiHitEffectMod.GetModifiedValue(this.m_useAllyMultiHitEffect);
		}
		else
		{
			result = this.m_useAllyMultiHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAllyMultipleHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyMultipleHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetAllyMultipleHitEffect()).MethodHandle;
			}
			result = this.m_cachedAllyMultipleHitEffect;
		}
		else
		{
			result = this.m_allyMultipleHitEffect;
		}
		return result;
	}

	public int GetSelfHealingAmount()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetSelfHealingAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfHealingAmountMod.GetModifiedValue(this.m_selfHealingAmount);
		}
		else
		{
			result = this.m_selfHealingAmount;
		}
		return result;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedSelfHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetSelfHitEffect()).MethodHandle;
			}
			result = this.m_cachedSelfHitEffect;
		}
		else
		{
			result = this.m_selfHitEffect;
		}
		return result;
	}

	public bool IncludeSelf()
	{
		bool result;
		if (this.GetSelfHealingAmount() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.IncludeSelf()).MethodHandle;
			}
			result = this.GetSelfHitEffect().m_applyEffect;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool IncludeAllies()
	{
		int result;
		if (this.GetAllyHealingAmount() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.IncludeAllies()).MethodHandle;
			}
			if (this.GetAllyEnergyGain() <= 0)
			{
				if (this.GetAllyHitEffect() != null)
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
					if (this.GetAllyHitEffect().m_applyEffect)
					{
						goto IL_7D;
					}
				}
				if (this.UseAllyMultiHitEffect())
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
					if (this.GetAllyMultipleHitEffect() != null)
					{
						result = (this.GetAllyMultipleHitEffect().m_applyEffect ? 1 : 0);
						goto IL_7B;
					}
				}
				result = 0;
				IL_7B:
				return result != 0;
			}
		}
		IL_7D:
		result = 1;
		return result != 0;
	}

	public bool IncludeEnemies()
	{
		int result;
		if (this.GetDamageAmount() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.IncludeEnemies()).MethodHandle;
			}
			if (this.GetEnemyHitEffect() != null)
			{
				if (this.GetEnemyHitEffect().m_applyEffect)
				{
					goto IL_69;
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
			if (this.UseEnemyMultiHitEffect() && this.GetEnemyMultipleHitEffect() != null)
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
				result = (this.GetEnemyMultipleHitEffect().m_applyEffect ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return result != 0;
		}
		IL_69:
		result = 1;
		return result != 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.IncludeEnemies())
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamageAmount());
			this.GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		}
		if (this.IncludeAllies())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetAllyHealingAmount());
			AbilityTooltipHelper.ReportEnergy(ref result, AbilityTooltipSubject.Ally, this.GetAllyEnergyGain());
			this.GetAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		}
		if (this.IncludeSelf())
		{
			AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetSelfHealingAmount());
			this.GetSelfHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = true;
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (!(boardSquare == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (boardSquare.\u0016())
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
				if (!(boardSquare == caster.\u0012()))
				{
					goto IL_67;
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
		}
		flag = false;
		IL_67:
		if (flag)
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
			if (targetIndex > 0)
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
				for (int i = 0; i < targetIndex; i++)
				{
					if (Board.\u000E().\u000E(currentTargets[i].GridPos) == boardSquare)
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
						flag = false;
					}
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
		}
		if (flag)
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
			if (boardSquare.OccupantActor != null)
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
				if (!this.m_targeterAllowOccupiedSquares)
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
					ActorData occupantActor = boardSquare.OccupantActor;
					bool flag2;
					if (NetworkClient.active)
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
						flag2 = occupantActor.\u0018();
					}
					else
					{
						flag2 = false;
					}
					bool flag3 = flag2;
					if (flag3)
					{
						List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
						bool flag4 = false;
						using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								ActorData x = enumerator.Current;
								if (x == occupantActor)
								{
									flag4 = true;
									goto IL_199;
								}
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
						}
						IL_199:
						flag = flag4;
					}
				}
			}
		}
		if (flag)
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
			int num;
			flag = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquare, caster.\u0012(), this.m_chargeThroughInvalidSquares, out num);
		}
		return flag;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return !caster.\u000E().HasQueuedAbilityOfType(typeof(TricksterMadeYouLook));
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			Ability.AddNameplateValueForOverlap(ref result, base.Targeters[i], targetActor, currentTargeterIndex, this.GetDamageAmount(), this.GetSubsequentDamageAmount(), AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy);
		}
		for (int j = 0; j <= currentTargeterIndex; j++)
		{
			Ability.AddNameplateValueForOverlap(ref result, base.Targeters[j], targetActor, currentTargeterIndex, this.GetAllyHealingAmount(), this.GetSubsequentHealingAmount(), AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally);
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterCatchMeIfYouCan abilityMod_TricksterCatchMeIfYouCan = modAsBase as AbilityMod_TricksterCatchMeIfYouCan;
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_TricksterCatchMeIfYouCan) ? this.m_damageAmount : abilityMod_TricksterCatchMeIfYouCan.m_damageAmountMod.GetModifiedValue(this.m_damageAmount), false);
		string name = "SubsequentDamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_TricksterCatchMeIfYouCan)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_TricksterCatchMeIfYouCan.m_subsequentDamageAmountMod.GetModifiedValue(this.m_subsequentDamageAmount);
		}
		else
		{
			val = this.m_subsequentDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_TricksterCatchMeIfYouCan)
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
			effectInfo = abilityMod_TricksterCatchMeIfYouCan.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			effectInfo = this.m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", this.m_enemyHitEffect, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_TricksterCatchMeIfYouCan)
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
			effectInfo2 = abilityMod_TricksterCatchMeIfYouCan.m_enemyMultipleHitEffectMod.GetModifiedValue(this.m_enemyMultipleHitEffect);
		}
		else
		{
			effectInfo2 = this.m_enemyMultipleHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EnemyMultipleHitEffect", this.m_enemyMultipleHitEffect, true);
		string name2 = "AllyHealingAmount";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_TricksterCatchMeIfYouCan)
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
			val2 = abilityMod_TricksterCatchMeIfYouCan.m_allyHealingAmountMod.GetModifiedValue(this.m_allyHealingAmount);
		}
		else
		{
			val2 = this.m_allyHealingAmount;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "SubsequentHealingAmount";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_TricksterCatchMeIfYouCan)
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
			val3 = abilityMod_TricksterCatchMeIfYouCan.m_subsequentHealingAmountMod.GetModifiedValue(this.m_subsequentHealingAmount);
		}
		else
		{
			val3 = this.m_subsequentHealingAmount;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "AllyEnergyGain";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_TricksterCatchMeIfYouCan)
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
			val4 = abilityMod_TricksterCatchMeIfYouCan.m_allyEnergyGainMod.GetModifiedValue(this.m_allyEnergyGain);
		}
		else
		{
			val4 = this.m_allyEnergyGain;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		StandardEffectInfo effectInfo3;
		if (abilityMod_TricksterCatchMeIfYouCan)
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
			effectInfo3 = abilityMod_TricksterCatchMeIfYouCan.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			effectInfo3 = this.m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "AllyHitEffect", this.m_allyHitEffect, true);
		StandardEffectInfo effectInfo4;
		if (abilityMod_TricksterCatchMeIfYouCan)
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
			effectInfo4 = abilityMod_TricksterCatchMeIfYouCan.m_allyMultipleHitEffectMod.GetModifiedValue(this.m_allyMultipleHitEffect);
		}
		else
		{
			effectInfo4 = this.m_allyMultipleHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo4, "AllyMultipleHitEffect", this.m_allyMultipleHitEffect, true);
		string name5 = "SelfHealingAmount";
		string empty5 = string.Empty;
		int val5;
		if (abilityMod_TricksterCatchMeIfYouCan)
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
			val5 = abilityMod_TricksterCatchMeIfYouCan.m_selfHealingAmountMod.GetModifiedValue(this.m_selfHealingAmount);
		}
		else
		{
			val5 = this.m_selfHealingAmount;
		}
		base.AddTokenInt(tokens, name5, empty5, val5, false);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_TricksterCatchMeIfYouCan) ? this.m_selfHitEffect : abilityMod_TricksterCatchMeIfYouCan.m_selfHitEffectMod.GetModifiedValue(this.m_selfHitEffect), "SelfHitEffect", this.m_selfHitEffect, true);
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(false);
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.OnAbilityAnimationRequest(ActorData, int, bool, Vector3)).MethodHandle;
					}
					if (!actorData.\u000E())
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
						Animator animator = actorData.\u000E();
						animator.SetFloat(TricksterCatchMeIfYouCan.animDistToGoal, 10f);
						animator.ResetTrigger(TricksterCatchMeIfYouCan.animStartDamageReaction);
						animator.SetInteger(TricksterCatchMeIfYouCan.animAttack, animationIndex);
						animator.SetBool(TricksterCatchMeIfYouCan.animCinematicCam, false);
						animator.SetTrigger(TricksterCatchMeIfYouCan.animStartAttack);
					}
				}
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
		}
	}

	public override void OnAbilityAnimationRequestProcessed(ActorData caster)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(false);
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.OnAbilityAnimationRequestProcessed(ActorData)).MethodHandle;
					}
					if (!actorData.\u000E())
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
						Animator animator = actorData.\u000E();
						animator.SetInteger(TricksterCatchMeIfYouCan.animAttack, 0);
						animator.SetBool(TricksterCatchMeIfYouCan.animCinematicCam, false);
					}
				}
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
	}

	public override void OnEvasionMoveStartEvent(ActorData caster)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(false);
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData != null && !actorData.\u000E())
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.OnEvasionMoveStartEvent(ActorData)).MethodHandle;
					}
					if (actorData.\u000E() != null)
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
						actorData.\u000E().EnableRendererAndUpdateVisibility();
						actorData.\u000E().gameObject.transform.localScale = Vector3.one;
						TricksterAfterImageNetworkBehaviour.SetMaterialEnabledForAfterImage(caster, actorData, true);
					}
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
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	private int GetAllyHealAmountForHitCount(int hitCount)
	{
		if (hitCount > 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetAllyHealAmountForHitCount(int)).MethodHandle;
			}
			return this.GetAllyHealingAmount() + (hitCount - 1) * this.GetSubsequentHealingAmount();
		}
		return this.GetAllyHealingAmount();
	}

	private int GetDamageAmountForHitCount(int hitCount)
	{
		if (hitCount > 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.GetDamageAmountForHitCount(int)).MethodHandle;
			}
			return this.GetDamageAmount() + (hitCount - 1) * this.GetSubsequentDamageAmount();
		}
		return this.GetDamageAmount();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TricksterCatchMeIfYouCan))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCatchMeIfYouCan.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_TricksterCatchMeIfYouCan);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
