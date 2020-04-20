using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UINameplatePanel : MonoBehaviour
{
	public UINameplateItem m_nameplateItemPrefab;

	public Sprite[] m_buffIconSprites = new Sprite[0xD];

	private Dictionary<ActorData, UINameplateItem> m_nameplates = new Dictionary<ActorData, UINameplateItem>();

	private bool m_nameplateTextVisible = true;

	private bool m_nameplateCombatTextvisible = true;

	private List<int> m_sortedActorIndexNameplates = new List<int>();

	public Dictionary<ActorData, UINameplateItem> GetNameplates()
	{
		return this.m_nameplates;
	}

	public static void SetIndividualNameplateText(ActorData theActor, string textToDisplay)
	{
		Dictionary<ActorData, UINameplateItem> nameplates = HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.GetNameplates();
		if (nameplates.ContainsKey(theActor))
		{
			UINameplateItem uinameplateItem = nameplates[theActor];
			uinameplateItem.SetDebugText(textToDisplay);
		}
	}

	public void StartTargetingNumberFadeout(ActorData actorData)
	{
		if (this.m_nameplates.ContainsKey(actorData))
		{
			UINameplateItem uinameplateItem = this.m_nameplates[actorData];
			uinameplateItem.StartTargetingNumberFadeout();
		}
	}

	public void ShowTargetingNumberForConfirmedTargeting(ActorData actorData)
	{
		if (this.m_nameplates.ContainsKey(actorData))
		{
			UINameplateItem uinameplateItem = this.m_nameplates[actorData];
			uinameplateItem.ShowTargetingNumberForConfirmedTargeting();
		}
	}

	public void UpdateBriefcaseThreshold(ActorData actorData, float percent)
	{
		if (this.m_nameplates.ContainsKey(actorData))
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.UpdateBriefcaseThreshold(ActorData, float)).MethodHandle;
			}
			UINameplateItem uinameplateItem = this.m_nameplates[actorData];
			uinameplateItem.UpdateBriefcaseThreshold(percent);
		}
	}

	public void RefreshNameplates()
	{
		using (Dictionary<ActorData, UINameplateItem>.Enumerator enumerator = this.m_nameplates.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, UINameplateItem> keyValuePair = enumerator.Current;
				UINameplateItem value = keyValuePair.Value;
				value.ForceFinishStatusAnims();
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.RefreshNameplates()).MethodHandle;
			}
		}
	}

	public void SetTextVisible(bool visible)
	{
		if (this.m_nameplateTextVisible != visible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.SetTextVisible(bool)).MethodHandle;
			}
			this.m_nameplateTextVisible = visible;
			foreach (KeyValuePair<ActorData, UINameplateItem> keyValuePair in this.m_nameplates)
			{
				UINameplateItem value = keyValuePair.Value;
				value.SetTextVisible(visible);
			}
		}
	}

	public void ToggleCombatTextVisible()
	{
		this.m_nameplateCombatTextvisible = !this.m_nameplateCombatTextvisible;
	}

	public void SetCombatTextVisible(bool visible)
	{
		this.m_nameplateCombatTextvisible = visible;
	}

	public void PlayCombatText(ActorData actorData, string text, CombatTextCategory category, BuffIconToDisplay icon)
	{
		if (this.m_nameplateCombatTextvisible && this.m_nameplates.ContainsKey(actorData))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.PlayCombatText(ActorData, string, CombatTextCategory, BuffIconToDisplay)).MethodHandle;
			}
			UINameplateItem uinameplateItem = this.m_nameplates[actorData];
			uinameplateItem.PlayCombatText(actorData, text, category, icon);
		}
	}

	public void SetDebugNameplateTextValues()
	{
		using (Dictionary<ActorData, UINameplateItem>.Enumerator enumerator = this.m_nameplates.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, UINameplateItem> keyValuePair = enumerator.Current;
				ActorData key = keyValuePair.Key;
				UINameplateItem value = keyValuePair.Value;
				value.SetDebugText("State: " + key.GetActorTurnSM().CurrentState.ToString());
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.SetDebugNameplateTextValues()).MethodHandle;
			}
		}
	}

	public void Update()
	{
		if (DebugParameters.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.Update()).MethodHandle;
			}
			if (DebugParameters.Get().GetParameterAsBool("DebugNameplates"))
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				this.SetDebugNameplateTextValues();
			}
		}
	}

	public void AddActor(ActorData actorData)
	{
		UINameplateItem uinameplateItem = UnityEngine.Object.Instantiate<UINameplateItem>(this.m_nameplateItemPrefab);
		this.m_nameplates[actorData] = uinameplateItem;
		uinameplateItem.transform.SetParent(base.transform);
		uinameplateItem.Setup(actorData);
		uinameplateItem.transform.localPosition = Vector3.zero;
		uinameplateItem.transform.localScale = new Vector3(1f, 1f, 1f);
		if (uinameplateItem.transform as RectTransform != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.AddActor(ActorData)).MethodHandle;
			}
			(uinameplateItem.transform as RectTransform).anchoredPosition = new Vector2(10000f, 10000f);
		}
		UIManager.SetGameObjectActive(uinameplateItem.m_parentTransform, false, null);
		UIManager.SetGameObjectActive(uinameplateItem.m_parentTransform, true, null);
		CanvasLayerManager.Get().NotifyAddedNewNameplate();
	}

	public void NotifyFlagStatusChange(ActorData theActor, bool holdingFlag)
	{
		if (theActor != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.NotifyFlagStatusChange(ActorData, bool)).MethodHandle;
			}
			if (this.m_nameplates.ContainsKey(theActor))
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				UINameplateItem uinameplateItem = this.m_nameplates[theActor];
				uinameplateItem.NotifyFlagStatusChange(holdingFlag);
			}
		}
	}

	public void NotifyStatusChange(ActorData theActor, StatusType status, bool gainedStatus)
	{
		if (this.m_nameplates.ContainsKey(theActor))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.NotifyStatusChange(ActorData, StatusType, bool)).MethodHandle;
			}
			UINameplateItem uinameplateItem = this.m_nameplates[theActor];
			if (gainedStatus)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				uinameplateItem.AddStatus(status);
			}
			else
			{
				uinameplateItem.RemoveStatus(status);
			}
		}
	}

	public void SetCatalystPipsVisible(ActorData theActor, bool visible)
	{
		if (this.m_nameplates.ContainsKey(theActor))
		{
			UINameplateItem uinameplateItem = this.m_nameplates[theActor];
			uinameplateItem.SetCatalystsVisible(visible);
		}
	}

	public void UpdateCatalysts(ActorData theActor, List<Ability> cardAbilities)
	{
		if (this.m_nameplates.ContainsKey(theActor))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.UpdateCatalysts(ActorData, List<Ability>)).MethodHandle;
			}
			UINameplateItem uinameplateItem = this.m_nameplates[theActor];
			uinameplateItem.UpdateCatalysts(cardAbilities);
		}
	}

	public void NotifyStatusDurationChange(ActorData theActor, StatusType status, int newDuration)
	{
		if (this.m_nameplates.ContainsKey(theActor))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.NotifyStatusDurationChange(ActorData, StatusType, int)).MethodHandle;
			}
			UINameplateItem uinameplateItem = this.m_nameplates[theActor];
			uinameplateItem.UpdateStatusDuration(status, newDuration);
		}
	}

	public void UpdateTargetingAbilityIndicator(ActorData targetingActor, Ability ability, AbilityData.ActionType action, int index)
	{
		if (this.m_nameplates.ContainsKey(targetingActor))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.UpdateTargetingAbilityIndicator(ActorData, Ability, AbilityData.ActionType, int)).MethodHandle;
			}
			UINameplateItem uinameplateItem = this.m_nameplates[targetingActor];
			uinameplateItem.UpdateTargetingAbilityIndicator(ability, action, index);
		}
	}

	public void TurnOffTargetingAbilityIndicator(ActorData targetingActor, int fromIndex)
	{
		if (this.m_nameplates.ContainsKey(targetingActor))
		{
			UINameplateItem uinameplateItem = this.m_nameplates[targetingActor];
			uinameplateItem.TurnOffTargetingAbilityIndicator(fromIndex);
		}
	}

	public void SpawnOverconForActor(ActorData actor, UIOverconData.NameToOverconEntry entry, bool skipValidation)
	{
		if (this.m_nameplates.ContainsKey(actor))
		{
			UINameplateItem uinameplateItem = this.m_nameplates[actor];
			uinameplateItem.SpawnOvercon(entry, skipValidation);
		}
	}

	public void UpdateSelfNameplate(ActorData theTargeted, Ability abilityTargeting, bool inCover, int currentTargeterIndex, bool inConfirm)
	{
		if (this.m_nameplates.ContainsKey(theTargeted))
		{
			UINameplateItem uinameplateItem = this.m_nameplates[theTargeted];
			uinameplateItem.UpdateSelfNameplate(abilityTargeting, inCover, currentTargeterIndex, inConfirm);
		}
	}

	public void UpdateNameplateTargeted(ActorData targetingActor, ActorData theTargeted, Ability abilityTargeting, bool inCover, int currentTargeterIndex, bool inConfirm)
	{
		if (this.m_nameplates.ContainsKey(theTargeted))
		{
			UINameplateItem uinameplateItem = this.m_nameplates[theTargeted];
			uinameplateItem.UpdateNameplateTargeted(targetingActor, abilityTargeting, inCover, currentTargeterIndex, inConfirm);
		}
	}

	public void UpdateNameplateUntargeted(ActorData theTargeted, bool doInstantHide = false)
	{
		if (this.m_nameplates.ContainsKey(theTargeted))
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.UpdateNameplateUntargeted(ActorData, bool)).MethodHandle;
			}
			UINameplateItem uinameplateItem = this.m_nameplates[theTargeted];
			uinameplateItem.UpdateNameplateUntargeted(doInstantHide);
		}
	}

	public void RemoveActor(ActorData actorData)
	{
		if (this.m_nameplates.ContainsKey(actorData))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.RemoveActor(ActorData)).MethodHandle;
			}
			UINameplateItem uinameplateItem = this.m_nameplates[actorData];
			this.m_nameplates.Remove(actorData);
			if (uinameplateItem != null)
			{
				UnityEngine.Object.Destroy(uinameplateItem.gameObject);
			}
		}
	}

	private void Clear()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			UnityEngine.Object.Destroy(child.gameObject);
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.Clear()).MethodHandle;
		}
		this.m_nameplates.Clear();
	}

	public void SortNameplates()
	{
		IOrderedEnumerable<KeyValuePair<ActorData, UINameplateItem>> orderedEnumerable = from r in this.m_nameplates
		orderby r.Value.m_distanceFromCamera
		select r;
		bool flag = false;
		if (orderedEnumerable.Count<KeyValuePair<ActorData, UINameplateItem>>() != this.m_sortedActorIndexNameplates.Count<int>())
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplatePanel.SortNameplates()).MethodHandle;
			}
			flag = true;
		}
		else
		{
			List<int>.Enumerator enumerator = this.m_sortedActorIndexNameplates.GetEnumerator();
			IEnumerator<KeyValuePair<ActorData, UINameplateItem>> enumerator2 = orderedEnumerable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!enumerator2.MoveNext())
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						goto IL_C4;
					}
				}
				else
				{
					int num = enumerator.Current;
					KeyValuePair<ActorData, UINameplateItem> keyValuePair = enumerator2.Current;
					int actorIndex = keyValuePair.Key.ActorIndex;
					if (num != actorIndex)
					{
						flag = true;
						break;
					}
				}
			}
		}
		IL_C4:
		if (flag)
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
			this.m_sortedActorIndexNameplates.Clear();
			IEnumerator<KeyValuePair<ActorData, UINameplateItem>> enumerator3 = orderedEnumerable.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					KeyValuePair<ActorData, UINameplateItem> keyValuePair2 = enumerator3.Current;
					int sortOrder = keyValuePair2.Value.GetSortOrder();
					int num2 = this.m_nameplates.Count - this.m_sortedActorIndexNameplates.Count;
					if (sortOrder != num2)
					{
						keyValuePair2.Value.SetSortOrder(num2);
					}
					this.m_sortedActorIndexNameplates.Add(keyValuePair2.Key.ActorIndex);
				}
			}
			finally
			{
				if (enumerator3 != null)
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
					enumerator3.Dispose();
				}
			}
			CanvasLayerManager.Get().UpdateNameplateOrder();
		}
	}
}
