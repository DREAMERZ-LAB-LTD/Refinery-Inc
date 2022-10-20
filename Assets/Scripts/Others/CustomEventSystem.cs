using UnityEngine;
using UnityEngine.EventSystems;

public class CustomEventSystem : EventSystem
{
    protected override void Awake()
    {
        var otherEventSystems = FindObjectsOfType<EventSystem>();
        if(otherEventSystems.Length > 1)
            Destroy(gameObject);

        base.Awake();
    }
}