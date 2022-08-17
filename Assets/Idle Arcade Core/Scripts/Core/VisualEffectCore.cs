using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade.Core
{
    public class VisualEffectCore : MonoBehaviour, IVisualEffect
    {
        [SerializeField, Range(0, 0.5f)] protected float duration = 0.2f;
        public virtual void OnAdding(Entity entity, Vector3 from, Vector3 to, Action OnColpleted)
        {
            StartCoroutine(MoveTo(entity.transform, from, to, OnColpleted));
        }

        protected IEnumerator MoveTo(Transform visualELemrnt, Vector3 from, Vector3 to, Action OnColpleted)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;
            float t;
            while (Time.time < endTime)
            {
                t = Mathf.InverseLerp(startTime, endTime, Time.time);
                visualELemrnt.localPosition = Vector3.Lerp(from, to, t);
                yield return null;
            }
            OnColpleted.Invoke();
        }

        public virtual void OnChanged(List<Entity> entitys) { }
    }
}