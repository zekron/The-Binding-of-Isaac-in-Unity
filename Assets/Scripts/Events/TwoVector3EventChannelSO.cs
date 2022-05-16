using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Event/Two Vector3 Event Channel")]
public class TwoVector3EventChannelSO : EventChannelBaseSO
{
    public UnityAction<Vector3, Vector3> OnEventRaised;
    public void RaiseEvent(Vector3 index0, Vector3 index1)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(index0, index1);
    }
}
