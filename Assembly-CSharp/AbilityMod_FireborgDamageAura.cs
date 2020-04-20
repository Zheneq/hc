using System;
using System.Collections.Generic;

public class AbilityMod_FireborgDamageAura : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_Shape m_targetSelectMod;

	[Separator("Damage Aura", true)]
	public AbilityModPropertyBool m_excludeTargetedActorMod;

	public AbilityModPropertyInt m_auraDurationMod;

	public AbilityModPropertyInt m_auraDurationIfSuperheatedMod;

	public AbilityModPropertyBool m_igniteIfNormalMod;

	public AbilityModPropertyBool m_igniteIfSuperheatedMod;

	[Separator("Effect on Cast Target", true)]
	public AbilityModPropertyEffectInfo m_onCastTargetAllyEffectMod;

	[Separator("Cooldown reduction", true)]
	public AbilityModPropertyInt m_cdrOnUltCastMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FireborgDamageAura);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(this.m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FireborgDamageAura fireborgDamageAura = targetAbility as FireborgDamageAura;
		if (fireborgDamageAura != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, this.m_auraDurationMod, "AuraDuration", string.Empty, fireborgDamageAura.m_auraDuration, true, false);
			AbilityMod.AddToken(tokens, this.m_auraDurationIfSuperheatedMod, "AuraDurationIfSuperheated", string.Empty, fireborgDamageAura.m_auraDurationIfSuperheated, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_onCastTargetAllyEffectMod, "OnCastTargetAllyEffect", fireborgDamageAura.m_onCastTargetAllyEffect, true);
			AbilityMod.AddToken(tokens, this.m_cdrOnUltCastMod, "CdrOnUltCast", string.Empty, fireborgDamageAura.m_cdrOnUltCast, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FireborgDamageAura fireborgDamageAura = base.GetTargetAbilityOnAbilityData(abilityData) as FireborgDamageAura;
		bool flag = fireborgDamageAura != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (fireborgDamageAura != null)
		{
			text += base.GetTargetSelectModDesc(this.m_targetSelectMod, fireborgDamageAura.m_targetSelectComp, "-- Target Select --");
			string str = text;
			AbilityModPropertyBool excludeTargetedActorMod = this.m_excludeTargetedActorMod;
			string prefix = "[ExcludeTargetedActor]";
			bool showBaseVal = flag;
			bool baseVal;
			if (flag)
			{
				baseVal = fireborgDamageAura.m_excludeTargetedActor;
			}
			else
			{
				baseVal = false;
			}
			text = str + base.PropDesc(excludeTargetedActorMod, prefix, showBaseVal, baseVal);
			string str2 = text;
			AbilityModPropertyInt auraDurationMod = this.m_auraDurationMod;
			string prefix2 = "[AuraDuration]";
			bool showBaseVal2 = flag;
			int baseVal2;
			if (flag)
			{
				baseVal2 = fireborgDamageAura.m_auraDuration;
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + base.PropDesc(auraDurationMod, prefix2, showBaseVal2, baseVal2);
			string str3 = text;
			AbilityModPropertyInt auraDurationIfSuperheatedMod = this.m_auraDurationIfSuperheatedMod;
			string prefix3 = "[AuraDurationIfSuperheated]";
			bool showBaseVal3 = flag;
			int baseVal3;
			if (flag)
			{
				baseVal3 = fireborgDamageAura.m_auraDurationIfSuperheated;
			}
			else
			{
				baseVal3 = 0;
			}
			text = str3 + base.PropDesc(auraDurationIfSuperheatedMod, prefix3, showBaseVal3, baseVal3);
			string str4 = text;
			AbilityModPropertyBool igniteIfNormalMod = this.m_igniteIfNormalMod;
			string prefix4 = "[IgniteIfNormal]";
			bool showBaseVal4 = flag;
			bool baseVal4;
			if (flag)
			{
				baseVal4 = fireborgDamageAura.m_igniteIfNormal;
			}
			else
			{
				baseVal4 = false;
			}
			text = str4 + base.PropDesc(igniteIfNormalMod, prefix4, showBaseVal4, baseVal4);
			string str5 = text;
			AbilityModPropertyBool igniteIfSuperheatedMod = this.m_igniteIfSuperheatedMod;
			string prefix5 = "[IgniteIfSuperheated]";
			bool showBaseVal5 = flag;
			bool baseVal5;
			if (flag)
			{
				baseVal5 = fireborgDamageAura.m_igniteIfSuperheated;
			}
			else
			{
				baseVal5 = false;
			}
			text = str5 + base.PropDesc(igniteIfSuperheatedMod, prefix5, showBaseVal5, baseVal5);
			string str6 = text;
			AbilityModPropertyEffectInfo onCastTargetAllyEffectMod = this.m_onCastTargetAllyEffectMod;
			string prefix6 = "[OnCastTargetAllyEffect]";
			bool showBaseVal6 = flag;
			StandardEffectInfo baseVal6;
			if (flag)
			{
				baseVal6 = fireborgDamageAura.m_onCastTargetAllyEffect;
			}
			else
			{
				baseVal6 = null;
			}
			text = str6 + base.PropDesc(onCastTargetAllyEffectMod, prefix6, showBaseVal6, baseVal6);
			text += base.PropDesc(this.m_cdrOnUltCastMod, "[CdrOnUltCast]", flag, (!flag) ? 0 : fireborgDamageAura.m_cdrOnUltCast);
		}
		return text;
	}
}
