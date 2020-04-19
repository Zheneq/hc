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
							RuntimeMethodHandle runtimeMethodHandle = methodof(_DisableGameObjectOnAnimationDoneEvent.DisableGameObjectOnAnimDoneListenerEvent()).MethodHandle;
						}
						if (currentAnimatorStateInfo.normalizedTime >= 1f)
						{
							this.m_stateName = currentAnimatorClipInfo[j].clip.name;
						}
					}
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(_DisableGameObjectOnAnimationDoneEvent.DoDisable()).MethodHandle;
				}
				UIManager.SetGameObjectActive(this.m_gameObjectsToDisable[i], false, null);
			}
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

	private void Update()
	{
		if (this.m_gameObjectsToDisable.Length > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_DisableGameObjectOnAnimationDoneEvent.Update()).MethodHandle;
			}
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
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!this.m_stateName.IsNullOrEmpty() && currentAnimatorClipInfo[j].clip.name != this.m_stateName)
						{
							goto IL_C1;
						}
						for (int k = 0; k < events.Length; k++)
						{
							if (events[k].functionName == "DisableGameObjectOnAnimDoneListenerEvent")
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
								if (currentAnimatorStateInfo.normalizedTime >= 1f)
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
									this.DoDisable();
								}
							}
						}
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						IL_128:
						j++;
						continue;
						IL_C1:
						this.DoDisable();
						goto IL_128;
					}
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
		}
	}
}
