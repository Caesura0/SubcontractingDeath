using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    private Transform player;

    SpriteRenderer SR;
    SpriteRenderer playerSR;

    Color color;

    [SerializeField] float activeTime;
    float timeActived;
    float alpha;
    [SerializeField] float alphaSet = 0.8f;
    [SerializeField] float alphaMultiplier = 0.85f;


    private void OnEnable()
    {
        SR = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSR = player.GetComponent<SpriteRenderer>();
        alpha = alphaSet;
        SR.sprite = playerSR.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActived = Time.time;
        Debug.Log("onenable");
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        SR.color = color;

        if (Time.time >= (timeActived + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
