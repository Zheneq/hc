using System.Collections.Generic;
using UnityEngine;

public class ClientGamePrefabInstantiator : MonoBehaviour
{
	public GameObject[] m_prefabs;

	private List<GameObject> m_instances;

	private static ClientGamePrefabInstantiator s_instance;

	public static ClientGamePrefabInstantiator Get()
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

	private void Start()
	{
		m_instances = new List<GameObject>();
	}

	public void InstantiatePrefabs()
	{
		if (m_instances != null)
		{
			if (m_instances.Count > 0)
			{
				Log.Error(string.Concat(GetType(), " already has instantiated prefab instances when trying to spawn new instances"));
				DestroyInstantiations();
			}
		}
		Log.Info(string.Concat(GetType(), " instantiating client prefabs"));
		if (m_prefabs == null)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_prefabs.Length; i++)
			{
				GameObject gameObject = m_prefabs[i];
				if (gameObject != null)
				{
					GameObject gameObject2 = Object.Instantiate(gameObject);
					Object.DontDestroyOnLoad(gameObject2);
					m_instances.Add(gameObject2);
				}
			}
			while (true)
			{
				switch (6)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void DestroyInstantiations()
	{
		Log.Info(string.Concat(GetType(), " destroying client prefabs"));
		if (m_instances == null)
		{
			return;
		}
		while (true)
		{
			if (m_instances.Count > 0)
			{
				while (true)
				{
					foreach (GameObject instance in m_instances)
					{
						Object.Destroy(instance);
					}
					m_instances.Clear();
					return;
				}
			}
			return;
		}
	}
}
