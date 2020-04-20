using System;
using System.Collections.Generic;
using UnityEngine;

public class TricksterCones : Ability
{
	[Header("-- Cone Targeting")]
	public ConeTargetingInfo m_coneInfo;

	[Header("-- Enemy Hit Damage and Effects")]
	public int m_damageAmount = 3;

	public int m_subsequentDamageAmount = 2;

	public StandardEffectInfo m_enemyHitEffect;

	[Space(10f)]
	public bool m_useEnemyMultiHitEffect;

	public StandardEffectInfo m_enemyMultipleHitEffect;

	[Header("-- Ally Hit Heal and Effects")]
	public int m_allyHealAmount;

	public int m_allySubsequentHealAmount;

	public StandardEffectInfo m_allyHitEffect;

	[Space(10f)]
	public bool m_useAllyMultiHitEffect;

	public StandardEffectInfo m_allyMultipleHitEffect;

	[Header("-- Self Hit Heal and Effects")]
	public int m_selfHealAmount;

	public StandardEffectInfo m_selfHitEffect;

	public StandardEffectInfo m_selfEffectForMultiHit;

	[Header("-- Cooldown Reduction Per Enemy Hit By Clone --")]
	public int m_cooldownReductionPerHitByClone;

	public AbilityData.ActionType m_cooldownReductionActionType = AbilityData.ActionType.ABILITY_1;

	[Header("-- For spawning spoils")]
	public bool m_spawnSpoilForEnemyHit;

	public bool m_spawnSpoilForAllyHit;

	public SpoilsSpawnData m_spoilSpawnInfo;

	public bool m_onlySpawnSpoilOnMultiHit = true;

	[Header("-- Sequences")]
	public GameObject m_projectileSequencePrefab;

	public float m_impactDelayTime = 0.35f;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;

	private AbilityMod_TricksterCones m_abilityMod;

	private ConeTargetingInfo m_cachedConeInfo;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedEnemyMultipleHitEffect;

	private StandardEffectInfo m_cachedAllyHitEffect;

	private StandardEffectInfo m_cachedAllyMultipleHitEffect;

	private StandardEffectInfo m_cachedSelfHitEffect;

	private StandardEffectInfo m_cachedSelfEffectForMultiHit;

	private SpoilsSpawnData m_cachedSpoilSpawnInfo;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.Start()).MethodHandle;
			}
			this.m_abilityName = "Trickster Cones";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_afterImageSyncComp = base.GetComponent<TricksterAfterImageNetworkBehaviour>();
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_TricksterCones(this, this.GetConeInfo(), 3, new AbilityUtil_Targeter_TricksterCones.GetCurrentNumberOfConesDelegate(this.GetNumCones), new AbilityUtil_Targeter_TricksterCones.GetConeInfoDelegate(this.GetConeOrigins), new AbilityUtil_Targeter_TricksterCones.GetConeInfoDelegate(this.GetConeDirections), new AbilityUtil_Targeter_TricksterCones.GetClampedTargetPosDelegate(this.GetFreePosForAim), true, false);
	}

	public int GetNumCones()
	{
		return this.m_afterImageSyncComp.GetValidAfterImages(true).Count + 1;
	}

	public Vector3 GetFreePosForAim(AbilityTarget currentTarget, ActorData caster)
	{
		List<ActorData> list = new List<ActorData>();
		list.Add(caster);
		list.AddRange(this.m_afterImageSyncComp.GetValidAfterImages(true));
		Vector3 vector;
		Vector3 result;
		this.m_afterImageSyncComp.CalcTargetingCenterAndAimAtPos(currentTarget.FreePos, caster, list, false, out vector, out result);
		return result;
	}

	public List<Vector3> GetConeOrigins(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		List<ActorData> list2 = new List<ActorData>();
		list2.Add(caster);
		list2.AddRange(this.m_afterImageSyncComp.GetValidAfterImages(true));
		foreach (ActorData actorData in list2)
		{
			list.Add(actorData.GetTravelBoardSquareWorldPositionForLos());
		}
		return list;
	}

	public List<Vector3> GetConeDirections(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		List<ActorData> list2 = new List<ActorData>();
		list2.Add(caster);
		list2.AddRange(this.m_afterImageSyncComp.GetValidAfterImages(true));
		using (List<ActorData>.Enumerator enumerator = list2.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				list.Add(targeterFreePos - actorData.GetTravelBoardSquareWorldPosition());
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetConeDirections(AbilityTarget, Vector3, ActorData)).MethodHandle;
			}
		}
		return list;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetConeInfo().m_radiusInSquares;
	}

	public override Ability.TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		return Ability.TargetingParadigm.Position;
	}

	private void SetCachedFields()
	{
		this.m_cachedConeInfo = ((!this.m_abilityMod) ? this.m_coneInfo : this.m_abilityMod.m_coneInfoMod.GetModifiedValue(this.m_coneInfo));
		StandardEffectInfo cachedEnemyHitEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.SetCachedFields()).MethodHandle;
			}
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		StandardEffectInfo cachedEnemyMultipleHitEffect;
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
			cachedEnemyMultipleHitEffect = this.m_abilityMod.m_enemyMultipleHitEffectMod.GetModifiedValue(this.m_enemyMultipleHitEffect);
		}
		else
		{
			cachedEnemyMultipleHitEffect = this.m_enemyMultipleHitEffect;
		}
		this.m_cachedEnemyMultipleHitEffect = cachedEnemyMultipleHitEffect;
		StandardEffectInfo cachedAllyHitEffect;
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
			cachedAllyHitEffect = this.m_abilityMod.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = this.m_allyHitEffect;
		}
		this.m_cachedAllyHitEffect = cachedAllyHitEffect;
		StandardEffectInfo cachedAllyMultipleHitEffect;
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
			cachedAllyMultipleHitEffect = this.m_abilityMod.m_allyMultipleHitEffectMod.GetModifiedValue(this.m_allyMultipleHitEffect);
		}
		else
		{
			cachedAllyMultipleHitEffect = this.m_allyMultipleHitEffect;
		}
		this.m_cachedAllyMultipleHitEffect = cachedAllyMultipleHitEffect;
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
		StandardEffectInfo cachedSelfEffectForMultiHit;
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
			cachedSelfEffectForMultiHit = this.m_abilityMod.m_selfEffectForMultiHitMod.GetModifiedValue(this.m_selfEffectForMultiHit);
		}
		else
		{
			cachedSelfEffectForMultiHit = this.m_selfEffectForMultiHit;
		}
		this.m_cachedSelfEffectForMultiHit = cachedSelfEffectForMultiHit;
		SpoilsSpawnData cachedSpoilSpawnInfo;
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
			cachedSpoilSpawnInfo = this.m_abilityMod.m_spoilSpawnInfoMod.GetModifiedValue(this.m_spoilSpawnInfo);
		}
		else
		{
			cachedSpoilSpawnInfo = this.m_spoilSpawnInfo;
		}
		this.m_cachedSpoilSpawnInfo = cachedSpoilSpawnInfo;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		ConeTargetingInfo result;
		if (this.m_cachedConeInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetConeInfo()).MethodHandle;
			}
			result = this.m_cachedConeInfo;
		}
		else
		{
			result = this.m_coneInfo;
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
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetDamageAmount()).MethodHandle;
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
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetSubsequentDamageAmount()).MethodHandle;
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
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetEnemyHitEffect()).MethodHandle;
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
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.UseEnemyMultiHitEffect()).MethodHandle;
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
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetEnemyMultipleHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyMultipleHitEffect;
		}
		else
		{
			result = this.m_enemyMultipleHitEffect;
		}
		return result;
	}

	public int GetAllyHealAmount()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetAllyHealAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_allyHealAmountMod.GetModifiedValue(this.m_allyHealAmount);
		}
		else
		{
			result = this.m_allyHealAmount;
		}
		return result;
	}

	public int GetAllySubsequentHealAmount()
	{
		return (!this.m_abilityMod) ? this.m_allySubsequentHealAmount : this.m_abilityMod.m_allySubsequentHealAmountMod.GetModifiedValue(this.m_allySubsequentHealAmount);
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetAllyHitEffect()).MethodHandle;
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
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.UseAllyMultiHitEffect()).MethodHandle;
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
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetAllyMultipleHitEffect()).MethodHandle;
			}
			result = this.m_cachedAllyMultipleHitEffect;
		}
		else
		{
			result = this.m_allyMultipleHitEffect;
		}
		return result;
	}

	public int GetSelfHealAmount()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetSelfHealAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfHealAmountMod.GetModifiedValue(this.m_selfHealAmount);
		}
		else
		{
			result = this.m_selfHealAmount;
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
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetSelfHitEffect()).MethodHandle;
			}
			result = this.m_cachedSelfHitEffect;
		}
		else
		{
			result = this.m_selfHitEffect;
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
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetSelfEffectForMultiHit()).MethodHandle;
			}
			result = this.m_cachedSelfEffectForMultiHit;
		}
		else
		{
			result = this.m_selfEffectForMultiHit;
		}
		return result;
	}

	public int GetCooldownReductionPerHitByClone()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetCooldownReductionPerHitByClone()).MethodHandle;
			}
			result = this.m_abilityMod.m_cooldownReductionPerHitByCloneMod.GetModifiedValue(this.m_cooldownReductionPerHitByClone);
		}
		else
		{
			result = this.m_cooldownReductionPerHitByClone;
		}
		return result;
	}

	public bool SpawnSpoilForEnemyHit()
	{
		return (!this.m_abilityMod) ? this.m_spawnSpoilForEnemyHit : this.m_abilityMod.m_spawnSpoilForEnemyHitMod.GetModifiedValue(this.m_spawnSpoilForEnemyHit);
	}

	public bool SpawnSpoilForAllyHit()
	{
		return (!this.m_abilityMod) ? this.m_spawnSpoilForAllyHit : this.m_abilityMod.m_spawnSpoilForAllyHitMod.GetModifiedValue(this.m_spawnSpoilForAllyHit);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetSpoilSpawnInfo()).MethodHandle;
			}
			result = this.m_cachedSpoilSpawnInfo;
		}
		else
		{
			result = this.m_spoilSpawnInfo;
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
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.OnlySpawnSpoilOnMultiHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_onlySpawnSpoilOnMultiHitMod.GetModifiedValue(this.m_onlySpawnSpoilOnMultiHit);
		}
		else
		{
			result = this.m_onlySpawnSpoilOnMultiHit;
		}
		return result;
	}

	private int CalcDamageFromNumHits(int numHits, int numFromCover)
	{
		return ActorMultiHitContext.CalcDamageFromNumHits(numHits, numFromCover, this.GetDamageAmount(), this.GetSubsequentDamageAmount());
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamageAmount());
		this.GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetAllyHealAmount());
		this.GetAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetSelfHealAmount());
		this.GetSelfHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		return result;
	}

	public override List<int> \u001D()
	{
		List<int> list = base.\u001D();
		list.Add(this.m_subsequentDamageAmount);
		list.Add(this.m_allySubsequentHealAmount);
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		AbilityUtil_Targeter_TricksterCones abilityUtil_Targeter_TricksterCones = base.Targeter as AbilityUtil_Targeter_TricksterCones;
		ActorData actorData = base.ActorData;
		if (abilityUtil_Targeter_TricksterCones != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
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
				if (abilityUtil_Targeter_TricksterCones.m_actorToHitCount.ContainsKey(targetActor))
				{
					int num = abilityUtil_Targeter_TricksterCones.m_actorToHitCount[targetActor];
					int numFromCover = abilityUtil_Targeter_TricksterCones.m_actorToCoverCount[targetActor];
					if (actorData.GetTeam() != targetActor.GetTeam())
					{
						int value = this.CalcDamageFromNumHits(num, numFromCover);
						dictionary[AbilityTooltipSymbol.Damage] = value;
					}
					else if (actorData != targetActor)
					{
						int value2 = this.GetAllyHealAmount() + (num - 1) * this.GetAllySubsequentHealAmount();
						dictionary[AbilityTooltipSymbol.Healing] = value2;
					}
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterCones abilityMod_TricksterCones = modAsBase as AbilityMod_TricksterCones;
		string name = "DamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_TricksterCones)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_TricksterCones.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			val = this.m_damageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "SubsequentDamageAmount";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_TricksterCones)
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
			val2 = abilityMod_TricksterCones.m_subsequentDamageAmountMod.GetModifiedValue(this.m_subsequentDamageAmount);
		}
		else
		{
			val2 = this.m_subsequentDamageAmount;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_TricksterCones)
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
			effectInfo = abilityMod_TricksterCones.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			effectInfo = this.m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", this.m_enemyHitEffect, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_TricksterCones)
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
			effectInfo2 = abilityMod_TricksterCones.m_enemyMultipleHitEffectMod.GetModifiedValue(this.m_enemyMultipleHitEffect);
		}
		else
		{
			effectInfo2 = this.m_enemyMultipleHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EnemyMultipleHitEffect", this.m_enemyMultipleHitEffect, true);
		string name3 = "AllyHealAmount";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_TricksterCones)
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
			val3 = abilityMod_TricksterCones.m_allyHealAmountMod.GetModifiedValue(this.m_allyHealAmount);
		}
		else
		{
			val3 = this.m_allyHealAmount;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		base.AddTokenInt(tokens, "AllySubsequentHealAmount", string.Empty, (!abilityMod_TricksterCones) ? this.m_allySubsequentHealAmount : abilityMod_TricksterCones.m_allySubsequentHealAmountMod.GetModifiedValue(this.m_allySubsequentHealAmount), false);
		StandardEffectInfo effectInfo3;
		if (abilityMod_TricksterCones)
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
			effectInfo3 = abilityMod_TricksterCones.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			effectInfo3 = this.m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "AllyHitEffect", this.m_allyHitEffect, true);
		StandardEffectInfo effectInfo4;
		if (abilityMod_TricksterCones)
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
			effectInfo4 = abilityMod_TricksterCones.m_allyMultipleHitEffectMod.GetModifiedValue(this.m_allyMultipleHitEffect);
		}
		else
		{
			effectInfo4 = this.m_allyMultipleHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo4, "AllyMultipleHitEffect", this.m_allyMultipleHitEffect, true);
		string name4 = "SelfHealAmount";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_TricksterCones)
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
			val4 = abilityMod_TricksterCones.m_selfHealAmountMod.GetModifiedValue(this.m_selfHealAmount);
		}
		else
		{
			val4 = this.m_selfHealAmount;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_TricksterCones) ? this.m_selfHitEffect : abilityMod_TricksterCones.m_selfHitEffectMod.GetModifiedValue(this.m_selfHitEffect), "SelfHitEffect", this.m_selfHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_TricksterCones) ? this.m_selfEffectForMultiHit : abilityMod_TricksterCones.m_selfEffectForMultiHitMod.GetModifiedValue(this.m_selfEffectForMultiHit), "SelfEffectForMultiHit", this.m_selfEffectForMultiHit, true);
		string name5 = "CooldownReductionPerHitByClone";
		string empty5 = string.Empty;
		int val5;
		if (abilityMod_TricksterCones)
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
			val5 = abilityMod_TricksterCones.m_cooldownReductionPerHitByCloneMod.GetModifiedValue(this.m_cooldownReductionPerHitByClone);
		}
		else
		{
			val5 = this.m_cooldownReductionPerHitByClone;
		}
		base.AddTokenInt(tokens, name5, empty5, val5, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TricksterCones))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_TricksterCones);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
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
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.OnAbilityAnimationRequest(ActorData, int, bool, Vector3)).MethodHandle;
					}
					if (!actorData.IsDead())
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
				switch (6)
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
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterCones.OnAbilityAnimationRequestProcessed(ActorData)).MethodHandle;
				}
				if (!actorData.IsDead())
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
					Animator modelAnimator = actorData.GetModelAnimator();
					modelAnimator.SetInteger("Attack", 0);
					modelAnimator.SetBool("CinematicCam", false);
				}
			}
		}
	}
}
