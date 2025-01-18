using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Contains the Enums, Dictionary<string, KeyCode>() and functions for setting Player control choices. 
/// </summary>
public class Control_Selection : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Accelerate_Value;
    [SerializeField] TextMeshProUGUI Escape_Value;
    [SerializeField] TextMeshProUGUI Detonate_Value;

    [SerializeField] float Countdown = 1.5f;

    public Dictionary<string, KeyCode> Controls = new Dictionary<string, KeyCode>();

    enum ValueSelection { Accelerate, Escape, Detonate, Done };
    ValueSelection Selection;
    void Start()
    {
        Controls = new Dictionary<string, KeyCode>();
        Controls["Exit"] = KeyCode.Escape;
        Selection = ValueSelection.Accelerate;
    }

    // Update is called once per frame
    void Update()
    {
        switch (Selection)
        {
            case ValueSelection.Accelerate:
                if(SetKey(Accelerate_Value, Selection.ToString()))
                {
                    Selection = ValueSelection.Escape;
                }
                break;
            case ValueSelection.Escape:
                if (SetKey(Escape_Value, Selection.ToString()))
                {
                    Selection = ValueSelection.Detonate;
                }
                break;
            case ValueSelection.Detonate:
                if (SetKey(Detonate_Value, Selection.ToString()))
                {
                    Selection = ValueSelection.Done;
                }
                break;
            case ValueSelection.Done:
                if(Countdown > 0f)
                {
                    Countdown -= Time.deltaTime;
                    break;
                }

                //Transition to next scene.
                GameManager.SetControls(Controls);
                GameManager.TransitionToNextScene();
                break;
            default:
                break;
        }

        GameManager.Exit();
    }

    /// <summary>
    /// Sets the dictionary value of the control string passed in as a parameter to the first valid button (keyboard or mouse) pressed.
    /// </summary>
    /// <param name="textMeshText"></param>
    /// <param name="controlKey"></param>
    /// <returns></returns>
    bool SetKey(TextMeshProUGUI textMeshText, string controlKey)
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            ///Checks that a key has been pressed and that the specific key has not already been assigned to another dictionary entry.
            if (Input.GetKeyDown(keyCode) && !Controls.ContainsValue(keyCode))
            {
                textMeshText.text = keyCode.ToString();
                Controls[controlKey] = keyCode;
                return true;
            }
        }
        return false;
    }
}
