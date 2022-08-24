/// <summary>
/// ��ǰ������
/// </summary>
public enum ChapterType
{
    Basement,
    Cellar,

    Caves,
    Catacombs,

    Depths,
    Necropolis,

    Womb,
    Utero,

    Cathedral,
    Sheol
}

public enum FloorCurseType
{
    None,

    CurseofDarkness,
    CurseoftheLost,
    CurseoftheLabyrinth,
}

/// <summary>
/// ��ǰ��������
/// </summary>
public enum RoomType
{
    Starting,

    Normal,

    Boss,
    MiniBoss,
    Devil,
    Angel,

    Treasure,
    Shop,
    Library,

    Arcade,

    Challenge,
    BossChallenge,

    Curse,
    Sacrifice,

    Secret,
    SuperSecret,

    Error,

}

/// <summary>
/// ��ǰ������
/// </summary>
public enum DoorType
{
    Normal,

    Boss,
    Devil,
    Angel,

    Shop,
    Treasure,
    Book,

    Arcade,

    Challenge,
    BossChallenge,

    Curse,

    Secret,
}

/// <summary>
/// ��ǰ��λ��
/// </summary>
public enum DoorPosition
{
    Up,
    Down,
    Left,
    Right,
}

/// <summary>
/// ��ǰ��״̬
/// </summary>
public enum DoorStatus
{
    Closed,
    Open,
    Broken,
}


public enum MiniMapIconStatus
{
    /// <summary>
    /// ��ʼ��
    /// </summary>
    None,
    /// <summary>
    /// δ��̽���ģ�һ��Ϊ�ڽ��ķ��䣬����ʹ����Card Sun
    /// </summary>
    Unexplored,
    /// <summary>
    /// ��ǰ���ڷ���
    /// </summary>
    Current,
    /// <summary>
    /// ��̽����
    /// </summary>
    Explored
}

/// <summary>
/// �������
/// </summary>
public enum PlayerCharacter
{
    Isaac = 1,
    Magdalene,
    Cain,
    Judas,
    Blue,
    Eve,
    Samson,

    MAXCOUNT
}

/// <summary>
/// ��ǰ��Ϸ״̬
/// </summary>
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver,
}

public enum CollectibleItemName
{
    The_Sad_Onion = 1,
    The_Poop = 41,
    Yum_Heart = 51,
}