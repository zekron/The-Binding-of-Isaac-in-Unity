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
    [SerializeField] private Sprite iconArrived;
    [SerializeField] private Sprite iconCurrent;
    [SerializeField] private Sprite iconUnknown;

    private Dictionary<MapCoordinate, MiniMapIconStatus> drawnList;
    private Texture2D emptyTexture;
    private Image mapImage;
    private int basicIconWidth;
    private int basicIconHeight;
    private int mapTextureHeight;
    private int mapTextureWidth;

    // Start is called before the first frame update
    void Start()
    {
        mapImage = GetComponent<Image>();

        basicIconWidth = (int)iconArrived.rect.width;
        basicIconHeight = (int)iconArrived.rect.height;

        emptyTexture = new Texture2D(basicIconWidth, basicIconHeight);
        for (int i = 0; i < basicIconWidth; i++)
        {
            for (int j = 0; j < basicIconHeight; j++)
            {
                emptyTexture.SetPixel(i, j, Color.clear);
            }
        }

        drawnList = new Dictionary<MapCoordinate, MiniMapIconStatus>();
        Activate(MapCoordinate.RoomOffsetPoint, MiniMapIconStatus.Current);
        Activate(MapCoordinate.RoomOffsetPoint + MapCoordinate.left, MiniMapIconStatus.Unknown);
        Activate(MapCoordinate.RoomOffsetPoint + MapCoordinate.right, MiniMapIconStatus.Unknown);
        Activate(MapCoordinate.RoomOffsetPoint + MapCoordinate.up, MiniMapIconStatus.Unknown);
        Activate(MapCoordinate.RoomOffsetPoint + MapCoordinate.up * 2, MiniMapIconStatus.Unknown);
        Activate(MapCoordinate.RoomOffsetPoint + MapCoordinate.down, MiniMapIconStatus.Unknown);
        mapTextureWidth = 3 * basicIconWidth;
        mapTextureHeight = 4 * basicIconHeight;

        DrawTexture();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Activate(MapCoordinate coordinate, MiniMapIconStatus status)
    {
        if (!drawnList.ContainsKey(coordinate))
        {
            drawnList.Add(coordinate, status);
        }
        else
        {
            drawnList[coordinate] = status;
        }
    }

    private void DrawTexture()
    {
        var result = new Texture2D(mapTextureWidth, mapTextureHeight);
        for (int i = 0; i < 3; i++)
        {
            result.SetPixels((int)iconCurrent.rect.x + i * basicIconWidth,
                             (int)iconCurrent.rect.y,
                             (int)iconCurrent.rect.width,
                             (int)iconCurrent.rect.height,
                             emptyTexture.GetPixels());
        }
        result.Apply();

        Sprite miniMap = Sprite.Create(result, new Rect(Vector3.zero, new Vector2(result.width, result.height)), Vector2.zero);

        mapImage.sprite = miniMap;
        mapImage.SetNativeSize();
    }

    private int dictionaryComparer()
    {
        foreach (var item in drawnList)
        {

        }
        return 1;
    }
}
