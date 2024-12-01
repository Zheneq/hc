using System;

public class DebugCommand_Options : DebugCommand
{
    public override bool AvailableInFrontEnd()
    {
        return true;
    }

    public override string GetDebugItemName()
    {
        return "Toggle DebugCamera";
    }

    public override string GetPath()
    {
        return "Options";
    }

    public override void OnIncreaseClick()
    {
        RunCommand("DebugCamera");
    }

    private void RunCommand(string parameter, bool enable)
    {
        if (DebugParameters.Get() == null)
        {
            DebugParameters.Instantiate();
        }
        DebugParameters.Get().SetParameter(parameter, enable);
        TextConsole.Get().Write($"{parameter}: {enable}");
    }

    private void RunCommand(string parameter)
    {
        if (DebugParameters.Get() == null)
        {
            DebugParameters.Instantiate();
        }
        bool enable = !DebugParameters.Get().GetParameterAsBool(parameter);
        DebugParameters.Get().SetParameter(parameter, enable);
        TextConsole.Get().Write($"{parameter}: {enable}");
    }

    public override string GetSlashCommand()
    {
        return "/options";
    }

    public override bool OnSlashCommand(string arguments)
    {
        ClientGameManager clientGameManager = ClientGameManager.Get();

        if (clientGameManager != null || clientGameManager.ClientAccessLevel == ClientAccessLevel.Admin)
        {
            string helpText = "\r\n/options\r\n" +
                              "\t\tDebugCamera\r\n" +
                              "\t\tCameraFarZoom\r\n" +
                              "\t\tSkipEndOfGameCheck\r\n" +
                              "\t\tDebugNameplates\r\n" +
                              "\t\tDisableBrush\r\n" +
                              "\t\tNoCooldowns\r\n" +
                              "\t\tShowBoardSquareVisGizmo\r\n" +
                              "\t\tInfiniteTP\r\n" +
                              "\t\tAllCharactersVisible\r\n" +
                              "\t\tSkipFogOfWarUpdateOnMovement\r\n" +
                              "\t\tCameraTiltControl\r\n" +
                              "\t\t\t\t[true | false]\r\n" +
                              "/options help";

            string[] array2 = arguments.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (array2.Length == 0 || string.IsNullOrEmpty(array2[0]) || array2[0].Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                goto help;
            }

            if (array2.Length == 2)
            {
                bool param2;
                if (bool.TryParse(array2[1], out param2))
                {
                    RunCommand(array2[0], param2);
                }
                else
                {
                    // Handle the case where the second argument is not a valid boolean
                    TextConsole.Get().Write("Second argument must be true or false.");
                    return false;
                }
            }
            else
            {
                RunCommand(array2[0]);
            }

            return false;

        help:
            TextConsole.Get().Write(helpText);
            return false;
        }
        return false;
    }
}