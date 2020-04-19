using System;
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
		if (this.m_dontDestroyOnLoad)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PrefabInstantiator.Awake()).MethodHandle;
			}
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		this.m_instantiatedGameObjects = new GameObject[this.m_prefabs.Length];
		if (this.m_prefabs != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int i = 0; i < this.m_prefabs.Length; i++)
			{
				GameObject gameObject = this.m_prefabs[i];
				if (gameObject == null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				else
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
					this.m_instantiatedGameObjects[i] = gameObject2;
					if (this.m_dontDestroyOnLoad)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.m_parentSpawnedObjectsIfPersistent)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							gameObject2.transform.parent = base.transform;
							goto IL_CE;
						}
					}
					if (this.m_prefabsDontDestroyOnLoad)
					{
						UnityEngine.Object.DontDestroyOnLoad(gameObject2);
					}
				}
				IL_CE:;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < this.m_instantiatedGameObjects.Length; i++)
		{
			UnityEngine.Object.Destroy(this.m_instantiatedGameObjects[i]);
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(PrefabInstantiator.OnDestroy()).MethodHandle;
		}
	}
}
