using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (GameManager.instance.Joystic == null) return;
        
        anim.SetBool("input", GameManager.instance.Joystic.Direction.magnitude > 0.01f);
    }
     
    
    private void OnDestroy()
    {
        anim.SetBool("input", false);
    }

}
