using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    // Tool to remember inputs while an action is happening.
    // Usage: - Call Listen() to start listening for inputs.
    //        - InputBuffer will remember the most recent input key.
    //          - The directional is still used at the relative call frame, so the pairing is (most recent key + current directional)
    //        - When you need the buffer, call Request(), which will return the most recent input and stop listening.

    public List<string> validInputs;

    private string recent = "-";
    private bool listen = false;

    void Update()
    {
        // Listen for inputs here; list is created in ascending priority
        if (listen)
        {
            foreach (string key in validInputs)
            {
                if (Input.GetButtonDown(key))
                {
                    Debug.Log("New key in buffer");
                    recent = key;
                }
            }
        }
        else
        {
            recent = "-";
        }
    }

    // Prompts the InputBuffer to start listening for inputs
    public void Listen()
    {
        recent = "-";
        StartCoroutine(WaitFrame());
    }
    IEnumerator WaitFrame()
    {

        //returning 0 will make it wait 1 frame
        yield return 0;

        listen = true;
    }

    public string Request()
    {
        listen = false;
        return recent;
    }

    public void Discard()
    {
        recent = "-";
        listen = false;
    }
}
