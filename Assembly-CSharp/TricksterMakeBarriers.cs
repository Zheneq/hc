using System;
using System.Collections.Generic;
using UnityEngine;

public class TricksterMakeBarriers : Ability
{
	[Header("-- Barrier Info")]
	public bool m_linkBarriers = true;

	public StandardBarrierData m_barrierData;

	[Header("-- Spoils Spawn on Ally Moved Through")]
	public SpoilsSpawnData m_spoilsSpawnOnEnemyMovedThrough;

	[Header("-- Spoils Spawn on Enemy Moved Through")]
	public SpoilsSpawnData m_spoilsSpawnOnAllyMovedThrough;

	[Header("-- Sequences -----------------------------")]
	public GameObject m_castSequencePrefab;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;

	private TricksterMakeBarriers_Damage m_chainAbility;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Hollusionary Wallogram";
		}
		this.m_sequencePrefab = this.m_castSequencePrefab;
		this.m_afterImageSyncComp = base.GetComponent<TricksterAfterImageNetworkBehaviour>();
		Ability[] chainAbilities = base.GetChainAbilities();
		if (chainAbilities.Length > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMakeBarriers.Start()).MethodHandle;
			}
			Ability ability = chainAbilities[0];
			if (ability != null && ability is TricksterMakeBarriers_Damage)
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
				this.m_chainAbility = (ability as TricksterMakeBarriers_Damage);
			}
		}
		base.Targeter = new AbilityUtil_Targeter_TricksterBarriers(this, this.m_afterImageSyncComp, this.GetRangeFromLine(), this.GetLineEndOffset(), this.GetRadiusAroundOrigin(), this.GetCapsulePenetrateLos(), true);
		base.ResetTooltipAndTargetingNumbers();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.m_chainAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMakeBarriers.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_chainAbility.m_damageAmount);
			this.m_chainAbility.m_enemyOnHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		}
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
		return validAfterImages.Count > 0;
	}

	public float GetRangeFromLine()
	{
		float result;
		if (this.m_chainAbility == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMakeBarriers.GetRangeFromLine()).MethodHandle;
			}
			result = 0f;
		}
		else
		{
			result = this.m_chainAbility.m_rangeFromLine;
		}
		return result;
	}

	public float GetLineEndOffset()
	{
		return (!(this.m_chainAbility == null)) ? this.m_chainAbility.m_lineEndOffset : 0f;
	}

	public float GetRadiusAroundOrigin()
	{
		float result;
		if (this.m_chainAbility == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMakeBarriers.GetRadiusAroundOrigin()).MethodHandle;
			}
			result = 0f;
		}
		else
		{
			result = this.m_chainAbility.m_radiusAroundOrigin;
		}
		return result;
	}

	public bool GetCapsulePenetrateLos()
	{
		bool result;
		if (this.m_chainAbility == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMakeBarriers.GetCapsulePenetrateLos()).MethodHandle;
			}
			result = false;
		}
		else
		{
			result = this.m_chainAbility.m_capsulePenetrateLos;
		}
		return result;
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
		for (int i = 0; i < validAfterImages.Count; i++)
		{
			Animator animator = validAfterImages[i].\u000E();
			animator.SetInteger("Attack", animationIndex);
			animator.SetBool("CinematicCam", cinecam);
			animator.SetTrigger("StartAttack");
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMakeBarriers.OnAbilityAnimationRequest(ActorData, int, bool, Vector3)).MethodHandle;
		}
	}

	public override void OnAbilityAnimationRequestProcessed(ActorData caster)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMakeBarriers.OnAbilityAnimationRequestProcessed(ActorData)).MethodHandle;
					}
					if (!actorData.\u000E())
					{
						Animator animator = actorData.\u000E();
						animator.SetInteger("Attack", 0);
						animator.SetBool("CinematicCam", false);
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
}
