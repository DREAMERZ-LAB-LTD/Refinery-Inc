using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    private Joystick joystic;

    void Start()
    {
        joystic = FindObjectOfType<Joystick>();
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (joystic == null)
        {
            joystic = FindObjectOfType<Joystick>();
            if (joystic == null)
                return;
        }
        anim.SetBool("input", joystic.Direction.magnitude > 0.01f);
    }
    
}
