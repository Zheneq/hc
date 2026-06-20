using UnityEngine;

public class UINewUIRoot : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}
}
