using System;
using System.Collections.Generic;
using UnityEngine;

public class TricksterCreateDamageFields : Ability
{
	[Header("-- Targeting --")]
	public bool m_addFieldAroundSelf = true;

	public bool m_useInitialShapeOverride;

	public AbilityAreaShape m_initialShapeOverride = AbilityAreaShape.Three_x_Three;

	[Header("-- Ground Field Info --")]
	public GroundEffectField m_groundFieldInfo;

	[Header("-- Self Effect for Multi Hit")]
	public StandardEffectInfo m_selfEffectForMultiHit;

	[Header("-- Extra Enemy Hit Effect On Cast")]
	public StandardEffectInfo m_extraEnemyEffectOnCast;

	[Header("-- Spoil spawn info")]
	public bool m_spawnSpoilForEnemyHit = true;

	public bool m_spawnSpoilForAllyHit;

	public SpoilsSpawnData m_spoilSpawnInfo;

	public bool m_onlySpawnSpoilOnMultiHit = true;

	[Header("-- use [Cast Sequence Prefab] to time spawning of ground effect (including temp satellite)")]
	public GameObject m_castSequencePrefab;

	[Header("   use [Temp Satellite Sequence Prefab] for satellites above each ground field")]
	public GameObject m_tempSatelliteSequencePrefab;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;

	private AbilityMod_TricksterCreateDamageFields m_abilityMod;

	private StandardEffectInfo m_cachedSelfEffectForMultiHit;

	private StandardEffectInfo m_cachedExtraEnemyEffectOnCast;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.Start()).MethodHandle;
			}
			this.m_abilityName = "Ground Fields";
		}
		this.Setup();
	}

	private void Setup()
	{
		if (this.m_afterImageSyncComp == null)
		{
			this.m_afterImageSyncComp = base.GetComponent<TricksterAfterImageNetworkBehaviour>();
		}
		this.SetCachedFields();
		GroundEffectField groundFieldInfo = this.GetGroundFieldInfo();
		AbilityAreaShape abilityAreaShape;
		if (this.UseInitialShapeOverride())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.Setup()).MethodHandle;
			}
			abilityAreaShape = this.GetInitialShapeOverride();
		}
		else
		{
			abilityAreaShape = groundFieldInfo.shape;
		}
		AbilityAreaShape shape = abilityAreaShape;
		base.Targeter = new AbilityUtil_Targeter_TricksterFlare(this, this.m_afterImageSyncComp, shape, groundFieldInfo.penetrateLos, groundFieldInfo.IncludeEnemies(), groundFieldInfo.IncludeAllies(), this.AddFieldAroundSelf());
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedSelfEffectForMultiHit;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.SetCachedFields()).MethodHandle;
			}
			cachedSelfEffectForMultiHit = this.m_abilityMod.m_selfEffectForMultiHitMod.GetModifiedValue(this.m_selfEffectForMultiHit);
		}
		else
		{
			cachedSelfEffectForMultiHit = this.m_selfEffectForMultiHit;
		}
		this.m_cachedSelfEffectForMultiHit = cachedSelfEffectForMultiHit;
		this.m_cachedExtraEnemyEffectOnCast = ((!this.m_abilityMod) ? this.m_extraEnemyEffectOnCast : this.m_abilityMod.m_extraEnemyEffectOnCastMod.GetModifiedValue(this.m_extraEnemyEffectOnCast));
	}

	public bool AddFieldAroundSelf()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.AddFieldAroundSelf()).MethodHandle;
			}
			result = this.m_abilityMod.m_addFieldAroundSelfMod.GetModifiedValue(this.m_addFieldAroundSelf);
		}
		else
		{
			result = this.m_addFieldAroundSelf;
		}
		return result;
	}

	public bool UseInitialShapeOverride()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.UseInitialShapeOverride()).MethodHandle;
			}
			result = this.m_abilityMod.m_useInitialShapeOverrideMod.GetModifiedValue(this.m_useInitialShapeOverride);
		}
		else
		{
			result = this.m_useInitialShapeOverride;
		}
		return result;
	}

	public AbilityAreaShape GetInitialShapeOverride()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.GetInitialShapeOverride()).MethodHandle;
			}
			result = this.m_abilityMod.m_initialShapeOverrideMod.GetModifiedValue(this.m_initialShapeOverride);
		}
		else
		{
			result = this.m_initialShapeOverride;
		}
		return result;
	}

	public GroundEffectField GetGroundFieldInfo()
	{
		GroundEffectField result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.GetGroundFieldInfo()).MethodHandle;
			}
			result = this.m_abilityMod.m_groundFieldInfoMod.GetModifiedValue(this.m_groundFieldInfo);
		}
		else
		{
			result = this.m_groundFieldInfo;
		}
		return result;
	}

	public StandardEffectInfo GetSelfEffectForMultiHit()
	{
		StandardEffectInfo result;
		if (this.m_cachedSelfEffectForMultiHit != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.GetSelfEffectForMultiHit()).MethodHandle;
			}
			result = this.m_cachedSelfEffectForMultiHit;
		}
		else
		{
			result = this.m_selfEffectForMultiHit;
		}
		return result;
	}

	public StandardEffectInfo GetExtraEnemyEffectOnCast()
	{
		return (this.m_cachedExtraEnemyEffectOnCast == null) ? this.m_extraEnemyEffectOnCast : this.m_cachedExtraEnemyEffectOnCast;
	}

	public bool SpawnSpoilForEnemyHit()
	{
		return (!this.m_abilityMod) ? this.m_spawnSpoilForEnemyHit : this.m_abilityMod.m_spawnSpoilForEnemyHitMod.GetModifiedValue(this.m_spawnSpoilForEnemyHit);
	}

	public bool SpawnSpoilForAllyHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.SpawnSpoilForAllyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_spawnSpoilForAllyHitMod.GetModifiedValue(this.m_spawnSpoilForAllyHit);
		}
		else
		{
			result = this.m_spawnSpoilForAllyHit;
		}
		return result;
	}

	public bool OnlySpawnSpoilOnMultiHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.OnlySpawnSpoilOnMultiHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_onlySpawnSpoilOnMultiHitMod.GetModifiedValue(this.m_onlySpawnSpoilOnMultiHit);
		}
		else
		{
			result = this.m_onlySpawnSpoilOnMultiHit;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		GroundEffectField groundFieldInfo = this.GetGroundFieldInfo();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, groundFieldInfo.damageAmount);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Secondary, groundFieldInfo.healAmount);
		AbilityTooltipHelper.ReportEnergy(ref result, AbilityTooltipSubject.Secondary, groundFieldInfo.energyGain);
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (!this.AddFieldAroundSelf())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			if (this.m_afterImageSyncComp != null)
			{
				return this.m_afterImageSyncComp.HasVaidAfterImages();
			}
		}
		return true;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		GroundEffectField groundFieldInfo = this.GetGroundFieldInfo();
		if (groundFieldInfo.IncludeEnemies())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			Ability.AddNameplateValueForOverlap(ref result, base.Targeter, targetActor, currentTargeterIndex, groundFieldInfo.damageAmount, groundFieldInfo.subsequentDamageAmount, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
		}
		if (groundFieldInfo.IncludeAllies())
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
			Ability.AddNameplateValueForOverlap(ref result, base.Targeter, targetActor, currentTargeterIndex, groundFieldInfo.healAmount, groundFieldInfo.subsequentHealAmount, AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Secondary);
			Ability.AddNameplateValueForOverlap(ref result, base.Targeter, targetActor, currentTargeterIndex, groundFieldInfo.energyGain, groundFieldInfo.subsequentEnergyGain, AbilityTooltipSymbol.Energy, AbilityTooltipSubject.Secondary);
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterCreateDamageFields abilityMod_TricksterCreateDamageFields = modAsBase as AbilityMod_TricksterCreateDamageFields;
		this.m_groundFieldInfo.AddTooltipTokens(tokens, "GroundEffect", false, null);
		StandardEffectInfo effectInfo;
		if (abilityMod_TricksterCreateDamageFields)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			effectInfo = abilityMod_TricksterCreateDamageFields.m_selfEffectForMultiHitMod.GetModifiedValue(this.m_selfEffectForMultiHit);
		}
		else
		{
			effectInfo = this.m_selfEffectForMultiHit;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "SelfEffectForMultiHit", this.m_selfEffectForMultiHit, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_TricksterCreateDamageFields)
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
			effectInfo2 = abilityMod_TricksterCreateDamageFields.m_extraEnemyEffectOnCastMod.GetModifiedValue(this.m_extraEnemyEffectOnCast);
		}
		else
		{
			effectInfo2 = this.m_extraEnemyEffectOnCast;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "ExtraEnemyEffectOnCast", this.m_extraEnemyEffectOnCast, true);
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.OnAbilityAnimationRequest(ActorData, int, bool, Vector3)).MethodHandle;
					}
					if (!actorData.IsDead())
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
						this.m_afterImageSyncComp.TurnToPosition(actorData, targetPos);
						Animator modelAnimator = actorData.GetModelAnimator();
						modelAnimator.SetInteger("Attack", animationIndex);
						modelAnimator.SetBool("CinematicCam", cinecam);
						modelAnimator.SetTrigger("StartAttack");
					}
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

	public override void OnAbilityAnimationRequestProcessed(ActorData caster)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
		foreach (ActorData actorData in validAfterImages)
		{
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.OnAbilityAnimationRequestProcessed(ActorData)).MethodHandle;
				}
				if (!actorData.IsDead())
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
					Animator modelAnimator = actorData.GetModelAnimator();
					modelAnimator.SetInteger("Attack", 0);
					modelAnimator.SetBool("CinematicCam", false);
				}
			}
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TricksterCreateDamageFields))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCreateDamageFields.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_TricksterCreateDamageFields);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
