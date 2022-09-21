using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    void Start() => anim = GetComponentInChildren<Animator>();
    void Update() => anim.SetBool("input", Input.GetMouseButton(0));
    
}
