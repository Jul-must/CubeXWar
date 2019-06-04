
public class PlayerDataClass
{
    public int networkPlayer;
    public string playerName;
    public int playerScore;
    public string playerTeam;
    public int playerDeaths;
    public int clientNumber;

    public PlayerDataClass Constructor()
    {
        PlayerDataClass capture = new PlayerDataClass();

        capture.networkPlayer = networkPlayer;
        capture.playerName = playerName;
        capture.playerScore = playerScore;
        capture.playerTeam = playerTeam;
        capture.playerDeaths = playerDeaths;
        capture.clientNumber = clientNumber;
        return capture;
    }
}