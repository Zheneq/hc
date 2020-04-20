using System;
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
		if (!(newAbility == null) && !(theOwner == null))
		{
			if (theOwner.IsVisibleToClient())
			{
				UIAbilityUsed uiabilityUsed = UnityEngine.Object.Instantiate<UIAbilityUsed>(this.m_abilityPrefab);
				uiabilityUsed.transform.SetParent(this.m_gridlayout.transform);
				uiabilityUsed.transform.localPosition = Vector3.zero;
				uiabilityUsed.transform.localScale = Vector3.one;
				uiabilityUsed.Setup(newAbility, theOwner);
				if (this.m_abilities == null)
				{
					this.m_abilities = new List<UIAbilityUsed>();
				}
				this.m_abilities.Add(uiabilityUsed);
				while (this.m_abilities.Count > 8)
				{
					UnityEngine.Object.Destroy(this.m_abilities[0].gameObject);
					this.m_abilities.RemoveAt(0);
				}
				return;
			}
		}
	}

	public void ClearAllAbilties(UIQueueListPanel.UIPhase phaseToClear = UIQueueListPanel.UIPhase.None)
	{
		if (this.m_abilities.Count == 0)
		{
			return;
		}
		if (phaseToClear != UIQueueListPanel.UIPhase.None)
		{
			for (int i = 0; i < this.m_abilities.Count; i++)
			{
				if (phaseToClear != UIQueueListPanel.UIPhase.None)
				{
					if (phaseToClear == UIQueueListPanel.GetUIPhaseFromAbilityPriority(this.m_abilities[i].GetAbilityRef().RunPriority))
					{
						GameObject gameObject = this.m_abilities[i].gameObject;
						this.m_abilities.RemoveAt(i);
						UnityEngine.Object.Destroy(gameObject);
						i--;
					}
				}
			}
		}
		else
		{
			using (List<UIAbilityUsed>.Enumerator enumerator = this.m_abilities.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIAbilityUsed uiabilityUsed = enumerator.Current;
					UnityEngine.Object.Destroy(uiabilityUsed.gameObject);
				}
			}
			this.m_abilities.Clear();
		}
	}
}
