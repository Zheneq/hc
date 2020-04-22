using UnityEngine;
using UnityEngine.EventSystems;

public class UICreditsScreen : UIScene
{
	public RectTransform m_container;

	public RectTransform m_scrollBox;

	public _SelectableBtn m_closeBtn;

	public _SelectableBtn m_pauseBtn;

	public _SelectableBtn m_playBtn;

	public float m_startPos = -1250f;

	public float m_endPos = 13600f;

	public float m_movePerSecond = 100f;

	private bool m_isVisible;

	private bool m_isPlaying;

	private bool m_wasCharModelShown;

	private static UICreditsScreen s_instance;

	public static UICreditsScreen Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.Credits;
	}

	public override void Awake()
	{
		base.Awake();
	}

	private void Start()
	{
		s_instance = this;
		UIManager.SetGameObjectActive(m_container, false);
		m_isVisible = false;
		m_closeBtn.spriteController.callback = CloseClicked;
		m_pauseBtn.spriteController.callback = PauseClicked;
		m_playBtn.spriteController.callback = PlayClicked;
		m_startPos += 600f;
		m_endPos += 600f;
	}

	private void OnDestroy()
	{
		if (s_instance == this)
		{
			s_instance = null;
		}
	}

	public void SetVisible(bool visible)
	{
		if (visible)
		{
			m_wasCharModelShown = UICharacterStoreAndProgressWorldObjects.Get().IsVisible();
			UICharacterStoreAndProgressWorldObjects.Get().SetVisible(false);
		}
		else if (m_wasCharModelShown)
		{
			while (true)
			{
				switch (5)
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
			m_wasCharModelShown = false;
			UICharacterStoreAndProgressWorldObjects.Get().SetVisible(true);
		}
		m_scrollBox.localPosition = new Vector3(0f, m_startPos, 0f);
		m_isPlaying = true;
		m_isVisible = visible;
		UIManager.SetGameObjectActive(m_pauseBtn, true);
		UIManager.SetGameObjectActive(m_playBtn, false);
		UIManager.SetGameObjectActive(m_container, visible);
	}

	private void CloseClicked(BaseEventData data)
	{
		SetVisible(false);
	}

	private void PlayClicked(BaseEventData date)
	{
		m_isPlaying = true;
		UIManager.SetGameObjectActive(m_pauseBtn, true);
		UIManager.SetGameObjectActive(m_playBtn, false);
	}

	private void PauseClicked(BaseEventData date)
	{
		m_isPlaying = false;
		UIManager.SetGameObjectActive(m_pauseBtn, false);
		UIManager.SetGameObjectActive(m_playBtn, true);
	}

	private void Update()
	{
		if (!m_isPlaying)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_isVisible)
			{
				while (true)
				{
					switch (5)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			Vector3 localPosition = m_scrollBox.localPosition;
			float y = localPosition.y;
			if (y >= m_endPos)
			{
				return;
			}
			y += Time.deltaTime * m_movePerSecond;
			if (y > m_endPos)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				y = m_endPos;
			}
			m_scrollBox.localPosition = new Vector3(0f, y, 0f);
			return;
		}
	}
}
