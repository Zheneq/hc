using System.Collections.Generic;
using UnityEngine;

public class TricksterFlare : Ability
{
	[Header("-- Targeting ")]
	public AbilityAreaShape m_flareShape = AbilityAreaShape.Three_x_Three;

	public bool m_flarePenetrateLos;

	public bool m_flareAroundSelf = true;

	[Header("-- Enemy Hit")]
	public bool m_includeEnemies = true;

	public int m_flareDamageAmount = 3;

	public int m_flareSubsequentDamageAmount = 2;

	public StandardEffectInfo m_enemyHitEffect;

	[Space(10f)]
	public bool m_useEnemyMultiHitEffect;

	public StandardEffectInfo m_enemyMultipleHitEffect;

	[Header("-- Ally Hit")]
	public bool m_includeAllies;

	public int m_flareHealAmount;

	public int m_flareSubsequentHealAmount;

	public StandardEffectInfo m_allyHitEffect;

	[Space(10f)]
	public bool m_useAllyMultiHitEffect;

	public StandardEffectInfo m_allyMultipleHitEffect;

	[Header("-- Self Hit")]
	public StandardEffectInfo m_selfHitEffectForMultiHit;

	[Header("-- Spoil spawn info")]
	public bool m_spawnSpoilForEnemyHit = true;

	public bool m_spawnSpoilForAllyHit;

	public SpoilsSpawnData m_spoilSpawnInfo;

	public bool m_onlySpawnSpoilOnMultiHit = true;

	[Header("-- Sequences ----------------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "Flare";
		}
		m_afterImageSyncComp = GetComponent<TricksterAfterImageNetworkBehaviour>();
		m_sequencePrefab = m_castSequencePrefab;
		base.Targeter = new AbilityUtil_Targeter_TricksterFlare(this, m_afterImageSyncComp, m_flareShape, m_flarePenetrateLos, m_includeEnemies, m_includeAllies, m_flareAroundSelf);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_flareDamageAmount);
		m_enemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Secondary, m_flareHealAmount);
		m_allyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		if (m_flareSubsequentDamageAmount > 0)
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
			if (m_flareSubsequentDamageAmount != m_flareDamageAmount)
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
				AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Tertiary, m_flareSubsequentDamageAmount);
			}
		}
		if (m_flareSubsequentHealAmount > 0)
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
			if (m_flareSubsequentHealAmount != m_flareHealAmount)
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
				AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Quaternary, m_flareSubsequentHealAmount);
			}
		}
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (!m_flareAroundSelf)
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
			if (m_afterImageSyncComp != null)
			{
				return m_afterImageSyncComp.HasVaidAfterImages();
			}
		}
		return true;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		if (m_includeEnemies)
		{
			Ability.AddNameplateValueForOverlap(ref symbolToValue, base.Targeter, targetActor, currentTargeterIndex, m_flareDamageAmount, m_flareSubsequentDamageAmount);
		}
		if (m_includeAllies)
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
			Ability.AddNameplateValueForOverlap(ref symbolToValue, base.Targeter, targetActor, currentTargeterIndex, m_flareHealAmount, m_flareSubsequentHealAmount, AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Secondary);
		}
		return symbolToValue;
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		foreach (ActorData item in validAfterImages)
		{
			if (item != null)
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
				if (!item.IsDead())
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
					m_afterImageSyncComp.TurnToPosition(item, targetPos);
					Animator modelAnimator = item.GetModelAnimator();
					modelAnimator.SetInteger("Attack", animationIndex);
					modelAnimator.SetBool("CinematicCam", cinecam);
					modelAnimator.SetTrigger("StartAttack");
				}
			}
		}
	}

	public override void OnAbilityAnimationRequestProcessed(ActorData caster)
	{
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		foreach (ActorData item in validAfterImages)
		{
			if (item != null && !item.IsDead())
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
				Animator modelAnimator = item.GetModelAnimator();
				modelAnimator.SetInteger("Attack", 0);
				modelAnimator.SetBool("CinematicCam", false);
			}
		}
	}
}
