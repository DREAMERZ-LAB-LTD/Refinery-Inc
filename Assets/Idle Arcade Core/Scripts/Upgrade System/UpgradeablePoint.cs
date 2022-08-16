using UnityEngine;

namespace IdleArcade.Core
{
    [RequireComponent(typeof(TriggerDetector))]
    public class UpgradeablePoint : MonoBehaviour, TriggerDetector.ITriggerable
    {
        [SerializeField] private string[] upgradeableIDs;
        public void OnEnter(Collider other)
        {
            UpgradeSystem.instance.Add(upgradeableIDs);
        }

        public void OnExit(Collider other)
        {
            UpgradeSystem.instance.Remove(upgradeableIDs);
        }




        public KeyCode enter, exit;
        void Update()
        { 
            if(Input.GetKeyDown(enter))
                UpgradeSystem.instance.Add(upgradeableIDs);
            if(Input.GetKeyDown(exit))
                UpgradeSystem.instance.Remove(upgradeableIDs);
        }
    }
}