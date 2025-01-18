using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    /// <summary>
    /// The Dictionary that stores the controls for the game.
    /// Although customisable, they do have default values.
    /// </summary>
    private static Dictionary<string, KeyCode> Controls = new Dictionary<string, KeyCode>()
    {
        ["Accelerate"] = KeyCode.Mouse0,
        ["Escape"] = KeyCode.Mouse1,
        ["Detonate"] = KeyCode.Mouse2,
        ["Exit"] = KeyCode.Escape
    };

    public static GameManager Instance 
    { get { return _instance; } }

    public static Dictionary<string, KeyCode> controls 
    { get { return Controls; } }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Assigns the Dictionary of Controls to the one specified in the parameters.
    /// </summary>
    /// <param name="controls"></param>
    public static void SetControls(Dictionary<string, KeyCode> controls)
    {
        Controls = controls;
    }

    /// <summary>
    /// Transitions to the next scene in the build load.
    /// </summary>
    public static void TransitionToNextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    public static void Exit()
    {
        if (Input.GetKey(Controls["Exit"]))
        {
            Application.Quit();
        }
    }
}
