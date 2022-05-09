using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Event/MiniMapCoordinate Event Channel")]
public class MiniMapCoordinateEventChannelSO : EventChannelBaseSO
{
    public UnityAction<MiniMapCoordinate> OnEventRaised;
    public void RaiseEvent(MiniMapCoordinate index)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(index);
    }
}
