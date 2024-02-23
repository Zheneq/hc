using System;
using System.Collections.Generic;
using System.Text;

public class AbilityMod_FireborgDash : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_ChargeAoE m_targetSelectMod;

	[Separator("Whether to add ground fire effect", true)]
	public AbilityModPropertyBool m_addGroundFireMod;

	public AbilityModPropertyInt m_groundFireDurationMod;

	public AbilityModPropertyInt m_groundFireDurationIfSuperheatedMod;

	public AbilityModPropertyBool m_igniteIfNormalMod;

	public AbilityModPropertyBool m_igniteIfSuperheatedMod;

	[Separator("Shield per Enemy Hit", true)]
	public AbilityModPropertyInt m_shieldPerEnemyHitMod;

	public AbilityModPropertyInt m_shieldDurationMod;

	[Separator("Cooldown Reduction", true)]
	public AbilityModPropertyInt m_cdrPerTurnIfLowHealthMod;

	public AbilityModPropertyInt m_lowHealthThreshMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FireborgDash);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FireborgDash fireborgDash = targetAbility as FireborgDash;
		if (!(fireborgDash != null))
		{
			return;
		}
		while (true)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, m_groundFireDurationMod, "GroundFireDuration", string.Empty, fireborgDash.m_groundFireDuration);
			AbilityMod.AddToken(tokens, m_groundFireDurationIfSuperheatedMod, "GroundFireDurationIfSuperheated", string.Empty, fireborgDash.m_groundFireDurationIfSuperheated);
			AbilityMod.AddToken(tokens, m_shieldPerEnemyHitMod, "ShieldPerEnemyHit", string.Empty, fireborgDash.m_shieldPerEnemyHit);
			AbilityMod.AddToken(tokens, m_shieldDurationMod, "ShieldDuration", string.Empty, fireborgDash.m_shieldDuration);
			AbilityMod.AddToken(tokens, m_cdrPerTurnIfLowHealthMod, "CdrPerTurnIfLowHealth", string.Empty, fireborgDash.m_cdrPerTurnIfLowHealth);
			AbilityMod.AddToken(tokens, m_lowHealthThreshMod, "LowHealthThresh", string.Empty, fireborgDash.m_lowHealthThresh);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FireborgDash fireborgDash = GetTargetAbilityOnAbilityData(abilityData) as FireborgDash;
		bool flag = fireborgDash != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (fireborgDash != null)
		{
			text += GetTargetSelectModDesc(m_targetSelectMod, fireborgDash.m_targetSelectComp, "-- Target Select --");
			string str = text;
			AbilityModPropertyBool addGroundFireMod = m_addGroundFireMod;
			int baseVal;
			if (flag)
			{
				baseVal = (fireborgDash.m_addGroundFire ? 1 : 0);
			}
			else
			{
				baseVal = 0;
			}

			text = new StringBuilder().Append(str).Append(PropDesc(addGroundFireMod, "[AddGroundFire]", flag, (byte)baseVal != 0)).ToString();
			text += PropDesc(m_groundFireDurationMod, "[GroundFireDuration]", flag, flag ? fireborgDash.m_groundFireDuration : 0);
			string str2 = text;
			AbilityModPropertyInt groundFireDurationIfSuperheatedMod = m_groundFireDurationIfSuperheatedMod;
			int baseVal2;
			if (flag)
			{
				baseVal2 = fireborgDash.m_groundFireDurationIfSuperheated;
			}
			else
			{
				baseVal2 = 0;
			}

			text = new StringBuilder().Append(str2).Append(PropDesc(groundFireDurationIfSuperheatedMod, "[GroundFireDurationIfSuperheated]", flag, baseVal2)).ToString();
			string str3 = text;
			AbilityModPropertyBool igniteIfNormalMod = m_igniteIfNormalMod;
			int baseVal3;
			if (flag)
			{
				baseVal3 = (fireborgDash.m_igniteIfNormal ? 1 : 0);
			}
			else
			{
				baseVal3 = 0;
			}

			text = new StringBuilder().Append(str3).Append(PropDesc(igniteIfNormalMod, "[IgniteIfNormal]", flag, (byte)baseVal3 != 0)).ToString();
			text += PropDesc(m_igniteIfSuperheatedMod, "[IgniteIfSuperheated]", flag, flag && fireborgDash.m_igniteIfSuperheated);
			string str4 = text;
			AbilityModPropertyInt shieldPerEnemyHitMod = m_shieldPerEnemyHitMod;
			int baseVal4;
			if (flag)
			{
				baseVal4 = fireborgDash.m_shieldPerEnemyHit;
			}
			else
			{
				baseVal4 = 0;
			}

			text = new StringBuilder().Append(str4).Append(PropDesc(shieldPerEnemyHitMod, "[ShieldPerEnemyHit]", flag, baseVal4)).ToString();
			string str5 = text;
			AbilityModPropertyInt shieldDurationMod = m_shieldDurationMod;
			int baseVal5;
			if (flag)
			{
				baseVal5 = fireborgDash.m_shieldDuration;
			}
			else
			{
				baseVal5 = 0;
			}

			text = new StringBuilder().Append(str5).Append(PropDesc(shieldDurationMod, "[ShieldDuration]", flag, baseVal5)).ToString();
			text += PropDesc(m_cdrPerTurnIfLowHealthMod, "[CdrPerTurnIfLowHealth]", flag, flag ? fireborgDash.m_cdrPerTurnIfLowHealth : 0);
			string str6 = text;
			AbilityModPropertyInt lowHealthThreshMod = m_lowHealthThreshMod;
			int baseVal6;
			if (flag)
			{
				baseVal6 = fireborgDash.m_lowHealthThresh;
			}
			else
			{
				baseVal6 = 0;
			}

			text = new StringBuilder().Append(str6).Append(PropDesc(lowHealthThreshMod, "[LowHealthThresh]", flag, baseVal6)).ToString();
		}
		return text;
	}
}
