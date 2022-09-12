using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumHandler
{
    
}

//ENUM CHOICE FOR COLOR
public enum ColorCode
{
    RED = 0,
    BLUE,
    YELLOW,
    ORANGE,
    VIOLET,
    WHITE,
    BLACK,
    GREEN
};

public enum QuestDescriptions
{
    NONE = -1,
    tutorial_color_r3,
    color_r5,
    color_r6
};
public enum DescriptiveQuest
{
    R3_COMPLETED_FIRE = 0,
    R3_OBTAINKEY,
    R5_REPAIR_WIRE,
    R5_ON_LIGHT,
    R6_ON_LEFT_LIGHT,
    R6_ON_DESKLIGHT
}

public enum QuestType
{
    NONE = -1,
    ObjectActivation,
    VineInteraction,
    WireRepair,
};

public enum ObjectCode
{
    redLines = 0,
    blueLines,
    yellowLines,
    orangeLines,
    violetLines,
    whiteLines,
    blackLines,
    blocker,
    JOURNAL,
    NOTE
};

public enum RespawnPoints
{
    NONE = -1,
    LEVEL1,
    LEVEL2,
    LEVEL3,
    LEVEL4,
    LEVEL4_CHASE,
    LEVEL5,
    LEVEL6
};

//General Identification of the pool type
public enum Pool_Type
{
    NONE = -1, 
    ENEMY, 
    COLOR,
    OBJECTIVE
};

//
public enum PuzzleItem
{
    NONE = -1,
    R2_JOURNAL,
    R3_KEY,
    R3_DOOR,
    R3_BUNSEN_BURNER,
    R5_WIRES,
    R6_VINE,
    R6_LEFT_WIRE,
    R6_RIGHT_WIRE,
    R6_DESK_LAMP
}

//
public enum E_ClueInteraction
{
    NONE = -1,
    R2_BUNSEN,
    R2_FIRE,
    R5_POTPLANT,
    R5_LIGHTPLANT
}

//
public enum CutSceneTypes
{
    None = 0,
    Level1Intro,
    Level2Intro,
    Level2JournalChecker,
    Level3Intro,
    Level3End,
    Level4Intro,
    Level4RatCage,
    MeltIceScene,
    Level5PlantGrow,
    Level6Transition,
    Level6Dead
}
