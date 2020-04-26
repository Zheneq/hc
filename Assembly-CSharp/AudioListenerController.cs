using UnityEngine;

public class AudioListenerController : MonoBehaviour
{
	private static AudioListenerController s_instance;

	internal static AudioListenerController Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDestroy()
	{
		s_instance = null;
	}
}
