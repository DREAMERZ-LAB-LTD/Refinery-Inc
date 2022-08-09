using UnityEngine;

namespace IdleArcade.Core
{
    [CreateAssetMenu(fileName = "New Upgradeable Field", menuName = "Idle Arcade/Upgrade/ Upgradeable Field")]
    public class UpgradeableField : ScriptableObject
    {
        public delegate void FieldChanged(float t);
        public FieldChanged OnFieldChanged;

        [SerializeField] private string iD;
        [SerializeField, Range(0, 1)] private float t = 0;

        public string ID => iD;
        public bool isUpgraded => t == 1;

        public float T
        {
            get
            {
                return t;
            }
            set
            {
                t = value;
                Mathf.Clamp01(t);
                
                if(OnFieldChanged!= null)
                    OnFieldChanged.Invoke(t);
            }
        }
    }
}