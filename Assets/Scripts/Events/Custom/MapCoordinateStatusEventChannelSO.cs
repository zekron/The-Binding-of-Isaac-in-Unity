using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Event/MapCoordinateStatus Event Channel")]
public class MapCoordinateStatusEventChannelSO : EventChannelBaseSO
{
    public UnityAction<GameCoordinate, MiniMapIconStatus> OnEventRaised;
    public void RaiseEvent(GameCoordinate mapCoordinate, MiniMapIconStatus status)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(mapCoordinate, status);
    }
}
