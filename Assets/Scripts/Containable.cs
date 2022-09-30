using UnityEngine;

namespace IdleArcade.Core
{
    public class Containable : MonoBehaviour
    {
        [Header("Container Setup")]
        [SerializeField] protected TransactionContainer[] containers;
        public TransactionContainer[] GetContainers => containers;
        public TransactionContainer GetContainer(string id)
        {
            for (int i = 0; i < containers.Length; i++)
            {
                if (containers[i].GetID == id)
                    return containers[i];
            }
            return null;
        }

        public void SetActiveContainers(bool active)
        {
            for (int i = 0; i < containers.Length; i++)
                containers[i].enabled = active;
        }
    }
}