using UnityEngine;

namespace IdleArcade.Core
{
    [RequireComponent(typeof(TriggerDetector))]
    public class UpgradeablePoint : MonoBehaviour, TriggerDetector.ITriggerable
    {
        [SerializeField] private UpgradeableLimiter[] upgradeableLimiters;
        public void OnEnter(Collider other)
        {
            for(int i =  0; i < upgradeableLimiters.Length; i++)
                UpgradeSystem.instance.Add(upgradeableLimiters[i].GetID);
        }

        public void OnExit(Collider other)
        {
            for (int i = 0; i < upgradeableLimiters.Length; i++)
                UpgradeSystem.instance.Remove(upgradeableLimiters[i].GetID);
        }




        public KeyCode enter, exit;
        void Update()
        { 
            //if(Input.GetKeyDown(enter))
            //    UpgradeSystem.instance.Add(upgradeableIDs);
            //if(Input.GetKeyDown(exit))
            //    UpgradeSystem.instance.Remove(upgradeableIDs);
        }
    }
}