using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveItemViewer : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Image skillBarImage;

    private void Awake()
    {
        ResetViewer();
    }

    public void ChangeItem(Sprite sprite, int charged)
    {
        if (!itemImage.enabled) itemImage.enabled = true;

        itemImage.sprite = sprite;
        skillBarImage.fillAmount = (float)charged / StaticData.MAX_NUMBER_OF_CHARGES;
    }

    public void ResetViewer()
    {
        itemImage.sprite = null;
        itemImage.enabled = false;
        skillBarImage.fillAmount = 1;
    }
}
