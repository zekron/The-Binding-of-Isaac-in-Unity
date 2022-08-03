using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Event/PickupData Event Channel")]
public class PickupDataEventChannelSO : EventChannelBaseSO
{
    public UnityAction<PickupData> OnEventRaised;
    public void RaiseEvent(PickupData pickupData)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(pickupData);
    }
}