
    using System;
    using UnityEngine;

    public abstract class AController
    {
        protected Action<Vector2> OnPositionUpdated;

        protected virtual void Initialize()
        {
            SubscriptionToAction();
            SubscriptionToControls();
            SetValuesControls();
        }

        protected abstract void SubscriptionToAction();
        protected abstract void SubscriptionToControls();
        protected abstract void SetValuesControls();
    }
