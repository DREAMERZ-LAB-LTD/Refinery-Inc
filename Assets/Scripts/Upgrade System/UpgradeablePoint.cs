using System.Collections;
using UnityEngine;

namespace IdleArcade.Core
{
    [RequireComponent(typeof(TriggerDetector))]
    public class UpgradeablePoint : MonoBehaviour, TriggerDetector.ITriggerable
    {
        [SerializeField] private UpgradeableLimiter[] upgradeableLimiters;

        public void OnEnter(Collider other)
        {
            StartCoroutine(UpgradPopupRoutine());
        }

        public void OnExit(Collider other)
        {
            StopAllCoroutines();
            for (int i = 0; i < upgradeableLimiters.Length; i++)
                UpgradeSystem.instance.Remove(upgradeableLimiters[i].GetID);
        }

        IEnumerator UpgradPopupRoutine()
        {
            while (Input.GetMouseButton(0))
                yield return null;

            for (int i = 0; i < upgradeableLimiters.Length; i++)
                UpgradeSystem.instance.Add(upgradeableLimiters[i].GetID);
        }
    }
}