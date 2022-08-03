using UnityEngine;

public interface ICollectInSlot
{
    public string ObjectTitle { get; }
    public string ObjectMessage { get; }
    public Sprite SpriteInSlot { get; }

    /// <summary>
    /// �ռ���������
    /// </summary>
    public void CollectInSlot();
    /// <summary>
    /// ʹ�õ������ڵ���
    /// </summary>
    public void Activate();
}