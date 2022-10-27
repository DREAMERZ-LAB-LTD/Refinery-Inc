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
        if (GameManager.instance.Joystic) 
            anim.SetBool("input", GameManager.instance.Joystic.Direction.magnitude > 0.01f);
    }
     
    
    private void OnDisable()
    {
        anim.SetBool("input", false);
    }

    public void OnHurry(bool ishurry)
    {
        anim.SetBool("hurry", ishurry);
    }  
    public void OnFailed(bool isFailed)
    {
        anim.SetBool("failed", isFailed);
    }

}
