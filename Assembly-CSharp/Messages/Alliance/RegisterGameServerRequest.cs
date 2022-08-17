// ROGUES
// SERVER

using UnityEngine.Networking;

#if SERVER
// added in rogues
public class RegisterGameServerRequest : AllianceMessageBase
{
    public LobbySessionInfo SessionInfo;
    public bool isPrivate;

    public override void Serialize(NetworkWriter writer)
    {
        base.Serialize(writer);
        SerializeObject(SessionInfo, writer);
        writer.Write(isPrivate);
    }

    // custom
    public override void Deserialize(NetworkReader reader)
    {
        base.Deserialize(reader);
        DeserializeObject(out SessionInfo, reader);
        isPrivate = reader.ReadBoolean();
    }
}
#endif