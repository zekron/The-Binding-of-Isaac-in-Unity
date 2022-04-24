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
    Devil,
    Angel,

    Treature,
    Shop,
    Library,

    Arcade,

    Challenge,
    BossChallenge,

    Curse,

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

    Cave,
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