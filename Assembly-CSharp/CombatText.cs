﻿using System;
using UnityEngine;
using UnityEngine.Networking;

public class CombatText : MonoBehaviour
{
	private static Color s_invalidColor = new Color(0.5f, 0.5f, 0.5f, 1f);

	private static Color s_damageColor = new Color(1f, 1f, 1f, 1f);

	private static Color s_healingColor = new Color(0f, 1f, 0.5f, 1f);

	private static Color s_tpDamageColor = new Color(0.5f, 0f, 0.5f, 1f);

	private static Color s_tpRecoveryColor = new Color(1f, 0f, 1f, 1f);

	private static Color s_buffGainColor = new Color(1f, 0.5f, 0.1f, 1f);

	private static Color s_buffLossColor = new Color(0.1f, 0.5f, 1f, 1f);

	private static Color s_debuffGainColor = new Color(1f, 0f, 0f, 1f);

	private static Color s_debuffLossColor = new Color(0.2f, 0f, 1f, 1f);

	private static Color s_otherColor = new Color(0.8f, 0.8f, 0.8f, 1f);

	public static Color GetColorOfCategory(CombatTextCategory category)
	{
		Color result = CombatText.s_invalidColor;
		switch (category)
		{
		case CombatTextCategory.Invalid:
			result = CombatText.s_invalidColor;
			break;
		case CombatTextCategory.Damage:
			result = CombatText.s_damageColor;
			break;
		case CombatTextCategory.Healing:
			result = CombatText.s_healingColor;
			break;
		case CombatTextCategory.TP_Damage:
			result = CombatText.s_tpDamageColor;
			break;
		case CombatTextCategory.TP_Recovery:
			result = CombatText.s_tpRecoveryColor;
			break;
		case CombatTextCategory.BuffGain:
			result = CombatText.s_buffGainColor;
			break;
		case CombatTextCategory.BuffLoss:
			result = CombatText.s_buffLossColor;
			break;
		case CombatTextCategory.DebuffGain:
			result = CombatText.s_debuffGainColor;
			break;
		case CombatTextCategory.DebuffLoss:
			result = CombatText.s_debuffLossColor;
			break;
		case CombatTextCategory.Other:
			result = CombatText.s_otherColor;
			break;
		}
		return result;
	}

	internal void Add(string combatText, string logText, CombatTextCategory category, BuffIconToDisplay icon)
	{
		if (!NetworkClient.active)
		{
			Log.Warning("Client only function called with no client is active", new object[0]);
			return;
		}
		ActorData component = base.GetComponent<ActorData>();
		if (component.IsVisibleToClient())
		{
			if (!ClientGameManager.Get().IsFastForward)
			{
				HUD_UI.Get().m_mainScreenPanel.m_combatTextPanel.QueueCombatText(component, combatText, category, icon);
			}
		}
		if (NetworkServer.active)
		{
			MatchLogger.Get().Log(logText);
		}
	}
}
