using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArrowRain : Ability
{
	[Separator("Targeting Info", true)]
	public float m_startRadius = 3f;

	public float m_endRadius = 3f;

	public float m_lineRadius = 3f;

	public float m_minRangeBetween = 1f;

	public float m_maxRangeBetween = 4f;

	[Header("-- Whether require LoS to end square of line")]
	public bool m_linePenetrateLoS;

	[Header("-- Whether check LoS for gameplay hits")]
	public bool m_aoePenetrateLoS;

	public int m_maxTargets = 5;

	[Separator("Enemy Hit", true)]
	public int m_damage = 0x28;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	public GameObject m_hitAreaSequencePrefab;

	private AbilityMod_ArcherArrowRain m_abilityMod;

	private ArcherHealingDebuffArrow m_healArrowAbility;

	private AbilityData.ActionType m_healArrowActionType = AbilityData.ActionType.INVALID_ACTION;

	private AbilityData m_abilityData;

	private ActorTargeting m_actorTargeting;

	private Archer_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedAdditionalEnemyHitEffect;

	private StandardEffectInfo m_cachedSingleEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.Start()).MethodHandle;
			}
			this.m_abilityName = "Arrow Rain";
		}
		this.m_abilityData = base.GetComponent<AbilityData>();
		if (this.m_abilityData != null)
		{
			this.m_healArrowAbility = (base.GetAbilityOfType(typeof(ArcherHealingDebuffArrow)) as ArcherHealingDebuffArrow);
			if (this.m_healArrowAbility != null)
			{
				this.m_healArrowActionType = this.m_abilityData.GetActionTypeOfAbility(this.m_healArrowAbility);
			}
		}
		this.m_actorTargeting = base.GetComponent<ActorTargeting>();
		this.m_syncComp = base.GetComponent<Archer_SyncComponent>();
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.Targeters.Clear();
		for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_CapsuleAoE abilityUtil_Targeter_CapsuleAoE = new AbilityUtil_Targeter_CapsuleAoE(this, this.GetStartRadius(), this.GetEndRadius(), this.GetLineRadius(), this.GetMaxTargets(), false, this.AoePenetrateLoS());
			abilityUtil_Targeter_CapsuleAoE.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_CapsuleAoE.ShowArcToShape = false;
			base.Targeters.Add(abilityUtil_Targeter_CapsuleAoE);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.Setup()).MethodHandle;
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return this.GetTargetData().Length;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			BoardSquare boardSquare = Board.\u000E().\u000E(currentTargets[targetIndex - 1].GridPos);
			BoardSquare boardSquare2 = Board.\u000E().\u000E(target.GridPos);
			if (boardSquare != null)
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
				if (boardSquare2 != null)
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
					float num = Vector3.Distance(boardSquare.ToVector3(), boardSquare2.ToVector3());
					if (num <= this.GetMaxRangeBetween() * Board.\u000E().squareSize)
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
						if (num >= this.GetMinRangeBetween() * Board.\u000E().squareSize)
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
							if (!this.LinePenetrateLoS())
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
								if (!boardSquare.\u0013(boardSquare2.x, boardSquare2.y))
								{
									return false;
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
							return true;
						}
					}
				}
			}
			return false;
		}
		return base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "Damage", string.Empty, this.m_damage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffect, "EnemyHitEffect", this.m_enemyHitEffect, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ArcherArrowRain))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_ArcherArrowRain);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyHitEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.SetCachedFields()).MethodHandle;
			}
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		StandardEffectInfo cachedAdditionalEnemyHitEffect;
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
			cachedAdditionalEnemyHitEffect = this.m_abilityMod.m_additionalEnemyHitEffect.GetModifiedValue(null);
		}
		else
		{
			cachedAdditionalEnemyHitEffect = null;
		}
		this.m_cachedAdditionalEnemyHitEffect = cachedAdditionalEnemyHitEffect;
		this.m_cachedSingleEnemyHitEffect = ((!this.m_abilityMod) ? null : this.m_abilityMod.m_singleEnemyHitEffectMod.GetModifiedValue(null));
	}

	public float GetStartRadius()
	{
		return (!this.m_abilityMod) ? this.m_startRadius : this.m_abilityMod.m_startRadiusMod.GetModifiedValue(this.m_startRadius);
	}

	public float GetEndRadius()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.GetEndRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_endRadiusMod.GetModifiedValue(this.m_endRadius);
		}
		else
		{
			result = this.m_endRadius;
		}
		return result;
	}

	public float GetLineRadius()
	{
		return (!this.m_abilityMod) ? this.m_lineRadius : this.m_abilityMod.m_lineRadiusMod.GetModifiedValue(this.m_lineRadius);
	}

	public float GetMinRangeBetween()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.GetMinRangeBetween()).MethodHandle;
			}
			result = this.m_abilityMod.m_minRangeBetweenMod.GetModifiedValue(this.m_minRangeBetween);
		}
		else
		{
			result = this.m_minRangeBetween;
		}
		return result;
	}

	public float GetMaxRangeBetween()
	{
		return (!this.m_abilityMod) ? this.m_maxRangeBetween : this.m_abilityMod.m_maxRangeBetweenMod.GetModifiedValue(this.m_maxRangeBetween);
	}

	public bool LinePenetrateLoS()
	{
		return (!this.m_abilityMod) ? this.m_linePenetrateLoS : this.m_abilityMod.m_linePenetrateLoSMod.GetModifiedValue(this.m_linePenetrateLoS);
	}

	public bool AoePenetrateLoS()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.AoePenetrateLoS()).MethodHandle;
			}
			result = this.m_abilityMod.m_aoePenetrateLoSMod.GetModifiedValue(this.m_aoePenetrateLoS);
		}
		else
		{
			result = this.m_aoePenetrateLoS;
		}
		return result;
	}

	public int GetMaxTargets()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.GetMaxTargets()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
		}
		return result;
	}

	public int GetDamage()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.GetDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damage);
		}
		else
		{
			result = this.m_damage;
		}
		return result;
	}

	public int GetDamageBelowHealthThreshold()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.GetDamageBelowHealthThreshold()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageBelowHealthThresholdMod.GetModifiedValue(this.GetDamage());
		}
		else
		{
			result = this.GetDamage();
		}
		return result;
	}

	public float GetHealthThresholdForBonusDamage()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.GetHealthThresholdForBonusDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_healthThresholdForDamageMod.GetModifiedValue(0f);
		}
		else
		{
			result = 0f;
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
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.GetEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAdditionalEnemyHitEffect()
	{
		return this.m_cachedAdditionalEnemyHitEffect;
	}

	public StandardEffectInfo GetSingleEnemyHitEffect()
	{
		return this.m_cachedSingleEnemyHitEffect;
	}

	public int GetTechPointRefundNoHits()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.GetTechPointRefundNoHits()).MethodHandle;
			}
			result = this.m_abilityMod.m_techPointRefundNoHits.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetDamage());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int num = this.GetDamage();
		if (targetActor.\u0012() <= this.GetHealthThresholdForBonusDamage())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			num = this.GetDamageBelowHealthThreshold();
		}
		if (this.IsReactionHealTarget(targetActor))
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
			num += this.m_healArrowAbility.GetExtraDamageToThisTargetFromCaster();
		}
		dictionary[AbilityTooltipSymbol.Damage] = num;
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[currentTargeterIndex].GetActorsInRange();
		using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityUtil_Targeter.ActorTarget actorTarget = enumerator.Current;
				if (this.IsReactionHealTarget(actorTarget.m_actor))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
					}
					return this.m_healArrowAbility.GetTechPointsPerHeal();
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
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	private bool IsReactionHealTarget(ActorData targetActor)
	{
		if (this.m_syncComp.m_healReactionTargetActor == targetActor.ActorIndex)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherArrowRain.IsReactionHealTarget(ActorData)).MethodHandle;
			}
			if (!this.m_syncComp.ActorHasUsedHealReaction(base.ActorData))
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
				return true;
			}
		}
		if (this.m_healArrowActionType != AbilityData.ActionType.INVALID_ACTION)
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
			if (this.m_actorTargeting != null)
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
				List<AbilityTarget> abilityTargetsInRequest = this.m_actorTargeting.GetAbilityTargetsInRequest(this.m_healArrowActionType);
				if (abilityTargetsInRequest != null)
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
					if (abilityTargetsInRequest.Count > 0)
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
						BoardSquare square = Board.\u000E().\u000E(abilityTargetsInRequest[0].GridPos);
						ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(square, true, false, base.ActorData);
						if (targetableActorOnSquare == targetActor)
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
				}
			}
		}
		return false;
	}
}
