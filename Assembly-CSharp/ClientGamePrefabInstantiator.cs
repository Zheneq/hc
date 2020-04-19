using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientGamePrefabInstantiator : MonoBehaviour
{
	public GameObject[] m_prefabs;

	private List<GameObject> m_instances;

	private static ClientGamePrefabInstantiator s_instance;

	public static ClientGamePrefabInstantiator Get()
	{
		return ClientGamePrefabInstantiator.s_instance;
	}

	private void Awake()
	{
		ClientGamePrefabInstantiator.s_instance = this;
	}

	private void OnDestroy()
	{
		ClientGamePrefabInstantiator.s_instance = null;
	}

	private void Start()
	{
		this.m_instances = new List<GameObject>();
	}

	public void InstantiatePrefabs()
	{
		if (this.m_instances != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientGamePrefabInstantiator.InstantiatePrefabs()).MethodHandle;
			}
			if (this.m_instances.Count > 0)
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
				Log.Error(base.GetType() + " already has instantiated prefab instances when trying to spawn new instances", new object[0]);
				this.DestroyInstantiations();
			}
		}
		Log.Info(base.GetType() + " instantiating client prefabs", new object[0]);
		if (this.m_prefabs != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int i = 0; i < this.m_prefabs.Length; i++)
			{
				GameObject gameObject = this.m_prefabs[i];
				if (gameObject != null)
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
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
					UnityEngine.Object.DontDestroyOnLoad(gameObject2);
					this.m_instances.Add(gameObject2);
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	public void DestroyInstantiations()
	{
		Log.Info(base.GetType() + " destroying client prefabs", new object[0]);
		if (this.m_instances != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientGamePrefabInstantiator.DestroyInstantiations()).MethodHandle;
			}
			if (this.m_instances.Count > 0)
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
				foreach (GameObject obj in this.m_instances)
				{
					UnityEngine.Object.Destroy(obj);
				}
				this.m_instances.Clear();
			}
		}
	}
}
