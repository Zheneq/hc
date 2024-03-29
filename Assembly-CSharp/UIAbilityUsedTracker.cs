using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilityUsedTracker : MonoBehaviour
{
	public const int kMaxItems = 8;

	public GridLayoutGroup m_gridlayout;

	public UIAbilityUsed m_abilityPrefab;

	public List<UIAbilityUsed> m_abilities;

	public void AddNewAbility(Ability newAbility, ActorData theOwner)
	{
		if (newAbility == null || theOwner == null)
		{
			return;
		}
		while (true)
		{
			if (!theOwner.IsActorVisibleToClient())
			{
				return;
			}
			UIAbilityUsed uIAbilityUsed = Object.Instantiate(m_abilityPrefab);
			uIAbilityUsed.transform.SetParent(m_gridlayout.transform);
			uIAbilityUsed.transform.localPosition = Vector3.zero;
			uIAbilityUsed.transform.localScale = Vector3.one;
			uIAbilityUsed.Setup(newAbility, theOwner);
			if (m_abilities == null)
			{
				m_abilities = new List<UIAbilityUsed>();
			}
			m_abilities.Add(uIAbilityUsed);
			while (m_abilities.Count > 8)
			{
				Object.Destroy(m_abilities[0].gameObject);
				m_abilities.RemoveAt(0);
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void ClearAllAbilties(UIQueueListPanel.UIPhase phaseToClear = UIQueueListPanel.UIPhase.None)
	{
		if (m_abilities.Count == 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (phaseToClear != UIQueueListPanel.UIPhase.None)
		{
			for (int i = 0; i < m_abilities.Count; i++)
			{
				if (phaseToClear == UIQueueListPanel.UIPhase.None)
				{
					continue;
				}
				if (phaseToClear == UIQueueListPanel.GetUIPhaseFromAbilityPriority(m_abilities[i].GetAbilityRef().RunPriority))
				{
					GameObject gameObject = m_abilities[i].gameObject;
					m_abilities.RemoveAt(i);
					Object.Destroy(gameObject);
					i--;
				}
			}
		}
		else
		{
			using (List<UIAbilityUsed>.Enumerator enumerator = m_abilities.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIAbilityUsed current = enumerator.Current;
					Object.Destroy(current.gameObject);
				}
			}
			m_abilities.Clear();
		}
	}
}
