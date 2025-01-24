using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterSceneOnView : MonoBehaviour
{
    private GameObject mainCam;
    private float rotAngle = 0;

    void Start()
    {
        StartCoroutine(SetScene());
    }
	
    //place this script on parent object of player to offset playspace based on initial look rotation, so they don't need to look around for the content
    private IEnumerator SetScene()
    {
        yield return new WaitForSeconds(.15f);
        mainCam = GameObject.FindWithTag("MainCamera");
        yield return new WaitForSeconds(.15f);
        if (mainCam != null)
        {
            rotAngle = mainCam.transform.rotation.eulerAngles.y;
        }
        yield return new WaitForEndOfFrame();
        transform.rotation = Quaternion.Euler(0, rotAngle, 0);
    }
}