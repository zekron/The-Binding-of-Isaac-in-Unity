using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Event/MapCoordinateStatus Event Channel")]
public class MapCoordinateStatusEventChannelSO : EventChannelBaseSO
{
    public UnityAction<MapCoordinate, MiniMapIconStatus> OnEventRaised;
    public void RaiseEvent(MapCoordinate mapCoordinate, MiniMapIconStatus status)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(mapCoordinate, status);
    }
}
