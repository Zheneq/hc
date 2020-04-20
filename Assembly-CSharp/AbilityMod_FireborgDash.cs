using System;
using System.Collections.Generic;

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
		targetSelect.SetTargetSelectMod(this.m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FireborgDash fireborgDash = targetAbility as FireborgDash;
		if (fireborgDash != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, this.m_groundFireDurationMod, "GroundFireDuration", string.Empty, fireborgDash.m_groundFireDuration, true, false);
			AbilityMod.AddToken(tokens, this.m_groundFireDurationIfSuperheatedMod, "GroundFireDurationIfSuperheated", string.Empty, fireborgDash.m_groundFireDurationIfSuperheated, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldPerEnemyHitMod, "ShieldPerEnemyHit", string.Empty, fireborgDash.m_shieldPerEnemyHit, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldDurationMod, "ShieldDuration", string.Empty, fireborgDash.m_shieldDuration, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrPerTurnIfLowHealthMod, "CdrPerTurnIfLowHealth", string.Empty, fireborgDash.m_cdrPerTurnIfLowHealth, true, false);
			AbilityMod.AddToken(tokens, this.m_lowHealthThreshMod, "LowHealthThresh", string.Empty, fireborgDash.m_lowHealthThresh, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FireborgDash fireborgDash = base.GetTargetAbilityOnAbilityData(abilityData) as FireborgDash;
		bool flag = fireborgDash != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (fireborgDash != null)
		{
			text += base.GetTargetSelectModDesc(this.m_targetSelectMod, fireborgDash.m_targetSelectComp, "-- Target Select --");
			string str = text;
			AbilityModPropertyBool addGroundFireMod = this.m_addGroundFireMod;
			string prefix = "[AddGroundFire]";
			bool showBaseVal = flag;
			bool baseVal;
			if (flag)
			{
				baseVal = fireborgDash.m_addGroundFire;
			}
			else
			{
				baseVal = false;
			}
			text = str + base.PropDesc(addGroundFireMod, prefix, showBaseVal, baseVal);
			text += base.PropDesc(this.m_groundFireDurationMod, "[GroundFireDuration]", flag, (!flag) ? 0 : fireborgDash.m_groundFireDuration);
			string str2 = text;
			AbilityModPropertyInt groundFireDurationIfSuperheatedMod = this.m_groundFireDurationIfSuperheatedMod;
			string prefix2 = "[GroundFireDurationIfSuperheated]";
			bool showBaseVal2 = flag;
			int baseVal2;
			if (flag)
			{
				baseVal2 = fireborgDash.m_groundFireDurationIfSuperheated;
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + base.PropDesc(groundFireDurationIfSuperheatedMod, prefix2, showBaseVal2, baseVal2);
			string str3 = text;
			AbilityModPropertyBool igniteIfNormalMod = this.m_igniteIfNormalMod;
			string prefix3 = "[IgniteIfNormal]";
			bool showBaseVal3 = flag;
			bool baseVal3;
			if (flag)
			{
				baseVal3 = fireborgDash.m_igniteIfNormal;
			}
			else
			{
				baseVal3 = false;
			}
			text = str3 + base.PropDesc(igniteIfNormalMod, prefix3, showBaseVal3, baseVal3);
			text += base.PropDesc(this.m_igniteIfSuperheatedMod, "[IgniteIfSuperheated]", flag, flag && fireborgDash.m_igniteIfSuperheated);
			string str4 = text;
			AbilityModPropertyInt shieldPerEnemyHitMod = this.m_shieldPerEnemyHitMod;
			string prefix4 = "[ShieldPerEnemyHit]";
			bool showBaseVal4 = flag;
			int baseVal4;
			if (flag)
			{
				baseVal4 = fireborgDash.m_shieldPerEnemyHit;
			}
			else
			{
				baseVal4 = 0;
			}
			text = str4 + base.PropDesc(shieldPerEnemyHitMod, prefix4, showBaseVal4, baseVal4);
			string str5 = text;
			AbilityModPropertyInt shieldDurationMod = this.m_shieldDurationMod;
			string prefix5 = "[ShieldDuration]";
			bool showBaseVal5 = flag;
			int baseVal5;
			if (flag)
			{
				baseVal5 = fireborgDash.m_shieldDuration;
			}
			else
			{
				baseVal5 = 0;
			}
			text = str5 + base.PropDesc(shieldDurationMod, prefix5, showBaseVal5, baseVal5);
			text += base.PropDesc(this.m_cdrPerTurnIfLowHealthMod, "[CdrPerTurnIfLowHealth]", flag, (!flag) ? 0 : fireborgDash.m_cdrPerTurnIfLowHealth);
			string str6 = text;
			AbilityModPropertyInt lowHealthThreshMod = this.m_lowHealthThreshMod;
			string prefix6 = "[LowHealthThresh]";
			bool showBaseVal6 = flag;
			int baseVal6;
			if (flag)
			{
				baseVal6 = fireborgDash.m_lowHealthThresh;
			}
			else
			{
				baseVal6 = 0;
			}
			text = str6 + base.PropDesc(lowHealthThreshMod, prefix6, showBaseVal6, baseVal6);
		}
		return text;
	}
}
