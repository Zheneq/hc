using System;
using System.Collections.Generic;
using I2.Loc;
using LobbyGameClientMessages;

public class TextConsole
{
    public struct Message
    {
        public string Text;
        public ConsoleMessageType MessageType;
        public CharacterType CharacterType;
        public bool DisplayDevTag;
        public long SenderAccountId;
        public string SenderHandle;
        public Team SenderTeam;
        public string RecipientHandle;
        public Team RestrictVisibiltyToTeam;
    }

    public struct AllowedEmojis
    {
        public List<int> emojis;
    }

    private static TextConsole s_instance;

    public string LastWhisperSenderHandle { get; private set; }

    public event Action<Message, AllowedEmojis> OnMessage;

    public TextConsole()
    {
        OnMessage = delegate { };
        ClientGameManager.Get().OnChatNotification += HandleChatNotification;
    }

    public static TextConsole Get()
    {
        return s_instance;
    }

    public static void Instantiate()
    {
        s_instance = new TextConsole();
    }

    public void Write(Message message, List<int> EmojisAllowed = null)
    {
        AllowedEmojis allowedEmojis = new AllowedEmojis
        {
            emojis = EmojisAllowed
        };
        OnMessage(message, allowedEmojis);
        UITextConsole.StoreMessage(message, allowedEmojis);
    }

    public void Write(string text, ConsoleMessageType messageType = ConsoleMessageType.SystemMessage)
    {
        Write(
            new Message
            {
                MessageType = messageType,
                Text = text
            });
    }

    public string RemoveRichTextTags(string theString)
    {
        string text = theString;
        if (text.IndexOf('<') != -1 && text.IndexOf('>') != -1)
        {
            text = text.Replace("<", "< ");
        }

        return text;
    }

    public void OnInputSubmitted(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return;
        }

        bool isInGame = GameManager.Get() != null
                        && GameManager.Get().GameInfo != null
                        && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped;

        input = ChatEmojiManager.Get().UnlocalizeEmojis(input);
        input = RemoveRichTextTags(input);

        string arguments;
        string command;
        if (input[0] == '/')
        {
            string[] array = input.Split((string[])null, 2, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length >= 2)
            {
                command = array[0];
                arguments = array[1];
            }
            else
            {
                command = input;
                arguments = string.Empty;
            }
        }
        else
        {
            command = isInGame ? "/team" : "/global";
            arguments = input;
        }

        command = command.Trim();
        if (!SlashCommands.Get().RunSlashCommand(command, arguments) && DebugCommands.Get() != null)
        {
            DebugCommands.Get().RunDebugCommand(command, arguments);
        }
    }

    public void HandleSetDevTagResponse(SetDevTagResponse response)
    {
        Write(
            new Message
            {
                Text = response.Success ? "Success" : "Failed",
                MessageType = ConsoleMessageType.SystemMessage
            });
    }

    public void HandleChatNotification(ChatNotification notification)
    {
        if (Options_UI.Get() != null
            && Options_UI.Get().GetEnableProfanityFilter()
            && notification.ConsoleMessageType != ConsoleMessageType.BroadcastMessage)
        {
            notification.Text = BannedWords.Get().FilterPhrase(
                notification.Text,
                LocalizationManager.CurrentLanguageCode);
        }

        Message message = new Message
        {
            Text = notification.LocalizedText == null ? notification.Text : notification.LocalizedText.ToString(),
            MessageType = notification.ConsoleMessageType,
            SenderAccountId = notification.SenderAccountId,
            SenderHandle = notification.SenderHandle,
            SenderTeam = notification.SenderTeam,
            RecipientHandle = notification.RecipientHandle,
            CharacterType = notification.CharacterType,
            DisplayDevTag = notification.DisplayDevTag
        };
        Write(message, notification.EmojisAllowed);

        switch (notification.ConsoleMessageType)
        {
            case ConsoleMessageType.WhisperChat:
            {
                ClientGameManager clientGameManager = ClientGameManager.Get();
                if (clientGameManager == null || clientGameManager.Handle != notification.SenderHandle)
                {
                    LastWhisperSenderHandle = notification.SenderHandle;
                }

                break;
            }
            case ConsoleMessageType.BroadcastMessage:
                SystemMenuBroadcast.Get().DisplaySystemMessage(notification);
                break;
        }
    }
}