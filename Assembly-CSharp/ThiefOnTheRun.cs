using System;
using System.Collections.Generic;
using UnityEngine;

public class ThiefOnTheRun : Ability
{
	[Header("-- Targeter")]
	public bool m_targeterMultiStep = true;

	public float m_minDistanceBetweenSteps = 2f;

	public float m_minDistanceBetweenAnySteps = -1f;

	public float m_maxDistanceBetweenSteps = 10f;

	[Header("-- Dash Hit Size")]
	public float m_dashRadius = 1f;

	public bool m_dashPenetrateLineOfSight;

	[Header("-- Hit Damage and Effect")]
	public int m_damageAmount;

	public int m_subsequentDamage;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Hid On Self")]
	public StandardEffectInfo m_effectOnSelfThroughSmokeField;

	public int m_cooldownReductionIfNoEnemy;

	public AbilityData.ActionType m_cooldownReductionOnAbility = AbilityData.ActionType.ABILITY_3;

	[Header("-- Spoil Powerup Spawn")]
	public SpoilsSpawnData m_spoilSpawnInfo;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private int m_numChargePiviots = 1;

	private AbilityMod_ThiefOnTheRun m_abilityMod;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedEffectOnSelfThroughSmokeField;

	private SpoilsSpawnData m_cachedSpoilSpawnInfo;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.Start()).MethodHandle;
			}
			this.m_abilityName = "On the Run";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		int numChargePiviots;
		if (this.m_targeterMultiStep)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.Setup()).MethodHandle;
			}
			numChargePiviots = Mathf.Max(base.GetNumTargets(), 1);
		}
		else
		{
			numChargePiviots = 1;
		}
		this.m_numChargePiviots = numChargePiviots;
		float dashRadius = this.GetDashRadius();
		if (this.GetExpectedNumberOfTargeters() < 2)
		{
			base.Targeter = new AbilityUtil_Targeter_ChargeAoE(this, dashRadius, dashRadius, dashRadius, -1, false, this.DashPenetrateLineOfSight());
		}
		else
		{
			base.ClearTargeters();
			int expectedNumberOfTargeters = this.GetExpectedNumberOfTargeters();
			for (int i = 0; i < expectedNumberOfTargeters; i++)
			{
				AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, dashRadius, dashRadius, dashRadius, -1, false, this.DashPenetrateLineOfSight());
				if (i < expectedNumberOfTargeters - 1)
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
					abilityUtil_Targeter_ChargeAoE.UseEndPosAsDamageOriginIfOverlap = true;
				}
				base.Targeters.Add(abilityUtil_Targeter_ChargeAoE);
				base.Targeters[i].SetUseMultiTargetUpdate(true);
			}
			for (;;)
			{
				switch (5)
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
		return this.m_numChargePiviots;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return (this.GetMaxDistanceBetweenSteps() - 0.5f) * (float)this.m_numChargePiviots;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyHitEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.SetCachedFields()).MethodHandle;
			}
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		this.m_cachedEffectOnSelfThroughSmokeField = ((!this.m_abilityMod) ? this.m_effectOnSelfThroughSmokeField : this.m_abilityMod.m_effectOnSelfThroughSmokeFieldMod.GetModifiedValue(this.m_effectOnSelfThroughSmokeField));
		this.m_cachedSpoilSpawnInfo = ((!this.m_abilityMod) ? this.m_spoilSpawnInfo : this.m_abilityMod.m_spoilSpawnInfoMod.GetModifiedValue(this.m_spoilSpawnInfo));
	}

	public float GetMinDistanceBetweenSteps()
	{
		return (!this.m_abilityMod) ? this.m_minDistanceBetweenSteps : this.m_abilityMod.m_minDistanceBetweenStepsMod.GetModifiedValue(this.m_minDistanceBetweenSteps);
	}

	public float GetMinDistanceBetweenAnySteps()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.GetMinDistanceBetweenAnySteps()).MethodHandle;
			}
			result = this.m_abilityMod.m_minDistanceBetweenAnyStepsMod.GetModifiedValue(this.m_minDistanceBetweenAnySteps);
		}
		else
		{
			result = this.m_minDistanceBetweenAnySteps;
		}
		return result;
	}

	public float GetMaxDistanceBetweenSteps()
	{
		return (!this.m_abilityMod) ? this.m_maxDistanceBetweenSteps : this.m_abilityMod.m_maxDistanceBetweenStepsMod.GetModifiedValue(this.m_maxDistanceBetweenSteps);
	}

	public float GetDashRadius()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.GetDashRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_dashRadiusMod.GetModifiedValue(this.m_dashRadius);
		}
		else
		{
			result = this.m_dashRadius;
		}
		return result;
	}

	public bool DashPenetrateLineOfSight()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.DashPenetrateLineOfSight()).MethodHandle;
			}
			result = this.m_abilityMod.m_dashPenetrateLineOfSightMod.GetModifiedValue(this.m_dashPenetrateLineOfSight);
		}
		else
		{
			result = this.m_dashPenetrateLineOfSight;
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
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.GetDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			result = this.m_damageAmount;
		}
		return result;
	}

	public int GetSubsequentDamage()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.GetSubsequentDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_subsequentDamageMod.GetModifiedValue(this.m_subsequentDamage);
		}
		else
		{
			result = this.m_subsequentDamage;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.GetEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelfThroughSmokeField()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnSelfThroughSmokeField != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.GetEffectOnSelfThroughSmokeField()).MethodHandle;
			}
			result = this.m_cachedEffectOnSelfThroughSmokeField;
		}
		else
		{
			result = this.m_effectOnSelfThroughSmokeField;
		}
		return result;
	}

	public int GetCooldownReductionIfNoEnemy()
	{
		return (!this.m_abilityMod) ? this.m_cooldownReductionIfNoEnemy : this.m_abilityMod.m_cooldownReductionIfNoEnemyMod.GetModifiedValue(this.m_cooldownReductionIfNoEnemy);
	}

	public SpoilsSpawnData GetSpoilSpawnInfo()
	{
		SpoilsSpawnData result;
		if (this.m_cachedSpoilSpawnInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.GetSpoilSpawnInfo()).MethodHandle;
			}
			result = this.m_cachedSpoilSpawnInfo;
		}
		else
		{
			result = this.m_spoilSpawnInfo;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_damageAmount);
		this.m_enemyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		ActorData actorData = base.ActorData;
		if (actorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			if (actorData.\u0012() != null)
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
				for (int i = 0; i <= currentTargeterIndex; i++)
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
					if (i >= base.Targeters.Count)
					{
						break;
					}
					BoardSquare y = (i <= 0) ? actorData.\u0012() : Board.\u000E().\u000E(base.Targeters[i - 1].LastUpdatingGridPos);
					int subsequentAmount = this.GetSubsequentDamage();
					if (targetActor.\u0012() == y)
					{
						subsequentAmount = 0;
					}
					Ability.AddNameplateValueForOverlap(ref result, base.Targeters[i], targetActor, currentTargeterIndex, this.GetDamageAmount(), subsequentAmount, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
				}
			}
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefOnTheRun abilityMod_ThiefOnTheRun = modAsBase as AbilityMod_ThiefOnTheRun;
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_ThiefOnTheRun) ? this.m_damageAmount : abilityMod_ThiefOnTheRun.m_damageAmountMod.GetModifiedValue(this.m_damageAmount), false);
		string name = "SubsequentDamage";
		string empty = string.Empty;
		int val;
		if (abilityMod_ThiefOnTheRun)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_ThiefOnTheRun.m_subsequentDamageMod.GetModifiedValue(this.m_subsequentDamage);
		}
		else
		{
			val = this.m_subsequentDamage;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_ThiefOnTheRun)
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
			effectInfo = abilityMod_ThiefOnTheRun.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			effectInfo = this.m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", this.m_enemyHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_ThiefOnTheRun) ? this.m_effectOnSelfThroughSmokeField : abilityMod_ThiefOnTheRun.m_effectOnSelfThroughSmokeFieldMod.GetModifiedValue(this.m_effectOnSelfThroughSmokeField), "EffectOnSelfThroughSmokeField", this.m_effectOnSelfThroughSmokeField, true);
		string name2 = "CooldownReductionIfNoEnemy";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_ThiefOnTheRun)
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
			val2 = abilityMod_ThiefOnTheRun.m_cooldownReductionIfNoEnemyMod.GetModifiedValue(this.m_cooldownReductionIfNoEnemy);
		}
		else
		{
			val2 = this.m_cooldownReductionIfNoEnemy;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (boardSquare == null || !boardSquare.\u0016())
		{
			return false;
		}
		bool result;
		if (this.GetExpectedNumberOfTargeters() < 2)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			result = (KnockbackUtils.BuildStraightLineChargePath(caster, boardSquare) != null);
		}
		else
		{
			BoardSquare boardSquare2;
			if (targetIndex == 0)
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
				boardSquare2 = caster.\u0012();
			}
			else
			{
				boardSquare2 = Board.\u000E().\u000E(currentTargets[targetIndex - 1].GridPos);
			}
			bool flag = KnockbackUtils.BuildStraightLineChargePath(caster, boardSquare, boardSquare2, false) != null;
			float squareSize = Board.\u000E().squareSize;
			float num = Vector3.Distance(boardSquare2.ToVector3(), boardSquare.ToVector3());
			bool flag2;
			if (num >= this.GetMinDistanceBetweenSteps() * squareSize)
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
				flag2 = (num <= this.GetMaxDistanceBetweenSteps() * squareSize);
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			if (flag)
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
				if (flag3 && this.GetMinDistanceBetweenAnySteps() > 0f)
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
					for (int i = 0; i < targetIndex; i++)
					{
						BoardSquare boardSquare3 = Board.\u000E().\u000E(currentTargets[i].GridPos);
						flag3 &= (Vector3.Distance(boardSquare3.ToVector3(), boardSquare.ToVector3()) >= this.GetMinDistanceBetweenAnySteps() * squareSize);
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
			bool flag4;
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
				flag4 = flag3;
			}
			else
			{
				flag4 = false;
			}
			result = flag4;
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ThiefOnTheRun))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefOnTheRun.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_ThiefOnTheRun);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
