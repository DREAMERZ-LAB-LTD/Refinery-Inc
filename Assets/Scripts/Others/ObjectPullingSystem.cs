using System.Collections.Generic;
using UnityEngine;
using IdleArcade.Core;
using System;

public class ObjectPullingSystem : MonoBehaviour
{
    [System.Serializable]
    private class Prefab
    {
        public int pullAmount;
        public Entity entity;
    }

    private class PullableObject
    {
        public string id;
        public Stack<Entity> objects = new Stack<Entity>();

        public PullableObject(string id)
        {
            this.id = id;
        }
    }

    [SerializeField] private Prefab[] prefabs;
    private List<PullableObject> pullableObjects = new List<PullableObject>();


    private void Awake()
    {
        for (int i = 0; i < prefabs.Length; i++)
            SpawnPrefab(prefabs[i].entity, prefabs[i].pullAmount);
    }

    private void Start()
    {
        if (GameManager.instance.pullingSystem == null)
            GameManager.instance.pullingSystem = this;
    }

    private void OnDestroy()
    {
        try
        {
            if (GameManager.instance.pullingSystem == this)
                GameManager.instance.pullingSystem = null;
        }
        catch (Exception e) { }
    }
        

    private PullableObject GetPullableObject(string id)
    {
        for (int i = 0; i < pullableObjects.Count; i++)
            if (pullableObjects[i].id == id)
                return pullableObjects[i];

        return null;
    }

    private Prefab GetPrefab(string id)
    {
        for (int i = 0; i < prefabs.Length; i++)
            if (prefabs[i].entity.GetID == id)
                return prefabs[i];

        return null;
    }

    private void SpawnPrefab(Entity entity, int amount)
    {
        var pullable = GetPullableObject(entity.GetID);
        if (pullable == null)
        { 
            pullable = new PullableObject(entity.GetID);
            pullableObjects.Add(pullable);
        }
       

        GameObject obj;
        Entity e;
        for (int i = 0; i < amount; i++)
        {
            obj = Instantiate(entity.gameObject, transform);
            obj.SetActive(false);
            obj.AddComponent<PullingErrorDebugger>();
            e = obj.GetComponent<Entity>();
            pullable.objects.Push(e);
        }
    }

    public Entity Pull(string id)
    {
        var pullable = GetPullableObject(id);
        if (pullable == null)
            return null;

        if (pullable.objects.Count == 0)
        {
            var prefab = GetPrefab(id);
            SpawnPrefab(prefab.entity, 10);
        }

        var entity = pullable.objects.Pop();
        entity.gameObject.SetActive(true);
        return entity;
    }

    public bool Push(Entity entity)
    {
        var pullable = GetPullableObject(entity.GetID);
        if (pullable == null)
            return false;

        entity.transform.parent = transform;
        entity.gameObject.SetActive(false);
        pullable.objects.Push(entity);

        return true;
    }
}
