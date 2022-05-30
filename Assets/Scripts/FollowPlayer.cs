using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Transform player;
    bool isFollowing;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("GhostTarget").GetComponent<Transform>();
        
    }

    private void Update()
    {
        if (isFollowing)
        {
            Vector3 direction = player.position - transform.position;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        
    }

    public void SetIsFollowing()
    {
        isFollowing = true;
    }
    public void UnSetIsFollowing()
    {
        isFollowing = false;
    }
}
