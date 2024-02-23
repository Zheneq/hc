using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UINameplatePanel : MonoBehaviour
{
	public UINameplateItem m_nameplateItemPrefab;

	public Sprite[] m_buffIconSprites = new Sprite[13];

	private Dictionary<ActorData, UINameplateItem> m_nameplates = new Dictionary<ActorData, UINameplateItem>();

	private bool m_nameplateTextVisible = true;

	private bool m_nameplateCombatTextvisible = true;

	private List<int> m_sortedActorIndexNameplates = new List<int>();

	public Dictionary<ActorData, UINameplateItem> GetNameplates()
	{
		return m_nameplates;
	}

	public static void SetIndividualNameplateText(ActorData theActor, string textToDisplay)
	{
		Dictionary<ActorData, UINameplateItem> nameplates = HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.GetNameplates();
		if (nameplates.ContainsKey(theActor))
		{
			UINameplateItem uINameplateItem = nameplates[theActor];
			uINameplateItem.SetDebugText(textToDisplay);
		}
	}

	public void StartTargetingNumberFadeout(ActorData actorData)
	{
		if (m_nameplates.ContainsKey(actorData))
		{
			UINameplateItem uINameplateItem = m_nameplates[actorData];
			uINameplateItem.StartTargetingNumberFadeout();
		}
	}

	public void ShowTargetingNumberForConfirmedTargeting(ActorData actorData)
	{
		if (m_nameplates.ContainsKey(actorData))
		{
			UINameplateItem uINameplateItem = m_nameplates[actorData];
			uINameplateItem.ShowTargetingNumberForConfirmedTargeting();
		}
	}

	public void UpdateBriefcaseThreshold(ActorData actorData, float percent)
	{
		if (!m_nameplates.ContainsKey(actorData))
		{
			return;
		}
		while (true)
		{
			UINameplateItem uINameplateItem = m_nameplates[actorData];
			uINameplateItem.UpdateBriefcaseThreshold(percent);
			return;
		}
	}

	public void RefreshNameplates()
	{
		using (Dictionary<ActorData, UINameplateItem>.Enumerator enumerator = m_nameplates.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UINameplateItem value = enumerator.Current.Value;
				value.ForceFinishStatusAnims();
			}
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
	}

	public void SetTextVisible(bool visible)
	{
		if (m_nameplateTextVisible == visible)
		{
			return;
		}
		while (true)
		{
			m_nameplateTextVisible = visible;
			foreach (KeyValuePair<ActorData, UINameplateItem> nameplate in m_nameplates)
			{
				UINameplateItem value = nameplate.Value;
				value.SetTextVisible(visible);
			}
			return;
		}
	}

	public void ToggleCombatTextVisible()
	{
		m_nameplateCombatTextvisible = !m_nameplateCombatTextvisible;
	}

	public void SetCombatTextVisible(bool visible)
	{
		m_nameplateCombatTextvisible = visible;
	}

	public void PlayCombatText(ActorData actorData, string text, CombatTextCategory category, BuffIconToDisplay icon)
	{
		if (!m_nameplateCombatTextvisible || !m_nameplates.ContainsKey(actorData))
		{
			return;
		}
		while (true)
		{
			UINameplateItem uINameplateItem = m_nameplates[actorData];
			uINameplateItem.PlayCombatText(actorData, text, category, icon);
			return;
		}
	}

	public void SetDebugNameplateTextValues()
	{
		using (Dictionary<ActorData, UINameplateItem>.Enumerator enumerator = m_nameplates.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, UINameplateItem> current = enumerator.Current;
				ActorData key = current.Key;
				UINameplateItem value = current.Value;
				value.SetDebugText(new StringBuilder().Append("State: ").Append(key.GetActorTurnSM().CurrentState).ToString());
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
	}

	public void Update()
	{
		if (DebugParameters.Get() == null)
		{
			return;
		}
		while (true)
		{
			if (DebugParameters.Get().GetParameterAsBool("DebugNameplates"))
			{
				while (true)
				{
					SetDebugNameplateTextValues();
					return;
				}
			}
			return;
		}
	}

	public void AddActor(ActorData actorData)
	{
		UINameplateItem uINameplateItem = Object.Instantiate(m_nameplateItemPrefab);
		m_nameplates[actorData] = uINameplateItem;
		uINameplateItem.transform.SetParent(base.transform);
		uINameplateItem.Setup(actorData);
		uINameplateItem.transform.localPosition = Vector3.zero;
		uINameplateItem.transform.localScale = new Vector3(1f, 1f, 1f);
		if (uINameplateItem.transform as RectTransform != null)
		{
			(uINameplateItem.transform as RectTransform).anchoredPosition = new Vector2(10000f, 10000f);
		}
		UIManager.SetGameObjectActive(uINameplateItem.m_parentTransform, false);
		UIManager.SetGameObjectActive(uINameplateItem.m_parentTransform, true);
		CanvasLayerManager.Get().NotifyAddedNewNameplate();
	}

	public void NotifyFlagStatusChange(ActorData theActor, bool holdingFlag)
	{
		if (!(theActor != null))
		{
			return;
		}
		while (true)
		{
			if (m_nameplates.ContainsKey(theActor))
			{
				while (true)
				{
					UINameplateItem uINameplateItem = m_nameplates[theActor];
					uINameplateItem.NotifyFlagStatusChange(holdingFlag);
					return;
				}
			}
			return;
		}
	}

	public void NotifyStatusChange(ActorData theActor, StatusType status, bool gainedStatus)
	{
		if (!m_nameplates.ContainsKey(theActor))
		{
			return;
		}
		while (true)
		{
			UINameplateItem uINameplateItem = m_nameplates[theActor];
			if (gainedStatus)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						uINameplateItem.AddStatus(status);
						return;
					}
				}
			}
			uINameplateItem.RemoveStatus(status);
			return;
		}
	}

	public void SetCatalystPipsVisible(ActorData theActor, bool visible)
	{
		if (m_nameplates.ContainsKey(theActor))
		{
			UINameplateItem uINameplateItem = m_nameplates[theActor];
			uINameplateItem.SetCatalystsVisible(visible);
		}
	}

	public void UpdateCatalysts(ActorData theActor, List<Ability> cardAbilities)
	{
		if (!m_nameplates.ContainsKey(theActor))
		{
			return;
		}
		while (true)
		{
			UINameplateItem uINameplateItem = m_nameplates[theActor];
			uINameplateItem.UpdateCatalysts(cardAbilities);
			return;
		}
	}

	public void NotifyStatusDurationChange(ActorData theActor, StatusType status, int newDuration)
	{
		if (!m_nameplates.ContainsKey(theActor))
		{
			return;
		}
		while (true)
		{
			UINameplateItem uINameplateItem = m_nameplates[theActor];
			uINameplateItem.UpdateStatusDuration(status, newDuration);
			return;
		}
	}

	public void UpdateTargetingAbilityIndicator(ActorData targetingActor, Ability ability, AbilityData.ActionType action, int index)
	{
		if (!m_nameplates.ContainsKey(targetingActor))
		{
			return;
		}
		while (true)
		{
			UINameplateItem uINameplateItem = m_nameplates[targetingActor];
			uINameplateItem.UpdateTargetingAbilityIndicator(ability, action, index);
			return;
		}
	}

	public void TurnOffTargetingAbilityIndicator(ActorData targetingActor, int fromIndex)
	{
		if (m_nameplates.ContainsKey(targetingActor))
		{
			UINameplateItem uINameplateItem = m_nameplates[targetingActor];
			uINameplateItem.TurnOffTargetingAbilityIndicator(fromIndex);
		}
	}

	public void SpawnOverconForActor(ActorData actor, UIOverconData.NameToOverconEntry entry, bool skipValidation)
	{
		if (m_nameplates.ContainsKey(actor))
		{
			UINameplateItem uINameplateItem = m_nameplates[actor];
			uINameplateItem.SpawnOvercon(entry, skipValidation);
		}
	}

	public void UpdateSelfNameplate(ActorData theTargeted, Ability abilityTargeting, bool inCover, int currentTargeterIndex, bool inConfirm)
	{
		if (m_nameplates.ContainsKey(theTargeted))
		{
			UINameplateItem uINameplateItem = m_nameplates[theTargeted];
			uINameplateItem.UpdateSelfNameplate(abilityTargeting, inCover, currentTargeterIndex, inConfirm);
		}
	}

	public void UpdateNameplateTargeted(ActorData targetingActor, ActorData theTargeted, Ability abilityTargeting, bool inCover, int currentTargeterIndex, bool inConfirm)
	{
		if (m_nameplates.ContainsKey(theTargeted))
		{
			UINameplateItem uINameplateItem = m_nameplates[theTargeted];
			uINameplateItem.UpdateNameplateTargeted(targetingActor, abilityTargeting, inCover, currentTargeterIndex, inConfirm);
		}
	}

	public void UpdateNameplateUntargeted(ActorData theTargeted, bool doInstantHide = false)
	{
		if (!m_nameplates.ContainsKey(theTargeted))
		{
			return;
		}
		while (true)
		{
			UINameplateItem uINameplateItem = m_nameplates[theTargeted];
			uINameplateItem.UpdateNameplateUntargeted(doInstantHide);
			return;
		}
	}

	public void RemoveActor(ActorData actorData)
	{
		if (!m_nameplates.ContainsKey(actorData))
		{
			return;
		}
		while (true)
		{
			UINameplateItem uINameplateItem = m_nameplates[actorData];
			m_nameplates.Remove(actorData);
			if (uINameplateItem != null)
			{
				Object.Destroy(uINameplateItem.gameObject);
			}
			return;
		}
	}

	private void Clear()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			Object.Destroy(child.gameObject);
		}
		while (true)
		{
			m_nameplates.Clear();
			return;
		}
	}

	public void SortNameplates()
	{
		IOrderedEnumerable<KeyValuePair<ActorData, UINameplateItem>> orderedEnumerable = m_nameplates.OrderBy((KeyValuePair<ActorData, UINameplateItem> r) => r.Value.m_distanceFromCamera);
		bool flag = false;
		if (orderedEnumerable.Count() != m_sortedActorIndexNameplates.Count())
		{
			flag = true;
		}
		else
		{
			List<int>.Enumerator enumerator = m_sortedActorIndexNameplates.GetEnumerator();
			IEnumerator<KeyValuePair<ActorData, UINameplateItem>> enumerator2 = orderedEnumerable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator2.MoveNext())
				{
					int current = enumerator.Current;
					int actorIndex = enumerator2.Current.Key.ActorIndex;
					if (current != actorIndex)
					{
						flag = true;
						break;
					}
					continue;
				}
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		while (true)
		{
			m_sortedActorIndexNameplates.Clear();
			IEnumerator<KeyValuePair<ActorData, UINameplateItem>> enumerator3 = orderedEnumerable.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					KeyValuePair<ActorData, UINameplateItem> current2 = enumerator3.Current;
					int sortOrder = current2.Value.GetSortOrder();
					int num = m_nameplates.Count - m_sortedActorIndexNameplates.Count;
					if (sortOrder != num)
					{
						current2.Value.SetSortOrder(num);
					}
					m_sortedActorIndexNameplates.Add(current2.Key.ActorIndex);
				}
			}
			finally
			{
				if (enumerator3 != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							enumerator3.Dispose();
							goto end_IL_0163;
						}
					}
				}
				end_IL_0163:;
			}
			CanvasLayerManager.Get().UpdateNameplateOrder();
			return;
		}
	}
}
