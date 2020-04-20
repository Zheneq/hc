using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using WebSocketSharp;

public class DiscordClientInterface : MonoBehaviour
{
	public Action<bool> OnConnected;

	public Action OnDisconnected;

	public Action<string> OnAuthorized;

	public Action<DiscordUserInfo> OnAuthenticated;

	public Action OnJoined;

	public Action OnLeft;

	public Action<DiscordUserInfo> OnUserJoined;

	public Action<DiscordUserInfo> OnUserLeft;

	public Action<DiscordUserInfo> OnUserSpeakingChanged;

	private static DiscordClientInterface s_instance;

	public static int s_RpcPortOverride;

	public static bool s_debugOutput;

	private Action<int, int, string> m_pushToTalkScanCallback;

	private List<DiscordUserInfo> m_discordChannelUsers;

	private static bool s_sdkInitialized;

	private WebSocketSharp.WebSocket m_webSocket;

	private string m_rpcUrl;

	private int m_rpcPortOffset;

	private bool m_retryToConnect;

	private DiscordAuthInfo m_authInfo;

	private DiscordUserInfo m_userInfo;

	private DiscordChannelInfo m_channelInfo;

	private JsonSerializerSettings m_jsonDateFormatSettings;

	private Scheduler m_rpcScheduler;

	private static readonly int RPC_PORT_BEGIN = 0x193F;

	private static readonly int RPC_PORT_END = 0x1949;

	private static readonly int RPC_COMMAND_TIMEOUT_SEC = 5;

	public DiscordClientInterface()
	{
		
		this.OnConnected = delegate(bool A_0)
			{
			};
		
		this.OnDisconnected = delegate()
			{
			};
		
		this.OnAuthorized = delegate(string A_0)
			{
			};
		
		this.OnAuthenticated = delegate(DiscordUserInfo A_0)
			{
			};
		
		this.OnJoined = delegate()
			{
			};
		this.OnLeft = delegate()
		{
		};
		
		this.OnUserJoined = delegate(DiscordUserInfo A_0)
			{
			};
		
		this.OnUserLeft = delegate(DiscordUserInfo A_0)
			{
			};
		
		this.OnUserSpeakingChanged = delegate(DiscordUserInfo A_0)
			{
			};
		
		this.OnErrorHolder = delegate(ErrorEventArgs A_0)
			{
			};
		this.m_discordChannelUsers = new List<DiscordUserInfo>();
		
	}

	public SynchronizationContext SynchronizationContext { get; set; }

	public static DiscordClientInterface Get()
	{
		return DiscordClientInterface.s_instance;
	}

	private Action<ErrorEventArgs> OnErrorHolder;
	public event Action<ErrorEventArgs> OnError
	{
		add
		{
			Action<ErrorEventArgs> action = this.OnErrorHolder;
			Action<ErrorEventArgs> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ErrorEventArgs>>(ref this.OnErrorHolder, (Action<ErrorEventArgs>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ErrorEventArgs> action = this.OnErrorHolder;
			Action<ErrorEventArgs> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ErrorEventArgs>>(ref this.OnErrorHolder, (Action<ErrorEventArgs>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static bool IsEnabled
	{
		get
		{
			return false;
		}
	}

	public static bool IsSdkEnabled
	{
		get
		{
			return false;
		}
	}

	public static bool IsInstalled
	{
		get
		{
			try
			{
				string name = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
				RegistryKey key = Registry.CurrentUser.OpenSubKey(name);
				if (key != null)
				{
					IEnumerator<RegistryKey> enumerator = (from keyName in key.GetSubKeyNames()
					select key.OpenSubKey(keyName)).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							RegistryKey registryKey = enumerator.Current;
							string text = registryKey.GetValue("DisplayName") as string;
							if (text != null && text.Contains("Discord"))
							{
								return true;
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					key.Close();
				}
			}
			catch (Exception ex)
			{
				Log.Error("Failed to check Discord installation in registry. {0}", new object[]
				{
					ex
				});
			}
			return false;
		}
	}

	public static bool CanJoinTeamChat
	{
		get
		{
			GameManager gameManager = GameManager.Get();
			if (gameManager != null && gameManager.GameInfo != null)
			{
				if (gameManager.GameInfo.GameStatus != GameStatus.Stopped && gameManager.GameInfo.GameConfig != null)
				{
					if (gameManager.GameInfo.GameConfig.GameType.IsQueueable() || gameManager.GameInfo.IsCustomGame)
					{
						return gameManager.PlayerInfo != null;
					}
				}
			}
			return false;
		}
	}

	public static bool CanJoinGroupChat
	{
		get
		{
			LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
			bool result;
			if (groupInfo != null)
			{
				result = groupInfo.InAGroup;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}

	public bool IsConnected
	{
		get
		{
			bool result;
			if (this.m_webSocket != null)
			{
				result = this.m_webSocket.IsAlive;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}

	public DiscordChannelInfo ChannelInfo
	{
		get
		{
			return this.m_channelInfo;
		}
	}

	public DiscordUserInfo UserInfo
	{
		get
		{
			return this.m_userInfo;
		}
	}

	public List<DiscordUserInfo> ChannelUsers
	{
		get
		{
			return this.m_discordChannelUsers;
		}
	}

	private int RpcPort
	{
		get
		{
			int result;
			if (DiscordClientInterface.s_RpcPortOverride != 0)
			{
				result = DiscordClientInterface.s_RpcPortOverride;
			}
			else
			{
				result = DiscordClientInterface.RPC_PORT_BEGIN + this.m_rpcPortOffset;
			}
			return result;
		}
	}

	private string RpcOrigin
	{
		get
		{
			string result;
			if (this.m_authInfo != null)
			{
				result = this.m_authInfo.RpcOrigin;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
	}

	private string ClientId
	{
		get
		{
			return (this.m_authInfo == null) ? string.Empty : this.m_authInfo.ClientId;
		}
	}

	private void Awake()
	{
		DiscordClientInterface.s_instance = this;
	}

	private void Start()
	{
		this.m_jsonDateFormatSettings = new JsonSerializerSettings
		{
			DateFormatHandling = DateFormatHandling.IsoDateFormat,
			DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
		};
		this.m_rpcScheduler = new Scheduler();
		this.m_rpcPortOffset = 0;
		DiscordClientInterface.s_RpcPortOverride = 0;
		this.SynchronizationContext = SynchronizationContext.Current;
	}

	private void Update()
	{
	}

	private static void symbol_001D(string symbol_001D, params object[] symbol_000E)
	{
		if (DiscordClientInterface.s_debugOutput)
		{
			string str = string.Format(symbol_001D, symbol_000E);
			Log.Info("Discord | " + str, new object[0]);
			if (SynchronizationContext.Current != null)
			{
				if (TextConsole.Get() != null)
				{
					TextConsole.Get().Write("Discord | " + str, ConsoleMessageType.symbol_001D);
				}
			}
		}
	}

	private static void symbol_001D(DiscordClientInterface.RpcResponse symbol_001D)
	{
		if (DiscordClientInterface.s_debugOutput)
		{
			string str = JsonConvert.SerializeObject(symbol_001D, Formatting.Indented);
			Log.Info("Discord | " + str, new object[0]);
		}
	}

	private void Authorize()
	{
		DiscordClientInterface.RpcRequest rpcRequest = new DiscordClientInterface.RpcRequest();
		rpcRequest.nonce = Guid.NewGuid().ToString();
		rpcRequest.args = new Dictionary<object, object>();
		rpcRequest.args["client_id"] = this.m_authInfo.ClientId;
		List<string> list = new List<string>();
		list.Add("rpc");
		list.Add("rpc.api");
		list.Add("guilds.join");
		rpcRequest.args["scopes"] = list;
		rpcRequest.args["rpc_token"] = this.m_authInfo.RpcToken;
		rpcRequest.cmd = "AUTHORIZE";
		string data = JsonConvert.SerializeObject(rpcRequest);
		this.m_webSocket.Send(data);
	}

	public void Authenticate(DiscordUserInfo userInfo)
	{
		this.m_userInfo = userInfo;
		DiscordClientInterface.RpcRequest rpcRequest = new DiscordClientInterface.RpcRequest();
		rpcRequest.nonce = Guid.NewGuid().ToString();
		rpcRequest.args = new Dictionary<object, object>();
		rpcRequest.args["access_token"] = this.m_userInfo.AccessToken;
		rpcRequest.cmd = "AUTHENTICATE";
		string data = JsonConvert.SerializeObject(rpcRequest);
		this.m_webSocket.Send(data);
	}

	private void Invoke(string jsonMessage)
	{
		DiscordClientInterface.RpcWebSocketEnvelope rpcWebSocketEnvelope = default(DiscordClientInterface.RpcWebSocketEnvelope);
		rpcWebSocketEnvelope.JsonMessage = jsonMessage;
		Action<string> messageDelegate = new Action<string>(this.Dispatch);
		Action<string> invokerDelegate = delegate(string msg)
		{
			messageDelegate(msg);
		};
		rpcWebSocketEnvelope.InvokerDelegate = invokerDelegate;
		rpcWebSocketEnvelope.MethodInfo = messageDelegate.Method;
		if (this.SynchronizationContext != null)
		{
			this.SynchronizationContext.Post(new SendOrPostCallback(this.InvokeAsync), rpcWebSocketEnvelope, rpcWebSocketEnvelope.MethodInfo);
		}
		else
		{
			this.InvokeAsync(rpcWebSocketEnvelope);
		}
	}

	private void InvokeAsync(object _envelope)
	{
		DiscordClientInterface.RpcWebSocketEnvelope rpcWebSocketEnvelope = (DiscordClientInterface.RpcWebSocketEnvelope)_envelope;
		rpcWebSocketEnvelope.InvokerDelegate(rpcWebSocketEnvelope.JsonMessage);
	}

	private void Dispatch(string jsonMessage)
	{
		try
		{
			DiscordClientInterface.RpcResponse rpcResponse = JsonConvert.DeserializeObject<DiscordClientInterface.RpcResponse>(jsonMessage, this.m_jsonDateFormatSettings);
			JObject jobject;
			if (rpcResponse.data != null)
			{
				jobject = JObject.Parse(rpcResponse.data.ToString());
			}
			else
			{
				jobject = null;
			}
			JObject jobject2 = jobject;
			DiscordClientInterface.symbol_001D(rpcResponse);
			if (rpcResponse.cmd == "CONNECTION_OPEN")
			{
				DiscordClientInterface.symbol_001D("Connected to 127.0.0.1:{0}", new object[]
				{
					this.RpcPort
				});
				bool obj = this.m_authInfo == null;
				this.OnConnected(obj);
				this.m_retryToConnect = false;
				this.m_rpcPortOffset = 0;
			}
			else if (rpcResponse.cmd == "CONNECTION_ERROR")
			{
				DiscordClientInterface.symbol_001D("Connection error to 127.0.0.1:{0}", new object[]
				{
					this.RpcPort
				});
			}
			else if (rpcResponse.cmd == "CONNECTION_CLOSE")
			{
				if (this.m_retryToConnect)
				{
					this.m_rpcPortOffset++;
					if (DiscordClientInterface.RPC_PORT_BEGIN + this.m_rpcPortOffset > DiscordClientInterface.RPC_PORT_END)
					{
						DiscordClientInterface.symbol_001D("Failed to connect to discord", new object[0]);
						this.Disconnect();
						this.OnDisconnected();
					}
					else
					{
						this.m_webSocket.Close();
						this.m_webSocket = null;
						this.TryConnect();
					}
				}
				else
				{
					DiscordClientInterface.symbol_001D("Disconnected from discord", new object[0]);
					if (this.m_authInfo != null)
					{
						this.OnLeft();
					}
					this.Disconnect();
					this.OnDisconnected();
				}
			}
			else if (rpcResponse.cmd == "DISPATCH")
			{
				if (rpcResponse.evt == "READY")
				{
					if (this.m_authInfo != null)
					{
						if (this.m_userInfo == null)
						{
							this.Authorize();
							goto IL_5C4;
						}
					}
				}
				if (rpcResponse.evt == "CAPTURE_SHORTCUT_CHANGE")
				{
					if (jobject2 != null)
					{
						JToken jtoken = jobject2.SelectToken("shortcut").ElementAt(0);
						int num = Convert.ToInt32(jtoken.SelectToken("type").ToString());
						int num2 = Convert.ToInt32(jtoken.SelectToken("code").ToString());
						string text = jtoken.SelectToken("name").ToString();
						if (ClientGameManager.Get() != null)
						{
							ClientGameManager.Get().SetPushToTalkKey(num, num2, text);
						}
						if (this.m_pushToTalkScanCallback != null)
						{
							this.m_pushToTalkScanCallback(num, num2, text);
						}
						this.ScanPushToTalkKey(false, null);
						this.RefreshSettings();
						goto IL_5C4;
					}
				}
				if (rpcResponse.evt == "VOICE_STATE_CREATE")
				{
					if (jobject2 != null)
					{
						bool flag = false;
						ulong num3 = ulong.Parse(jobject2.SelectToken("user.id").ToString());
						for (int i = 0; i < this.m_discordChannelUsers.Count; i++)
						{
							if (this.m_discordChannelUsers[i].UserId == num3)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							DiscordUserInfo discordUserInfo = new DiscordUserInfo();
							discordUserInfo.UserId = num3;
							discordUserInfo.UserName = jobject2.SelectToken("user.username").ToString();
							discordUserInfo.Discriminator = jobject2.SelectToken("user.discriminator").ToString();
							this.m_discordChannelUsers.Add(discordUserInfo);
							this.OnUserJoined(discordUserInfo);
						}
						goto IL_5C4;

					}
				}
				if (rpcResponse.evt == "VOICE_STATE_DELETE")
				{
					if (jobject2 != null)
					{
						ulong num4 = ulong.Parse(jobject2.SelectToken("user.id").ToString());
						for (int j = 0; j < this.m_discordChannelUsers.Count; j++)
						{
							if (this.m_discordChannelUsers[j].UserId == num4)
							{
								DiscordUserInfo obj2 = this.m_discordChannelUsers[j];
								this.m_discordChannelUsers.RemoveAt(j);
								this.OnUserLeft(obj2);
								break;
							}
						}
						goto IL_5C4;

					}
				}
				if (!(rpcResponse.evt == "SPEAKING_START"))
				{
					if (!(rpcResponse.evt == "SPEAKING_STOP"))
					{
						goto IL_5C4;
					}
				}
				if (jobject2 != null)
				{
					ulong num5 = ulong.Parse(jobject2.SelectToken("user_id").ToString());
					for (int k = 0; k < this.m_discordChannelUsers.Count; k++)
					{
						if (this.m_discordChannelUsers[k].UserId == num5)
						{
							this.m_discordChannelUsers[k].IsSpeaking = (rpcResponse.evt == "SPEAKING_START");
							this.OnUserSpeakingChanged(this.m_discordChannelUsers[k]);
							break;
						}
					}
				}
				IL_5C4:;
			}
			else if (rpcResponse.cmd == "AUTHORIZE")
			{
				string obj3 = jobject2.SelectToken("code").ToString();
				this.OnAuthorized(obj3);
			}
			else if (rpcResponse.cmd == "AUTHENTICATE")
			{
				if (rpcResponse.evt.IsNullOrEmpty())
				{
					JToken jtoken2 = jobject2.SelectToken("user.id");
					this.m_userInfo.UserId = ulong.Parse(jtoken2.ToString());
					this.OnAuthenticated(this.m_userInfo);
				}
				else
				{
					DiscordClientInterface.symbol_001D("Failed to authenticate RPC connection {0}", new object[]
					{
						jobject2
					});
				}
			}
			else if (rpcResponse.cmd == "TRY_SELECT_VOICE_CHANNEL")
			{
				JToken jtoken3 = jobject2.SelectToken("code");
				int num6 = int.Parse(jtoken3.ToString());
				bool force = num6 == 0x138B;
				this.TrySelectVoiceChannel(force);
			}
			else if (rpcResponse.cmd == "SELECT_VOICE_CHANNEL")
			{
				if (rpcResponse.evt.IsNullOrEmpty())
				{
					DiscordClientInterface.symbol_001D("Connected to voice channel {0}", new object[]
					{
						this.m_channelInfo.VoiceChannelId
					});
					this.OnJoined();
				}
				else
				{
					DiscordClientInterface.symbol_001D("Failed to connect to voice channel {0} {1}. Retrying", new object[]
					{
						this.m_channelInfo.VoiceChannelId,
						jobject2
					});
					this.RetrySelectVoiceChannel(jobject2);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("DISCORD | exception {0}", new object[]
			{
				ex
			});
		}
	}

	private void HandleOnOpen(object sender, EventArgs e)
	{
		DiscordClientInterface.RpcResponse value = new DiscordClientInterface.RpcResponse
		{
			cmd = "CONNECTION_OPEN"
		};
		string jsonMessage = JsonConvert.SerializeObject(value);
		this.Invoke(jsonMessage);
	}

	private void HandleOnMessage(object sender, MessageEventArgs e)
	{
		string data = e.Data;
		this.Invoke(data);
	}

	private void HandleOnError(object sender, ErrorEventArgs e)
	{
		DiscordClientInterface.RpcResponse value = new DiscordClientInterface.RpcResponse
		{
			cmd = "CONNECTION_ERROR",
			evt = e.Exception.Message
		};
		string jsonMessage = JsonConvert.SerializeObject(value);
		this.Invoke(jsonMessage);
	}

	private void HandleOnClose(object sender, CloseEventArgs e)
	{
		DiscordClientInterface.RpcResponse value = new DiscordClientInterface.RpcResponse
		{
			cmd = "CONNECTION_CLOSE",
			evt = e.Reason
		};
		string jsonMessage = JsonConvert.SerializeObject(value);
		this.Invoke(jsonMessage);
	}

	public void Connect(DiscordAuthInfo authInfo, int portOffset = 0)
	{
		this.m_authInfo = authInfo;
		this.m_rpcPortOffset = portOffset;
		if (DiscordClientInterface.IsSdkEnabled)
		{
			if (!DiscordClientInterface.s_sdkInitialized)
			{
				this.InitializeSdk();
				return;
			}
		}
		this.TryConnect();
	}

	private void InitializeSdk()
	{
		string name = "Hydrogen.DiscordSdk";
		Mutex mutex = new Mutex(true, name, out bool flag);
		if (flag)
		{
			if (mutex != null)
			{
				string clientId = this.m_authInfo.ClientId;
				string resourcePath = Application.dataPath + "/../";
				GameBridge.SetReadyCallback(new GameBridge.ReadyCallback(this.HandleSdkReadyCallback), UIntPtr.Zero);
				GameBridge.SetUpdatingCallback(new GameBridge.UpdatingCallback(this.HandleSdkUpdatingCallback), UIntPtr.Zero);
				GameBridge.SetErrorCallback(new GameBridge.ErrorCallback(this.HandleSdkErrorCallback), UIntPtr.Zero);
				
				GameBridge.CaptureOutput(delegate(uint type, string message, UIntPtr context)
					{
						DiscordClientInterface.symbol_001D("[OUTPUT] {0}", new object[]
						{
							message
						});
					}, UIntPtr.Zero);
				GameBridge.Initialize(clientId, resourcePath);
				DiscordClientInterface.s_sdkInitialized = true;
				DiscordClientInterface.symbol_001D("initialized Sdk", new object[0]);
				return;
			}
		}
		DiscordClientInterface.symbol_001D("failed to initialize Sdk. Discord does not support launching multiple Sdk processes on the same computer at one time.", new object[0]);
	}

	public static void Shutdown()
	{
		if (DiscordClientInterface.s_sdkInitialized)
		{
			GameBridge.Shutdown();
			DiscordClientInterface.s_sdkInitialized = false;
			DiscordClientInterface.symbol_001D("shut down Sdk", new object[0]);
		}
	}

	private void HandleSdkReadyCallback(ushort port, UIntPtr context)
	{
		DiscordClientInterface.symbol_001D("SdkReadyCallback {0}", new object[]
		{
			port
		});
		DiscordClientInterface.s_RpcPortOverride = (int)port;
		this.TryConnect();
	}

	private void HandleSdkUpdatingCallback(uint progress, UIntPtr context)
	{
		DiscordClientInterface.symbol_001D("SdkUpdatingCallback {0}", new object[]
		{
			progress
		});
	}

	private void HandleSdkErrorCallback(uint code, [MarshalAs(UnmanagedType.LPStr)] string message, UIntPtr context)
	{
		DiscordClientInterface.symbol_001D("SdkErrorCallback {0} {1}", new object[]
		{
			code,
			message
		});
	}

	private void TryConnect()
	{
		if (this.m_webSocket != null)
		{
			if (this.m_webSocket.IsAlive)
			{
				DiscordClientInterface.symbol_001D("Already connected to discord", new object[0]);
				return;
			}
		}
		this.m_rpcUrl = string.Format("ws://127.0.0.1:{0}/?v=1&client_id={1}&encoding=json", this.RpcPort, this.ClientId);
		DiscordClientInterface.symbol_001D("Connecting to 127.0.0.1:{0}", new object[]
		{
			this.RpcPort
		});
		this.m_webSocket = new WebSocketSharp.WebSocket(this.m_rpcUrl, new string[0]);
		this.m_webSocket.OnOpen += this.HandleOnOpen;
		this.m_webSocket.OnMessage += this.HandleOnMessage;
		this.m_webSocket.OnError += this.HandleOnError;
		this.m_webSocket.OnClose += this.HandleOnClose;
		if (DiscordClientInterface.s_debugOutput)
		{
			this.m_webSocket.Logger.Level = LogLevel.Trace;
		}
		this.m_webSocket.Origin = this.RpcOrigin;
		this.m_webSocket.WaitTime = TimeSpan.FromMilliseconds(100.0);
		this.m_webSocket.ConnectAsync();
		this.m_retryToConnect = true;
	}

	public void Disconnect()
	{
		if (this.m_webSocket != null)
		{
			this.m_webSocket.Close();
			this.m_webSocket = null;
		}
		this.m_retryToConnect = false;
		this.m_rpcPortOffset = 0;
		this.m_authInfo = null;
		this.m_userInfo = null;
		this.m_channelInfo = null;
		this.m_discordChannelUsers.Clear();
	}

	public void SelectVoiceChannel(DiscordChannelInfo channelInfo)
	{
		this.m_channelInfo = channelInfo;
		this.TrySelectVoiceChannel(false);
	}

	private void TrySelectVoiceChannel(bool force = false)
	{
		if (this.m_webSocket != null && this.m_webSocket.IsAlive)
		{
			if (this.m_channelInfo != null)
			{
				DiscordClientInterface.RpcRequest rpcRequest = new DiscordClientInterface.RpcRequest();
				rpcRequest.nonce = Guid.NewGuid().ToString();
				rpcRequest.args = new Dictionary<object, object>();
				rpcRequest.args["channel_id"] = this.m_channelInfo.VoiceChannelId.ToString();
				rpcRequest.args["timeout"] = DiscordClientInterface.RPC_COMMAND_TIMEOUT_SEC;
				if (force)
				{
					rpcRequest.args["force"] = force;
				}
				rpcRequest.cmd = "SELECT_VOICE_CHANNEL";
				string data = JsonConvert.SerializeObject(rpcRequest);
				this.m_webSocket.Send(data);
			}
		}
	}

	private void RetrySelectVoiceChannel(object responseData = null)
	{
		Action action = delegate()
		{
			DiscordClientInterface.RpcResponse value = new DiscordClientInterface.RpcResponse
			{
				cmd = "TRY_SELECT_VOICE_CHANNEL",
				data = responseData
			};
			string jsonMessage = JsonConvert.SerializeObject(value);
			this.Invoke(jsonMessage);
		};
		this.m_rpcScheduler.AddTask(action, (int)TimeSpan.FromSeconds(2.0).TotalMilliseconds, true);
	}

	public void RefreshSettings()
	{
		if (this.m_webSocket != null)
		{
			if (this.m_webSocket.IsAlive)
			{
				if (this.m_channelInfo == null)
				{
				}
				else
				{
					Options_UI options_UI = Options_UI.Get();
					if (options_UI != null)
					{
						return;
					}
					DiscordClientInterface.RpcRequest rpcRequest = new DiscordClientInterface.RpcRequest();
					rpcRequest.nonce = Guid.NewGuid().ToString();
					rpcRequest.args = new Dictionary<object, object>();
					rpcRequest.args["input"] = new
					{
						volume = options_UI.GetVoiceVolume()
					};
					rpcRequest.args["output"] = new
					{
						volume = options_UI.GetMicVolume()
					};
					rpcRequest.args["mute"] = options_UI.GetVoiceMute();
					rpcRequest.cmd = "SET_VOICE_SETTINGS";
					Dictionary<object, object> dictionary = new Dictionary<object, object>();
					rpcRequest.args["mode"] = dictionary;
					Dictionary<object, object> dictionary2 = dictionary;
					object key = "type";
					object value;
					if (options_UI.GetVoicePushToTalk())
					{
						value = "PUSH_TO_TALK";
					}
					else
					{
						value = "VOICE_ACTIVITY";
					}
					dictionary2[key] = value;
					if (ClientGameManager.Get() != null)
					{
						if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
						{
							AccountComponent accountComponent = ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
							dictionary["shortcut"] = new object[]
							{
								new
								{
									type = accountComponent.PushToTalkKeyType,
									code = accountComponent.PushToTalkKeyCode,
									name = accountComponent.PushToTalkKeyName
								}
							};
						}
					}
					string data = JsonConvert.SerializeObject(rpcRequest);
					this.m_webSocket.Send(data);
					return;
				}
			}
		}
	}

	public bool ScanPushToTalkKey(bool start, Action<int, int, string> callback)
	{
		if (this.m_webSocket != null)
		{
			if (this.m_webSocket.IsAlive)
			{
				if (this.m_channelInfo != null)
				{
					this.m_pushToTalkScanCallback = callback;
					DiscordClientInterface.RpcRequest rpcRequest = new DiscordClientInterface.RpcRequest();
					rpcRequest.nonce = Guid.NewGuid().ToString();
					rpcRequest.args = new Dictionary<object, object>();
					Dictionary<object, object> args = rpcRequest.args;
					object key = "action";
					object value;
					if (start)
					{
						value = "START";
					}
					else
					{
						value = "STOP";
					}
					args[key] = value;
					rpcRequest.cmd = "CAPTURE_SHORTCUT";
					string data = JsonConvert.SerializeObject(rpcRequest);
					this.m_webSocket.Send(data);
					return true;
				}
			}
		}
		return false;
	}

	internal struct RpcWebSocketEnvelope
	{
		public string JsonMessage;

		public MethodInfo MethodInfo;

		public Action<string> InvokerDelegate;
	}

	internal class RpcRequest
	{
		public string nonce { get; set; }

		public Dictionary<object, object> args { get; set; }

		public string cmd { get; set; }
	}

	internal class RpcResponse
	{
		public string cmd { get; set; }

		public object data { get; set; }

		public string evt { get; set; }

		public string nonce { get; set; }
	}
}
