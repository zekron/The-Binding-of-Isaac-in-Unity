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

    private Dictionary<MapCoordinate, MiniMapIconStatus> drawnList;
    private Texture2D emptyTexture;
    private Image mapImage;
    private int basicIconWidth;
    private int basicIconHeight;

    // Start is called before the first frame update
    void Start()
    {
        mapImage = GetComponent<Image>();

        basicIconWidth = (int)iconExplored.rect.width;
        basicIconHeight = (int)iconExplored.rect.height;

        emptyTexture = new Texture2D(basicIconWidth, basicIconHeight);
        for (int i = 0; i < basicIconWidth; i++)
        {
            for (int j = 0; j < basicIconHeight; j++)
            {
                emptyTexture.SetPixel(i, j, Color.clear);
            }
        }

        drawnList = new Dictionary<MapCoordinate, MiniMapIconStatus>();
        Activate(MapCoordinate.RoomOffsetPoint, MiniMapIconStatus.Unexplored);
        Activate(MapCoordinate.RoomOffsetPoint + MapCoordinate.left, MiniMapIconStatus.Unexplored);
        Activate(MapCoordinate.RoomOffsetPoint + MapCoordinate.right, MiniMapIconStatus.Unexplored);
        Activate(MapCoordinate.RoomOffsetPoint + MapCoordinate.up, MiniMapIconStatus.Unexplored);
        Activate(MapCoordinate.RoomOffsetPoint + MapCoordinate.up * 2, MiniMapIconStatus.Unexplored);
        Activate(MapCoordinate.RoomOffsetPoint + MapCoordinate.down, MiniMapIconStatus.Unexplored);

        DrawTexture();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Activate(MapCoordinate.RoomOffsetPoint, MiniMapIconStatus.Current);
        }
    }


    MapCoordinate currentCellCoordinate;
    private void Activate(MapCoordinate coordinate, MiniMapIconStatus status)
    {
        if (!drawnList.ContainsKey(coordinate))
        {
            drawnList.Add(coordinate, status);
            RefreshMiniMapData(coordinate);
        }
        else
        {
            drawnList[coordinate] = status;
            DrawCell(coordinate);
        }
        if (status == MiniMapIconStatus.Current)
        {
            currentCellCoordinate = coordinate;
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

    private void DrawCell(MapCoordinate mapCoordinate)
    {
        var texture = miniMapSprite.texture;
        SetMiniMapCellPixels(texture, mapCoordinate);

        texture.Apply();
        mapImage.sprite = miniMapSprite = Sprite.Create(texture,
                                                        new Rect(Vector3.zero, new Vector2(texture.width, texture.height)),
                                                        Vector2.zero);

    }
    private void DrawTexture()
    {
        var topRight = MapCoordinate2MiniMapCoordinate(miniMapTopRightPoint + MapCoordinate.one);
        var texture = new Texture2D(topRight.x * basicIconWidth, topRight.y * basicIconHeight);

        for (int y = 0; y < topRight.y; y++)
        {
            for (int x = 0; x < topRight.x; x++)
            {
                SetMiniMapCellPixels(texture, MiniMapCoordinate2MapCoordinate(x, y));
            }

        }
        texture.Apply();

        mapImage.sprite = miniMapSprite = Sprite.Create(texture,
                                                        new Rect(Vector3.zero, new Vector2(texture.width, texture.height)),
                                                        Vector2.zero);
        mapImage.SetNativeSize();
    }

    private void SetMiniMapCellPixels(Texture2D texture, MapCoordinate mapCoordinate)
    {
        MapCoordinate miniMapCoordinate = MapCoordinate2MiniMapCoordinate(mapCoordinate);
        texture.SetPixels(
            (int)iconCurrent.rect.x + miniMapCoordinate.x * basicIconWidth,
            (int)iconCurrent.rect.y + miniMapCoordinate.y * basicIconHeight,
            (int)iconCurrent.rect.width,
            (int)iconCurrent.rect.height,
            GetMiniMapCellPixels(drawnList.ContainsKey(mapCoordinate) ? drawnList[mapCoordinate] : MiniMapIconStatus.None));
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
    private MapCoordinate MiniMapCoordinate2MapCoordinate(MapCoordinate coordinate)
    {
        return new MapCoordinate(coordinate + miniMapOriginalPoint);
    }
    private MapCoordinate MiniMapCoordinate2MapCoordinate(int x, int y)
    {
        return new MapCoordinate(x + miniMapOriginalPoint.x, y + miniMapOriginalPoint.y);
    }
}
