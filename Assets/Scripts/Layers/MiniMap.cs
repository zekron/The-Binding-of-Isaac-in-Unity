using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// View: Mini Map
/// </summary>
[RequireComponent(typeof(Image))]
public class MiniMap : MonoBehaviour
{
    [SerializeField] private Sprite iconExplored;
    [SerializeField] private Sprite iconCurrent;
    [SerializeField] private Sprite iconUnexplored;
    [SerializeField] private MiniMapCoordinateEventChannelSO onMiniMapCoordinateChangedEvent;
    [SerializeField] private MapCoordinateEventChannelSO onCreateRoomEvent;
    [SerializeField] private MapCoordinateStatusEventChannelSO onEnterRoomEvent;

    private Dictionary<MapCoordinate, MiniMapIconStatus> coordinateDict;
    private List<MiniMapCoordinate> coordinateList;
    private MapCoordinate currentCellCoordinate;
    private Texture2D emptyTexture;
    private Image mapImage;
    private int basicIconWidth;
    private int basicIconHeight;

    private void OnEnable()
    {
        onCreateRoomEvent.OnEventRaised += OnCreateRoom;
        onEnterRoomEvent.OnEventRaised += OnMoving;
    }
    private void OnDisable()
    {
        onCreateRoomEvent.OnEventRaised -= OnCreateRoom;
        onEnterRoomEvent.OnEventRaised -= OnMoving;
    }
    private void Awake()
    {
        mapImage = GetComponent<Image>();

        basicIconWidth = (int)iconExplored.rect.width;
        basicIconHeight = (int)iconExplored.rect.height;

        coordinateDict = new Dictionary<MapCoordinate, MiniMapIconStatus>();
        coordinateList = new List<MiniMapCoordinate>();

        currentCellCoordinate = MapCoordinate.RoomOffsetPoint;
    }

    // Start is called before the first frame update
    void Start()
    {
        emptyTexture = GameLogicUtility.GetEmptyTexture(basicIconWidth, basicIconHeight);

        mapImage.sprite = miniMapSprite = Sprite.Create(emptyTexture,
                                                        new Rect(Vector2.zero,
                                                                 new Vector2(emptyTexture.width, emptyTexture.height)),
                                                        Vector2.zero);

        #region Test
        OnCreateRoom(MapCoordinate.RoomOffsetPoint);
        OnCreateRoom(MapCoordinate.RoomOffsetPoint + MapCoordinate.left);
        OnCreateRoom(MapCoordinate.RoomOffsetPoint + MapCoordinate.right);
        OnCreateRoom(MapCoordinate.RoomOffsetPoint + MapCoordinate.up);
        OnCreateRoom(MapCoordinate.RoomOffsetPoint + MapCoordinate.up * 2);
        OnCreateRoom(MapCoordinate.RoomOffsetPoint + MapCoordinate.down);
        #endregion

        //DrawTexture();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            onEnterRoomEvent.RaiseEvent(MapCoordinate.zero,
                                        MiniMapIconStatus.Current);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            onEnterRoomEvent.RaiseEvent(MapCoordinate.zero,
                                        MiniMapIconStatus.Unexplored);

            onEnterRoomEvent.RaiseEvent(MapCoordinate.up,
                                        MiniMapIconStatus.Current);
        }
    }

    /// <summary>
    /// 更新 MiniMap 字典信息
    /// </summary>
    /// <param name="coordinate"></param>
    /// <param name="status"></param>
    private void Activate(MapCoordinate coordinate, MiniMapIconStatus status)
    {
        if (!coordinateDict.ContainsKey(coordinate))
        {
            coordinateDict.Add(coordinate, status);
        }
        else
        {
            coordinateDict[coordinate] = status;
        }
        //if (status != MiniMapIconStatus.None) RefreshMiniMapData(coordinate);

        //if (coordinateDict[coordinate] != status)
        //{
        //    DrawCell(coordinate);
        //}
        //if (status == MiniMapIconStatus.Current)
        //{
        //    DrawCurrentCell(currentCellCoordinate = coordinate);
        //    currentCellCoordinate = coordinate;
        //}
    }

    private void OnCreateRoom(MapCoordinate mapCoordinate)
    {
        Activate(mapCoordinate, MiniMapIconStatus.None);
    }

    private void OnMoving(MapCoordinate direction, MiniMapIconStatus status)
    {
        MapCoordinate coordinate = currentCellCoordinate + direction;
        Activate(coordinate, status);

        switch (status)
        {
            case MiniMapIconStatus.Unexplored:
            case MiniMapIconStatus.Explored:
                DrawCell(coordinate);
                break;
            case MiniMapIconStatus.Current:
                DrawCurrentCell(coordinate);
                currentCellCoordinate = coordinate;
                break;
            default:
                CustomDebugger.ThrowException(string.Format("Fatal Variable: {0}", status));
                break;
        }
    }

    private MapCoordinate miniMapOriginalPoint = new MapCoordinate(int.MaxValue, int.MaxValue);
    private MapCoordinate miniMapTopRightPoint = new MapCoordinate(0, 0);
    private void RefreshMiniMapData(MapCoordinate coordinate)
    {
        miniMapOriginalPoint.x = Mathf.Min(coordinate.x, miniMapOriginalPoint.x);
        miniMapOriginalPoint.y = Mathf.Min(coordinate.y, miniMapOriginalPoint.y);

        miniMapTopRightPoint.x = Mathf.Max(coordinate.x, miniMapTopRightPoint.x);
        miniMapTopRightPoint.y = Mathf.Max(coordinate.y, miniMapTopRightPoint.y);
    }

    Sprite miniMapSprite;

    private void DrawCurrentCell(MapCoordinate mapCoordinate)
    {
        var toBeDrawnList = new List<MapCoordinate>(5);
        toBeDrawnList.Add(mapCoordinate);

        for (int i = 0; i < MapCoordinate.directionArray.Length; i++)
        {
            MapCoordinate neighbour = mapCoordinate + MapCoordinate.GetMoveDirectionPoint(MapCoordinate.directionArray[i]);
            if (coordinateDict.ContainsKey(neighbour))
            {
                if (coordinateDict[neighbour] != MiniMapIconStatus.Explored)
                {
                    Activate(neighbour, MiniMapIconStatus.Unexplored);//TODO: Test
                    toBeDrawnList.Add(neighbour);
                }
            }
        }

        //ShiftTexture
        var oldTexture = miniMapSprite.texture;
        var texture = ResizeTexture(mapCoordinate);
        var direction = mapCoordinate - currentCellCoordinate;
        if (direction.y != 0)
        {
            for (int rowCount = 0; rowCount < Mathf.Abs(direction.y); rowCount++)
            {
                SetMiniMapRowPixels(texture, currentCellCoordinate + (direction.y > 0 ? MapCoordinate.up : MapCoordinate.down));
            }
        }

        for (int i = 0; i < toBeDrawnList.Count; i++)
        {
            SetMiniMapCellPixels(texture, MapCoordinate2MiniMapCoordinate(toBeDrawnList[i]));
        }

        texture.Apply();
        mapImage.sprite = miniMapSprite = Sprite.Create(texture,
                                                        new Rect(Vector3.zero, new Vector2(texture.width, texture.height)),
                                                        Vector2.zero);
        mapImage.SetNativeSize();
    }

    private void DrawCell(MapCoordinate mapCoordinate)
    {
        var texture = miniMapSprite.texture;
        SetMiniMapCellPixels(texture, MapCoordinate2MiniMapCoordinate(mapCoordinate));

        texture.Apply();
        mapImage.sprite = miniMapSprite = Sprite.Create(texture,
                                                        new Rect(Vector3.zero, new Vector2(texture.width, texture.height)),
                                                        Vector2.zero);
        mapImage.SetNativeSize();
    }

    private Texture2D ResizeTexture(MapCoordinate coordinate)
    {
        RefreshMiniMapData(coordinate);
        var topRight = MapCoordinate2MiniMapCoordinate(miniMapTopRightPoint + MapCoordinate.one);

        return GameLogicUtility.GetEmptyTexture(topRight.x * basicIconWidth, topRight.y * basicIconHeight);
    }

    private void DrawTexture()
    {
        var topRight = MapCoordinate2MiniMapCoordinate(miniMapTopRightPoint + MapCoordinate.one);
        var texture = new Texture2D(topRight.x * basicIconWidth, topRight.y * basicIconHeight);

        for (int y = 0; y < topRight.y; y++)
        {
            for (int x = 0; x < topRight.x; x++)
            {
                SetMiniMapCellPixels(texture, x, y);
            }

        }
        texture.Apply();

        mapImage.sprite = miniMapSprite = Sprite.Create(texture,
                                                        new Rect(Vector3.zero, new Vector2(texture.width, texture.height)),
                                                        Vector2.zero);
        mapImage.SetNativeSize();
    }

    private void SetMiniMapRowPixels(Texture2D texture, MapCoordinate mapCoordinate)
    {
        MapCoordinate miniMapCoordinate = MapCoordinate2MiniMapCoordinate(mapCoordinate);
        for (int i = 0; i < miniMapTopRightPoint.x; i++)
        {
            SetMiniMapCellPixels(texture, i, miniMapCoordinate.y);
        }
    }

    /// <summary>
    /// 用指定网格 <paramref name="miniMapCoordinate"/> 填充贴图到 <paramref name="texture"/>
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="miniMapCoordinate"></param>
    private void SetMiniMapCellPixels(Texture2D texture, MapCoordinate miniMapCoordinate)
    {
        MapCoordinate mapCoordinate = MiniMapCoordinate2MapCoordinate(miniMapCoordinate);
        texture.SetPixels(
            miniMapCoordinate.x * basicIconWidth,
            miniMapCoordinate.y * basicIconHeight,
            (int)iconCurrent.rect.width,
            (int)iconCurrent.rect.height,
            GetMiniMapCellPixels(coordinateDict.ContainsKey(mapCoordinate) ? coordinateDict[mapCoordinate] : MiniMapIconStatus.None));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="x">MiniMapCoordinate.x</param>
    /// <param name="y">MiniMapCoordinate.y</param>
    private void SetMiniMapCellPixels(Texture2D texture, int x, int y)
    {
        MapCoordinate mapCoordinate = MiniMapCoordinate2MapCoordinate(x, y);
        texture.SetPixels(
            x * basicIconWidth,
            y * basicIconHeight,
            (int)iconCurrent.rect.width,
            (int)iconCurrent.rect.height,
            GetMiniMapCellPixels(coordinateDict.ContainsKey(mapCoordinate) ? coordinateDict[mapCoordinate] : MiniMapIconStatus.None));
    }

    private Color[] GetMiniMapCellPixels(MiniMapIconStatus status)
    {
        switch (status)
        {
            case MiniMapIconStatus.None:
                return emptyTexture.GetPixels();
            case MiniMapIconStatus.Unexplored:
                return iconUnexplored.texture.GetPixels();
            case MiniMapIconStatus.Current:
                return iconCurrent.texture.GetPixels();
            case MiniMapIconStatus.Explored:
                return iconExplored.texture.GetPixels();
            default:
                return emptyTexture.GetPixels();
        }
    }

    private MapCoordinate MapCoordinate2MiniMapCoordinate(MapCoordinate coordinate)
    {
        return new MapCoordinate(coordinate - miniMapOriginalPoint);
    }
    private MapCoordinate MapCoordinate2MiniMapCoordinate(int x, int y)
    {
        return new MapCoordinate(x - miniMapOriginalPoint.x, y - miniMapOriginalPoint.y);
    }
    private MapCoordinate MiniMapCoordinate2MapCoordinate(MapCoordinate coordinate)
    {
        return new MapCoordinate(coordinate + miniMapOriginalPoint);
    }
    private MapCoordinate MiniMapCoordinate2MapCoordinate(int x, int y)
    {
        return new MapCoordinate(x + miniMapOriginalPoint.x, y + miniMapOriginalPoint.y);
    }
}
