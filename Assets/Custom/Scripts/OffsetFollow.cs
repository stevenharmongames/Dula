using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFollow : MonoBehaviour
{
    private Vector3 vel = Vector3.zero;
    public Transform targetPos;
    private Transform mainCam;
    public Vector3 offset = new Vector3(.15f, -.01f, .4f);
    public bool lookat = true;

    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera").transform;
    }
    void FixedUpdate()
    {
        // Calculate the target position with the offset
        Vector3 targetPosition = targetPos.TransformPoint(offset);

        // Smoothly move the follower towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, .3f);
        if (lookat)
        {
            transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward,
            mainCam.transform.rotation * Vector3.up);
        }
    }
}
