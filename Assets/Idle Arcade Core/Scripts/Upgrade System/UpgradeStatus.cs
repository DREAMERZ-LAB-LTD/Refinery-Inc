using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace IdleArcade.Core
{
    public class UpgradeStatus : Entity
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text name_Text;
        [SerializeField] private TMP_Text price_Text;
        [SerializeField] private Image progress;
        [SerializeField] Button UpgradeButton;

        private UpgradeableDataFields.Data data = null;
        public UpgradeableDataFields.Data Data
        {
            get { return data; }
            set
            {
                this.data = value;

                if (name_Text)
                    name_Text.text = data.Name;
                if (price_Text)
                    price_Text.text = "80";

                if (icon) { }
                if (progress) { }

                if (UpgradeButton)
                {
                    if (data.isUpgraded)
                        UpgradeButton.interactable = false;
                    else
                        UpgradeButton.onClick.AddListener(Upgrade);
                }
            }
        }

        public void Upgrade()
        {
            data.T = 1;
        }
    }
}