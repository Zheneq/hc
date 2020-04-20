using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

internal class ReplayPlayManager : MonoBehaviour, IGameEventListener
{
	private static ReplayPlayManager s_instance;

	private bool m_appStateLoadingPassed;

	private Replay m_playingReplay;

	private Replay m_prospectiveReplay;

	private bool m_fastForward;

	private ReplayTimestamp m_seekTarget = default(ReplayTimestamp);

	internal static ReplayPlayManager Get()
	{
		return ReplayPlayManager.s_instance;
	}

	private void Awake()
	{
		ReplayPlayManager.s_instance = this;
		this.ResetState();
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.AppStateChanged);
	}

	private void ResetState()
	{
		this.m_seekTarget = default(ReplayTimestamp);
	}

	private void Update()
	{
		if (this.m_playingReplay != null)
		{
			if (GameFlowData.Get() != null)
			{
				if (ReplayTimestamp.Current() < this.m_seekTarget)
				{
					Log.Info("Fastforwarding replay to {0} . . .", new object[]
					{
						this.m_seekTarget
					});
					this.FastForward(this.m_seekTarget);
					return;
				}
			}
			this.m_playingReplay.PlaybackUpdate();
		}
	}

	private void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.AppStateChanged);
		ReplayPlayManager.s_instance = null;
	}

	public void StartReplay(string filename)
	{
		if (AppState.IsInGame())
		{
			TextConsole.Get().Write("Failed to start replay: already ingame", ConsoleMessageType.SystemMessage);
			return;
		}
		this.ResetState();
		try
		{
			this.m_prospectiveReplay = JsonUtility.FromJson<Replay>(File.ReadAllText(filename));
		}
		catch (Exception ex)
		{
			Debug.LogFormat("Failed; {0}", new object[]
			{
				ex
			});
			try
			{
				this.m_prospectiveReplay = JsonUtility.FromJson<Replay>(File.ReadAllText(filename + ".arr"));
			}
			catch (Exception ex2)
			{
				Debug.LogFormat("Failed; {0}", new object[]
				{
					ex2
				});
				TextConsole.Get().Write("Failed to start replay: file could not be read", ConsoleMessageType.SystemMessage);
			}
		}
		if (this.m_prospectiveReplay == null)
		{
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ReplayIssue", "FrontEnd"), StringUtil.TR("InvalidReplay", "FrontEnd"), StringUtil.TR("Ok", "Global"), null, -1, false);
		}
		else if (this.m_prospectiveReplay.m_versionMini != BuildVersion.MiniVersionString)
		{
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ReplayIssue", "FrontEnd"), StringUtil.TR("ObsoleteReplay", "FrontEnd"), StringUtil.TR("Ok", "Global"), null, -1, false);
		}
		else if (this.m_prospectiveReplay.m_versionFull != BuildVersion.FullVersionString)
		{
			UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("ReplayIssue", "FrontEnd"), StringUtil.TR("OldReplay", "FrontEnd"), StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate(UIDialogBox _)
			{
				this.m_playingReplay = this.m_prospectiveReplay;
				this.m_prospectiveReplay = null;
				this.m_appStateLoadingPassed = false;
				this.m_playingReplay.StartPlayback();
			}, null, false, false);
		}
		else
		{
			this.m_playingReplay = this.m_prospectiveReplay;
			this.m_prospectiveReplay = null;
			this.m_appStateLoadingPassed = false;
			this.m_playingReplay.StartPlayback();
		}
	}

	public bool IsPlayback()
	{
		return this.m_playingReplay != null;
	}

	public PersistedCharacterMatchData GetPlaybackMatchData()
	{
		if (this.m_playingReplay != null)
		{
			return this.m_playingReplay.GetMatchData();
		}
		return null;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.AppStateChanged)
		{
			if (this.m_appStateLoadingPassed)
			{
				if (this.m_playingReplay != null)
				{
					if (!AppState.IsInGame())
					{
						if (AppState.GetCurrent() != AppState_GameLoading.Get())
						{
							this.m_playingReplay.FinishPlayback();
							this.m_playingReplay = null;
						}
					}
				}
			}
			else if (AppState.GetCurrent() == AppState_GameLoading.Get())
			{
				this.m_appStateLoadingPassed = true;
			}
		}
	}

	private void FastForward(ReplayTimestamp target)
	{
		if (this.m_playingReplay != null)
		{
			this.m_fastForward = true;
			try
			{
				this.m_playingReplay.PlaybackFastForward(target);
			}
			finally
			{
				this.m_fastForward = false;
			}
			Log.Info("{0}: Fastforwarded", new object[]
			{
				Time.unscaledTime
			});
			if (GameFlowData.Get() != null)
			{
				if (ReplayTimestamp.Current() >= target)
				{
					GameEventManager.Get().FireEvent(GameEventManager.EventType.ReplaySeekFinished, null);
				}
			}
		}
	}

	public void Restart()
	{
		if (this.m_playingReplay != null)
		{
			if (GameFlowData.Get())
			{
				using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().GetActors().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						actorData.OnReplayRestart();
					}
				}
			}
			foreach (NetworkIdentity networkIdentity in UnityEngine.Object.FindObjectsOfType<NetworkIdentity>())
			{
				if (networkIdentity.sceneId.IsEmpty())
				{
					if (!(networkIdentity.gameObject.GetComponent<CameraManager>() != null))
					{
						UnityEngine.Object.Destroy(networkIdentity.gameObject);
					}
				}
			}
			if (SequenceManager.Get())
			{
				SequenceManager.Get().ClearAllSequences();
			}
			GameEventManager.Get().FireEvent(GameEventManager.EventType.ReplayRestart, null);
			this.m_playingReplay.PlaybackRestart();
		}
	}

	public void Seek(ReplayTimestamp target)
	{
		Log.Info("{0}: Setting replay seek to {1} . . .", new object[]
		{
			Time.unscaledTime,
			target
		});
		this.m_seekTarget = target;
		if (target <= ReplayTimestamp.Current())
		{
			this.Restart();
		}
		this.Update();
	}

	public bool IsFastForward()
	{
		return this.m_fastForward;
	}

	public void Pause()
	{
		GameTime.scale = 0f;
	}

	public void Resume()
	{
		GameTime.scale = 1f;
	}
}
