using UnityEngine;

namespace IdleArcade.Core
{
    public class Limiter : Entity
    {
        [SerializeField,Tooltip("Total range")]
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
        /// <summary>
        /// return true if limiter fully upgraded
        /// </summary>
        public bool isUpgraded => t == 1;

        /// <summary>
        /// upgrade Limit based on T value
        /// </summary>
        /// <param name="dt">Value must be 0 to 1</param>
        /// <param name="id">Target limit ID</param>
        /// <returns></returns>
        public bool Upgrade(float dt, string id)
        {
            if (this.GetID != id) return false;
            if (t == 1) return false;

            dt = Mathf.Abs(dt);
            t += dt;
            Mathf.Clamp01(t);
            return true;
        }
    }
}