using System;
using UnityEngine;

public class AudioListenerController : MonoBehaviour
{
	private static AudioListenerController s_instance;

	internal static AudioListenerController Get()
	{
		return AudioListenerController.s_instance;
	}

	private void Awake()
	{
		AudioListenerController.s_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDestroy()
	{
		AudioListenerController.s_instance = null;
	}
}
