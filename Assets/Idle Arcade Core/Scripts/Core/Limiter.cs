using UnityEngine;

namespace IdleArcade.Core
{
    public class Limiter : Entity
    {
        [SerializeField, Tooltip("Total range")]
        protected Vector2 range;
        [SerializeField, Range(0, 1), Tooltip("Blend amount from min of max value of Range")]
        protected float t = 0.2f;
 

        /// <summary>
        /// Return total range
        /// </summary>
        public Vector2 GetRange => range;
        /// <summary>
        /// return current maxmimum value of this limit
        /// </summary>
        public float GetCurrent => Mathf.Lerp(range.x, range.y, t);

    }
}