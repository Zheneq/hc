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
		if (m_abilityName == "Base Ability")
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "Upheaval";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_KnockbackLaser(this, ModdedLaserWidth(), ModdedLaserLength(), m_penetrateLineOfSight, ModdedMaxTargets(), ModdedKnockbackDistanceMin(), ModdedKnockbackDistanceMax(), m_knockbackType, false);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return ModdedLaserLength();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damageAmount));
		return list;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, ModdedOnHitDamage()));
		return list;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RageBeastKnockback abilityMod_RageBeastKnockback = modAsBase as AbilityMod_RageBeastKnockback;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_RageBeastKnockback)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			val = abilityMod_RageBeastKnockback.m_maxTargetMod.GetModifiedValue(m_maxTargets);
		}
		else
		{
			val = m_maxTargets;
		}
		AddTokenInt(tokens, "MaxTargets", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_RageBeastKnockback)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			val2 = abilityMod_RageBeastKnockback.m_onHitDamageMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			val2 = m_damageAmount;
		}
		AddTokenInt(tokens, "DamageAmount", empty2, val2);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RageBeastKnockback))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_abilityMod = (abilityMod as AbilityMod_RageBeastKnockback);
					SetupTargeter();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public int ModdedMaxTargets()
	{
		int result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_maxTargets;
		}
		else
		{
			result = m_abilityMod.m_maxTargetMod.GetModifiedValue(m_maxTargets);
		}
		return result;
	}

	public float ModdedLaserWidth()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_targeterWidthMod.GetModifiedValue(m_laserWidth) : m_laserWidth;
	}

	public float ModdedLaserLength()
	{
		float result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_laserDistance;
		}
		else
		{
			result = m_abilityMod.m_targeterLengthMod.GetModifiedValue(m_laserDistance);
		}
		return result;
	}

	public int ModdedOnHitDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_damageAmount;
		}
		else
		{
			result = m_abilityMod.m_onHitDamageMod.GetModifiedValue(m_damageAmount);
		}
		return result;
	}

	public float ModdedKnockbackDistanceMin()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_knockbackDistanceMinMod.GetModifiedValue(m_knockbackDistanceMin) : m_knockbackDistanceMin;
	}

	public float ModdedKnockbackDistanceMax()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_knockbackDistanceMaxMod.GetModifiedValue(m_knockbackDistanceMax) : m_knockbackDistanceMax;
	}

	private float GetKnockbackDist(AbilityTarget target, Vector3 casterPos, Vector3 knockbackStartPos)
	{
		Vector3 vector = target.FreePos - casterPos;
		Vector3 vector2 = knockbackStartPos - casterPos;
		vector.y = 0f;
		vector2.y = 0f;
		float num = (vector.magnitude - vector2.magnitude) / Board.SquareSizeStatic;
		float num2 = ModdedKnockbackDistanceMin();
		float num3 = ModdedKnockbackDistanceMax();
		if (num < num2)
		{
			return num2;
		}
		if (num > num3)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return num3;
				}
			}
		}
		return num;
	}
}
