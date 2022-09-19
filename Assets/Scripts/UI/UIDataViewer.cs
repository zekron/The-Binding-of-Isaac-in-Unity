using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using AssetBundleFramework;

public class UIDataViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text bombText;
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private Image goldenKeyImg;

    [Header("Viewers")]
    [SerializeField] private HealthBarViewer healthBarViewer;
    [SerializeField] private ActiveItemViewer activeItemViewer;
    [SerializeField] private MiniMapViewer miniMapViewer;
    [SerializeField] private ArrowViewer arrowViewer;
    [Header("Events")]
    [SerializeField] private HealthDataEventChannelSO onPlayerHealthDataChanged;
    [SerializeField] private PickupDataEventChannelSO onPlayerPickupDataChanged;
    [SerializeField] private BooleanEventChannelSO onPlayerGetGoldenKey;
    [SerializeField] private MapCoordinateEventChannelSO onCreateRoomEvent;
    [SerializeField] private MapCoordinateStatusEventChannelSO onEnterRoomEvent;
    [SerializeField] private ActiveItemEventChannelSO onActiveItemChanged;

    private void OnEnable()
    {
        onActiveItemChanged = AssetBundleManager.Instance.LoadAsset<ActiveItemEventChannelSO>("OnActiveItemChanged Event SO.asset");

        onPlayerHealthDataChanged.OnEventRaised += RefreshHealthData;

        onPlayerPickupDataChanged.OnEventRaised += RefreshPickupData;
        onPlayerGetGoldenKey.OnEventRaised += RefreshGoldenKeyImg;

        onCreateRoomEvent.OnEventRaised += MiniMapCreateRoom;
        onEnterRoomEvent.OnEventRaised += MiniMapEnterRoom;

        onActiveItemChanged.OnEventRaised += ChangeActiveItem;
    }

    private void OnDisable()
    {
        onPlayerHealthDataChanged.OnEventRaised -= RefreshHealthData;

        onPlayerPickupDataChanged.OnEventRaised -= RefreshPickupData;
        onPlayerGetGoldenKey.OnEventRaised -= RefreshGoldenKeyImg;

        onCreateRoomEvent.OnEventRaised -= MiniMapCreateRoom;
        onEnterRoomEvent.OnEventRaised -= MiniMapEnterRoom;

        onActiveItemChanged.OnEventRaised -= ChangeActiveItem;
    }

    private void RefreshPickupData(PickupData data)
    {
        coinText.text = data.coin.ToString("D2");
        bombText.text = data.bomb.ToString("D2");
        keyText.text = data.key.ToString("D2");
    }

    private void RefreshHealthData(HealthData data)
    {
        healthBarViewer.SetHealthBar(data);
    }

    private void RefreshGoldenKeyImg(bool boolean)
    {
        goldenKeyImg.enabled = boolean;
    }

    private void MiniMapCreateRoom(GameCoordinate mapCoordinate)
    {
        miniMapViewer.OnCreateRoom(mapCoordinate);
    }

    private void MiniMapEnterRoom(GameCoordinate direction, MiniMapIconStatus status)
    {
        miniMapViewer.OnMoving(direction, status);
    }

    private void ChangeActiveItem(Sprite sprite,int charged)
    {
        activeItemViewer.ChangeItem(sprite, charged);
    }
}
