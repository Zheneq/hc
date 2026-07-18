public class FrontEndFullScreenAnnouncements : UIScene
{
	private UIIntroductions[] Introductions;

	private static FrontEndFullScreenAnnouncements s_instance;

	public static FrontEndFullScreenAnnouncements Get()
	{
		return s_instance;
	}

	public override void Awake()
	{
		s_instance = this;
		Introductions = base.gameObject.GetComponentsInChildren<UIIntroductions>(true);
		base.Awake();
	}

	public override SceneType GetSceneType()
	{
		return SceneType.FrontEndFullScreenAnnouncements;
	}

	public void SetIntroductionVisible(AccountComponent.UIStateIdentifier UIState, int pageNum = 0)
	{
		if (Introductions == null)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < Introductions.Length; i++)
			{
				if (Introductions[i].UIState == UIState)
				{
					Introductions[i].DisplayIntroduction(pageNum);
				}
			}
			return;
		}
	}
}
