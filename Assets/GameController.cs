using JetFistGames.RetroTVFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public LogController logController;
    public JukeboxController jukeboxController;
    public CRTEffect Effects;

    private Player player;
    private Tile current;

    void Awake()
    {
        Effects.QuantizeRGB = false;
        Effects.EnableTVCurvature = true;
        Effects.enabled = true;
    }

    void Start()
    {
        this.player = new Player("You", 10, 0, 1, logController, new Inventory(10, logController));

        current = new RockyRoom(-1, logController, this);
        new R0_Forest_Start(0, logController, this);
        current.enter();
        BeginGame();
    }

    private void BeginGame()
    {
        logController.Read((res) =>
        {
            string[] splitRes = res.Split(' ');
            string[] args = new string[splitRes.Length - 1];
            for(int i = 0; i < splitRes.Length - 1; i++)
            {
                args[i] = splitRes[i + 1];
            }
            current.InvokeAction(splitRes[0].ToLower(), args);
            BeginGame();
        });
    }

    public void setCurrentTile(Tile t)
    {
        this.current = t;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public JukeboxController GetJukebox()
    {
        return jukeboxController;
    }
}