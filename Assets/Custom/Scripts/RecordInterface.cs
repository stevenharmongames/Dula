using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Video;
using UnityEngine.Animations.Rigging;
using TMPro;

public class RecordInterface : MonoBehaviour
{
    public BVHRecorder recscript;
    private string filenametext;
    public string savedir = @"C:\Users\steve\Desktop\Recordings";
    [Tooltip("Rec / Stop")]
    public Text recordingtext;
    [Tooltip("Bottom Text")]
    public TMP_Text msgtext;
    private bool isrecording = false;
    private int curTake = 0;

    public Animator anim;
    public OVRBody ovrBodyScript;
    public OVRUnityHumanoidSkeletonRetargeter ovrRetargeterScript;
    public Rig myRig;

    private AudioClip recordedClip;
    [SerializeField] AudioSource audioSource;
    private string filePath = "recording.wav";
    private string directoryPath = @"C:\Users\steve\Desktop\Recordings";
    private float startTime;
    private float recordingLength;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            recscript.directory = Application.persistentDataPath;
        }
#endif

#if UNITY_STANDALONE
                recscript.directory = System.IO.Directory.GetCurrentDirectory();
#endif

#if UNITY_EDITOR
        recscript.directory = savedir;
#endif

        filenametext = "take_" + curTake /*+ System.DateTime.Now.ToString()*/  + ".bvh";
        msgtext.enabled = true;
        msgtext.text = "Your Message";
    }

    public void HitRecord()
    {
        isrecording = !isrecording;
        if (isrecording)
        {
            recordingtext.text = "Stop ■";
        }
        else
        {
            recordingtext.text = "Rec ⦿";
        }
        StartCoroutine(RecBlink());
    }

    private IEnumerator RecBlink()
    {
        if (isrecording)
        {
            recscript.clearCapture();
            yield return new WaitForSeconds(.1f);
            recscript.capturing = true;

            if (ovrBodyScript != null && ovrRetargeterScript != null)
            {
                ovrBodyScript.enabled = false;
                ovrRetargeterScript.enabled = false;
                myRig.weight = 0f;
            }

            if (myRig == null)
            {
                anim.enabled = true;
            }
            yield return new WaitForSeconds(.1f);
            anim.SetTrigger("TPose");
            yield return new WaitForSeconds(.5f);
            anim.StopPlayback();
            if (myRig == null)
            {
                anim.enabled = false;
            }

            if (ovrBodyScript != null && ovrRetargeterScript != null)
            {
                ovrBodyScript.enabled = true;
                ovrRetargeterScript.enabled = true;
                myRig.weight = 1f;
            }
            StartRecording();
        }
        else
        {
            recscript.capturing = false;

            yield return new WaitForSeconds(1f);
            recscript.saveBVH();

            curTake++;
            filenametext = "take_" + /*System.DateTime.Now.ToString()*/ curTake + ".bvh";

            msgtext.enabled = true;
#if UNITY_ANDROID
            msgtext.text = filenametext + ".bvh saved @ " + Application.persistentDataPath;
#endif

#if UNITY_STANDALONE
                        msgtext.text = filenametext + ".bvh saved @ " + System.IO.Directory.GetCurrentDirectory();
#endif
#if UNITY_EDITOR
            msgtext.text = filenametext + ".bvh saved @ " + savedir;
#endif
            msgtext.enabled = true;
            msgtext.text = "Recorded!";
            yield return new WaitForSeconds(1);
            msgtext.enabled = false;
            StopRecording();
        }
    }

    public void StartRecording()
    {
        string device = Microphone.devices[0];
        int sampleRate = 44100;
        int lengthSec = 3599;

        recordedClip = Microphone.Start(device, false, lengthSec, sampleRate);
        startTime = Time.realtimeSinceStartup;
    }

    public void StopRecording()
    {
        Microphone.End(null);
        recordingLength = Time.realtimeSinceStartup - startTime;
        recordedClip = TrimClip(recordedClip, recordingLength);
        SaveRecording();
    }

    public void SaveRecording()
    {
        if (recordedClip != null)
        {
            filePath = Path.Combine(directoryPath, filePath);
            WavUtility.Save(filePath, recordedClip);
            Debug.Log("Recording saved as " + filePath);
        }
        else
        {
            Debug.LogError("No recording found to save.");
        }
    }

    private AudioClip TrimClip(AudioClip clip, float length)
    {
        int samples = (int)(clip.frequency * length);
        float[] data = new float[samples];
        clip.GetData(data, 0);

        AudioClip trimmedClip = AudioClip.Create(clip.name, samples,
            clip.channels, clip.frequency, false);
        trimmedClip.SetData(data, 0);

        return trimmedClip;
    }
}
