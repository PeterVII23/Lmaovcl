using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static bool isGameStarted;
    public static bool GameOver;
    public GameObject gameOverPanel;
    // Start is called before the first frame update
    void Start()
    {
        isGameStarted = false;
        Time.timeScale = 1;
        GameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SwipeManager.tap)
        {
            isGameStarted = true;
        }

        if (GameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
    }
}
