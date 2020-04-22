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
		Color result = s_invalidColor;
		switch (category)
		{
		case CombatTextCategory.Invalid:
			result = s_invalidColor;
			break;
		case CombatTextCategory.Damage:
			result = s_damageColor;
			break;
		case CombatTextCategory.Healing:
			result = s_healingColor;
			break;
		case CombatTextCategory.TP_Damage:
			result = s_tpDamageColor;
			break;
		case CombatTextCategory.TP_Recovery:
			result = s_tpRecoveryColor;
			break;
		case CombatTextCategory.BuffGain:
			result = s_buffGainColor;
			break;
		case CombatTextCategory.BuffLoss:
			result = s_buffLossColor;
			break;
		case CombatTextCategory.DebuffGain:
			result = s_debuffGainColor;
			break;
		case CombatTextCategory.DebuffLoss:
			result = s_debuffLossColor;
			break;
		case CombatTextCategory.Other:
			result = s_otherColor;
			break;
		}
		return result;
	}

	internal void Add(string combatText, string logText, CombatTextCategory category, BuffIconToDisplay icon)
	{
		if (!NetworkClient.active)
		{
			Log.Warning("Client only function called with no client is active");
			return;
		}
		ActorData component = GetComponent<ActorData>();
		if (component.IsVisibleToClient())
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
			if (!ClientGameManager.Get().IsFastForward)
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
				HUD_UI.Get().m_mainScreenPanel.m_combatTextPanel.QueueCombatText(component, combatText, category, icon);
			}
		}
		if (!NetworkServer.active)
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
			MatchLogger.Get().Log(logText);
			return;
		}
	}
}
