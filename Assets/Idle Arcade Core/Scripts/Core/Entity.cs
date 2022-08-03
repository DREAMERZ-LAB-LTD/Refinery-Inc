using UnityEngine;

namespace IdleArcade.Core
{
    public class Entity : MonoBehaviour
    {
        [Tooltip("Unique ID to identify specifically from other")]
        [SerializeField] private string id;

        /// <summary>
        /// Return unique ID to identify specifically from other
        /// </summary>
        public string GetID => id;
    }
}