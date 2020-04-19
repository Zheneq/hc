using System;
using UnityEngine;

public class UICardBar : MonoBehaviour
{
	public UICardButton[] m_cardButtons;

	private AbilityData m_abilityData;

	public void Setup(AbilityData abilityData)
	{
		this.m_abilityData = abilityData;
		this.Rebuild();
	}

	public void Rebuild()
	{
		if (this.m_abilityData != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICardBar.Rebuild()).MethodHandle;
			}
			AbilityData.AbilityEntry[] abilityEntries = this.m_abilityData.abilityEntries;
			for (int i = 0; i < 3; i++)
			{
				AbilityData.ActionType actionType = i + AbilityData.ActionType.CARD_0;
				this.m_cardButtons[i].Setup(abilityEntries[(int)actionType], actionType, this.m_abilityData, i);
			}
		}
	}

	public void DoAbilityButtonClick(KeyPreference abilitySelectDown)
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.m_cardButtons[i].GetKeyPreference() == abilitySelectDown)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICardBar.DoAbilityButtonClick(KeyPreference)).MethodHandle;
				}
				this.m_cardButtons[i].OnCardButtonClick(null);
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
	}
}
