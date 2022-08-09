using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade.Core
{
    public class VisualPatternCore : MonoBehaviour
    {
        [SerializeField] Vector2 gride;
        private TransactionVisualCore tarnsactionVisual = null;
        protected virtual void Awake()
        {
            tarnsactionVisual = GetComponentInChildren<TransactionVisualCore>();
            if(tarnsactionVisual == null)
                tarnsactionVisual = GetComponentInParent<TransactionVisualCore>();

            if(tarnsactionVisual)
                tarnsactionVisual.OnChangedVisual += OnChanged;
        }
        protected virtual void OnDestroy()
        {
            if (tarnsactionVisual)
                tarnsactionVisual.OnChangedVisual -= OnChanged;
        }
        protected virtual void OnChanged(List<Entity> entitys)
        {
            Transform entity = null;
            for (int i = 0; i < entitys.Count; i++)
            {
                entity = entitys[i].transform;
                entity.parent = transform;
                entity.localPosition = GetLocalPosition(i, gride, entity.localScale);
                entity.localRotation = Quaternion.identity;
            }
        }

        protected virtual Vector3 GetLocalPosition(int index, Vector2 gride, Vector3 scale)
        {
            int x = Mathf.FloorToInt(index / (gride.x * gride.y));
            int gridOffset = Mathf.FloorToInt(x * gride.x * gride.y);
            int z = Mathf.FloorToInt((index - gridOffset) / gride.y);
            int y = Mathf.FloorToInt((index - gridOffset) - z * gride.y);

            return new Vector3(x * scale.x, y * scale.y, z * scale.z);
        }
    }
}