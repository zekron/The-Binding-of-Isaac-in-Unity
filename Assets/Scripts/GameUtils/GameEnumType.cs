/// <summary>
/// 当前层类型
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
/// 当前房间类型
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
/// 当前门类型
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
/// 当前门位置
/// </summary>
public enum DoorPosition
{
    Up,
    Down,
    Left,
    Right,
}

/// <summary>
/// 当前门状态
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
    /// 初始化
    /// </summary>
    None,
    /// <summary>
    /// 未经探索的，一般为邻近的房间，或者使用了Card Sun
    /// </summary>
    Unexplored,
    /// <summary>
    /// 当前所在房间
    /// </summary>
    Current,
    /// <summary>
    /// 已探索的
    /// </summary>
    Explored
}

/// <summary>
/// 玩家名字
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
/// 当前游戏状态
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