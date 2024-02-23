using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITauntSelectionButton : MonoBehaviour
{
	public Button m_buttonHitBox;

	public Image m_theSprite;

	public TextMeshProUGUI m_tauntName;

	private AbilityData.AbilityEntry m_abilityEntry;

	private AbilityData.ActionType m_actionType;

	private CameraShotSequence m_tauntRef;

	private void Start()
	{
		if (!(m_buttonHitBox != null))
		{
			return;
		}
		while (true)
		{
			UIEventTriggerUtils.AddListener(m_buttonHitBox.gameObject, EventTriggerType.PointerClick, SelectedTaunt);
			return;
		}
	}

	public void SelectedTaunt(BaseEventData data)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		Debug.Log(new StringBuilder().Append("Selected taunt ").Append(m_tauntRef.m_name).Append(" with ID ").Append(m_tauntRef.m_uniqueTauntID).Append(" to play TauntNumber #").Append(m_tauntRef.m_tauntNumber).ToString());
		if (activeOwnedActorData != null)
		{
			activeOwnedActorData.GetComponent<ActorCinematicRequests>().SendAbilityCinematicRequest(m_actionType, true, m_tauntRef.m_tauntNumber, m_tauntRef.m_uniqueTauntID);
		}
		HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.SelectedTaunt(m_actionType);
		HUD_UI.Get().m_mainScreenPanel.m_characterProfile.OnCloseSelectionClick(data);
	}

	public void SetupTaunt(AbilityData.ActionType actionType, AbilityData.AbilityEntry entry, CameraShotSequence taunt)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		m_actionType = actionType;
		m_abilityEntry = entry;
		m_theSprite.sprite = m_abilityEntry.ability.sprite;
		m_tauntRef = taunt;
		List<CharacterTaunt> taunts = activeOwnedActorData.GetCharacterResourceLink().m_taunts;
		for (int i = 0; i < taunts.Count; i++)
		{
			if (taunts[i].m_uniqueID != taunt.m_uniqueTauntID)
			{
				continue;
			}
			while (true)
			{
				m_tauntName.text = new StringBuilder().Append(m_abilityEntry.ability.GetNameString()).Append(": ").Append(activeOwnedActorData.GetCharacterResourceLink().GetTauntName(i)).ToString();
				return;
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
