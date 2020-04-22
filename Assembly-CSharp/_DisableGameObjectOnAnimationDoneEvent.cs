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
		m_animator = base.gameObject.GetComponent<Animator>();
	}

	public void DisableGameObjectOnAnimDoneListenerEvent()
	{
		m_isListening = true;
		for (int i = 0; i < m_animator.layerCount; i++)
		{
			AnimatorClipInfo[] currentAnimatorClipInfo = m_animator.GetCurrentAnimatorClipInfo(i);
			AnimatorStateInfo currentAnimatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(i);
			for (int j = 0; j < currentAnimatorClipInfo.Length; j++)
			{
				AnimationEvent[] events = currentAnimatorClipInfo[j].clip.events;
				for (int k = 0; k < events.Length; k++)
				{
					if (events[k].functionName == "DisableGameObjectOnAnimDoneListenerEvent")
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (currentAnimatorStateInfo.normalizedTime >= 1f)
						{
							m_stateName = currentAnimatorClipInfo[j].clip.name;
						}
					}
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						goto end_IL_00b1;
					}
					continue;
					end_IL_00b1:
					break;
				}
			}
		}
	}

	private void DoDisable()
	{
		m_isListening = false;
		m_stateName = string.Empty;
		for (int i = 0; i < m_gameObjectsToDisable.Length; i++)
		{
			if (m_gameObjectsToDisable[i].activeSelf)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				UIManager.SetGameObjectActive(m_gameObjectsToDisable[i], false);
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void Update()
	{
		if (m_gameObjectsToDisable.Length <= 0)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_isListening)
			{
				return;
			}
			int layerCount = m_animator.layerCount;
			for (int i = 0; i < layerCount; i++)
			{
				AnimatorClipInfo[] currentAnimatorClipInfo = m_animator.GetCurrentAnimatorClipInfo(i);
				AnimatorStateInfo currentAnimatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(i);
				for (int j = 0; j < currentAnimatorClipInfo.Length; j++)
				{
					AnimationEvent[] events = currentAnimatorClipInfo[j].clip.events;
					if (events.Length != 0)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_stateName.IsNullOrEmpty() || !(currentAnimatorClipInfo[j].clip.name != m_stateName))
						{
							for (int k = 0; k < events.Length; k++)
							{
								if (!(events[k].functionName == "DisableGameObjectOnAnimDoneListenerEvent"))
								{
									continue;
								}
								while (true)
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
									while (true)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									DoDisable();
								}
							}
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							continue;
						}
					}
					DoDisable();
				}
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}
}
