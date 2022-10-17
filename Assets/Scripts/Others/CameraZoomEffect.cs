using Cinemachine;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraZoomEffect : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    [SerializeField] private Vector2 zoomRange = new Vector2(15, 12);
    [SerializeField] private float zoomInSpeed = 1;
    [SerializeField] private float zoomOutSpeed = 1;
    float t = 0;
    private void Start()
    {
        Application.quitting += StopAllCoroutines;

        cam = GetComponent<CinemachineVirtualCamera>();
       
        if(cam)
            cam.m_Lens.OrthographicSize = zoomRange.x;
    }
    private void LateUpdate()
    {
        if (cam == null)
            return;

        if (GameManager.instance.Joystic == null) return;
        

        if (GameManager.instance.Joystic.Direction.magnitude > 0.01f)
        {
            t += zoomInSpeed * Time.fixedDeltaTime;
            t = Mathf.Clamp01(t);
            cam.m_Lens.OrthographicSize = Mathf.Lerp(zoomRange.x, zoomRange.y, t);
        }
        else if (t > 0)
        { 
            t -= zoomOutSpeed * Time.fixedDeltaTime;
            t = Mathf.Clamp01(t);
            cam.m_Lens.OrthographicSize = Mathf.Lerp(zoomRange.x, zoomRange.y, t);
        }
    }

    private void OnEnable()
    {
        StopAllCoroutines();
    }
    private void OnDisable()
    {

        StartCoroutine(ZoomOutRoutnie());
        IEnumerator ZoomOutRoutnie()
        {
            float startZoom = cam.m_Lens.OrthographicSize;

            float t;
            float startTime = Time.time;
            float endTime = startTime + 1;
            while (Time.time < endTime)
            {
                t = Mathf.InverseLerp(startTime, endTime, Time.time);
                cam.m_Lens.OrthographicSize = Mathf.Lerp(startZoom, zoomRange.x, t);
                yield return null;
            }
        }
    }

}
