using System;

public class FrontEndFullScreenAnnouncements : UIScene
{
	private UIIntroductions[] Introductions;

	private static FrontEndFullScreenAnnouncements s_instance;

	public static FrontEndFullScreenAnnouncements Get()
	{
		return FrontEndFullScreenAnnouncements.s_instance;
	}

	public override void Awake()
	{
		FrontEndFullScreenAnnouncements.s_instance = this;
		this.Introductions = base.gameObject.GetComponentsInChildren<UIIntroductions>(true);
		base.Awake();
	}

	public override SceneType GetSceneType()
	{
		return SceneType.FrontEndFullScreenAnnouncements;
	}

	public void SetIntroductionVisible(AccountComponent.UIStateIdentifier UIState, int pageNum = 0)
	{
		if (this.Introductions != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndFullScreenAnnouncements.SetIntroductionVisible(AccountComponent.UIStateIdentifier, int)).MethodHandle;
			}
			for (int i = 0; i < this.Introductions.Length; i++)
			{
				if (this.Introductions[i].UIState == UIState)
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
					this.Introductions[i].DisplayIntroduction(pageNum);
				}
			}
		}
	}
}
