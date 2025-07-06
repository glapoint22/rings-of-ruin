using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        // Targeting input - more efficient with else if
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                GameEvents.RaiseShiftTabPressed(); // Reverse cycling
            }
            else
            {
                GameEvents.RaiseTabPressed(); // Forward cycling
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameEvents.RaiseEscapePressed();
        }
        else if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            GameEvents.RaiseMouseTargetPressed(Input.mousePosition);
        }
        else if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            GameEvents.RaiseRightMousePressed(Input.mousePosition);
        }
    }
}