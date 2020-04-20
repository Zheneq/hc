using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class _DisableGameObjectOnAnimationDoneEvent : MonoBehaviour
{
	public GameObject[] m_gameObjectsToDisable;

	private Animator m_animator;

	private bool m_isListening;

	private string m_stateName = string.Empty;

	private void Awake()
	{
		this.m_animator = base.gameObject.GetComponent<Animator>();
	}

	public void DisableGameObjectOnAnimDoneListenerEvent()
	{
		this.m_isListening = true;
		for (int i = 0; i < this.m_animator.layerCount; i++)
		{
			AnimatorClipInfo[] currentAnimatorClipInfo = this.m_animator.GetCurrentAnimatorClipInfo(i);
			AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(i);
			for (int j = 0; j < currentAnimatorClipInfo.Length; j++)
			{
				AnimationEvent[] events = currentAnimatorClipInfo[j].clip.events;
				for (int k = 0; k < events.Length; k++)
				{
					if (events[k].functionName == "DisableGameObjectOnAnimDoneListenerEvent")
					{
						if (currentAnimatorStateInfo.normalizedTime >= 1f)
						{
							this.m_stateName = currentAnimatorClipInfo[j].clip.name;
						}
					}
				}
			}
		}
	}

	private void DoDisable()
	{
		this.m_isListening = false;
		this.m_stateName = string.Empty;
		for (int i = 0; i < this.m_gameObjectsToDisable.Length; i++)
		{
			if (this.m_gameObjectsToDisable[i].activeSelf)
			{
				UIManager.SetGameObjectActive(this.m_gameObjectsToDisable[i], false, null);
			}
		}
	}

	private void Update()
	{
		if (this.m_gameObjectsToDisable.Length > 0)
		{
			if (this.m_isListening)
			{
				int layerCount = this.m_animator.layerCount;
				for (int i = 0; i < layerCount; i++)
				{
					AnimatorClipInfo[] currentAnimatorClipInfo = this.m_animator.GetCurrentAnimatorClipInfo(i);
					AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(i);
					int j = 0;
					while (j < currentAnimatorClipInfo.Length)
					{
						AnimationEvent[] events = currentAnimatorClipInfo[j].clip.events;
						if (events.Length == 0)
						{
							goto IL_C1;
						}
						if (!this.m_stateName.IsNullOrEmpty() && currentAnimatorClipInfo[j].clip.name != this.m_stateName)
						{
							goto IL_C1;
						}
						for (int k = 0; k < events.Length; k++)
						{
							if (events[k].functionName == "DisableGameObjectOnAnimDoneListenerEvent")
							{
								if (currentAnimatorStateInfo.normalizedTime >= 1f)
								{
									this.DoDisable();
								}
							}
						}
						IL_128:
						j++;
						continue;
						IL_C1:
						this.DoDisable();
						goto IL_128;
					}
				}
			}
		}
	}
}
