using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade.Core
{
    public class VisualPatternCore : MonoBehaviour, IVisualPattern
    {
        [SerializeField] Vector2 gride;
      
        public virtual void OnChanged(List<Entity> entitys)
        {
            Transform entity = null;
            var rotation = Quaternion.identity;
            for (int i = 0; i < entitys.Count; i++)
            {
                entity = entitys[i].transform;
                entity.parent = transform;
                entity.localPosition = GetLocalPointOf(i, entity.localScale);
                entity.localRotation = rotation;
            }
        }

        public virtual Vector3 GetLocalPointOf(int index, Vector3 scale)
        {
            int x = Mathf.FloorToInt(index / (gride.x * gride.y));
            int gridOffset = Mathf.FloorToInt(x * gride.x * gride.y);
            int z = Mathf.FloorToInt((index - gridOffset) / gride.y);
            int y = Mathf.FloorToInt((index - gridOffset) - z * gride.y);

            return new Vector3(x * scale.x, y * scale.y, z * scale.z);
        }
    }
}