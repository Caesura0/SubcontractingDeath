using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    PlayerController player;
    LevelManager levelManager;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

    }

    private void OnEnable()
    {
        if (player == null) return;
        Time.timeScale = 0;
        player.enabled = false;
    }
    private void OnDisable()
    {
        if (player == null) return;
        Time.timeScale = 1;
        player.enabled = true;
    }

    public void Quit()
    {
        levelManager.QuitGame();
    }
}
