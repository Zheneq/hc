using System;
using System.Collections.Generic;
using UnityEngine;

public class RageBeastKnockback : Ability
{
	public float m_laserWidth;

	public float m_laserDistance;

	public bool m_penetrateLineOfSight;

	public int m_maxTargets;

	public float m_knockbackDistanceMin;

	public float m_knockbackDistanceMax;

	public KnockbackType m_knockbackType;

	public int m_damageAmount;

	public StandardEffectInfo m_onHitEffect;

	public int m_damageToMoverOnCollision = 2;

	public int m_damageToOtherOnCollision;

	public int m_damageCollisionWithGeo = 2;

	public GameObject m_hitActorSequencePrefab;

	public GameObject m_hitGeoSequencePrefab;

	private AbilityMod_RageBeastKnockback m_abilityMod;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastKnockback.Start()).MethodHandle;
			}
			this.m_abilityName = "Upheaval";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_KnockbackLaser(this, this.ModdedLaserWidth(), this.ModdedLaserLength(), this.m_penetrateLineOfSight, this.ModdedMaxTargets(), this.ModdedKnockbackDistanceMin(), this.ModdedKnockbackDistanceMax(), this.m_knockbackType, false);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.ModdedLaserLength();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.m_damageAmount)
		};
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.ModdedOnHitDamage())
		};
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RageBeastKnockback abilityMod_RageBeastKnockback = modAsBase as AbilityMod_RageBeastKnockback;
		string name = "MaxTargets";
		string empty = string.Empty;
		int val;
		if (abilityMod_RageBeastKnockback)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastKnockback.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_RageBeastKnockback.m_maxTargetMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			val = this.m_maxTargets;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "DamageAmount";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_RageBeastKnockback)
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
			val2 = abilityMod_RageBeastKnockback.m_onHitDamageMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			val2 = this.m_damageAmount;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RageBeastKnockback))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastKnockback.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_RageBeastKnockback);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public int ModdedMaxTargets()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastKnockback.ModdedMaxTargets()).MethodHandle;
			}
			result = this.m_maxTargets;
		}
		else
		{
			result = this.m_abilityMod.m_maxTargetMod.GetModifiedValue(this.m_maxTargets);
		}
		return result;
	}

	public float ModdedLaserWidth()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_targeterWidthMod.GetModifiedValue(this.m_laserWidth) : this.m_laserWidth;
	}

	public float ModdedLaserLength()
	{
		float result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastKnockback.ModdedLaserLength()).MethodHandle;
			}
			result = this.m_laserDistance;
		}
		else
		{
			result = this.m_abilityMod.m_targeterLengthMod.GetModifiedValue(this.m_laserDistance);
		}
		return result;
	}

	public int ModdedOnHitDamage()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastKnockback.ModdedOnHitDamage()).MethodHandle;
			}
			result = this.m_damageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_onHitDamageMod.GetModifiedValue(this.m_damageAmount);
		}
		return result;
	}

	public float ModdedKnockbackDistanceMin()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_knockbackDistanceMinMod.GetModifiedValue(this.m_knockbackDistanceMin) : this.m_knockbackDistanceMin;
	}

	public float ModdedKnockbackDistanceMax()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_knockbackDistanceMaxMod.GetModifiedValue(this.m_knockbackDistanceMax) : this.m_knockbackDistanceMax;
	}

	private float GetKnockbackDist(AbilityTarget target, Vector3 casterPos, Vector3 knockbackStartPos)
	{
		Vector3 vector = target.FreePos - casterPos;
		Vector3 vector2 = knockbackStartPos - casterPos;
		vector.y = 0f;
		vector2.y = 0f;
		float num = (vector.magnitude - vector2.magnitude) / Board.SquareSizeStatic;
		float num2 = this.ModdedKnockbackDistanceMin();
		float num3 = this.ModdedKnockbackDistanceMax();
		if (num < num2)
		{
			return num2;
		}
		if (num > num3)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastKnockback.GetKnockbackDist(AbilityTarget, Vector3, Vector3)).MethodHandle;
			}
			return num3;
		}
		return num;
	}
}
