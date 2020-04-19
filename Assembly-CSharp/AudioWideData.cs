using System;
using UnityEngine;

public class AudioWideData : MonoBehaviour
{
	private static AudioWideData s_instance;

	[Header("Conversations")]
	public ConversationTemplate[] m_conversations;

	public static AudioWideData Get()
	{
		return AudioWideData.s_instance;
	}

	private void Awake()
	{
		AudioWideData.s_instance = this;
	}

	private void OnDestroy()
	{
		AudioWideData.s_instance = null;
	}
}
