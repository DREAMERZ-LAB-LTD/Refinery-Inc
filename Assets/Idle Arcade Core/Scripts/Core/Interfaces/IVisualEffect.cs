using System;
using System.Collections.Generic;
using UnityEngine;
namespace IdleArcade.Core
{
    public interface IVisualEffect
    {
        public void OnAdding(Entity entity, Vector3 from, Vector3 to, Action OnColpleted);
        public void OnChanged(List<Entity> entitys);
    }
}