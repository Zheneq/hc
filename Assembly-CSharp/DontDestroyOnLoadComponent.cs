using UnityEngine;

public class DontDestroyOnLoadComponent : MonoBehaviour
{
	private void Awake()
	{
		if (!(base.gameObject.transform.parent == null))
		{
			return;
		}
		while (true)
		{
			Object.DontDestroyOnLoad(base.gameObject);
			return;
		}
	}
}
