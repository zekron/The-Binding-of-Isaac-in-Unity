using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Event/HealthData Event Channel")]
public class HealthDataEventChannelSO : EventChannelBaseSO
{
    public UnityAction<HealthData> OnEventRaised;
    public void RaiseEvent(HealthData healthData)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(healthData);
    }
}
