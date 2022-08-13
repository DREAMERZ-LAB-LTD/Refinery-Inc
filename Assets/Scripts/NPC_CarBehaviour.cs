using System.Collections;
using UnityEngine;
using SWS;
using IdleArcade.Core;

public class NPC_CarBehaviour : MonoBehaviour
{
    public splineMove mover;
    public int aPointIndex;
    public int bPointIndex;

    public TransactionContainer selfContainer;

    private void OnEnable()
    {
        mover.movementChangeEvent += OnChangePoint;
    }
    private void OnDisable()
    {
        mover.movementChangeEvent -= OnChangePoint;
        StopAllCoroutines();
    }




    private void OnChangePoint(int index)
    {
        Debug.Log("Point At=" + index);
       
        
        if (aPointIndex == index)
        { 
            StopAllCoroutines();
            Debug.Log("Point A");
            StartCoroutine(OnSideA());
        }  
        if (bPointIndex == index)
        { 
            StopAllCoroutines();
            Debug.Log("Point B");
            StartCoroutine(OnSideB());
        }
    }

    private IEnumerator OnSideA()
    {
        mover.Pause();

        while (!selfContainer.isFilledUp)
        {
            yield return new WaitForSeconds(1);
        }

        mover.Resume();
    }
    private IEnumerator OnSideB()
    {
        while (true)
        {
            mover.Pause();

            while (!selfContainer.isEmpty)
            {
                yield return new WaitForSeconds(1);
            }

            mover.Resume();
            yield return new WaitForSeconds(1);
        }
    }

}
