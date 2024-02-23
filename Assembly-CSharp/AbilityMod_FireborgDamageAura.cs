using System;
using System.Collections.Generic;
using System.Text;

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
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FireborgDamageAura fireborgDamageAura = targetAbility as FireborgDamageAura;
		if (!(fireborgDamageAura != null))
		{
			return;
		}
		while (true)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, m_auraDurationMod, "AuraDuration", string.Empty, fireborgDamageAura.m_auraDuration);
			AbilityMod.AddToken(tokens, m_auraDurationIfSuperheatedMod, "AuraDurationIfSuperheated", string.Empty, fireborgDamageAura.m_auraDurationIfSuperheated);
			AbilityMod.AddToken_EffectMod(tokens, m_onCastTargetAllyEffectMod, "OnCastTargetAllyEffect", fireborgDamageAura.m_onCastTargetAllyEffect);
			AbilityMod.AddToken(tokens, m_cdrOnUltCastMod, "CdrOnUltCast", string.Empty, fireborgDamageAura.m_cdrOnUltCast);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FireborgDamageAura fireborgDamageAura = GetTargetAbilityOnAbilityData(abilityData) as FireborgDamageAura;
		bool flag = fireborgDamageAura != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (fireborgDamageAura != null)
		{
			text += GetTargetSelectModDesc(m_targetSelectMod, fireborgDamageAura.m_targetSelectComp, "-- Target Select --");
			string str = text;
			AbilityModPropertyBool excludeTargetedActorMod = m_excludeTargetedActorMod;
			int baseVal;
			if (flag)
			{
				baseVal = (fireborgDamageAura.m_excludeTargetedActor ? 1 : 0);
			}
			else
			{
				baseVal = 0;
			}

			text = new StringBuilder().Append(str).Append(PropDesc(excludeTargetedActorMod, "[ExcludeTargetedActor]", flag, (byte)baseVal != 0)).ToString();
			string str2 = text;
			AbilityModPropertyInt auraDurationMod = m_auraDurationMod;
			int baseVal2;
			if (flag)
			{
				baseVal2 = fireborgDamageAura.m_auraDuration;
			}
			else
			{
				baseVal2 = 0;
			}

			text = new StringBuilder().Append(str2).Append(PropDesc(auraDurationMod, "[AuraDuration]", flag, baseVal2)).ToString();
			string str3 = text;
			AbilityModPropertyInt auraDurationIfSuperheatedMod = m_auraDurationIfSuperheatedMod;
			int baseVal3;
			if (flag)
			{
				baseVal3 = fireborgDamageAura.m_auraDurationIfSuperheated;
			}
			else
			{
				baseVal3 = 0;
			}

			text = new StringBuilder().Append(str3).Append(PropDesc(auraDurationIfSuperheatedMod, "[AuraDurationIfSuperheated]", flag, baseVal3)).ToString();
			string str4 = text;
			AbilityModPropertyBool igniteIfNormalMod = m_igniteIfNormalMod;
			int baseVal4;
			if (flag)
			{
				baseVal4 = (fireborgDamageAura.m_igniteIfNormal ? 1 : 0);
			}
			else
			{
				baseVal4 = 0;
			}

			text = new StringBuilder().Append(str4).Append(PropDesc(igniteIfNormalMod, "[IgniteIfNormal]", flag, (byte)baseVal4 != 0)).ToString();
			string str5 = text;
			AbilityModPropertyBool igniteIfSuperheatedMod = m_igniteIfSuperheatedMod;
			int baseVal5;
			if (flag)
			{
				baseVal5 = (fireborgDamageAura.m_igniteIfSuperheated ? 1 : 0);
			}
			else
			{
				baseVal5 = 0;
			}

			text = new StringBuilder().Append(str5).Append(PropDesc(igniteIfSuperheatedMod, "[IgniteIfSuperheated]", flag, (byte)baseVal5 != 0)).ToString();
			string str6 = text;
			AbilityModPropertyEffectInfo onCastTargetAllyEffectMod = m_onCastTargetAllyEffectMod;
			object baseVal6;
			if (flag)
			{
				baseVal6 = fireborgDamageAura.m_onCastTargetAllyEffect;
			}
			else
			{
				baseVal6 = null;
			}

			text = new StringBuilder().Append(str6).Append(PropDesc(onCastTargetAllyEffectMod, "[OnCastTargetAllyEffect]", flag, (StandardEffectInfo)baseVal6)).ToString();
			text += PropDesc(m_cdrOnUltCastMod, "[CdrOnUltCast]", flag, flag ? fireborgDamageAura.m_cdrOnUltCast : 0);
		}
		return text;
	}
}
