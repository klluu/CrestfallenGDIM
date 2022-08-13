 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float followSpeed = 7f;
    private float yPos = 0.5f;
    private Vector3 _offset = new Vector3(1, .5f);
    private Transform player;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    void FixedUpdate()
    {
        Vector2 targetPos = player.position;
        Vector2 smoothPos = Vector2.LerpUnclamped(transform.position, targetPos, .5f);

        transform.position = (Vector3)smoothPos + _offset;
    }
}
