using System;
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
		this.previousMixState = this.snapshot_match_intro;
	}

	public void SetMix_DecisionCam()
	{
		if (this.previousMixState == this.snapshot_match_intro)
		{
			this.snapshot_game.TransitionToOnRealMixer(this.transitionTime_match_intro_to_match_start);
		}
		else if (this.previousMixState == this.snapshot_taunt)
		{
			this.snapshot_resolve.TransitionToOnRealMixer(this.transitionTime_resolve_from_taunt);
		}
		else
		{
			if (this.previousMixState == this.snapshot_resolve)
			{
				this.snapshot_resolve.TransitionToOnRealMixer(this.transitionTime_decision_from_action);
			}
			if (!(this.previousMixState != this.snapshot_taunt))
			{
				if (!(this.previousMixState != this.snapshot_resolve))
				{
					goto IL_D7;
				}
			}
			this.snapshot_game.TransitionToOnRealMixer(this.transitionTime_game);
		}
		IL_D7:
		this.previousMixState = this.snapshot_game;
	}

	public void SetMix_TauntCam()
	{
		this.snapshot_taunt.TransitionToOnRealMixer(this.transitionTime_taunt);
		this.previousMixState = this.snapshot_taunt;
	}

	public void SetMix_ResolveCam()
	{
		if (this.previousMixState == this.snapshot_taunt)
		{
			this.snapshot_resolve.TransitionToOnRealMixer(this.transitionTime_resolve_from_taunt);
		}
		else
		{
			this.snapshot_resolve.TransitionToOnRealMixer(this.transitionTime_resolve);
		}
		this.previousMixState = this.snapshot_resolve;
	}

	public void SetMix_Menu()
	{
		this.snapshot_menu.TransitionToOnRealMixer(this.transitionTime_menu);
		this.previousMixState = this.snapshot_menu;
	}

	public void SetMix_LoadingScreen()
	{
		this.snapshot_loading.TransitionToOnRealMixer(this.transitionTime_loading_screen);
		this.previousMixState = this.snapshot_loading;
	}

	public void SetMix_GameOver()
	{
		this.snapshot_game_over.TransitionToOnRealMixer(this.transitionTime_snapshot_game_over);
	}

	public void SetMix_StartVideoPlayback()
	{
		this.snapshot_video.TransitionToOnRealMixer(this.transitionTime_video_playback);
	}

	public void SetMix_StopVideoPlayback()
	{
		this.previousMixState.TransitionToOnRealMixer(this.transitionTime_video_playback);
	}

	public void SetMix_MatchIntro()
	{
		this.snapshot_match_intro.TransitionToOnRealMixer(this.transitionTime_match_intro_from_menu);
		this.previousMixState = this.snapshot_match_intro;
	}
}
