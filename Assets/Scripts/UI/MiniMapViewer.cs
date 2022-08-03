using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// View: Mini Map
/// </summary>
[RequireComponent(typeof(Image))]
public class MiniMapViewer : MonoBehaviour
{
    [SerializeField] private Sprite iconExplored;
    [SerializeField] private Sprite iconCurrent;
    [SerializeField] private Sprite iconUnexplored;

    private Dictionary<GameCoordinate, MiniMapIconStatus> coordinateDict;
    private GameCoordinate miniMapOriginalPoint = new GameCoordinate(int.MaxValue, int.MaxValue);
    private GameCoordinate miniMapTopRightPoint = new GameCoordinate(0, 0);
    private GameCoordinate currentCellCoordinate;
    private Texture2D emptyTexture;
    private Image mapImage;
    private int basicIconWidth;
    private int basicIconHeight;

    private void Awake()
    {
        mapImage = GetComponent<Image>();

        basicIconWidth = (int)iconExplored.rect.width;
        basicIconHeight = (int)iconExplored.rect.height;

        coordinateDict = new Dictionary<GameCoordinate, MiniMapIconStatus>();

        currentCellCoordinate = GameCoordinate.RoomOffsetPoint;
    }

    // Start is called before the first frame update
    void Start()
    {
        emptyTexture = GameLogicUtility.GetEmptyTexture(basicIconWidth, basicIconHeight);

        mapImage.sprite = miniMapSprite = Sprite.Create(emptyTexture,
                                                        new Rect(Vector2.zero,
                                                                 new Vector2(emptyTexture.width, emptyTexture.height)),
                                                        Vector2.zero);
    }

    #region Test
    // Update is called once per frame
    //void Update()
    //{
        //if (Input.GetKeyDown(KeyCode.Keypad8))
        //{
        //    OnMoving(GameCoordinate.zero, MiniMapIconStatus.Unexplored);

        //    OnMoving(GameCoordinate.up, MiniMapIconStatus.Current);
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad2))
        //{
        //    OnMoving(GameCoordinate.zero, MiniMapIconStatus.Unexplored);

        //    OnMoving(GameCoordinate.down, MiniMapIconStatus.Current);
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad4))
        //{
        //    OnMoving(GameCoordinate.zero, MiniMapIconStatus.Unexplored);

        //    OnMoving(GameCoordinate.left, MiniMapIconStatus.Current);
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad6))
        //{
        //    OnMoving(GameCoordinate.zero, MiniMapIconStatus.Unexplored);

        //    OnMoving(GameCoordinate.right, MiniMapIconStatus.Current);
        //} 
    //}
    #endregion

    public void OnCreateRoom(GameCoordinate mapCoordinate)
    {
        Activate(mapCoordinate, MiniMapIconStatus.None);
    }

    public void OnMoving(GameCoordinate direction, MiniMapIconStatus status)
    {
        GameCoordinate coordinate = currentCellCoordinate + direction;
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

    /// <summary>
    /// 更新 MiniMap 字典信息
    /// </summary>
    /// <param name="coordinate"></param>
    /// <param name="status"></param>
    private void Activate(GameCoordinate coordinate, MiniMapIconStatus status)
    {
        if (!coordinateDict.ContainsKey(coordinate))
        {
            coordinateDict.Add(coordinate, status);
        }
        else
        {
            coordinateDict[coordinate] = status;
        }
    }

    private void RefreshMiniMapData(GameCoordinate coordinate)
    {
        miniMapOriginalPoint.x = Mathf.Min(coordinate.x, miniMapOriginalPoint.x);
        miniMapOriginalPoint.y = Mathf.Min(coordinate.y, miniMapOriginalPoint.y);

        miniMapTopRightPoint.x = Mathf.Max(coordinate.x, miniMapTopRightPoint.x);
        miniMapTopRightPoint.y = Mathf.Max(coordinate.y, miniMapTopRightPoint.y);
    }

    Sprite miniMapSprite;

    private void DrawCurrentCell(GameCoordinate mapCoordinate)
    {
        var direction = mapCoordinate - currentCellCoordinate;  //新坐标移动方向

        var toBeDrawnList = new List<GameCoordinate>(5);
        toBeDrawnList.Add(mapCoordinate);

        for (int i = 0; i < GameCoordinate.directionArray.Length; i++)
        {
            GameCoordinate neighbour = mapCoordinate + GameCoordinate.GetMoveDirectionPoint(GameCoordinate.directionArray[i]);
            if (coordinateDict.ContainsKey(neighbour))
            {
                if (coordinateDict[neighbour] != MiniMapIconStatus.Explored
                    && !neighbour.Equals(currentCellCoordinate))
                {
                    Activate(neighbour, MiniMapIconStatus.Unexplored);//TODO: Test
                    toBeDrawnList.Add(neighbour);
                }
                switch (GameCoordinate.directionArray[i])
                {
                    //如果该方向上有房间而且旧小地图处于边缘，则需要往该方向平移平移
                    case GameCoordinate.MoveDirection.Down when currentCellCoordinate.y == miniMapOriginalPoint.y:
                    case GameCoordinate.MoveDirection.Left when currentCellCoordinate.x == miniMapOriginalPoint.x:
                        direction += GameCoordinate.GetMoveDirectionPoint(GameCoordinate.directionArray[i]);
                        break;
                }
            }
        }

        var texture = ResizeTexture(mapCoordinate);
        var oldTexture = miniMapSprite.texture;
        //如果方向为往下或者往左但是这个方向上没有房间，旧小地图则不用往这个方向（的反方向）平移
        if (!toBeDrawnList.Contains(mapCoordinate + GameCoordinate.down) && direction.y == -1) direction.y = 0;
        if (!toBeDrawnList.Contains(mapCoordinate + GameCoordinate.left) && direction.x == -1) direction.x = 0;
        //ShiftTexture
        if (!currentCellCoordinate.Equals(GameCoordinate.zero))
            if (oldTexture.width != texture.width || oldTexture.height != texture.height)
                ShiftTexture(oldTexture, texture, direction);
            else
                texture.SetPixels(oldTexture.GetPixels());

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

    private void DrawCell(GameCoordinate mapCoordinate)
    {
        var texture = miniMapSprite.texture;
        SetMiniMapCellPixels(texture, MapCoordinate2MiniMapCoordinate(mapCoordinate));

        texture.Apply();
        mapImage.sprite = miniMapSprite = Sprite.Create(texture,
                                                        new Rect(Vector3.zero, new Vector2(texture.width, texture.height)),
                                                        Vector2.zero);
        mapImage.SetNativeSize();
    }

    private Texture2D ResizeTexture(GameCoordinate coordinate)
    {
        if (coordinateDict.ContainsKey(coordinate + GameCoordinate.up)) RefreshMiniMapData(coordinate + GameCoordinate.up);
        if (coordinateDict.ContainsKey(coordinate + GameCoordinate.down)) RefreshMiniMapData(coordinate + GameCoordinate.down);
        if (coordinateDict.ContainsKey(coordinate + GameCoordinate.left)) RefreshMiniMapData(coordinate + GameCoordinate.left);
        if (coordinateDict.ContainsKey(coordinate + GameCoordinate.right)) RefreshMiniMapData(coordinate + GameCoordinate.right);
        if (coordinateDict.ContainsKey(coordinate)) RefreshMiniMapData(coordinate);

        var topRight = MapCoordinate2MiniMapCoordinate(miniMapTopRightPoint + GameCoordinate.one);

        return GameLogicUtility.GetEmptyTexture(topRight.x * basicIconWidth, topRight.y * basicIconHeight);
    }

    private void ShiftTexture(Texture2D textureNeed2Shift, Texture2D baseTexture, GameCoordinate direction)
    {
        baseTexture.SetPixels(direction.x > 0 ? 0 : direction.x * -basicIconWidth,
                              direction.y > 0 ? 0 : direction.y * -basicIconHeight,
                              textureNeed2Shift.width,
                              textureNeed2Shift.height,
                              textureNeed2Shift.GetPixels());
        baseTexture.Apply();
    }

    /// <summary>
    /// 用指定网格 <paramref name="miniMapCoordinate"/> 填充贴图到 <paramref name="texture"/>
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="miniMapCoordinate"></param>
    private void SetMiniMapCellPixels(Texture2D texture, GameCoordinate miniMapCoordinate)
    {
        GameCoordinate mapCoordinate = MiniMapCoordinate2MapCoordinate(miniMapCoordinate);
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
        GameCoordinate mapCoordinate = MiniMapCoordinate2MapCoordinate(x, y);
        texture.SetPixels(
            x * basicIconWidth,
            y * basicIconHeight,
            (int)iconCurrent.rect.width,
            (int)iconCurrent.rect.height,
            GetMiniMapCellPixels(coordinateDict.ContainsKey(mapCoordinate) ? coordinateDict[mapCoordinate] : MiniMapIconStatus.None));
    }

    /// <summary>
    /// Test
    /// </summary>
    private void DrawTexture()
    {
        var topRight = MapCoordinate2MiniMapCoordinate(miniMapTopRightPoint + GameCoordinate.one);
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

    private GameCoordinate MapCoordinate2MiniMapCoordinate(GameCoordinate coordinate)
    {
        return new GameCoordinate(coordinate - miniMapOriginalPoint);
    }
    private GameCoordinate MapCoordinate2MiniMapCoordinate(int x, int y)
    {
        return new GameCoordinate(x - miniMapOriginalPoint.x, y - miniMapOriginalPoint.y);
    }
    private GameCoordinate MiniMapCoordinate2MapCoordinate(GameCoordinate coordinate)
    {
        return new GameCoordinate(coordinate + miniMapOriginalPoint);
    }
    private GameCoordinate MiniMapCoordinate2MapCoordinate(int x, int y)
    {
        return new GameCoordinate(x + miniMapOriginalPoint.x, y + miniMapOriginalPoint.y);
    }
}
