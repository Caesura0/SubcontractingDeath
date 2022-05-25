using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGiver : MonoBehaviour
{
    PlayerData player;
    [SerializeField] bool masterWallJump;
    [SerializeField] bool masterDash;
    [SerializeField] int totalJumps = 2;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
    }

    public void EnableWallJump()
    {
        if (player != null)
        {
            player.wallJumpMaster = masterWallJump;
        }
    }
    public void EnableDash()
    {
        if(player != null) 
        {
            player.dashMaster = masterDash;
        }
        
    }
    public void AddJump()
    {
        if (player != null)
        {
            player.jumpNumber = totalJumps;
        }
    }
}
