using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarViewer : MonoBehaviour
{
    private const int MAX_HEART_ICON = 12;
    private HealthData currentHealthData;
    private GridLayoutGroup layoutGroup;
    private List<HeartIcon> heartList = new List<HeartIcon>(MAX_HEART_ICON);

    private void Awake()
    {
        layoutGroup = GetComponent<GridLayoutGroup>();
        heartList.AddRange(GetComponentsInChildren<HeartIcon>(true));
    }

    public void SetHealthBar(HealthData data)
    {
        currentHealthData = data;

        int pivot = 0;
        var redHeartCount = data.RedHeart;
        if (redHeartCount != 0)
        {
            pivot = redHeartCount / 2 + redHeartCount % 2;
            for (int i = 0; i < pivot && i < MAX_HEART_ICON; i++)
            {
                heartList[i].SetImage(i == pivot - 1 && redHeartCount % 2 == 1 ? HeartSO.HeartTypeInUI.RedHalf : HeartSO.HeartTypeInUI.RedFull);
            }
            if (pivot >= MAX_HEART_ICON) return;

            var containerCount = data.RedHeartContainers - pivot;
            if (containerCount != 0)
            {
                for (int i = pivot; i < containerCount + pivot && i < MAX_HEART_ICON; i++)
                {
                    heartList[i].SetImage(HeartSO.HeartTypeInUI.RedEmpty);
                }
                pivot += containerCount;
                if (pivot >= MAX_HEART_ICON) return;
            }
        }

        var soulHeartCount = data.SoulHeart;
        if (soulHeartCount != 0)
        {
            soulHeartCount = soulHeartCount / 2;
            for (int i = pivot; i < pivot + soulHeartCount && i < MAX_HEART_ICON; i++)
            {
                heartList[i].SetImage(HeartSO.HeartTypeInUI.SoulFull);
            }
            pivot += soulHeartCount;
            if (pivot >= MAX_HEART_ICON) return;

            if (data.SoulHeart % 2 == 1 && pivot < MAX_HEART_ICON)
            {
                heartList[pivot].SetImage(HeartSO.HeartTypeInUI.SoulHalf);
                pivot++;
                if (pivot >= MAX_HEART_ICON) return;
            }
        }

        var whiteHeartCount = data.WhiteHeart;
        if (whiteHeartCount != 0 && pivot < MAX_HEART_ICON)
        {
            heartList[pivot].SetImage(HeartSO.HeartTypeInUI.WhiteHalf);
            pivot++;
            if (pivot >= MAX_HEART_ICON) return;
        }

        for (int i = pivot; i < heartList.Count; i++)
        {
            heartList[i].SetImage(HeartSO.HeartTypeInUI.NULL);
        }
    }

    [ContextMenu("ResetHealthBar")]
    private void ResetHeartBar()
    {
        heartList.Clear();
        heartList.AddRange(GetComponentsInChildren<HeartIcon>(true));
        SetHealthBar(new HealthData(12, 0, 0));
    }

    #region Test
    //[SerializeField] private int testRed = 3;
    //[SerializeField] private int testSoul = 3;
    //[SerializeField] private int testWhite = 1;
    //[ContextMenu("SetHealthBar")]
    //private void Test_SetHealthBar()
    //{
    //    layoutGroup = GetComponent<GridLayoutGroup>();
    //    heartList.Clear();
    //    heartList.AddRange(GetComponentsInChildren<HeartIcon>(true));

    //    SetHealthBar(new HealthData(testRed, testSoul, testWhite));
    //}
    #endregion
}
