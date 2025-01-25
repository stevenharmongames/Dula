using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScene : MonoBehaviour
{

    
    public void Scene(string sceneName)
    {
        Debug.Log("Loading scene!!!");
        SceneManager.LoadScene("SampleScene");
    }
}