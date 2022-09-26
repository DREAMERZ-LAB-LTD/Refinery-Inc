using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerExprence : MonoBehaviour
{
    public delegate void ChangeLevel(int level);
    public ChangeLevel OnChangeLevel;

    [Header("Level Setup")]
    [SerializeField] private Vector2Int range = new Vector2Int(0, 100);
    [SerializeField] private int[] levelSegment;

    [Header("Progress Text Setup")]
    [SerializeField] private string preMessage;
    [SerializeField] private string postMessage;
    [SerializeReference] private TextMeshProUGUI progressText;

    [Header("Progress Image Setup")]
    [SerializeReference] private Image progressBar;

    public int Progress
    {
        get { return PlayerPrefs.GetInt("Experience_Progress"); }
        set
        {
            value = Mathf.Clamp(value, range.x, range.y);
            PlayerPrefs.SetInt("Experience_Progress", value);
        }
    }
    public int Level
    {
        get { return PlayerPrefs.GetInt("Exprence_Level"); }
        set 
        {
            if(Level != value)
                if (OnChangeLevel != null)
                    OnChangeLevel.Invoke(value);

            PlayerPrefs.SetInt("Exprence_Level", value);
        }
    }

    private void Start()
    {
        UpdateUI(Level, Progress);
        GameManager.instance.playerExprence = this;
    }


    public void AddReview(int dt)
    {
        int progress = Progress + dt;
        for (int index = 0; index < levelSegment.Length; index++)
            if (progress < levelSegment[index])
            {
                Level = index;
                break;
            }
        Progress = progress;
        UpdateUI(Level, progress);
    }

    private void UpdateUI(int levelIndex, int progress)
    {
        int min = levelIndex == 0 ? range.x : levelSegment[levelIndex - 1];
        int max = levelIndex == levelSegment.Length - 1 ?  range.y : levelSegment[levelIndex];

        if (progressBar)
            progressBar.fillAmount = Mathf.InverseLerp(min, max, progress);

        if (progressText)
            progressText.text = preMessage + (levelIndex + 1) + postMessage;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            AddReview(5);
        if (Input.GetKeyDown(KeyCode.B))
            AddReview(-5);
    }
    private void OnDisable()
    {
      //  Level = 0;
      //  Progress = 0;
    }
}
