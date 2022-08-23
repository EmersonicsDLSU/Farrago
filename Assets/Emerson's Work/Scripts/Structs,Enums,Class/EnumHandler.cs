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
    LEVEL6,
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
    KEY,
    DOOR
}