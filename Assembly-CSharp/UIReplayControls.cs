using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIReplayControls : MonoBehaviour
{
	public _SelectableBtn m_playBtn;

	public _SelectableBtn m_pauseBtn;

	public _SelectableBtn m_rewindBtn;

	public _SelectableBtn m_fastForwardBtn;

	private void Start()
	{
		this.m_playBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PlayClicked);
		this.m_pauseBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PauseClicked);
		this.m_rewindBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.RewindClicked);
		this.m_fastForwardBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.FFClicked);
		UIManager.SetGameObjectActive(this.m_playBtn, false, null);
		if (ReplayPlayManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIReplayControls.Start()).MethodHandle;
			}
			if (ReplayPlayManager.Get().IsPlayback())
			{
				UIManager.SetGameObjectActive(base.gameObject, true, null);
				return;
			}
		}
		UIManager.SetGameObjectActive(base.gameObject, false, null);
	}

	private void PlayClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_playBtn, false, null);
		UIManager.SetGameObjectActive(this.m_pauseBtn, true, null);
		ReplayPlayManager.Get().Resume();
	}

	private void PauseClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_playBtn, true, null);
		UIManager.SetGameObjectActive(this.m_pauseBtn, false, null);
		ReplayPlayManager.Get().Pause();
	}

	private void RewindClicked(BaseEventData data)
	{
		ReplayPlayManager.Get().Seek(new ReplayTimestamp
		{
			turn = GameFlowData.Get().CurrentTurn - 1,
			phase = AbilityPriority.INVALID
		});
	}

	private void FFClicked(BaseEventData data)
	{
		ReplayPlayManager.Get().Seek(new ReplayTimestamp
		{
			turn = GameFlowData.Get().CurrentTurn + 1,
			phase = AbilityPriority.INVALID
		});
	}

	private void Update()
	{
		this.m_fastForwardBtn.SetDisabled(ReplayPlayManager.Get().IsFastForward());
	}
}
