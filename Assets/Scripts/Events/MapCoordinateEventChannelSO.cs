using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Event/MapCoordinate Event Channel")]
public class MapCoordinateEventChannelSO : EventChannelBaseSO
{
    public UnityAction<MapCoordinate> OnEventRaised;
    public void RaiseEvent(MapCoordinate index)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(index);
    }
}
