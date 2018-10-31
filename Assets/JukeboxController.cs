using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeboxController : MonoBehaviour {

    public static string TEXT_BLIP    = "textType.ogg";
    public static string WALK         = "Walk.mp3";
    public static string PICKUP       = "pickUp.mp3";
    public static string DROP         = "drop.mp3";
    public static string EQUIP        = "equip.mp3";
    public static string UNEQUIP      = "unequip.mp3";
    public static string CONSUME      = "drink.mp3";
    public static string WOODHIT      = "woodHit.mp3";
    public static string SWORDHIT     = "swordHit.mp3";
    public static string HITMETAL     = "hitmetal.mp3";
    public static string UNARMEDHIT   = "hit.mp3";
    public static string WATERDIP     = "waterDip.mp3";
    public static string WATERJUMP    = "waterJump.mp3";
    public static string JUMP         = "jump.mp3";
    public static string WALK_WOOD    = "walkWood.mp3";
    public static string WOODCRACK    = "woodCrack.mp3";
    public static string MOVINGTREES  = "movingTrees.mp3";
    public static string PICKUP_EPIC  = "epicPickUp.mp3";
    public static string WALK_DIZZY   = "dizzyWalk.mp3";

    public AudioSource audioSource;
    private string basePath;

    void Awake()
    {
        basePath = Application.dataPath + "/Audio/";
    }

    public void playSound(string name)
    {
        StartCoroutine(play(name));
    }

    private IEnumerator play(string name)
    {

        WWW www = new WWW("file:///" + basePath + name);
        yield return www;
        AudioClip clip = www.GetAudioClip(true);
        audioSource.PlayOneShot(clip);        
    }

}
