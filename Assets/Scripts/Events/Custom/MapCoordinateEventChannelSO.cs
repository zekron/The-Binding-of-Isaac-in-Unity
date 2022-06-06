using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Event/MapCoordinate Event Channel")]
public class MapCoordinateEventChannelSO : EventChannelBaseSO
{
    public UnityAction<GameCoordinate> OnEventRaised;
    public void RaiseEvent(GameCoordinate index)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(index);
    }
}
