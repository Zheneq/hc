using UnityEngine;
using UnityEngine.Audio;

public class MixSnapshots : MonoBehaviour
{
	[Tooltip("Unity Game Mixer snapshot preset ..")]
	public AudioMixerSnapshot snapshot_taunt;

	[Tooltip("Transition time TO this state from other states")]
	public float transitionTime_taunt = 0.3f;

	[Tooltip("Unity Game Mixer snapshot preset ..")]
	public AudioMixerSnapshot snapshot_game;

	[Tooltip("Transition time TO this state from other states")]
	public float transitionTime_game = 0.3f;

	[Tooltip("Unity Game Mixer snapshot preset ..")]
	public AudioMixerSnapshot snapshot_menu;

	[Tooltip("Transition time TO this state from other states")]
	public float transitionTime_menu = 0.1f;

	[Tooltip("Unity Game Mixer snapshot preset ..")]
	public AudioMixerSnapshot snapshot_resolve;

	[Tooltip("Transition time TO this state from any other state")]
	public float transitionTime_resolve = 0.3f;

	[Tooltip("Transition time TO this state when returning from Taunt Cam")]
	public float transitionTime_resolve_from_taunt = 5f;

	[Tooltip("Transition time TO this state when returning from Taunt Cam")]
	public float transitionTime_decision_from_action = 3f;

	[Tooltip("Loading Screen Mixer snapshot preset ..")]
	public AudioMixerSnapshot snapshot_loading;

	[Tooltip("Transition time TO this state when For Loading Screen")]
	public float transitionTime_loading_screen = 1f;

	[Tooltip("Game Over Screen Mixer snapshot preset ..")]
	public AudioMixerSnapshot snapshot_game_over;

	[Tooltip("Transition time TO this state when For Game Over Screen")]
	public float transitionTime_snapshot_game_over = 1f;

	[Tooltip("Video Mixer snapshot preset ..")]
	public AudioMixerSnapshot snapshot_video;

	[Tooltip("Transition time TO this state when For Video Playback")]
	public float transitionTime_video_playback = 0.5f;

	[Tooltip("Match Intro Game Mixer snapshot preset ..")]
	public AudioMixerSnapshot snapshot_match_intro;

	[Tooltip("Transition time TO this state from any other state")]
	public float transitionTime_match_intro_from_menu = 0.3f;

	[Tooltip("Transition time FROM this state TO The next state")]
	public float transitionTime_match_intro_to_match_start = 0.3f;

	[Header("-- Audio --")]
	public AudioMixer m_mixer;

	private AudioMixerSnapshot previousMixState;

	private void Awake()
	{
		AudioManager.SetMixerSnapshotManager(this);
	}

	private void Start()
	{
		previousMixState = snapshot_match_intro;
	}

	public void SetMix_DecisionCam()
	{
		if (previousMixState == snapshot_match_intro)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			snapshot_game.TransitionToOnRealMixer(transitionTime_match_intro_to_match_start);
		}
		else if (previousMixState == snapshot_taunt)
		{
			snapshot_resolve.TransitionToOnRealMixer(transitionTime_resolve_from_taunt);
		}
		else
		{
			if (previousMixState == snapshot_resolve)
			{
				snapshot_resolve.TransitionToOnRealMixer(transitionTime_decision_from_action);
			}
			if (!(previousMixState != snapshot_taunt))
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
				if (!(previousMixState != snapshot_resolve))
				{
					goto IL_00d7;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			snapshot_game.TransitionToOnRealMixer(transitionTime_game);
		}
		goto IL_00d7;
		IL_00d7:
		previousMixState = snapshot_game;
	}

	public void SetMix_TauntCam()
	{
		snapshot_taunt.TransitionToOnRealMixer(transitionTime_taunt);
		previousMixState = snapshot_taunt;
	}

	public void SetMix_ResolveCam()
	{
		if (previousMixState == snapshot_taunt)
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
			snapshot_resolve.TransitionToOnRealMixer(transitionTime_resolve_from_taunt);
		}
		else
		{
			snapshot_resolve.TransitionToOnRealMixer(transitionTime_resolve);
		}
		previousMixState = snapshot_resolve;
	}

	public void SetMix_Menu()
	{
		snapshot_menu.TransitionToOnRealMixer(transitionTime_menu);
		previousMixState = snapshot_menu;
	}

	public void SetMix_LoadingScreen()
	{
		snapshot_loading.TransitionToOnRealMixer(transitionTime_loading_screen);
		previousMixState = snapshot_loading;
	}

	public void SetMix_GameOver()
	{
		snapshot_game_over.TransitionToOnRealMixer(transitionTime_snapshot_game_over);
	}

	public void SetMix_StartVideoPlayback()
	{
		snapshot_video.TransitionToOnRealMixer(transitionTime_video_playback);
	}

	public void SetMix_StopVideoPlayback()
	{
		previousMixState.TransitionToOnRealMixer(transitionTime_video_playback);
	}

	public void SetMix_MatchIntro()
	{
		snapshot_match_intro.TransitionToOnRealMixer(transitionTime_match_intro_from_menu);
		previousMixState = snapshot_match_intro;
	}
}
