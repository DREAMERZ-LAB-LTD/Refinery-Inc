using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraZoomBlender : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] private float speed = 1;
    [SerializeField] private Vector2 zoomRange = new Vector2(15, 12);

    public void BlendZoom(float t)
    {
        cam.m_Lens.OrthographicSize = Mathf.Lerp(zoomRange.x, zoomRange.y, t);
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(MotionRoutine());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }


    private IEnumerator MotionRoutine()
    {
        float t = 0;
        zoomRange.x = cam.m_Lens.OrthographicSize;
        while (t < 1)
        {
            t += speed * Time.deltaTime;
            t = Mathf.Clamp01(t);
            cam.m_Lens.OrthographicSize = Mathf.Lerp(zoomRange.x, zoomRange.y, t);
            yield return null;
        }
    }
}
