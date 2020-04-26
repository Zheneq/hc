using UnityEngine;

public class AudioWideData : MonoBehaviour
{
	private static AudioWideData s_instance;

	[Header("Conversations")]
	public ConversationTemplate[] m_conversations;

	public static AudioWideData Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}
}
