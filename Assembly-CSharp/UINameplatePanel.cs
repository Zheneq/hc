using System.Collections.Generic;
using System.Linq;
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
        Dictionary<ActorData, UINameplateItem> nameplates =
            HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.GetNameplates();
        if (nameplates.TryGetValue(theActor, out UINameplateItem nameplateItem))
        {
            nameplateItem.SetDebugText(textToDisplay);
        }
    }

    public void StartTargetingNumberFadeout(ActorData actorData)
    {
        if (m_nameplates.TryGetValue(actorData, out UINameplateItem nameplateItem))
        {
            nameplateItem.StartTargetingNumberFadeout();
        }
    }

    public void ShowTargetingNumberForConfirmedTargeting(ActorData actorData)
    {
        if (m_nameplates.TryGetValue(actorData, out UINameplateItem nameplateItem))
        {
            nameplateItem.ShowTargetingNumberForConfirmedTargeting();
        }
    }

    public void UpdateBriefcaseThreshold(ActorData actorData, float percent)
    {
        if (m_nameplates.TryGetValue(actorData, out UINameplateItem nameplateItem))
        {
            nameplateItem.UpdateBriefcaseThreshold(percent);
        }
    }

    public void RefreshNameplates()
    {
        foreach (UINameplateItem nameplateItem in m_nameplates.Values)
        {
            nameplateItem.ForceFinishStatusAnims();
        }
    }

    public void SetTextVisible(bool visible)
    {
        if (m_nameplateTextVisible == visible)
        {
            return;
        }

        m_nameplateTextVisible = visible;
        foreach (UINameplateItem nameplateItem in m_nameplates.Values)
        {
            nameplateItem.SetTextVisible(visible);
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
        if (m_nameplateCombatTextvisible && m_nameplates.TryGetValue(actorData, out UINameplateItem nameplateItem))
        {
            nameplateItem.PlayCombatText(actorData, text, category, icon);
        }
    }

    public void SetDebugNameplateTextValues()
    {
        foreach (KeyValuePair<ActorData, UINameplateItem> nameplate in m_nameplates)
        {
            nameplate.Value.SetDebugText("State: " + nameplate.Key.GetActorTurnSM().CurrentState);
        }
    }

    public void Update()
    {
        if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("DebugNameplates"))
        {
            SetDebugNameplateTextValues();
        }
    }

    public void AddActor(ActorData actorData)
    {
        UINameplateItem nameplateItem = Instantiate(m_nameplateItemPrefab);
        m_nameplates[actorData] = nameplateItem;
        nameplateItem.transform.SetParent(transform);
        nameplateItem.Setup(actorData);
        nameplateItem.transform.localPosition = Vector3.zero;
        nameplateItem.transform.localScale = new Vector3(1f, 1f, 1f);
        if (nameplateItem.transform as RectTransform != null)
        {
            (nameplateItem.transform as RectTransform).anchoredPosition = new Vector2(10000f, 10000f);
        }

        UIManager.SetGameObjectActive(nameplateItem.m_parentTransform, false);
        UIManager.SetGameObjectActive(nameplateItem.m_parentTransform, true);
        CanvasLayerManager.Get().NotifyAddedNewNameplate();
    }

    public void NotifyFlagStatusChange(ActorData theActor, bool holdingFlag)
    {
        if (theActor != null && m_nameplates.TryGetValue(theActor, out UINameplateItem nameplateItem))
        {
            nameplateItem.NotifyFlagStatusChange(holdingFlag);
        }
    }

    public void NotifyStatusChange(ActorData theActor, StatusType status, bool gainedStatus)
    {
        if (m_nameplates.TryGetValue(theActor, out UINameplateItem nameplateItem))
        {
            if (gainedStatus)
            {
                nameplateItem.AddStatus(status);
            }
            else
            {
                nameplateItem.RemoveStatus(status);
            }
        }
    }

    public void SetCatalystPipsVisible(ActorData theActor, bool visible)
    {
        if (m_nameplates.TryGetValue(theActor, out UINameplateItem nameplateItem))
        {
            nameplateItem.SetCatalystsVisible(visible);
        }
    }

    public void UpdateCatalysts(ActorData theActor, List<Ability> cardAbilities)
    {
        if (m_nameplates.TryGetValue(theActor, out UINameplateItem nameplateItem))
        {
            nameplateItem.UpdateCatalysts(cardAbilities);
        }
    }

    public void NotifyStatusDurationChange(ActorData theActor, StatusType status, int newDuration)
    {
        if (m_nameplates.TryGetValue(theActor, out UINameplateItem nameplateItem))
        {
            nameplateItem.UpdateStatusDuration(status, newDuration);
        }
    }

    public void UpdateTargetingAbilityIndicator(
        ActorData targetingActor,
        Ability ability,
        AbilityData.ActionType action,
        int index)
    {
        if (m_nameplates.TryGetValue(targetingActor, out UINameplateItem nameplateItem))
        {
            nameplateItem.UpdateTargetingAbilityIndicator(ability, action, index);
        }
    }

    public void TurnOffTargetingAbilityIndicator(ActorData targetingActor, int fromIndex)
    {
        if (m_nameplates.TryGetValue(targetingActor, out UINameplateItem nameplateItem))
        {
            nameplateItem.TurnOffTargetingAbilityIndicator(fromIndex);
        }
    }

    public void SpawnOverconForActor(ActorData actor, UIOverconData.NameToOverconEntry entry, bool skipValidation)
    {
        if (m_nameplates.TryGetValue(actor, out UINameplateItem nameplateItem))
        {
            nameplateItem.SpawnOvercon(entry, skipValidation);
        }
    }

    public void UpdateSelfNameplate(
        ActorData theTargeted,
        Ability abilityTargeting,
        bool inCover,
        int currentTargeterIndex,
        bool inConfirm)
    {
        if (m_nameplates.TryGetValue(theTargeted, out UINameplateItem nameplateItem))
        {
            nameplateItem.UpdateSelfNameplate(abilityTargeting, inCover, currentTargeterIndex, inConfirm);
        }
    }

    public void UpdateNameplateTargeted(
        ActorData targetingActor,
        ActorData theTargeted,
        Ability abilityTargeting,
        bool inCover,
        int currentTargeterIndex,
        bool inConfirm)
    {
        if (m_nameplates.TryGetValue(theTargeted, out var nameplateItem))
        {
            nameplateItem.UpdateNameplateTargeted(
                targetingActor,
                abilityTargeting,
                inCover,
                currentTargeterIndex,
                inConfirm);
        }
    }

    public void UpdateNameplateUntargeted(ActorData theTargeted, bool doInstantHide = false)
    {
        if (m_nameplates.TryGetValue(theTargeted, out UINameplateItem nameplateItem))
        {
            nameplateItem.UpdateNameplateUntargeted(doInstantHide);
        }
    }

    public void RemoveActor(ActorData actorData)
    {
        if (m_nameplates.TryGetValue(actorData, out UINameplateItem nameplateItem))
        {
            m_nameplates.Remove(actorData);
            if (nameplateItem != null)
            {
                Destroy(nameplateItem.gameObject);
            }
        }
    }

    private void Clear()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }

        m_nameplates.Clear();
    }

    public void SortNameplates()
    {
        IOrderedEnumerable<KeyValuePair<ActorData, UINameplateItem>> sortedNameplates = m_nameplates
            .OrderBy(r => r.Value.m_distanceFromCamera);
        bool isUpdateNeeded = false;
        if (sortedNameplates.Count() != m_sortedActorIndexNameplates.Count)
        {
            isUpdateNeeded = true;
        }
        else
        {
            List<int>.Enumerator oldSortedNameplates = m_sortedActorIndexNameplates.GetEnumerator();
            IEnumerator<KeyValuePair<ActorData, UINameplateItem>>
                newSortedNameplates = sortedNameplates.GetEnumerator();
            while (oldSortedNameplates.MoveNext() && newSortedNameplates.MoveNext())
            {
                int oldIndex = oldSortedNameplates.Current;
                int newIndex = newSortedNameplates.Current.Key.ActorIndex;
                if (oldIndex != newIndex)
                {
                    isUpdateNeeded = true;
                    break;
                }
            }
        }

        if (isUpdateNeeded)
        {
            m_sortedActorIndexNameplates.Clear();

            foreach (KeyValuePair<ActorData, UINameplateItem> nameplateItem in sortedNameplates)
            {
                int oldIndex = nameplateItem.Value.GetSortOrder();
                int newIndex = m_nameplates.Count - m_sortedActorIndexNameplates.Count;
                if (oldIndex != newIndex)
                {
                    nameplateItem.Value.SetSortOrder(newIndex);
                }

                m_sortedActorIndexNameplates.Add(nameplateItem.Key.ActorIndex);
            }

            CanvasLayerManager.Get().UpdateNameplateOrder();
        }
    }
}