using UnityEngine;

public class PrefabInstantiator : MonoBehaviour
{
	public bool m_prefabsDontDestroyOnLoad = true;

	public bool m_destroyInstantiatedPrefabsOnDestroy;

	public bool m_dontDestroyOnLoad;

	[Space(5f)]
	[Tooltip("If not destroying on load, spawned prefabs will be parented under this prefab instantiator")]
	public bool m_parentSpawnedObjectsIfPersistent = true;

	public GameObject[] m_prefabs;

	private GameObject[] m_instantiatedGameObjects;

	private void Awake()
	{
		if (m_dontDestroyOnLoad)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
		m_instantiatedGameObjects = new GameObject[m_prefabs.Length];
		if (m_prefabs == null)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_prefabs.Length; i++)
			{
				GameObject gameObject = m_prefabs[i];
				if (gameObject == null)
				{
					continue;
				}
				GameObject gameObject2 = Object.Instantiate(gameObject);
				m_instantiatedGameObjects[i] = gameObject2;
				if (m_dontDestroyOnLoad)
				{
					if (m_parentSpawnedObjectsIfPersistent)
					{
						gameObject2.transform.parent = base.transform;
						continue;
					}
				}
				if (m_prefabsDontDestroyOnLoad)
				{
					Object.DontDestroyOnLoad(gameObject2);
				}
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < m_instantiatedGameObjects.Length; i++)
		{
			Object.Destroy(m_instantiatedGameObjects[i]);
		}
		while (true)
		{
			return;
		}
	}
}
