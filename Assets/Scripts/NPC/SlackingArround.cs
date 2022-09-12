using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SlackingArround : MonoBehaviour
{
    [Header("Behaviour Setup")]
    [SerializeField] private Vector2Int ashamedDuration = new Vector2Int(10, 20);
    [SerializeField] private Vector2Int aliveDuration = new Vector2Int(10, 20);

    [Header("Status Setup")]
    [SerializeField] private SlackingAroundStatus statusPrefab;
    [SerializeField] private Vector3 offset;
    private SlackingAroundStatus status = null;
    
    private Camera cam = null;
    private Coroutine routine;

    [Header("Callback Events")]
    [SerializeField] private UnityEvent OnStartSlacking;
    [SerializeField] private UnityEvent OnStopSlacking;

    private void Start()
    {
        var obj = Instantiate(statusPrefab.gameObject);
        status = obj.GetComponent<SlackingAroundStatus>();
        if (status)
        {
            obj.SetActive(false);
            status.wakeUpBtn.onClick.AddListener(OnClickAwakeBtn);
        }
        else
            Destroy(obj);
    }
    private void OnDestroy()
    {
        if (status)
            Destroy(status.gameObject);
    }
    private void OnEnable()
    {
        cam = Camera.main;
        StartSlacking();
    }

    private void OnDisable()
    {
        StopSlacking();
    }

    private void LateUpdate()
    {
        if(status.gameObject.activeInHierarchy)
            status.transform.position = cam.WorldToScreenPoint(transform.position + offset);
    }

    private void OnClickAwakeBtn()
    {
        StopSlacking();
        StartSlacking();
    }

    private void StartSlacking()
    {
        if (routine != null)
            StopCoroutine(routine);

        float ashamedDuration = Random.Range(this.ashamedDuration.x, this.ashamedDuration.y);
        float aliveDuration = Random.Range(this.aliveDuration.x, this.aliveDuration.y);
        routine = StartCoroutine(SlackingRoutine(ashamedDuration, aliveDuration));
    }

    private void StopSlacking()
    {
        if (routine != null)
            StopCoroutine(routine);

        if (status)
            status.gameObject.SetActive(false);

        OnStopSlacking.Invoke();
    }


    private IEnumerator SlackingRoutine(float ashamedDuration, float duration)
    {
        yield return new WaitForSeconds(ashamedDuration);
        float startTime = Time.time;
        float endTime = startTime + duration;
        float t;
        status.gameObject.SetActive(true);
        status.wakeUpBtn.interactable = false;
        while (Time.time < endTime)
        {
            t = Mathf.InverseLerp(startTime, endTime, Time.time);
            status.peogressBar.fillAmount = t;
            yield return null;
        }
        Transform tns = status.transform;
        while (tns.position.x > 0 && tns.position.x < Screen.width &&
              tns.position.y > 0 && tns.position.y < Screen.height)
        {
            yield return new WaitForSeconds(ashamedDuration / 2.0f);
        }
        status.wakeUpBtn.interactable = true;
        OnStartSlacking.Invoke();
    }

}
