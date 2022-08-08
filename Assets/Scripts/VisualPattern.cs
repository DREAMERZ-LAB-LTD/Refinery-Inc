using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPattern : MonoBehaviour
{
    [SerializeField] Vector3 scale;
    [SerializeField] Vector2 gride;

    public int index;
    void Start()
    {

    }

    public Vector3 GetLocalPositionBy(int index, Vector2 gride, Vector3 scale)
    {
        int x = Mathf.FloorToInt(index / (gride.x * gride.y));
        int gridOffset = Mathf.FloorToInt(x * gride.x * gride.y);
        int z = Mathf.FloorToInt((index - gridOffset) / gride.y);
        int y = Mathf.FloorToInt((index - gridOffset) - z * gride.y);

        return new Vector3(x * scale.x, y * scale.y, z * scale.z);
    }


    void Update()
    {
        var origine = transform.position;
        var localPoint = GetLocalPositionBy(index, gride, scale);
        Debug.DrawLine(origine, transform.TransformPoint(localPoint));

        if (Input.GetMouseButtonDown(1))
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = transform;
            cube.transform.localScale = scale;
            cube.transform.localPosition = localPoint;
            cube.transform.localRotation = Quaternion.identity;
            index++;
        }
    }
}
