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
		m_playBtn.spriteController.callback = PlayClicked;
		m_pauseBtn.spriteController.callback = PauseClicked;
		m_rewindBtn.spriteController.callback = RewindClicked;
		m_fastForwardBtn.spriteController.callback = FFClicked;
		UIManager.SetGameObjectActive(m_playBtn, false);
		if (ReplayPlayManager.Get() != null)
		{
			while (true)
			{
				switch (1)
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
			if (ReplayPlayManager.Get().IsPlayback())
			{
				UIManager.SetGameObjectActive(base.gameObject, true);
				return;
			}
		}
		UIManager.SetGameObjectActive(base.gameObject, false);
	}

	private void PlayClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_playBtn, false);
		UIManager.SetGameObjectActive(m_pauseBtn, true);
		ReplayPlayManager.Get().Resume();
	}

	private void PauseClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_playBtn, true);
		UIManager.SetGameObjectActive(m_pauseBtn, false);
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
		m_fastForwardBtn.SetDisabled(ReplayPlayManager.Get().IsFastForward());
	}
}
