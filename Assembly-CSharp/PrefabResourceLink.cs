using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PrefabResourceLink
{
	private static Dictionary<string, SavedResourceLink> s_loadedResourceLinks = new Dictionary<string, SavedResourceLink>();

	[SerializeField]
	private string m_resourcePath;

	[SerializeField]
	private string m_GUID;

	[SerializeField]
	private string m_debugPrefabPath;

	public string GUID => m_GUID;

	internal string ResourcePath => m_resourcePath;

	public bool IsEmpty => string.IsNullOrEmpty(m_resourcePath);

	public void SetValues(string resourcePath, string GUID, string prefabPath)
	{
		if (!Application.isEditor)
		{
			return;
		}
		while (true)
		{
			m_resourcePath = resourcePath;
			m_GUID = GUID;
			m_debugPrefabPath = prefabPath;
			return;
		}
	}

	public GameObject GetPrefab(bool returnNullOnLoadFail = false)
	{
		SavedResourceLink value = null;
		if (string.IsNullOrEmpty(m_resourcePath))
		{
			Log.Warning("Attempted to get prefab for NULL or empty resource path, ignoring.");
		}
		else if (!s_loadedResourceLinks.TryGetValue(m_resourcePath, out value))
		{
			GameObject gameObject = Resources.Load(m_resourcePath) as GameObject;
			if (gameObject == null)
			{
				while (true)
				{
					if (returnNullOnLoadFail)
					{
						if (Application.isEditor)
						{
							Log.Error("Failed to load Resource Link prefab from " + m_resourcePath);
						}
						return null;
					}
					throw new ApplicationException("Failed to load Resource Link prefab from " + m_resourcePath);
				}
			}
			value = AddLoadedLink(gameObject);
		}
		object result;
		if (value == null)
		{
			result = null;
		}
		else
		{
			result = value.prefabReference;
		}
		return (GameObject)result;
	}

	public IEnumerator PreLoadPrefabAsync()
	{
		if (string.IsNullOrEmpty(m_resourcePath))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Log.Warning("Attempted to load NULL or empty resource path, ignoring.");
					yield break;
				}
			}
		}
		if (s_loadedResourceLinks.TryGetValue(m_resourcePath, out SavedResourceLink _))
		{
			yield break;
		}
		while (true)
		{
			yield return Resources.LoadAsync(m_resourcePath);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}

	internal void UnloadPrefab()
	{
		if (!s_loadedResourceLinks.TryGetValue(m_resourcePath, out SavedResourceLink value) || !(value != null))
		{
			return;
		}
		while (true)
		{
			if (value.prefabReference != null)
			{
				while (true)
				{
					s_loadedResourceLinks.Remove(m_resourcePath);
					return;
				}
			}
			return;
		}
	}

	internal static bool HasLoadedResourceLinkForPath(string resourcePath)
	{
		int result;
		if (s_loadedResourceLinks != null)
		{
			result = (s_loadedResourceLinks.ContainsKey(resourcePath) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private SavedResourceLink AddLoadedLink(GameObject loadedLinkObject)
	{
		if (loadedLinkObject == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Log.Error("Could not load saved Resource Link from: " + m_resourcePath);
					return null;
				}
			}
		}
		SavedResourceLink component = loadedLinkObject.GetComponent<SavedResourceLink>();
		if (component == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Log.Error("Could not load saved Resource Link at [" + m_resourcePath + "] does not have a SavedResourceLink component");
					return null;
				}
			}
		}
		if (component.prefabReference == null)
		{
			Log.Error("Resource Link at [" + m_resourcePath + "] has a null prefab reference.  This can happen if the referenced prefab was deleted.  The original path was [" + m_debugPrefabPath + "]");
			return null;
		}
		if (s_loadedResourceLinks.ContainsKey(m_resourcePath))
		{
			Log.Error("Prefab resource link already contains a loaded entry for path - replacing it: " + m_resourcePath);
			s_loadedResourceLinks[m_resourcePath] = component;
		}
		else
		{
			s_loadedResourceLinks.Add(m_resourcePath, component);
		}
		return component;
	}

	public GameObject InstantiatePrefab(bool returnNullOnNullPrefab = false)
	{
		GameObject prefab = GetPrefab(returnNullOnNullPrefab);
		if (prefab == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		return UnityEngine.Object.Instantiate(prefab);
	}

	internal static void Stream(IBitStream stream, ref PrefabResourceLink link)
	{
		if (link == null)
		{
			link = new PrefabResourceLink();
		}
		stream.Serialize(ref link.m_resourcePath);
		stream.Serialize(ref link.m_GUID);
	}

	internal static void UnloadAll()
	{
		s_loadedResourceLinks.Clear();
	}

	public override string ToString()
	{
		bool flag = false;
		if (Application.isEditor)
		{
			flag = true;
		}
		if (flag)
		{
			if (!string.IsNullOrEmpty(m_debugPrefabPath))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return m_debugPrefabPath;
					}
				}
			}
		}
		return $"GUID: {m_GUID}";
	}

	public string GetResourcePath()
	{
		return m_resourcePath;
	}
}
