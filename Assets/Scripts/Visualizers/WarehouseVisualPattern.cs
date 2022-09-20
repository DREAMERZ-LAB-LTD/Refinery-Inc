using IdleArcade.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseVisualPattern : VisualPatternCore
{
    [System.Serializable]
    private class VisualAnchore
    {
        public string id;
        public Transform pivot;
        [HideInInspector]
        public int activeElements = 0;
    }

    [SerializeField] private VisualAnchore[] anchores; 
    public override void OnChanged(List<Entity> entitys)
    {
        Transform entity;
        for (int i = 0; i < entitys.Count; i++)
        {
            entity = entitys[i].transform;
            for (int j = 0; j < anchores.Length; j++)
            {
                if (entitys[i].GetID == anchores[j].id)
                { 
                    entity.parent = anchores[j].pivot;
                    entity.localPosition = GetLocalPointOf(anchores[j].activeElements, entity.localScale);
                    entity.localRotation = Quaternion.identity;
                    anchores[j].activeElements++;
                }
            }
        }

        for (int j = 0; j < anchores.Length; j++)
            anchores[j].activeElements = 0;
    }

}
