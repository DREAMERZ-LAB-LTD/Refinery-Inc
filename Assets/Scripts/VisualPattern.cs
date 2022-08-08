using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPattern : MonoBehaviour
{
    [SerializeField] Vector3 scale;
    [SerializeField] int sizeX;
    [SerializeField] int sizeY;
    public GameObject[] visualStack;

    public int index;
    void Start()
    {
        
    }

    public Vector3 GetPosition(int index, int sizeX, int sizeY, Vector3 scale)
    {
        int x = index / (sizeX * sizeY);
        int gridOffset = x * sizeX * sizeX;
        int z = (index - gridOffset) / sizeY;
        int y = (index - gridOffset) - z * sizeY;

        return new Vector3(x * scale.x, y * scale.y, z * scale.z);
    }


    void Update()
    {
        var origine = transform.position;
        var localPoint = GetPosition(index, sizeX, sizeY, scale);
        Debug.DrawLine(origine, origine + localPoint);

        if (Input.GetMouseButtonDown(1))
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localScale = scale;
            cube.transform.position = origine + localPoint;
            cube.transform.parent = transform;
            index++;
        }
    }
}
