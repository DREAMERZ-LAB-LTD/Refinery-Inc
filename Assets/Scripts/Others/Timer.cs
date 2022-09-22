using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Timer : MonoBehaviour
{
    [System.Flags]
    public enum StartSource
    { 
        OnEnable    = 0x000001,
        OnAwake     = 0x000010,
        OnStart     = 0x000100 
    }

    public enum UpdateMode
    {
        Discreate,
        InASecond,
        Continuous
    }

    #region Member Variables
#if UNITY_EDITOR
    [Tooltip("Will never use, \n Just for track why we are using this timer"),
    SerializeField, TextArea(5, 10)]
    private string Description;
#endif

    [Header("Timer Setup")]
    [Tooltip("Counter target time in seconds")]
    public float targetTime = 0;
    [SerializeField, Tooltip("If true timer will start on Awake")]
    private StartSource startSource ;
    [SerializeField, Tooltip("Timer will updating each of the delta seconds otherwise updating after complete each of the second")]
    private UpdateMode updateMode = UpdateMode.Discreate;
    private bool isPaused = false;

    [Header("ValueRender Setup On UI")]
    [SerializeField, Tooltip("If true timer will show how much time left \n otherwise will show continus increase number")]
    bool showLeftTime = false;
    [SerializeField, Tooltip("message will print before the time value on the UI screen")]
    private string preMessage = string.Empty;
    [SerializeField, Tooltip("message will print after the time value on the UI screen")]
    private string postMessage = string.Empty;
    [SerializeField, Tooltip("Where timer value will going to render, \n it could be Slider, Image, Text & TextMeshProUGUI")]
    private MonoBehaviour valueRender = null;

    private Slider valueRendererSlider = null;
    private Image valueRendererImg = null;
    private Text valueRendererTxt = null;
    private TextMeshProUGUI valueRendererTMP = null;


    [Header("Timer Lifecycle Callback Events")]
    [SerializeField] private UnityEvent OnTimerStart;
    [SerializeField] private UnityEvent OnTimerUpdating;
    [SerializeField] private UnityEvent OnTimerOver;

    [Header("Timer Control Callback Events")]
    [SerializeField] private UnityEvent OnTimerPause;
    [SerializeField] private UnityEvent OnTimerResume;
    [SerializeField] private UnityEvent OnTimerStop;

    private Coroutine curretnCounter = null;
    private float time = 0;
    public float GetRemainTime => Mathf.Clamp(targetTime - time, 0, targetTime);

    #endregion  Member Variables
    private void OnEnable()
    {
        OnInitialize();
        if ((startSource & StartSource.OnEnable) == startSource)
            StartTimer();
    }
    private void Awake()
    {
        OnInitialize();
        if ((startSource & StartSource.OnAwake) == startSource)
            StartTimer();
    }

    private void Start()
    {
        if ((startSource & StartSource.OnStart) == startSource)
            StartTimer();
    }


    private void OnInitialize()
    {
        if (valueRender)
        {
            var renderType = valueRender.GetType();

            if (renderType == typeof(Text) && valueRendererTxt == null)
                valueRendererTxt = valueRender.GetComponent<Text>();
            if (renderType == typeof(TextMeshProUGUI) && valueRendererTMP == null)
                valueRendererTMP = valueRender.GetComponent<TextMeshProUGUI>();
            if (renderType == typeof(Slider) && valueRendererSlider == null)
                valueRendererSlider = valueRender.GetComponent<Slider>();
            if (renderType == typeof(Image) && valueRendererImg == null)
                valueRendererImg = valueRender.GetComponent<Image>();
            if (valueRendererImg)
                valueRendererImg.type = Image.Type.Filled;
        }
    }


    #region Timer Control
    public void StartTimer()
    {
        if (!gameObject.activeInHierarchy || !enabled)
            return;

        time = 0;
        StopTimer();
        OnTimerStart.Invoke();
        curretnCounter = StartCoroutine(Counter());
    }


    public void PauseTimer()
    {
        OnTimerPause.Invoke();
        isPaused = true;
    }

    public void ResumeTimer()
    {
        OnTimerResume.Invoke();
        isPaused = false;
    }


    public void StopTimer()
    {
        if (curretnCounter != null)
        { 
            StopCoroutine(curretnCounter);
            OnTimerStop.Invoke();
        }
    }
    #endregion Timer Control

    #region Timer Mechanism
    /// <summary>
    /// actual time counter thread
    /// </summary>
    /// <returns></returns>
    private IEnumerator Counter()
    {
        bool complete = time >= targetTime;
        float timeOffset = Time.time;
        while (!complete)
        {
            while (isPaused)
                yield return null;


            switch (updateMode)
            {

                case UpdateMode.Discreate:
                    yield return new WaitForSeconds(targetTime);
                    time = targetTime;
                    complete = true;
                    break;

                case UpdateMode.InASecond:
                    time++;
                    complete = time > targetTime - 1;
                    yield return new WaitForSeconds(1);
                    break;

                case UpdateMode.Continuous:
                    time = Time.time - timeOffset;
                    complete = time > targetTime ;
                    yield return null;
                    break;
            }

            RenderValue(time);
            OnTimerUpdating.Invoke();
        }
        OnTimerOver.Invoke();
    }


    /// <summary>
    /// preapering time to show forward or inverse
    /// </summary>
    /// <param name="currentTime">Timer current time</param>
    private void RenderValue(float currentTime)
    {
        if (valueRender == null)
            return;

        if (valueRendererSlider)
        {
            valueRendererSlider.maxValue = targetTime;
            valueRendererSlider.value = currentTime;
        }
        if (valueRendererImg)
        {
            valueRendererImg.fillAmount = currentTime / targetTime;
        }

        
        int time = Mathf.CeilToInt(currentTime);
        int target = Mathf.CeilToInt(targetTime);
        if (showLeftTime)
            time = target - time;


        string message = preMessage + time + postMessage;
        if (valueRendererTMP)
            valueRendererTMP.text = message;
        if(valueRendererTxt)
            valueRendererTxt.text = message;
    }



    #endregion  Timer Mechanism
}