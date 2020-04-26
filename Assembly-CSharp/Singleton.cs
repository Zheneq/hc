using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	private static T s_instance;

	public static T Get()
	{
		return s_instance;
	}

	protected virtual void Awake()
	{
		s_instance = (this as T);
	}

	protected virtual void OnDestroy()
	{
		s_instance = (T)null;
	}
}
