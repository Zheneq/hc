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

	public string GUID
	{
		get
		{
			return this.m_GUID;
		}
	}

	internal string ResourcePath
	{
		get
		{
			return this.m_resourcePath;
		}
	}

	public bool IsEmpty
	{
		get
		{
			return string.IsNullOrEmpty(this.m_resourcePath);
		}
	}

	public void SetValues(string resourcePath, string GUID, string prefabPath)
	{
		if (Application.isEditor)
		{
			this.m_resourcePath = resourcePath;
			this.m_GUID = GUID;
			this.m_debugPrefabPath = prefabPath;
		}
	}

	public GameObject GetPrefab(bool returnNullOnLoadFail = false)
	{
		SavedResourceLink savedResourceLink = null;
		if (string.IsNullOrEmpty(this.m_resourcePath))
		{
			Log.Warning("Attempted to get prefab for NULL or empty resource path, ignoring.", new object[0]);
		}
		else if (!PrefabResourceLink.s_loadedResourceLinks.TryGetValue(this.m_resourcePath, out savedResourceLink))
		{
			GameObject gameObject = Resources.Load(this.m_resourcePath) as GameObject;
			if (gameObject == null)
			{
				if (returnNullOnLoadFail)
				{
					if (Application.isEditor)
					{
						Log.Error("Failed to load Resource Link prefab from " + this.m_resourcePath, new object[0]);
					}
					return null;
				}
				throw new ApplicationException("Failed to load Resource Link prefab from " + this.m_resourcePath);
			}
			else
			{
				savedResourceLink = this.AddLoadedLink(gameObject);
			}
		}
		GameObject result;
		if (savedResourceLink == null)
		{
			result = null;
		}
		else
		{
			result = savedResourceLink.prefabReference;
		}
		return result;
	}

	public IEnumerator PreLoadPrefabAsync()
	{
		SavedResourceLink loadedLink;
		if (string.IsNullOrEmpty(this.m_resourcePath))
		{
			Log.Warning("Attempted to load NULL or empty resource path, ignoring.", new object[0]);
		}
		else if (!PrefabResourceLink.s_loadedResourceLinks.TryGetValue(this.m_resourcePath, out loadedLink))
		{
			ResourceRequest request = Resources.LoadAsync(this.m_resourcePath);
			yield return request;
			if (request != null)
			{
				if (request.asset != null)
				{
					this.AddLoadedLink(request.asset as GameObject);
					goto IL_13C;
				}
			}
			Log.Error("Prefab load failed for {0}", new object[]
			{
				this.m_resourcePath
			});
		}
		IL_13C:
		yield break;
	}

	internal void UnloadPrefab()
	{
		SavedResourceLink savedResourceLink;
		if (PrefabResourceLink.s_loadedResourceLinks.TryGetValue(this.m_resourcePath, out savedResourceLink) && savedResourceLink != null)
		{
			if (savedResourceLink.prefabReference != null)
			{
				PrefabResourceLink.s_loadedResourceLinks.Remove(this.m_resourcePath);
			}
		}
	}

	internal static bool HasLoadedResourceLinkForPath(string resourcePath)
	{
		bool result;
		if (PrefabResourceLink.s_loadedResourceLinks != null)
		{
			result = PrefabResourceLink.s_loadedResourceLinks.ContainsKey(resourcePath);
		}
		else
		{
			result = false;
		}
		return result;
	}

	private SavedResourceLink AddLoadedLink(GameObject loadedLinkObject)
	{
		if (loadedLinkObject == null)
		{
			Log.Error("Could not load saved Resource Link from: " + this.m_resourcePath, new object[0]);
			return null;
		}
		SavedResourceLink component = loadedLinkObject.GetComponent<SavedResourceLink>();
		if (component == null)
		{
			Log.Error("Could not load saved Resource Link at [" + this.m_resourcePath + "] does not have a SavedResourceLink component", new object[0]);
			return null;
		}
		if (component.prefabReference == null)
		{
			Log.Error(string.Concat(new string[]
			{
				"Resource Link at [",
				this.m_resourcePath,
				"] has a null prefab reference.  This can happen if the referenced prefab was deleted.  The original path was [",
				this.m_debugPrefabPath,
				"]"
			}), new object[0]);
			return null;
		}
		if (PrefabResourceLink.s_loadedResourceLinks.ContainsKey(this.m_resourcePath))
		{
			Log.Error("Prefab resource link already contains a loaded entry for path - replacing it: " + this.m_resourcePath, new object[0]);
			PrefabResourceLink.s_loadedResourceLinks[this.m_resourcePath] = component;
		}
		else
		{
			PrefabResourceLink.s_loadedResourceLinks.Add(this.m_resourcePath, component);
		}
		return component;
	}

	public GameObject InstantiatePrefab(bool returnNullOnNullPrefab = false)
	{
		GameObject prefab = this.GetPrefab(returnNullOnNullPrefab);
		if (prefab == null)
		{
			return null;
		}
		return UnityEngine.Object.Instantiate<GameObject>(prefab);
	}

	internal unsafe static void Stream(IBitStream stream, ref PrefabResourceLink link)
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
		PrefabResourceLink.s_loadedResourceLinks.Clear();
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
			if (!string.IsNullOrEmpty(this.m_debugPrefabPath))
			{
				return this.m_debugPrefabPath;
			}
		}
		return string.Format("GUID: {0}", this.m_GUID);
	}

	public string GetResourcePath()
	{
		return this.m_resourcePath;
	}
}
