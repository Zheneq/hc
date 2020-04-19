using System;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieGuard : Ability
{
	[Header("-- Shield effect")]
	public StandardEffectInfo m_shieldEffectInfo;

	[Header("-- Hit reactions")]
	public int m_techPointGainPerCoveredHit = 5;

	public int m_techPointGainPerTooCloseForCoverHit;

	public StandardEffectInfo m_coveredHitReactionEffect;

	public StandardEffectInfo m_tooCloseForCoverHitReactionEffect;

	[Header("-- Duration --")]
	public int m_coverDuration = 1;

	public bool m_coverLastsForever = true;

	[Header("-- Cover Ignore Min Dist?")]
	public bool m_coverIgnoreMinDist = true;

	[Header("-- Sequences")]
	public GameObject m_removeShieldSequencePrefab;

	public GameObject m_applyShieldSequencePrefab;

	private Valkyrie_SyncComponent m_syncComponent;

	private AbilityMod_ValkyrieGuard m_abilityMod;

	private StandardEffectInfo m_cachedShieldEffectInfo;

	private StandardEffectInfo m_cachedCoveredHitReactionEffect;

	private StandardEffectInfo m_cachedTooCloseForCoverHitReactionEffect;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieGuard.Start()).MethodHandle;
			}
			this.m_abilityName = "Aim Shield";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_syncComponent = base.GetComponent<Valkyrie_SyncComponent>();
		base.Targeter = new AbilityUtil_Targeter_ValkyrieGuard(this, 1f, true, false, false)
		{
			m_addCasterToActorsInRange = true
		};
		this.SetCachedFields();
	}

	private void SetCachedFields()
	{
		this.m_cachedShieldEffectInfo = ((!this.m_abilityMod) ? this.m_shieldEffectInfo : this.m_abilityMod.m_shieldEffectInfoMod.GetModifiedValue(this.m_shieldEffectInfo));
		StandardEffectInfo cachedCoveredHitReactionEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieGuard.SetCachedFields()).MethodHandle;
			}
			cachedCoveredHitReactionEffect = this.m_abilityMod.m_coveredHitReactionEffectMod.GetModifiedValue(this.m_coveredHitReactionEffect);
		}
		else
		{
			cachedCoveredHitReactionEffect = this.m_coveredHitReactionEffect;
		}
		this.m_cachedCoveredHitReactionEffect = cachedCoveredHitReactionEffect;
		StandardEffectInfo cachedTooCloseForCoverHitReactionEffect;
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
			cachedTooCloseForCoverHitReactionEffect = this.m_abilityMod.m_tooCloseForCoverHitReactionEffectMod.GetModifiedValue(this.m_tooCloseForCoverHitReactionEffect);
		}
		else
		{
			cachedTooCloseForCoverHitReactionEffect = this.m_tooCloseForCoverHitReactionEffect;
		}
		this.m_cachedTooCloseForCoverHitReactionEffect = cachedTooCloseForCoverHitReactionEffect;
	}

	public StandardEffectInfo GetShieldEffectInfo()
	{
		StandardEffectInfo result;
		if (this.m_cachedShieldEffectInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieGuard.GetShieldEffectInfo()).MethodHandle;
			}
			result = this.m_cachedShieldEffectInfo;
		}
		else
		{
			result = this.m_shieldEffectInfo;
		}
		return result;
	}

	public int GetTechPointGainPerCoveredHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieGuard.GetTechPointGainPerCoveredHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_techPointGainPerCoveredHitMod.GetModifiedValue(this.m_techPointGainPerCoveredHit);
		}
		else
		{
			result = this.m_techPointGainPerCoveredHit;
		}
		return result;
	}

	public int GetTechPointGainPerTooCloseForCoverHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieGuard.GetTechPointGainPerTooCloseForCoverHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_techPointGainPerTooCloseForCoverHitMod.GetModifiedValue(this.m_techPointGainPerTooCloseForCoverHit);
		}
		else
		{
			result = this.m_techPointGainPerTooCloseForCoverHit;
		}
		return result;
	}

	public StandardEffectInfo GetCoveredHitReactionEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedCoveredHitReactionEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieGuard.GetCoveredHitReactionEffect()).MethodHandle;
			}
			result = this.m_cachedCoveredHitReactionEffect;
		}
		else
		{
			result = this.m_coveredHitReactionEffect;
		}
		return result;
	}

	public StandardEffectInfo GetTooCloseForCoverHitReactionEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedTooCloseForCoverHitReactionEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieGuard.GetTooCloseForCoverHitReactionEffect()).MethodHandle;
			}
			result = this.m_cachedTooCloseForCoverHitReactionEffect;
		}
		else
		{
			result = this.m_tooCloseForCoverHitReactionEffect;
		}
		return result;
	}

	public int GetExtraDamageNextShieldThrowPerCoveredHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieGuard.GetExtraDamageNextShieldThrowPerCoveredHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageNextShieldThrowPerCoveredHitMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetMaxExtraDamageNextShieldThrow()
	{
		return (!this.m_abilityMod) ? 0 : this.m_abilityMod.m_maxExtraDamageNextShieldThrow.GetModifiedValue(0);
	}

	public int GetCoverDuration()
	{
		return (!this.m_abilityMod) ? this.m_coverDuration : this.m_abilityMod.m_coverDurationMod.GetModifiedValue(this.m_coverDuration);
	}

	public bool CoverLastsForever()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieGuard.CoverLastsForever()).MethodHandle;
			}
			result = this.m_abilityMod.m_coverLastsForeverMod.GetModifiedValue(this.m_coverLastsForever);
		}
		else
		{
			result = this.m_coverLastsForever;
		}
		return result;
	}

	public AbilityModCooldownReduction GetCooldownReductionOnNoBlock()
	{
		AbilityModCooldownReduction result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieGuard.GetCooldownReductionOnNoBlock()).MethodHandle;
			}
			result = this.m_abilityMod.m_cooldownReductionNoBlocks;
		}
		else
		{
			result = null;
		}
		return result;
	}

	public bool CoverIgnoreMinDist()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieGuard.CoverIgnoreMinDist()).MethodHandle;
			}
			result = this.m_abilityMod.m_coverIgnoreMinDistMod.GetModifiedValue(this.m_coverIgnoreMinDist);
		}
		else
		{
			result = this.m_coverIgnoreMinDist;
		}
		return result;
	}

	public int GetExtraAbsorb()
	{
		if (this.m_syncComponent)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieGuard.GetExtraAbsorb()).MethodHandle;
			}
			return this.m_syncComponent.m_extraAbsorbForGuard;
		}
		return 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyrieGuard))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ValkyrieGuard);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, this.m_shieldEffectInfo, "ShieldEffectInfo", this.m_shieldEffectInfo, true);
		base.AddTokenInt(tokens, "TechPointGainPerCoveredHit", string.Empty, this.m_techPointGainPerCoveredHit, false);
		base.AddTokenInt(tokens, "TechPointGainPerTooCloseForCoverHit", string.Empty, this.m_techPointGainPerTooCloseForCoverHit, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_coveredHitReactionEffect, "CoveredHitReactionEffect", this.m_coveredHitReactionEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_tooCloseForCoverHitReactionEffect, "TooCloseForCoverHitReactionEffect", this.m_tooCloseForCoverHitReactionEffect, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Self, 0)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor == base.ActorData)
		{
			dictionary[AbilityTooltipSymbol.Absorb] = this.GetExtraAbsorb();
		}
		return dictionary;
	}

	public override Vector3 GetRotateToTargetPos(List<AbilityTarget> targets, ActorData caster)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(targets[0].GridPos);
		if (boardSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieGuard.GetRotateToTargetPos(List<AbilityTarget>, ActorData)).MethodHandle;
			}
			Vector3 b;
			VectorUtils.GetDirectionAndOffsetToClosestSide(boardSquare, targets[0].FreePos, false, out b);
			return boardSquare.ToVector3() + b;
		}
		return base.GetRotateToTargetPos(targets, caster);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (caster != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieGuard.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			if (caster.\u000E() != null)
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
				return !caster.\u000E().HasQueuedAbilityOfType(typeof(ValkyrieDashAoE));
			}
		}
		return false;
	}

	public override Ability.TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		return Ability.TargetingParadigm.Direction;
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = 1f;
		max = 1f;
		return true;
	}
}
