using IdleArcade.Core;
using General.Library;
public class PlayerCoinContainer : TransactionContainer
{
    protected void Start()
    {
        OnChangedValue += OnChange;
        ScoreManager.instance.OnChangedAddListner(OnUpdateCoin);
        ScoreManager.instance.AddScore(0);
    }

    private void OnUpdateCoin(int dt, int newScore)
    {
        m_amount = newScore;
    }

    private void OnChange(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {
        if(delta < 0)
            ScoreManager.instance.AddScore(delta);
    }

}
