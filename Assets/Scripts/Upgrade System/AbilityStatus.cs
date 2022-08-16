using UnityEngine;
using UnityEngine.UI;
using TMPro;
using General.Library;
namespace IdleArcade.Core
{
    public class AbilityStatus : Entity
    {
        [Header("Unlocking Field Setup")]
        [SerializeField] private GameObject unlockingPanel;
        [SerializeField] Button UnlockButton;


        [Header("Upgrade Field Setup")]
        [SerializeField] private GameObject upgradePanel;
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
                data = value;

                unlockingPanel.SetActive(!data.isUnlocked);
                upgradePanel.SetActive(data.isUnlocked);


                if (name_Text)
                    name_Text.text = data.name;

                if (price_Text)
                    price_Text.text ="$ " + (data.isUnlocked ? data.upgradePrice.ToString() : data.unlockPrice.ToString());

                if (icon)
                    icon.sprite = data.icon;
                if (progress)
                    progress.fillAmount = data.T;
                

                if (UpgradeButton)
                {
                    if (data.isUpgraded)
                        UpgradeButton.interactable = false;
                    else
                        UpgradeButton.onClick.AddListener(OnClickUpgrade);
                }

                if (UnlockButton)
                    UnlockButton.onClick.AddListener(OnClickUnlock);
            }
        }

        private void OnClickUnlock()
        {
            if (!ScoreManager.instance.AddScore(-Mathf.Abs(data.unlockPrice), data.coinID))
                return;

            data.isUnlocked = true;
            unlockingPanel.SetActive(false);
            upgradePanel.SetActive(true);

            if (price_Text)
                price_Text.text = "$ " + (data.isUnlocked ? data.upgradePrice.ToString() : data.unlockPrice.ToString());
        }

        private void OnClickUpgrade()
        {
            if (!ScoreManager.instance.AddScore(-Mathf.Abs(data.upgradePrice), data.coinID))
                return;

            data.T += data.dt;

            if (progress)
                progress.fillAmount = data.T;

            if (data.isUpgraded)
                if (UpgradeButton)
                    UpgradeButton.interactable = false;
        }   
    }
}