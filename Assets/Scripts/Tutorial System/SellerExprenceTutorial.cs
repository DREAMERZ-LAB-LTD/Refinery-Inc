using UnityEngine;
using UnityEngine.Events;

public class SellerExprenceTutorial : MonoBehaviour, PlayerExprence.IExprenceLevel
{

    [SerializeField] private UnityEvent OnLevelTwo;
    [SerializeField] private UnityEvent OnLevelThree;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
            OnLevelTwo.Invoke();  
        if (Input.GetKeyDown(KeyCode.Alpha3))
            OnLevelThree.Invoke();
    }
    public void OnChangeEcperience(int level)
    {
      
    }
}
