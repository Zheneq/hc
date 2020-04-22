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
		if (RuntimeStaticSceneContainer != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIManager.SetGameObjectActive(RuntimeStaticSceneContainer, visible);
		}
		if (RuntimeSemiStaticSceneContainer != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(RuntimeSemiStaticSceneContainer, visible);
		}
		if (RuntimeCameraMovementSceneContainer != null)
		{
			UIManager.SetGameObjectActive(RuntimeCameraMovementSceneContainer, visible);
		}
		if (!(RuntimePerFrameSceneContainer != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			UIManager.SetGameObjectActive(RuntimePerFrameSceneContainer, visible);
			return;
		}
	}
}
