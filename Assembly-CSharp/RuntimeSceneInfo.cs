using System;
using UnityEngine;

public class RuntimeSceneInfo
{
	public UISceneDisplayInfo DisplayInfo;

	public IUIScene RuntimeScene;

	public GameObject RuntimeSceneContainer;

	public GameObject RuntimeStaticSceneContainer;

	public GameObject RuntimeSemiStaticSceneContainer;

	public GameObject RuntimeCameraMovementSceneContainer;

	public GameObject RuntimePerFrameSceneContainer;

	public void SetBatchScenesVisible(bool visible)
	{
		if (this.RuntimeStaticSceneContainer != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RuntimeSceneInfo.SetBatchScenesVisible(bool)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.RuntimeStaticSceneContainer, visible, null);
		}
		if (this.RuntimeSemiStaticSceneContainer != null)
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
			UIManager.SetGameObjectActive(this.RuntimeSemiStaticSceneContainer, visible, null);
		}
		if (this.RuntimeCameraMovementSceneContainer != null)
		{
			UIManager.SetGameObjectActive(this.RuntimeCameraMovementSceneContainer, visible, null);
		}
		if (this.RuntimePerFrameSceneContainer != null)
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
			UIManager.SetGameObjectActive(this.RuntimePerFrameSceneContainer, visible, null);
		}
	}
}
