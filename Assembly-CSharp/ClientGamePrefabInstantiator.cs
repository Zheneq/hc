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
			if (this.m_instances.Count > 0)
			{
				Log.Error(base.GetType() + " already has instantiated prefab instances when trying to spawn new instances", new object[0]);
				this.DestroyInstantiations();
			}
		}
		Log.Info(base.GetType() + " instantiating client prefabs", new object[0]);
		if (this.m_prefabs != null)
		{
			for (int i = 0; i < this.m_prefabs.Length; i++)
			{
				GameObject gameObject = this.m_prefabs[i];
				if (gameObject != null)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
					UnityEngine.Object.DontDestroyOnLoad(gameObject2);
					this.m_instances.Add(gameObject2);
				}
			}
		}
	}

	public void DestroyInstantiations()
	{
		Log.Info(base.GetType() + " destroying client prefabs", new object[0]);
		if (this.m_instances != null)
		{
			if (this.m_instances.Count > 0)
			{
				foreach (GameObject obj in this.m_instances)
				{
					UnityEngine.Object.Destroy(obj);
				}
				this.m_instances.Clear();
			}
		}
	}
}
