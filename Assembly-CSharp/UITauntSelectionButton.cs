using System;
using System.Collections.Generic;
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
		if (this.m_buttonHitBox != null)
		{
			UIEventTriggerUtils.AddListener(this.m_buttonHitBox.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.SelectedTaunt));
		}
	}

	public void SelectedTaunt(BaseEventData data)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		Debug.Log(string.Concat(new object[]
		{
			"Selected taunt ",
			this.m_tauntRef.m_name,
			" with ID ",
			this.m_tauntRef.m_uniqueTauntID,
			" to play TauntNumber #",
			this.m_tauntRef.m_tauntNumber
		}));
		if (activeOwnedActorData != null)
		{
			activeOwnedActorData.GetComponent<ActorCinematicRequests>().SendAbilityCinematicRequest(this.m_actionType, true, this.m_tauntRef.m_tauntNumber, this.m_tauntRef.m_uniqueTauntID);
		}
		HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.SelectedTaunt(this.m_actionType);
		HUD_UI.Get().m_mainScreenPanel.m_characterProfile.OnCloseSelectionClick(data);
	}

	public void SetupTaunt(AbilityData.ActionType actionType, AbilityData.AbilityEntry entry, CameraShotSequence taunt)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		this.m_actionType = actionType;
		this.m_abilityEntry = entry;
		this.m_theSprite.sprite = this.m_abilityEntry.ability.sprite;
		this.m_tauntRef = taunt;
		List<CharacterTaunt> taunts = activeOwnedActorData.GetCharacterResourceLink().m_taunts;
		for (int i = 0; i < taunts.Count; i++)
		{
			if (taunts[i].m_uniqueID == taunt.m_uniqueTauntID)
			{
				this.m_tauntName.text = string.Format("{0}: {1}", this.m_abilityEntry.ability.GetNameString(), activeOwnedActorData.GetCharacterResourceLink().GetTauntName(i));
				return;
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			return;
		}
	}
}
