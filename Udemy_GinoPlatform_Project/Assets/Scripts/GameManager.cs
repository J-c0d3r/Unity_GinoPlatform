using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int starsPlayer;
    int lifesPlayer;
    bool isGameRunning = false;

    private static GameManager gm;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        starsPlayer = 0;
        lifesPlayer = 3;
    }

    void Start()
    {

    }

    public void switchGameRun(bool run)
    {
        isGameRunning = run;
    }

    public bool getGameRun()
    {
        return isGameRunning;
    }

    public void setStartLife(int start, int life)
    {
        starsPlayer = start;
        lifesPlayer = life;
    }

    public int getStar()
    {
        return starsPlayer;
    }

    public int getLife()
    {
        return lifesPlayer;
    }
}
