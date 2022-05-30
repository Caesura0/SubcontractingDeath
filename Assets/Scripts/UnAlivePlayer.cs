using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnAlivePlayer : MonoBehaviour
{

    [SerializeField] ParticleSystem deathFX;
    [SerializeField] FadeAway fade;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision");
        if (collision.gameObject.CompareTag("Hazard"))
        {
            var player = GetComponent<PlayerController>();
            player.isInDialogue = true;
            fade.PlayerFading();
            deathFX.Play();
            var go = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            go.Invoke("ResetLevelAtCheckPoint", 1.5f);
            fade.Invoke("UnFade", 1.5f);
        }

    }
}
