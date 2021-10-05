using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImagePlayer : MonoBehaviour
{
    private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
    [SerializeField]
    private float alphaSet = 0.8f;
    private float alphaMultiplayer = 0.95f;

    private Transform player;
    private SpriteRenderer sr;
    private SpriteRenderer playerSr;

    private Color color;

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSr = player.GetComponentInChildren<SpriteRenderer>();

        alpha = alphaSet;
        sr.sprite = playerSr.sprite;
        sr.flipX = playerSr.flipX;
        sr.sortingOrder = playerSr.sortingOrder;
        sr.material = playerSr.material;

        transform.localScale = playerSr.transform.localScale;
        transform.position = player.position;
        transform.position = new Vector2(player.position.x, playerSr.transform.position.y);
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    private void LateUpdate()
    {
        alpha *= alphaMultiplayer;
        color = new Color(1f,1f,1f,alpha);
        sr.color = color;

        if(Time.time >= (timeActivated + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
