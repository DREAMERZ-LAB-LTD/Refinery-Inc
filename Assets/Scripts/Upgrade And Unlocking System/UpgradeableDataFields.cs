using UnityEngine;

namespace IdleArcade.Core
{
    [CreateAssetMenu(fileName = "New Upgradeable Data", menuName = "Idle Arcade/Upgrade/ Upgradeable Data")]
    public class UpgradeableDataFields : ScriptableObject
    {
        public delegate void ChangedFieldValue(float t);
        public delegate void ChangedUnlockValue(bool isUnlocked);
        [System.Serializable] public class Data
        {
            public ChangedFieldValue OnChanged;
            public ChangedUnlockValue OnUnlocking;

            [Header("Visual Element")]
            [SerializeField] public string iD;
            [SerializeField] public string name;
            [SerializeField] public Sprite icon;

            [Header("Upgrade & Unlocking Status")]
            [SerializeField] private bool m_isUnlocked = false;
            [SerializeField, Range(0, 1)]  private float t = 0.2f;
            [SerializeField, Range(0, 1)]  public float dt = 0.1f;

            [Header("Pricing Setup")]
            [SerializeField] public string coinID;
            [SerializeField] public int unlockPrice = 10;
            [SerializeField] public int upgradePrice = 10;


            public bool isUpgraded => t == 1;

            public bool isUnlocked
            {
                get
                {
                    if (PlayerPrefs.HasKey(iD + "Unlock"))
                    {
                        int status = PlayerPrefs.GetInt(iD + "Unlock");
                        return status == 1 ? true : false;
                    }
                    else 
                    {
                        int status = m_isUnlocked ? 1 : 0;
                        PlayerPrefs.SetInt(iD + "Unlock", status);
                        return m_isUnlocked;
                    }
                }
                set
                {
                    m_isUnlocked = value;
                    int status = m_isUnlocked ? 1 : 0;
                    PlayerPrefs.SetInt(iD + "Unlock", status);

                    if (OnUnlocking != null)
                        OnUnlocking.Invoke(value);
                }
            }

            public float T
            {
                get
                {
                    if (PlayerPrefs.HasKey(iD + "upgrade"))
                    {
                         return PlayerPrefs.GetFloat(iD + "upgrade");
                    }
                    else
                    {
                        t = Mathf.Clamp01(t);
                        PlayerPrefs.SetFloat(iD + "upgrade", t);
                        return t;
                    }
                }
                set
                {
                    t = Mathf.Clamp01(value);
                    PlayerPrefs.SetFloat(iD + "upgrade", t);
                    if (OnChanged!= null)
                        OnChanged.Invoke(t);
                }
            }

#if UNITY_EDITOR
            public void RefreshPresistantData()
            {
                int status = m_isUnlocked ? 1 : 0;
                PlayerPrefs.SetInt(iD + "Unlock", status);
                PlayerPrefs.SetFloat(iD + "upgrade", Mathf.Clamp01(t)); 
            }
#endif
        }


        [Header("Upgradeable Data Fields")]
        [SerializeField] public Data[] fields;

#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < fields.Length; i++)
                fields[i].RefreshPresistantData();
        }
#endif
    }
}