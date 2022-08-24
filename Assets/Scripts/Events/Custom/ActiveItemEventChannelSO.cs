using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Event/ActiveItem Event Channel")]
public class ActiveItemEventChannelSO : EventChannelBaseSO
{
    public UnityAction<Sprite, int> OnEventRaised;
    public void RaiseEvent(Sprite value1, int value2)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value1, value2);
    }
}
