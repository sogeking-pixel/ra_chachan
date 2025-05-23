using UnityEngine;
using Vuforia;

public class CustomObserverEventHandler : DefaultObserverEventHandler
{
    public BreathingEffect breathingEffect;

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        if (breathingEffect != null)
            breathingEffect.OnTargetFound();
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        breathingEffect.OnTargetLost();
        
    }
}
