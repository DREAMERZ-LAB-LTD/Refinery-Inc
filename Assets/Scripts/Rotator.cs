using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 direction = Vector3.up;
    private void Update() => transform.Rotate(direction);
}
