using System;
using UnityEngine;

[ExecuteInEditMode]
public class ReplaceableVFXPrefab : MonoBehaviour, IGameEventListener
{
	public PrefabResourceLink m_prefabLink;

	private GameObject m_prefab;

	private bool m_prefabInstantiated;

	private void Awake()
	{
		if (Application.isPlaying)
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReplaceVFXPrefab);
		}
	}

	private void Start()
	{
		this.m_prefabInstantiated = (base.transform.childCount > 0);
		if (base.transform.childCount > 1)
		{
			Log.Error("VFX prefab bug: {0} has {1} children of the ReplaceableVFXPrefab object, but should have at most 1", new object[]
			{
				base.gameObject.name,
				base.transform.childCount
			});
		}
		if (!this.m_prefabInstantiated)
		{
			this.m_prefabInstantiated = this.InstantiatePrefab();
		}
	}

	private bool InstantiatePrefab()
	{
		bool flag = false;
		if (this.m_prefab == null)
		{
			this.m_prefab = this.m_prefabLink.GetPrefab(true);
		}
		if (this.m_prefab != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_prefab);
			if (gameObject != null)
			{
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				flag = true;
				if (!Application.isPlaying)
				{
					gameObject.name = string.Format("{0}_DoNotEdit_AutoCreatedByParentReplaceableVFXPrefab", gameObject.name);
				}
			}
		}
		if (!flag)
		{
			if (Application.isPlaying)
			{
				string message = "Failed to instantiate prefab link {0}";
				object[] array = new object[1];
				int num = 0;
				object obj;
				if (this.m_prefabLink == null)
				{
					obj = "NULL";
				}
				else
				{
					obj = this.m_prefabLink.ResourcePath;
				}
				array[num] = obj;
				Log.Error(message, array);
			}
			else
			{
				string format = "Failed to instantiate prefab link {0}";
				object[] array2 = new object[1];
				int num2 = 0;
				object obj2;
				if (this.m_prefabLink == null)
				{
					obj2 = "NULL";
				}
				else
				{
					obj2 = this.m_prefabLink.ResourcePath;
				}
				array2[num2] = obj2;
				Debug.LogErrorFormat(format, array2);
			}
		}
		return flag;
	}

	private void OnDestroy()
	{
		if (Application.isPlaying)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReplaceVFXPrefab);
		}
	}

	void IGameEventListener.OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		PrefabResourceLink prefabResourceLink = this.m_prefabLink;
		if (this != null)
		{
			if (base.transform != null)
			{
				GameEventManager.ReplaceVFXPrefab replaceVFXPrefab = (GameEventManager.ReplaceVFXPrefab)args;
				if (!this.m_prefabInstantiated && replaceVFXPrefab != null)
				{
					if (replaceVFXPrefab.vfxRoot != null && replaceVFXPrefab.vfxRoot == base.transform.root)
					{
						prefabResourceLink = replaceVFXPrefab.characterResourceLink.ReplacePrefabResourceLink(prefabResourceLink, replaceVFXPrefab.characterVisualInfo);
						if (prefabResourceLink != null)
						{
							this.m_prefab = prefabResourceLink.GetPrefab(false);
						}
						this.m_prefabInstantiated = this.InstantiatePrefab();
						goto IL_174;
					}
				}
				if (replaceVFXPrefab == null)
				{
					string text;
					if (base.transform.parent != null)
					{
						text = base.transform.parent.name;
					}
					else if (this.m_prefabLink != null)
					{
						text = this.m_prefabLink.ToString();
					}
					else
					{
						text = base.transform.name;
					}
					string str = text;
					Log.Error("ReplaceableVFXPRefab on " + str + " received event with null argument", new object[0]);
				}
				IL_174:
				return;
			}
		}
		Log.Error("ReplaceableVFXPRefab reference or transform is null on event " + eventType.ToString(), new object[0]);
	}
}
