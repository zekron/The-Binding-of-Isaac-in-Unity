using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveItemViewer : MonoBehaviour
{
    private const int MAX_NUMBER_OF_CHARGES = 6;

    [SerializeField] private Image itemImage;
    [SerializeField] private Image skillBarImage;

    public void ChangeItem()
    {
        if (!itemImage.enabled) itemImage.enabled = true;

        //itemImage = newItemData.itemImage;
        //skillBarImage.fillAmount = newItemData.charged / MAX_NUMBER_OF_CHARGES;
    }

    public void ResetViewer()
    {
        itemImage.sprite = null;
        itemImage.enabled = false;
        skillBarImage.fillAmount = 1;
    }
}
