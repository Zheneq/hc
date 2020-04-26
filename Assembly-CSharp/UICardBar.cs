using UnityEngine;

public class UICardBar : MonoBehaviour
{
	public UICardButton[] m_cardButtons;

	private AbilityData m_abilityData;

	public void Setup(AbilityData abilityData)
	{
		m_abilityData = abilityData;
		Rebuild();
	}

	public void Rebuild()
	{
		if (!(m_abilityData != null))
		{
			return;
		}
		while (true)
		{
			AbilityData.AbilityEntry[] abilityEntries = m_abilityData.abilityEntries;
			for (int i = 0; i < 3; i++)
			{
				AbilityData.ActionType actionType = (AbilityData.ActionType)(i + 7);
				m_cardButtons[i].Setup(abilityEntries[(int)actionType], actionType, m_abilityData, i);
			}
			return;
		}
	}

	public void DoAbilityButtonClick(KeyPreference abilitySelectDown)
	{
		for (int i = 0; i < 3; i++)
		{
			if (m_cardButtons[i].GetKeyPreference() == abilitySelectDown)
			{
				m_cardButtons[i].OnCardButtonClick(null);
			}
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}
}
