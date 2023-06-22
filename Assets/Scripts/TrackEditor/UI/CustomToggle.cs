using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomToggle : MonoBehaviour
{
    public List<GameObject> firstGroup = new List<GameObject>();
    public List<GameObject> secondGroup = new List<GameObject>();

    enum State { FIRST, SECOND }
    State toggleState = State.FIRST;

    public void ToggleButtons()
    {
        switch (toggleState)
        {
            case State.FIRST:
                toggleState = State.SECOND;
                SetStates(false, true);
                break;
            case State.SECOND:
                toggleState = State.FIRST;
                SetStates(true, false);
                break;
        }
    }

    private void SetStates(bool firstState, bool secondState)
    {
        foreach (GameObject obj in firstGroup)
            obj.SetActive(firstState);
        foreach (GameObject obj in secondGroup)
            obj.SetActive(secondState);
    }

    public void ResetButtons()
    {
        toggleState = State.FIRST;
        SetStates(true, false);
    }
}
