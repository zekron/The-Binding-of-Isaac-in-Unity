using UnityEngine.Events;

public class EnumEventChannelSO<T> : EventChannelBaseSO where T : System.Enum
{
    public UnityAction<T> OnEventRaised;
    public void RaiseEvent(T index)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(index);
    }
}
