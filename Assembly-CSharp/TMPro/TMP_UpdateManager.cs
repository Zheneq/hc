using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	public class TMP_UpdateManager
	{
		private static TMP_UpdateManager s_Instance;

		private readonly List<TMP_Text> m_LayoutRebuildQueue = new List<TMP_Text>();

		private Dictionary<int, int> m_LayoutQueueLookup = new Dictionary<int, int>();

		private readonly List<TMP_Text> m_GraphicRebuildQueue = new List<TMP_Text>();

		private Dictionary<int, int> m_GraphicQueueLookup = new Dictionary<int, int>();

		protected TMP_UpdateManager()
		{
			Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(this.OnCameraPreRender));
		}

		public static TMP_UpdateManager instance
		{
			get
			{
				if (TMP_UpdateManager.s_Instance == null)
				{
					TMP_UpdateManager.s_Instance = new TMP_UpdateManager();
				}
				return TMP_UpdateManager.s_Instance;
			}
		}

		public static void RegisterTextElementForLayoutRebuild(TMP_Text element)
		{
			TMP_UpdateManager.instance.InternalRegisterTextElementForLayoutRebuild(element);
		}

		private bool InternalRegisterTextElementForLayoutRebuild(TMP_Text element)
		{
			int instanceID = element.GetInstanceID();
			if (this.m_LayoutQueueLookup.ContainsKey(instanceID))
			{
				return false;
			}
			this.m_LayoutQueueLookup[instanceID] = instanceID;
			this.m_LayoutRebuildQueue.Add(element);
			return true;
		}

		public static void RegisterTextElementForGraphicRebuild(TMP_Text element)
		{
			TMP_UpdateManager.instance.InternalRegisterTextElementForGraphicRebuild(element);
		}

		private bool InternalRegisterTextElementForGraphicRebuild(TMP_Text element)
		{
			int instanceID = element.GetInstanceID();
			if (this.m_GraphicQueueLookup.ContainsKey(instanceID))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_UpdateManager.InternalRegisterTextElementForGraphicRebuild(TMP_Text)).MethodHandle;
				}
				return false;
			}
			this.m_GraphicQueueLookup[instanceID] = instanceID;
			this.m_GraphicRebuildQueue.Add(element);
			return true;
		}

		private void OnCameraPreRender(Camera cam)
		{
			for (int i = 0; i < this.m_LayoutRebuildQueue.Count; i++)
			{
				this.m_LayoutRebuildQueue[i].Rebuild(CanvasUpdate.Prelayout);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_UpdateManager.OnCameraPreRender(Camera)).MethodHandle;
			}
			if (this.m_LayoutRebuildQueue.Count > 0)
			{
				this.m_LayoutRebuildQueue.Clear();
				this.m_LayoutQueueLookup.Clear();
			}
			for (int j = 0; j < this.m_GraphicRebuildQueue.Count; j++)
			{
				this.m_GraphicRebuildQueue[j].Rebuild(CanvasUpdate.PreRender);
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_GraphicRebuildQueue.Count > 0)
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
				this.m_GraphicRebuildQueue.Clear();
				this.m_GraphicQueueLookup.Clear();
			}
		}

		public static void UnRegisterTextElementForRebuild(TMP_Text element)
		{
			TMP_UpdateManager.instance.InternalUnRegisterTextElementForGraphicRebuild(element);
			TMP_UpdateManager.instance.InternalUnRegisterTextElementForLayoutRebuild(element);
		}

		private void InternalUnRegisterTextElementForGraphicRebuild(TMP_Text element)
		{
			int instanceID = element.GetInstanceID();
			TMP_UpdateManager.instance.m_GraphicRebuildQueue.Remove(element);
			this.m_GraphicQueueLookup.Remove(instanceID);
		}

		private void InternalUnRegisterTextElementForLayoutRebuild(TMP_Text element)
		{
			int instanceID = element.GetInstanceID();
			TMP_UpdateManager.instance.m_LayoutRebuildQueue.Remove(element);
			this.m_LayoutQueueLookup.Remove(instanceID);
		}
	}
}
