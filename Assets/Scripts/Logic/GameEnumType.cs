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

    Cave,
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