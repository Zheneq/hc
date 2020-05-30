using System.Collections.Generic;
using UnityEngine;

public class RampartBarricade_Prep : Ability
{
	[Header("-- Barrier Aiming")]
	public bool m_allowAimAtDiagonals;

	[Header("-- Knockback Hit Damage and Effect")]
	public int m_damageAmount = 10;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Laser and Knockback")]
	public float m_laserRange = 2f;

	public bool m_laserLengthIgnoreLos = true;

	public bool m_penetrateLos;

	public float m_knockbackDistance = 2f;

	public KnockbackType m_knockbackType;

	[Header("-- Sequences")]
	public GameObject m_removeShieldSequencePrefab;

	public GameObject m_applyShieldSequencePrefab;

	private bool m_snapToGrid = true;

	private Passive_Rampart m_passive;

	private AbilityMod_RampartBarricade_Prep m_abilityMod;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Barricade";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_passive == null)
		{
			m_passive = (GetComponent<PassiveData>().GetPassiveOfType(typeof(Passive_Rampart)) as Passive_Rampart);
		}
		if (m_passive != null)
		{
			Passive_Rampart passive = m_passive;
			object cachedShieldBarrierData;
			if (m_abilityMod != null)
			{
				cachedShieldBarrierData = m_abilityMod.m_shieldBarrierDataMod;
			}
			else
			{
				cachedShieldBarrierData = null;
			}
			passive.SetCachedShieldBarrierData((AbilityModPropertyBarrierDataV2)cachedShieldBarrierData);
		}
		else
		{
			Log.Error(GetDebugIdentifier(string.Empty) + " did not find passive component Passive_Rampart");
		}
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_RampartKnockbackBarrier(this, GetLaserWidth(), GetLaserRange(), LaserLengthIgnoreLos(), GetKnockbackDistance(), m_knockbackType, PenetrateLos(), m_snapToGrid, AllowAimAtDiagonals());
		base.Targeter.SetAffectedGroups(true, false, AffectCaster());
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
	}

	public bool AllowAimAtDiagonals()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_allowAimAtDiagonalsMod.GetModifiedValue(m_allowAimAtDiagonals);
		}
		else
		{
			result = m_allowAimAtDiagonals;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		return (!m_abilityMod) ? m_damageAmount : m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
		{
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	public float GetLaserRange()
	{
		return (!m_abilityMod) ? m_laserRange : m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange);
	}

	public bool LaserLengthIgnoreLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserLengthIgnoreLosMod.GetModifiedValue(m_laserLengthIgnoreLos);
		}
		else
		{
			result = m_laserLengthIgnoreLos;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		return (!m_abilityMod) ? m_penetrateLos : m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos);
	}

	public float GetKnockbackDistance()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance);
		}
		else
		{
			result = m_knockbackDistance;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		return m_passive.GetShieldBarrierData().m_width;
	}

	public KnockbackType GetKnockbackType()
	{
		return m_knockbackType;
	}

	public bool AffectCaster()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = (m_abilityMod.m_effectToSelfOnCast.m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
		m_enemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AppendTooltipNumbersFromBaseModEffects(ref numbers);
		return numbers;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RampartBarricade_Prep))
		{
			m_abilityMod = (abilityMod as AbilityMod_RampartBarricade_Prep);
		}
		Setup();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartBarricade_Prep abilityMod_RampartBarricade_Prep = modAsBase as AbilityMod_RampartBarricade_Prep;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_RampartBarricade_Prep)
		{
			val = abilityMod_RampartBarricade_Prep.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			val = m_damageAmount;
		}
		AddTokenInt(tokens, "DamageAmount", empty, val);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_RampartBarricade_Prep)
		{
			effectInfo = abilityMod_RampartBarricade_Prep.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			effectInfo = m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", m_enemyHitEffect);
		Passive_Rampart component = GetComponent<Passive_Rampart>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			if (component.m_normalShieldBarrierData != null)
			{
				while (true)
				{
					component.m_normalShieldBarrierData.AddTooltipTokens(tokens, "ShieldBarrier");
					return;
				}
			}
			return;
		}
	}

	public override Vector3 GetRotateToTargetPos(List<AbilityTarget> targets, ActorData caster)
	{
		GetBarrierPositionAndFacing(targets, out Vector3 position, out Vector3 facing);
		return position + facing;
	}

	public void GetBarrierPositionAndFacing(List<AbilityTarget> targets, out Vector3 position, out Vector3 facing)
	{
		facing = targets[0].AimDirection;
		position = targets[0].FreePos;
		if (m_snapToGrid)
		{
			BoardSquare boardSquareSafe = Board.Get().GetSquare(targets[0].GridPos);
			if (boardSquareSafe != null)
			{
				facing = VectorUtils.GetDirectionAndOffsetToClosestSide(boardSquareSafe, targets[0].FreePos, AllowAimAtDiagonals(), out Vector3 offset);
				position = boardSquareSafe.ToVector3() + offset;
			}
		}
	}
}
