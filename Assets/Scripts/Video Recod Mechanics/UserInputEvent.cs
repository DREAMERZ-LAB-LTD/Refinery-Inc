using UnityEngine;
using UnityEngine.Events;

public class UserInputEvent : MonoBehaviour
{
    [Header("User Input Key Code")]
    [SerializeField] private KeyCode input;

    [Header("Callback Events")]
    [SerializeField] private UnityEvent OnKeyDown;
    [SerializeField] private UnityEvent OnKey;
    [SerializeField] private UnityEvent OnKeyUp;

    private void Update()
    {
        if (Input.GetKeyDown(input))
            OnKeyDown.Invoke();
        if (Input.GetKey(input))
            OnKey.Invoke();
        if (Input.GetKeyUp(input))
            OnKeyUp.Invoke();
    }
}
