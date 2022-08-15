namespace IdleArcade.Core
{
    public class UpgradeableLimiter : Limiter
    {
        protected virtual void Start()
        {
            var data = UpgradeSystem.instance.GetDataField(GetID);
            if (data!= null)
            {
                t = data.T;
                data.OnFieldChanged += OnUpgraded;
            }
        }

        protected virtual void OnDestroy()
        {
            var data = UpgradeSystem.instance.GetDataField(GetID);
            if (data != null)
                data.OnFieldChanged -= OnUpgraded;
        }

        protected virtual void OnUpgraded(float t) => this.t = t;
    }
}
