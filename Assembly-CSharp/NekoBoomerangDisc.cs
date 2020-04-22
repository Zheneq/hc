using System.Collections.Generic;
using UnityEngine;

public class NekoBoomerangDisc : Ability
{
	[Separator("Targeting", true)]
	public float m_laserLength = 6.5f;

	public float m_laserWidth = 1f;

	public float m_aoeRadiusAtEnd = 1f;

	public int m_maxTargets;

	[Header("-- Disc return end radius")]
	public float m_discReturnEndRadius;

	[Separator("Damage stuff", true)]
	public int m_directDamage = 25;

	public int m_returnTripDamage = 10;

	public bool m_returnTripIgnoreCover = true;

	[Header("-- Extra Damage")]
	public int m_extraDamageIfHitByReturnDisc;

	public int m_extraReturnDamageIfHitNoOne;

	[Separator("Shielding for target hit on throw (applied on start of next turn)", true)]
	public int m_shieldPerTargetHitOnThrow;

	public StandardActorEffectData m_shieldEffectData;

	[Header("Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_returnTripSequencePrefab;

	public GameObject m_persistentDiscSequencePrefab;

	private AbilityMod_NekoBoomerangDisc m_abilityMod;

	private Neko_SyncComponent m_syncComp;

	private StandardActorEffectData m_cachedShieldEffectData;

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
			m_abilityName = "Boomerang Disc";
		}
		Setup();
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Neko_SyncComponent>();
		SetCachedFields();
		AbilityUtil_Targeter_NekoDisc abilityUtil_Targeter_NekoDisc = new AbilityUtil_Targeter_NekoDisc(this, GetLaserWidth(), GetLaserLength(), GetAoeRadiusAtEnd(), false, GetMaxTargets(), false, true);
		abilityUtil_Targeter_NekoDisc.m_affectCasterDelegate = TargeterIncludeCaster;
		base.Targeter = abilityUtil_Targeter_NekoDisc;
	}

	private bool TargeterIncludeCaster(ActorData caster, List<ActorData> actorsSoFar)
	{
		int result;
		if (GetShieldPerTargetHitOnThrow() > 0)
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
			result = ((actorsSoFar.Count > 0) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedShieldEffectData;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (3)
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
			cachedShieldEffectData = m_abilityMod.m_shieldEffectDataMod.GetModifiedValue(m_shieldEffectData);
		}
		else
		{
			cachedShieldEffectData = m_shieldEffectData;
		}
		m_cachedShieldEffectData = cachedShieldEffectData;
	}

	public float GetLaserLength()
	{
		return (!m_abilityMod) ? m_laserLength : m_abilityMod.m_laserLengthMod.GetModifiedValue(m_laserLength);
	}

	public float GetLaserWidth()
	{
		float result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
		}
		else
		{
			result = m_laserWidth;
		}
		return result;
	}

	public float GetAoeRadiusAtEnd()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (3)
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
			result = m_abilityMod.m_aoeRadiusAtEndMod.GetModifiedValue(m_aoeRadiusAtEnd);
		}
		else
		{
			result = m_aoeRadiusAtEnd;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
		}
		else
		{
			result = m_maxTargets;
		}
		return result;
	}

	public float GetDiscReturnEndRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
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
			result = m_abilityMod.m_discReturnEndRadiusMod.GetModifiedValue(m_discReturnEndRadius);
		}
		else
		{
			result = m_discReturnEndRadius;
		}
		return result;
	}

	public int GetDirectDamage()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_directDamageMod.GetModifiedValue(m_directDamage);
		}
		else
		{
			result = m_directDamage;
		}
		return result;
	}

	public int GetReturnTripDamage()
	{
		return (!m_abilityMod) ? m_returnTripDamage : m_abilityMod.m_returnTripDamageMod.GetModifiedValue(m_returnTripDamage);
	}

	public bool ReturnTripIgnoreCover()
	{
		bool result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_returnTripIgnoreCoverMod.GetModifiedValue(m_returnTripIgnoreCover);
		}
		else
		{
			result = m_returnTripIgnoreCover;
		}
		return result;
	}

	public int GetExtraDamageIfHitByReturnDisc()
	{
		return (!m_abilityMod) ? m_extraDamageIfHitByReturnDisc : m_abilityMod.m_extraDamageIfHitByReturnDiscMod.GetModifiedValue(m_extraDamageIfHitByReturnDisc);
	}

	public int GetExtraReturnDamageIfHitNoOne()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_extraReturnDamageIfHitNoOneMod.GetModifiedValue(m_extraReturnDamageIfHitNoOne);
		}
		else
		{
			result = m_extraReturnDamageIfHitNoOne;
		}
		return result;
	}

	public int GetShieldPerTargetHitOnThrow()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (3)
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
			result = m_abilityMod.m_shieldPerTargetHitOnThrowMod.GetModifiedValue(m_shieldPerTargetHitOnThrow);
		}
		else
		{
			result = m_shieldPerTargetHitOnThrow;
		}
		return result;
	}

	public StandardActorEffectData GetShieldEffectData()
	{
		StandardActorEffectData result;
		if (m_cachedShieldEffectData != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedShieldEffectData;
		}
		else
		{
			result = m_shieldEffectData;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "DirectDamage", string.Empty, m_directDamage);
		AddTokenInt(tokens, "ReturnTripDamage", string.Empty, m_returnTripDamage);
		AddTokenInt(tokens, "ExtraDamageIfHitByReturnDisc", string.Empty, m_extraDamageIfHitByReturnDisc);
		AddTokenInt(tokens, "ExtraReturnDamageIfHitNoOne", string.Empty, m_extraReturnDamageIfHitNoOne);
		AddTokenInt(tokens, "ShieldPerTargetHitOnThrow", string.Empty, m_shieldPerTargetHitOnThrow);
		m_shieldEffectData.AddTooltipTokens(tokens, "ShieldEffectData");
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_directDamage);
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 1);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			int num = GetDirectDamage();
			if (m_syncComp != null)
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
				if (GetExtraDamageIfHitByReturnDisc() > 0)
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
					if (m_syncComp.IsActorTargetedByReturningDiscs(targetActor))
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
						num += GetExtraDamageIfHitByReturnDisc();
					}
				}
			}
			results.m_damage = num;
		}
		else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
		{
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			int num2 = results.m_absorb = GetShieldPerTargetHitOnThrow() * visibleActorsCountByTooltipSubject;
		}
		return true;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserLength() + GetAoeRadiusAtEnd();
	}

	public static BoardSquare GetDiscEndSquare(Vector3 startPos, Vector3 endPos)
	{
		Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(startPos, endPos);
		return KnockbackUtils.GetLastValidBoardSquareInLine(startPos, coneLosCheckPos, true, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_NekoBoomerangDisc))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_NekoBoomerangDisc);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	public override int GetTheatricsSortPriority(AbilityData.ActionType actionType)
	{
		return 3;
	}
}
