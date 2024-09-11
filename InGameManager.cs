using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance { get; set; }
    public GameSettings settings;

    public BonusGame bonusGame;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public BonusGame BonusGame
    {
        get
        {
            if (bonusGame == null)
            {
                bonusGame = FindObjectOfType<BonusGame>();
            }

            return bonusGame;
        }

        set
        {
            bonusGame = value;
        }
    }

    public void ScoreUpdate()
    {
        if(BonusGame != null)
        {
            BonusGame.ScoreUpdate();
        }
    }

    public void UpgradeChar()
    {
        if(BonusGame != null)
        {
            BonusGame.UpgradeChar();
        }
    }
}