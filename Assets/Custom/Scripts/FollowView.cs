using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowView : MonoBehaviour
{
    private GameObject mainCam;
    public float distance = 1.0f;
    public float smoothTime = 0.3f;
    public float vertOffset = -0.5f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera");
    }

    void FixedUpdate()
    {
        SetView();
    }

    void SetView()
    {
        if (mainCam != null)
        {
            Vector3 targetPosition = mainCam.transform.TransformPoint(new Vector3(0, vertOffset, distance));

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            Vector3 lookAtPos = new Vector3(mainCam.transform.position.x, transform.position.y, mainCam.transform.position.z);
            transform.LookAt(lookAtPos);
        }
    }
}
