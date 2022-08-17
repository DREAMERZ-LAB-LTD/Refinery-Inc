using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade.Core
{
    public interface IVisualPattern
    {
        public void OnChanged(List<Entity> entitys);
        public Vector3 GetLocalPointOf(int index, Vector3 scale);
    }
}