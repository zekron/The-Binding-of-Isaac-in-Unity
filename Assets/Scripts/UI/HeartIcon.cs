using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartIcon : MonoBehaviour
{
    [SerializeField] private HeartSO heartSO;

    private HeartSO.HeartTypeInUI heartType;
    private Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetImage(HeartSO.HeartTypeInUI type)
    {
        if (heartImage == null) heartImage = GetComponent<Image>();

        if (type == HeartSO.HeartTypeInUI.NULL)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            if (type != heartType)
            {
                heartType = type;
                heartImage.sprite = heartSO.HeartSpritesInUI[(int)type];
            }
        }
    }
}
