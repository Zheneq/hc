using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

public class ClientCrashReportThreadedJob : ThreadedJob
{
    internal enum State
    {
        CreatingArchive,
        WaitingForClientToConnectToLobbyServer,
        WaitingForArchiveNameResponse,
        MoveArchiveAndReadBytes,
        CleanUp,
        Succeeded,
        Failed,
        Canceled
    }

    private State m_state;
    private EasedFloat m_progress = new EasedFloat(0f);
    private Dictionary<string, string> m_userKeyValues = new Dictionary<string, string>();
    private string m_userMessage;
    private string m_logMessage;
    private string m_outputLogPath;
    private string m_outputLogPathCopy;
    private BugReportType m_bugReportType;
    private string m_temporaryCachePath;
    private string m_crashDumpDirectoryPath;
    private bool m_stateSucceeeded;
    private string m_tempArchivePath;
    private int m_tempArchiveFileSize;
    private float m_progressValue;
    private string m_archiveFileName;
    private byte[] m_crashReportBytes;

    internal bool IsFinished => m_state == State.Succeeded || m_state == State.Failed || m_state == State.Canceled;
    internal bool IsSucceeded => m_stateSucceeeded;

    internal ClientCrashReportThreadedJob(
        string crashDumpDirectoryPath,
        BugReportType bugReportType = BugReportType.Crash,
        string userMessage = null,
        string logMessage = null)
    {
        m_userMessage = userMessage;
        m_logMessage = logMessage;
        m_bugReportType = bugReportType;
        try
        {
            m_outputLogPath = Path.Combine(Application.dataPath, "output_log.txt");
            m_temporaryCachePath = Application.temporaryCachePath ?? string.Empty;
            if (Path.DirectorySeparatorChar == '\\')
            {
                m_temporaryCachePath = m_temporaryCachePath.Replace("/", "\\");
            }

            m_outputLogPathCopy = Path.Combine(m_temporaryCachePath, Path.GetFileName(m_outputLogPath));
            m_crashDumpDirectoryPath = crashDumpDirectoryPath;
            try
            {
                long num = SystemInfo.systemMemorySize;
                m_userKeyValues["MemoryTotalPhysical"] = (num * 1048576).ToString();
                string[] commandLineArgs = Environment.GetCommandLineArgs() ?? new string[0];
                m_userKeyValues["CommandLine"] = $"\"{string.Join(string.Empty, Environment.GetCommandLineArgs())}\"";
                m_userKeyValues["Language"] = Application.systemLanguage.ToString();
                m_userKeyValues["SettingsPath"] = Application.persistentDataPath;
                m_userKeyValues["SupportsRenderTextureFormat_Depth"] = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth).ToString();
                m_userKeyValues["SupportsRenderTextureFormat_RFloat"] = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat).ToString();
                m_userKeyValues["SupportsRenderTextureFormat_RHalf"] = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RHalf).ToString();
                m_userKeyValues["CurrentResolution"] = Screen.currentResolution.ToString();
                if (Options_UI.Get() != null)
                {
                    m_userKeyValues["GraphicsQuality"] = Options_UI.Get().GetCurrentGraphicsQuality().ToString();
                    m_userKeyValues["GraphicsQualityEverSetManually"] = Options_UI.Get().GetGraphicsQualityEverSetManually().ToString();
                }

                Resolution[] resolutions = Screen.resolutions;
                if (resolutions != null)
                {
                    for (int i = 0; i < resolutions.Length; i++)
                    {
                        m_userKeyValues[$"Resolution{i}"] = resolutions[i].ToString();
                    }
                }

                if (userMessage != null)
                {
                    m_userKeyValues[" UserMessage"] = userMessage;
                }

                PropertyInfo[] properties = typeof(SystemInfo).GetProperties(BindingFlags.Static | BindingFlags.Public);
                foreach (PropertyInfo propertyInfo in properties)
                {
                    m_userKeyValues.Add(propertyInfo.Name, propertyInfo.GetValue(null, null).ToString());
                }
            }
            catch (Exception exception)
            {
                Log.Exception(exception);
            }

            StartThread();
        }
        catch (Exception exception)
        {
            Log.Exception(exception);
        }
    }

    internal void Cancel()
    {
        if (ClientGameManager.Get() != null)
        {
            ClientGameManager.Get().OnConnectedToLobbyServer -= OnRegisterGameClientResponse;
        }

        try
        {
            Abort();
        }
        catch (Exception exception)
        {
            Log.Exception(exception);
        }

        m_state = State.Canceled;
    }

    protected override void ThreadFunction()
    {
        try
        {
            State state = m_state;
            if (state != State.CreatingArchive)
            {
                if (state != State.MoveArchiveAndReadBytes)
                {
                    if (state != State.CleanUp)
                    {
                        return;
                    }

                    CrashReportArchiver.DeleteArchives(m_temporaryCachePath);
                    m_progressValue = 0.95f;
                    if (m_stateSucceeeded)
                    {
                        try
                        {
                            Directory.Delete(m_crashDumpDirectoryPath, true);
                        }
                        catch (Exception exception)
                        {
                            Log.Exception(exception);
                        }
                    }

                    m_progressValue = 1f;
                    m_stateSucceeeded = true;
                    return;
                }

                m_progressValue = 0.4f;
                string text = Path.Combine(m_temporaryCachePath, m_archiveFileName);
                m_progressValue = 0.5f;
                try
                {
                    File.Move(m_tempArchivePath, text);
                }
                catch (Exception exception)
                {
                    Log.Exception(exception);
                }

                m_progressValue = 0.6f;
                if (File.Exists(text))
                {
                    try
                    {
                        m_crashReportBytes = File.ReadAllBytes(text);
                        m_stateSucceeeded = m_crashReportBytes != null && m_crashReportBytes.Length > 0;
                    }
                    catch (Exception exception)
                    {
                        Log.Exception(exception);
                        m_stateSucceeeded = false;
                    }
                }
                else
                {
                    Log.Error("Failed to move file {0} to {1}", m_tempArchivePath, text);
                    m_state = State.Failed;
                }

                return;
            }

            m_progressValue = 0.05f;
            m_stateSucceeeded = true;
            if (!Directory.Exists(m_crashDumpDirectoryPath))
            {
                if (m_bugReportType != 0)
                {
                    try
                    {
                        DirectoryInfo directoryInfo = Directory.CreateDirectory(m_crashDumpDirectoryPath);
                        if (directoryInfo == null || !directoryInfo.Exists)
                        {
                            Log.Error("Failed to create temp directory for user bug report at {0}",
                                m_crashDumpDirectoryPath);
                            m_stateSucceeeded = false;
                        }

                        using (new StreamWriter(m_crashDumpDirectoryPath + "\\crash.dmp"))
                        {
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Exception(exception);
                        m_stateSucceeeded = false;
                    }
                }
                else
                {
                    Log.Error("Directory does not exist for crash report {0}", m_crashDumpDirectoryPath);
                    m_stateSucceeeded = false;
                }
            }

            if (m_stateSucceeeded)
            {
                if (!string.IsNullOrEmpty(m_userMessage))
                {
                    try
                    {
                        using (StreamWriter userMsgWriter = new StreamWriter(m_crashDumpDirectoryPath + "\\UserMessage.txt"))
                        {
                            userMsgWriter.Write(m_userMessage);
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Exception(exception);
                        m_stateSucceeeded = m_bugReportType != BugReportType.Bug && m_bugReportType != BugReportType.Exception;
                    }
                }
            }

            if (m_stateSucceeeded && m_bugReportType != 0)
            {
                try
                {
                    StringBuilder stringBuilder = new StringBuilder(m_logMessage != null ? m_logMessage + "\n" : string.Empty);
                    if (File.Exists(m_outputLogPath))
                    {
                        File.Copy(m_outputLogPath, m_outputLogPathCopy, true);
                        if (File.Exists(m_outputLogPathCopy))
                        {
                            using (StreamReader streamReader = File.OpenText(m_outputLogPathCopy))
                            {
                                if (streamReader.BaseStream.Length <= 262144)
                                {
                                    streamReader.BaseStream.Seek(-streamReader.BaseStream.Length, SeekOrigin.End);
                                }
                                else
                                {
                                    streamReader.BaseStream.Seek(-262144L, SeekOrigin.End);
                                    stringBuilder.AppendFormat("(Skipped {0} bytes)\n", streamReader.BaseStream.Length - 262144);
                                }

                                stringBuilder.Append(streamReader.ReadToEnd());
                            }

                            using (StreamWriter streamWriter3 = new StreamWriter(Path.Combine(m_crashDumpDirectoryPath,
                                       Path.GetFileName(m_outputLogPath))))
                            {
                                streamWriter3.Write(stringBuilder.ToString());
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.Exception(exception);
                }
            }

            if (m_stateSucceeeded)
            {
                try
                {
                    StringBuilder crushDumpBuilder = new StringBuilder();
                    if (ClientBootstrap.Instance != null)
                    {
                        if (File.Exists(ClientBootstrap.Instance.GetFileLogCurrentPath()))
                        {
                            string path = Path.Combine(m_temporaryCachePath, "AtlasReactor.txt");
                            File.Copy(ClientBootstrap.Instance.GetFileLogCurrentPath(), path, true);
                            if (File.Exists(path))
                            {
                                using (StreamReader streamReader = File.OpenText(path))
                                {
                                    if (streamReader.BaseStream.Length <= 262144)
                                    {
                                        streamReader.BaseStream.Seek(-streamReader.BaseStream.Length, SeekOrigin.End);
                                    }
                                    else
                                    {
                                        streamReader.BaseStream.Seek(-262144L, SeekOrigin.End);
                                        crushDumpBuilder.AppendFormat("(Skipped {0} bytes)\n", streamReader.BaseStream.Length - 262144);
                                    }

                                    crushDumpBuilder.Append(streamReader.ReadToEnd());
                                }

                                using (StreamWriter crashDumpWriter = new StreamWriter(Path.Combine(m_crashDumpDirectoryPath, "AtlasReactor.txt")))
                                {
                                    crashDumpWriter.Write(crushDumpBuilder.ToString());
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.Exception(exception);
                }
            }

            if (m_stateSucceeeded)
            {
                m_stateSucceeeded = CrashReportArchiver.CreateArchiveFromCrashDumpDirectory(
                    out m_tempArchivePath,
                    out m_tempArchiveFileSize,
                    m_crashDumpDirectoryPath,
                    m_temporaryCachePath,
                    m_userKeyValues,
                    m_bugReportType);
            }

            m_progressValue = 0.2f;
        }
        catch (Exception exception8)
        {
            Log.Exception(exception8);
            m_state = State.Failed;
        }
    }

    protected override void OnThreadFunctionReturned()
    {
        switch (m_state)
        {
            case State.Failed:
            case State.Canceled:
                break;
            case State.CreatingArchive:
                if (m_stateSucceeeded)
                {
                    if (!ClientGameManager.Get().IsConnectedToLobbyServer)
                    {
                        ClientGameManager.Get().OnConnectedToLobbyServer += OnRegisterGameClientResponse;
                        m_state = State.WaitingForClientToConnectToLobbyServer;
                    }
                    else
                    {
                        RequestArchiveName();
                    }
                }
                else
                {
                    m_state = State.Failed;
                }

                break;
            case State.MoveArchiveAndReadBytes:
                if (m_stateSucceeeded)
                {
                    Log.Info("Move and read of {0} succeeded: {1}", m_archiveFileName, m_stateSucceeeded);
                    try
                    {
                        Log.Info("Attempting to build URL to send crash report {0}", m_archiveFileName);
                        string crashServerAndArchiveURL = $"http://debug.triongames.com/v2/archive/{m_archiveFileName}";
                        ClientCrashReportDetector.Get().UploadArchive(crashServerAndArchiveURL, m_crashReportBytes, OnUploadArchiveEndStatus);
                    }
                    catch (Exception exception)
                    {
                        Log.Exception(exception);
                    }
                }
                else
                {
                    m_state = State.Failed;
                }

                break;
            case State.CleanUp:
                if (m_stateSucceeeded)
                {
                    Log.Info("Clean up of {0} succeeded: {1}", m_archiveFileName, m_stateSucceeeded);
                    m_state = State.Succeeded;
                }
                else
                {
                    m_state = State.Failed;
                }

                break;
            default:
                m_state = State.Failed;
                Log.Error("Unexpected state for completed worker thread: {0}", m_state.ToString());
                break;
        }
    }

    private void RequestArchiveName()
    {
        if (ClientGameManager.Get().RequestCrashReportArchiveName(m_tempArchiveFileSize, OnCrashReportArchiveNameResponse))
        {
            m_state = State.WaitingForArchiveNameResponse;
        }
        else
        {
            Log.Error("Failed to request crash report archive name");
            m_state = State.Failed;
        }
    }

    private void OnCrashReportArchiveNameResponse(CrashReportArchiveNameResponse response)
    {
        if (!response.Success)
        {
            Log.Error($"Crash archive naming failed: {response.ErrorMessage}");
            Cancel();
            m_state = State.Failed;
            return;
        }

        if (m_state == State.Failed || m_state == State.Canceled)
        {
            return;
        }

        if (m_state != State.WaitingForArchiveNameResponse)
        {
            Log.Error($"Unexpected worker thread state when receiving archive name: {m_state}");
            Cancel();
            m_state = State.Failed;
            return;
        }

        if (IsThreadAlive)
        {
            Log.Error("Unexpected busy worker thread when receiving archive name");
            Cancel();
            m_state = State.Failed;
        }
        else if (string.IsNullOrEmpty(response.ArchiveName))
        {
            Log.Error("Unexpected empty archive name received");
            Cancel();
            m_state = State.Failed;
        }
        else
        {
            m_archiveFileName = string.Copy(response.ArchiveName);
            m_state = State.MoveArchiveAndReadBytes;
            StartThread();
        }
    }

    private void OnRegisterGameClientResponse(RegisterGameClientResponse response)
    {
        if (!response.Success)
        {
            Log.Error("Client encountered an error while connecting to lobby server {0}", response.ErrorMessage);
            Cancel();
            m_state = State.Failed;
            return;
        }

        ClientGameManager.Get().OnConnectedToLobbyServer -= OnRegisterGameClientResponse;
        if (m_state == State.WaitingForClientToConnectToLobbyServer)
        {
            RequestArchiveName();
        }
        else
        {
            Log.Error("Unexpected worker thread state when receiving lobby server connect: {0}", m_state.ToString());
            Cancel();
            m_state = State.Failed;
        }
    }

    private void OnUploadArchiveEndStatus(bool success)
    {
        if (success)
        {
            m_state = State.CleanUp;
            StartThread();
        }
        else
        {
            m_state = State.Failed;
        }
    }
}