using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingMode : MonoBehaviour
{
    private GameObject mainCam;
    public float avgSit = 1.165882f, avgStand = 1.503125f;
    private float hmdHeight = 0, distToSit = 0, distToStand = 0;
    private bool standing = true;
    private Transform trackingSpaceTrans;

    public int MovingAverageLength = 60; //made public in case you want to change it in the Inspector, if not, could be declared Constant
    private int count;
    private float movingAverage;

    public enum InteractionType
    {
        OffsetTracking,
        OffsetTeleports,
    }
    public InteractionType sittingType = InteractionType.OffsetTracking;
    public Transform[] teleportTrans;
    private float teleportHeight = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera");
        trackingSpaceTrans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckSitting();
    }

    void CheckSitting()
    {
        hmdHeight = AverageOverFrames(mainCam.transform.localPosition.y);

        distToSit = Mathf.Abs(avgSit - hmdHeight);
        distToStand = Mathf.Abs(avgStand - hmdHeight);
        if (distToSit > distToStand)
        {
            if (standing == false)
            {

                if (sittingType == InteractionType.OffsetTracking)
                {
                    trackingSpaceTrans.position = new Vector3(0, 0, 0);
                }
                else if (sittingType == InteractionType.OffsetTeleports)
                {
                    foreach (Transform teleport in teleportTrans)
                    {
                        teleportHeight = teleport.localPosition.y + .44f;
                        Vector3 newPosition = new Vector3(teleport.localPosition.x, teleportHeight, teleport.localPosition.z);
                        teleport.localPosition = newPosition;
                    }
                }
                standing = true;
            }

        }
        else
        {
            if (standing == true)
            {
                if (sittingType == InteractionType.OffsetTracking)
                {
                    trackingSpaceTrans.position = new Vector3(0, 0.337243f, 0);
                }
                else if (sittingType == InteractionType.OffsetTeleports)
                {
                    foreach (Transform teleport in teleportTrans)
                    {
                        teleportHeight = teleport.localPosition.y - .44f;
                        Vector3 newPosition = new Vector3(teleport.localPosition.x, teleportHeight, teleport.localPosition.z);
                        teleport.localPosition = newPosition;
                    }
                }
                standing = false;
            }


        }
    }

    float AverageOverFrames(float newVal)
    {
        count++;

        //This will calculate the MovingAverage AFTER the very first value of the MovingAverage
        if (count > MovingAverageLength)
        {
            movingAverage = movingAverage + (newVal - movingAverage) / (MovingAverageLength + 1);

            //Debug.Log("Moving Average: " + movingAverage); //for testing purposes

        }
        else
        {
            //NOTE: The MovingAverage will not have a value until at least "MovingAverageLength" values are known (10 values per your requirement)
            movingAverage += newVal;

            //This will calculate ONLY the very first value of the MovingAverage,
            if (count == MovingAverageLength)
            {
                movingAverage = movingAverage / count;

                //Debug.Log("Moving Average: " + movingAverage); //for testing purposes
            }
        }

        return movingAverage;
    }
}
