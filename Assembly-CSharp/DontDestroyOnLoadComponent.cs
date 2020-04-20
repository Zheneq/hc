using System;
using UnityEngine;

public class DontDestroyOnLoadComponent : MonoBehaviour
{
	private void Awake()
	{
		if (base.gameObject.transform.parent == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}
}
