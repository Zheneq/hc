using System;
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
		return UICreditsScreen.s_instance;
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
		UICreditsScreen.s_instance = this;
		UIManager.SetGameObjectActive(this.m_container, false, null);
		this.m_isVisible = false;
		this.m_closeBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseClicked);
		this.m_pauseBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PauseClicked);
		this.m_playBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PlayClicked);
		this.m_startPos += 600f;
		this.m_endPos += 600f;
	}

	private void OnDestroy()
	{
		if (UICreditsScreen.s_instance == this)
		{
			UICreditsScreen.s_instance = null;
		}
	}

	public void SetVisible(bool visible)
	{
		if (visible)
		{
			this.m_wasCharModelShown = UICharacterStoreAndProgressWorldObjects.Get().IsVisible();
			UICharacterStoreAndProgressWorldObjects.Get().SetVisible(false);
		}
		else if (this.m_wasCharModelShown)
		{
			this.m_wasCharModelShown = false;
			UICharacterStoreAndProgressWorldObjects.Get().SetVisible(true);
		}
		this.m_scrollBox.localPosition = new Vector3(0f, this.m_startPos, 0f);
		this.m_isPlaying = true;
		this.m_isVisible = visible;
		UIManager.SetGameObjectActive(this.m_pauseBtn, true, null);
		UIManager.SetGameObjectActive(this.m_playBtn, false, null);
		UIManager.SetGameObjectActive(this.m_container, visible, null);
	}

	private void CloseClicked(BaseEventData data)
	{
		this.SetVisible(false);
	}

	private void PlayClicked(BaseEventData date)
	{
		this.m_isPlaying = true;
		UIManager.SetGameObjectActive(this.m_pauseBtn, true, null);
		UIManager.SetGameObjectActive(this.m_playBtn, false, null);
	}

	private void PauseClicked(BaseEventData date)
	{
		this.m_isPlaying = false;
		UIManager.SetGameObjectActive(this.m_pauseBtn, false, null);
		UIManager.SetGameObjectActive(this.m_playBtn, true, null);
	}

	private void Update()
	{
		if (this.m_isPlaying)
		{
			if (!this.m_isVisible)
			{
			}
			else
			{
				float num = this.m_scrollBox.localPosition.y;
				if (num >= this.m_endPos)
				{
					return;
				}
				num += Time.deltaTime * this.m_movePerSecond;
				if (num > this.m_endPos)
				{
					num = this.m_endPos;
				}
				this.m_scrollBox.localPosition = new Vector3(0f, num, 0f);
				return;
			}
		}
	}
}
