using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MiniMapViewer : MonoBehaviour
{
    [SerializeField] private Sprite iconExplored;
    [SerializeField] private Sprite iconCurrent;
    [SerializeField] private Sprite iconUnexplored;

    private Texture2D emptyTexture;
    private Sprite miniMapSprite;
    private Image mapImage;
    private int basicIconWidth;
    private int basicIconHeight;
    // Start is called before the first frame update
    void Start()
    {
        mapImage = GetComponent<Image>();

        basicIconWidth = (int)iconExplored.rect.width;
        basicIconHeight = (int)iconExplored.rect.height;

        emptyTexture = GameLogicUtility.GetEmptyTexture(basicIconWidth, basicIconHeight);

        mapImage.sprite = miniMapSprite = Sprite.Create(emptyTexture,
                                                        new Rect(Vector2.zero,
                                                                 new Vector2(emptyTexture.width, emptyTexture.height)),
                                                        Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Utilities
    private MapCoordinate miniMapOriginalPoint = new MapCoordinate(int.MaxValue, int.MaxValue);
    private MapCoordinate miniMapTopRightPoint = new MapCoordinate(0, 0);
    private void RefreshMiniMapData(MapCoordinate coordinate)
    {
        miniMapOriginalPoint.x = Mathf.Min(coordinate.x, miniMapOriginalPoint.x);
        miniMapOriginalPoint.y = Mathf.Min(coordinate.y, miniMapOriginalPoint.y);

        miniMapTopRightPoint.x = Mathf.Max(coordinate.x, miniMapTopRightPoint.x);
        miniMapTopRightPoint.y = Mathf.Max(coordinate.y, miniMapTopRightPoint.y);
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
    #endregion
}
