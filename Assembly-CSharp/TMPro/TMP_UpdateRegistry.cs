using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	public class TMP_UpdateRegistry
	{
		private static TMP_UpdateRegistry s_Instance;

		private readonly List<ICanvasElement> m_LayoutRebuildQueue = new List<ICanvasElement>();

		private Dictionary<int, int> m_LayoutQueueLookup = new Dictionary<int, int>();

		private readonly List<ICanvasElement> m_GraphicRebuildQueue = new List<ICanvasElement>();

		private Dictionary<int, int> m_GraphicQueueLookup = new Dictionary<int, int>();

		protected TMP_UpdateRegistry()
		{
			Canvas.willRenderCanvases += this.PerformUpdateForCanvasRendererObjects;
		}

		public static TMP_UpdateRegistry instance
		{
			get
			{
				if (TMP_UpdateRegistry.s_Instance == null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_UpdateRegistry.get_instance()).MethodHandle;
					}
					TMP_UpdateRegistry.s_Instance = new TMP_UpdateRegistry();
				}
				return TMP_UpdateRegistry.s_Instance;
			}
		}

		public static void RegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			TMP_UpdateRegistry.instance.InternalRegisterCanvasElementForLayoutRebuild(element);
		}

		private bool InternalRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			int instanceID = (element as UnityEngine.Object).GetInstanceID();
			if (this.m_LayoutQueueLookup.ContainsKey(instanceID))
			{
				return false;
			}
			this.m_LayoutQueueLookup[instanceID] = instanceID;
			this.m_LayoutRebuildQueue.Add(element);
			return true;
		}

		public static void RegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			TMP_UpdateRegistry.instance.InternalRegisterCanvasElementForGraphicRebuild(element);
		}

		private bool InternalRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			int instanceID = (element as UnityEngine.Object).GetInstanceID();
			if (this.m_GraphicQueueLookup.ContainsKey(instanceID))
			{
				return false;
			}
			this.m_GraphicQueueLookup[instanceID] = instanceID;
			this.m_GraphicRebuildQueue.Add(element);
			return true;
		}

		private void PerformUpdateForCanvasRendererObjects()
		{
			for (int i = 0; i < this.m_LayoutRebuildQueue.Count; i++)
			{
				ICanvasElement canvasElement = TMP_UpdateRegistry.instance.m_LayoutRebuildQueue[i];
				canvasElement.Rebuild(CanvasUpdate.Prelayout);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_UpdateRegistry.PerformUpdateForCanvasRendererObjects()).MethodHandle;
			}
			if (this.m_LayoutRebuildQueue.Count > 0)
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
				this.m_LayoutRebuildQueue.Clear();
				this.m_LayoutQueueLookup.Clear();
			}
			for (int j = 0; j < this.m_GraphicRebuildQueue.Count; j++)
			{
				ICanvasElement canvasElement2 = TMP_UpdateRegistry.instance.m_GraphicRebuildQueue[j];
				canvasElement2.Rebuild(CanvasUpdate.PreRender);
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
			if (this.m_GraphicRebuildQueue.Count > 0)
			{
				this.m_GraphicRebuildQueue.Clear();
				this.m_GraphicQueueLookup.Clear();
			}
		}

		private void PerformUpdateForMeshRendererObjects()
		{
			Debug.Log("Perform update of MeshRenderer objects.");
		}

		public static void UnRegisterCanvasElementForRebuild(ICanvasElement element)
		{
			TMP_UpdateRegistry.instance.InternalUnRegisterCanvasElementForLayoutRebuild(element);
			TMP_UpdateRegistry.instance.InternalUnRegisterCanvasElementForGraphicRebuild(element);
		}

		private void InternalUnRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			int instanceID = (element as UnityEngine.Object).GetInstanceID();
			TMP_UpdateRegistry.instance.m_LayoutRebuildQueue.Remove(element);
			this.m_GraphicQueueLookup.Remove(instanceID);
		}

		private void InternalUnRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			int instanceID = (element as UnityEngine.Object).GetInstanceID();
			TMP_UpdateRegistry.instance.m_GraphicRebuildQueue.Remove(element);
			this.m_LayoutQueueLookup.Remove(instanceID);
		}
	}
}
