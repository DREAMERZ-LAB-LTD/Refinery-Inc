using UnityEngine;

public class ScalingEffect : MonoBehaviour
{
    [Header("Scaling Effect Setup")]
    [SerializeField] private float speed = 7;
    [SerializeField] private Vector3 scaleEffect = new Vector3(0.1f, 0.1f, 0.1f);

    private Vector3 initialScale;
    private void Awake()
    {
        initialScale = transform.localScale;
    }
    private void OnDestroy()
    {
        transform.localScale = initialScale;
    }

    private void Update()
    {
        transform.localScale = initialScale + scaleEffect * Mathf.Sin(Time.time * speed);
    }

}
