using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UILayerManager
{
	public CanvasLayerInfo[] CanvasLayers;

	private GameObject LayersContainer;

	[NonSerialized]
	private UICameraLayerInfo m_parentInfo;

	private bool init;

	public UICameraLayerInfo ParentInfo
	{
		get
		{
			return this.m_parentInfo;
		}
	}

	public int ObjectLayerValue
	{
		get
		{
			int result;
			if (this.m_parentInfo.CamType == RenderMode.WorldSpace)
			{
				result = 0xC;
			}
			else
			{
				result = 5;
			}
			return result;
		}
	}

	public int SetSceneVisible(IEnumerable<SceneType> aScenes, bool visible, SceneVisibilityParameters parameters)
	{
		int num = 0;
		for (int i = 0; i < this.CanvasLayers.Length; i++)
		{
			num += this.CanvasLayers[i].SetSceneVisible(aScenes, visible, parameters);
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UILayerManager.SetSceneVisible(IEnumerable<SceneType>, bool, SceneVisibilityParameters)).MethodHandle;
		}
		return num;
	}

	public void Init(UICameraLayerInfo info)
	{
		if (!this.init)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UILayerManager.Init(UICameraLayerInfo)).MethodHandle;
			}
			this.init = true;
			this.m_parentInfo = info;
			this.LayersContainer = new GameObject();
			this.LayersContainer.name = "Layer Container";
			UIManager.ReparentTransform(this.LayersContainer.transform, this.m_parentInfo.CameraLayerContainer.gameObject.transform);
			List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
			for (int i = 0; i < this.CanvasLayers.Length; i++)
			{
				this.CanvasLayers[i].ScenesContainer = new GameObject();
				this.CanvasLayers[i].ScenesContainer.name = "(Canvas Container)" + this.CanvasLayers[i].CanvasLayerName;
				UIManager.ReparentTransform(this.CanvasLayers[i].ScenesContainer.transform, this.LayersContainer.gameObject.transform);
				this.CanvasLayers[i].Init(this);
				KeyValuePair<int, int> item = new KeyValuePair<int, int>(i, this.CanvasLayers[i].LayerPriority);
				list.Add(item);
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (list.Count > 1)
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
				List<KeyValuePair<int, int>> list2 = list;
				if (UILayerManager.<>f__am$cache0 == null)
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
					UILayerManager.<>f__am$cache0 = delegate(KeyValuePair<int, int> keyA, KeyValuePair<int, int> keyB)
					{
						if (keyA.Value > keyB.Value)
						{
							return 1;
						}
						if (keyA.Value < keyB.Value)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!true)
							{
								RuntimeMethodHandle runtimeMethodHandle2 = methodof(UILayerManager.<Init>m__0(KeyValuePair<int, int>, KeyValuePair<int, int>)).MethodHandle;
							}
							return -1;
						}
						return 0;
					};
				}
				list2.Sort(UILayerManager.<>f__am$cache0);
				for (int j = 0; j < list.Count; j++)
				{
					this.CanvasLayers[list[j].Key].ScenesContainer.transform.SetAsLastSibling();
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
			this.LayersContainer.SetLayerRecursively(this.ObjectLayerValue);
		}
	}

	public Canvas GetBatchCanvas(IUIScene theScene, CanvasBatchType type)
	{
		Canvas canvas = null;
		for (int i = 0; i < this.CanvasLayers.Length; i++)
		{
			canvas = this.CanvasLayers[i].GetBatchCanvas(theScene, type);
			if (canvas != null)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UILayerManager.GetBatchCanvas(IUIScene, CanvasBatchType)).MethodHandle;
				}
				return canvas;
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			return canvas;
		}
	}

	public Canvas GetDefaultCanvas(IUIScene theScene)
	{
		Canvas canvas = null;
		for (int i = 0; i < this.CanvasLayers.Length; i++)
		{
			canvas = this.CanvasLayers[i].GetDefaultCanvas(theScene);
			if (canvas != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UILayerManager.GetDefaultCanvas(IUIScene)).MethodHandle;
				}
				return canvas;
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			return canvas;
		}
	}

	public Canvas GetDefaultCanvas(SceneType theScene)
	{
		Canvas canvas = null;
		for (int i = 0; i < this.CanvasLayers.Length; i++)
		{
			canvas = this.CanvasLayers[i].GetDefaultCanvas(theScene);
			if (canvas != null)
			{
				return canvas;
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UILayerManager.GetDefaultCanvas(SceneType)).MethodHandle;
			return canvas;
		}
		return canvas;
	}

	public int GetNameplateCanvasLayer()
	{
		int num = -1;
		for (int i = 0; i < this.CanvasLayers.Length; i++)
		{
			num = this.CanvasLayers[i].GetNameplateCanvasLayer();
			if (num != -1)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UILayerManager.GetNameplateCanvasLayer()).MethodHandle;
				}
				break;
			}
		}
		return num;
	}

	public RuntimeSceneInfo RegisterUIScene(IUIScene scene)
	{
		for (int i = 0; i < this.CanvasLayers.Length; i++)
		{
			RuntimeSceneInfo runtimeSceneInfo = this.CanvasLayers[i].RegisterUIScene(scene);
			if (runtimeSceneInfo != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UILayerManager.RegisterUIScene(IUIScene)).MethodHandle;
				}
				runtimeSceneInfo.RuntimeSceneContainer.SetLayerRecursively(this.ObjectLayerValue);
				return runtimeSceneInfo;
			}
		}
		return null;
	}

	public List<UISceneDisplayInfo> SetGameState(UIManager.ClientState newState)
	{
		List<UISceneDisplayInfo> list = new List<UISceneDisplayInfo>();
		for (int i = 0; i < this.CanvasLayers.Length; i++)
		{
			list.AddRange(this.CanvasLayers[i].SetGameState(newState));
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UILayerManager.SetGameState(UIManager.ClientState)).MethodHandle;
		}
		return list;
	}
}
