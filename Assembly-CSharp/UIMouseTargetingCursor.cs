using System;
using TMPro;
using UnityEngine;

public class UIMouseTargetingCursor : MonoBehaviour
{
	public TextMeshProUGUI m_clicksLeftLabel;

	public Animator m_animator;

	private Vector2 originalSizeOfCursor;

	private float originalFontSize;

	private bool initialized;

	private bool m_isVisible;

	private void Init()
	{
		if (!this.initialized)
		{
			this.initialized = true;
			this.originalSizeOfCursor = (base.gameObject.transform as RectTransform).sizeDelta;
			this.originalFontSize = this.m_clicksLeftLabel.fontSize;
		}
	}

	public void ShowTargetCursor()
	{
		if (!this.m_isVisible)
		{
			this.m_isVisible = true;
			UIManager.SetGameObjectActive(this, true, null);
			this.m_animator.Play("MouseCursorTargetDefaultCLICK");
			this.DoUpdate(false);
		}
	}

	public void HideTargetCursor()
	{
		this.m_isVisible = false;
		if (this.m_animator.isActiveAndEnabled)
		{
			this.m_animator.Play("MouseCursorTargetDefaultEXIT", 0);
		}
	}

	public void DoUpdate(bool fromUpdate = false)
	{
		this.Init();
		if (this.m_isVisible)
		{
			Canvas componentInParent = base.gameObject.GetComponentInParent<Canvas>();
			Vector2 vector = new Vector2(Input.mousePosition.x / (float)Screen.width, Input.mousePosition.y / (float)Screen.height - 1f);
			Vector2 sizeDelta = (componentInParent.transform as RectTransform).sizeDelta;
			Vector2 anchoredPosition = new Vector2(vector.x * sizeDelta.x, vector.y * sizeDelta.y);
			Vector2 vector2 = new Vector2((float)Screen.width / sizeDelta.x, (float)Screen.height / sizeDelta.y);
			(base.gameObject.transform as RectTransform).sizeDelta = new Vector2(this.originalSizeOfCursor.x / vector2.x, this.originalSizeOfCursor.y / vector2.y);
			this.m_clicksLeftLabel.fontSize = this.originalFontSize / vector2.y;
			anchoredPosition.y -= (base.gameObject.transform as RectTransform).sizeDelta.y;
			if (fromUpdate)
			{
				Cursor.visible = false;
			}
			(base.gameObject.transform as RectTransform).anchoredPosition = anchoredPosition;
			int num = 1;
			if (GameFlowData.Get() != null)
			{
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					AbilityData abilityData = GameFlowData.Get().activeOwnedActorData.GetAbilityData();
					ActorTurnSM actorTurnSM = GameFlowData.Get().activeOwnedActorData.GetActorTurnSM();
					Ability selectedAbility = abilityData.GetSelectedAbility();
					if (abilityData != null)
					{
						if (actorTurnSM != null)
						{
							if (selectedAbility != null)
							{
								int expectedNumberOfTargeters = selectedAbility.GetExpectedNumberOfTargeters();
								int targetSelectionIndex = actorTurnSM.GetTargetSelectionIndex();
								num = expectedNumberOfTargeters - targetSelectionIndex;
							}
						}
					}
				}
			}
			if (this.m_clicksLeftLabel != null)
			{
				if (fromUpdate)
				{
					if (!(this.m_clicksLeftLabel.text != num.ToString()))
					{
						goto IL_2B2;
					}
				}
				this.m_animator.Play("MouseCursorTargetDefaultCLICK");
				IL_2B2:
				this.m_clicksLeftLabel.text = num.ToString();
			}
		}
	}

	private void LateUpdate()
	{
		this.DoUpdate(true);
	}
}
