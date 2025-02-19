using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TimedLevelLoader : MonoBehaviour
{
    public float timer;
    private float timeout = 0;
    private float countdown = 0;
    public bool nextLevel = true;
    public int level = 1;
    public bool canskip = false;
    private bool pressed = false;
    private float skipTime = 1;
    private bool loaded = false;
    public bool quit = false;
    [Tooltip("If yes, ignore timed part")]
    public bool funcCall = false;

    public void LoadTargLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    void Update()
    {
        if (!funcCall)
        {
            timeout += Time.deltaTime;
            if (canskip)
            {
                if (countdown > skipTime)
                {
                    if (!pressed && Input.anyKey)
                    {
                        if (!quit)
                        {
                            if (nextLevel)
                            {
                                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                            }
                            else
                            {
                                SceneManager.LoadScene(level);
                            }
                        }
                        else
                        {
                            Application.Quit();
                        }
                        pressed = true;
                    }
                }
                else
                {
                    countdown += Time.deltaTime;
                }
            }


            if (timeout >= timer)
            {
                if (!loaded)
                {
                    if (!quit)
                    {
                        if (nextLevel)
                        {
                            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
                        }
                        else
                        {
                            SceneManager.LoadScene(level);
                        }
                    }
                    else
                    {
                        Application.Quit();
                    }
                    loaded = true;
                }
            }
        }
    }    
}
