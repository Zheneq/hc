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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityUsedTracker.AddNewAbility(Ability, ActorData)).MethodHandle;
			}
			if (theOwner.IsVisibleToClient())
			{
				UIAbilityUsed uiabilityUsed = UnityEngine.Object.Instantiate<UIAbilityUsed>(this.m_abilityPrefab);
				uiabilityUsed.transform.SetParent(this.m_gridlayout.transform);
				uiabilityUsed.transform.localPosition = Vector3.zero;
				uiabilityUsed.transform.localScale = Vector3.one;
				uiabilityUsed.Setup(newAbility, theOwner);
				if (this.m_abilities == null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_abilities = new List<UIAbilityUsed>();
				}
				this.m_abilities.Add(uiabilityUsed);
				while (this.m_abilities.Count > 8)
				{
					UnityEngine.Object.Destroy(this.m_abilities[0].gameObject);
					this.m_abilities.RemoveAt(0);
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				return;
			}
		}
	}

	public void ClearAllAbilties(UIQueueListPanel.UIPhase phaseToClear = UIQueueListPanel.UIPhase.None)
	{
		if (this.m_abilities.Count == 0)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityUsedTracker.ClearAllAbilties(UIQueueListPanel.UIPhase)).MethodHandle;
			}
			return;
		}
		if (phaseToClear != UIQueueListPanel.UIPhase.None)
		{
			for (int i = 0; i < this.m_abilities.Count; i++)
			{
				if (phaseToClear != UIQueueListPanel.UIPhase.None)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (phaseToClear == UIQueueListPanel.GetUIPhaseFromAbilityPriority(this.m_abilities[i].GetAbilityRef().RunPriority))
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
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_abilities.Clear();
		}
	}
}
