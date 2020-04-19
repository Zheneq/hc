using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	private static T s_instance;

	public static T Get()
	{
		return Singleton<T>.s_instance;
	}

	protected virtual void Awake()
	{
		Singleton<T>.s_instance = (this as T);
	}

	protected virtual void OnDestroy()
	{
		Singleton<T>.s_instance = (T)((object)null);
	}
}
