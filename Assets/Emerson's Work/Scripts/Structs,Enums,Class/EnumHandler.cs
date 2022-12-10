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
    color_r6,
    color_r8
};
public enum DescriptiveQuest
{
    R3_COMPLETED_FIRE = 0,
    R3_OBTAINKEY,
    R5_REPAIR_WIRE,
    R5_ON_LIGHT,
    R6_ON_LEFT_LIGHT,
    R6_ON_DESKLIGHT,
    R8_COMPLETED_FIRE,
    R8_REPAIR_WIRE1,
    R8_REPAIR_WIRE2,
    R8_COLOR_PLANT,
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
    LEVEL7,
    LEVEL8,
    LEVEL9,
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
    R5_FLASHLIGHT,
    R5_WIRES,
    R6_VINE,
    R6_LEFT_WIRE,
    R6_RIGHT_WIRE,
    R6_DESK_LAMP,
    R8_WIRES_1,
    R8_WIRES_2,
    R8_BUNSEN_BURNER,
    R8_PLANT,
    R9_CURE_POTION
}

//
public enum E_ClueInteraction
{
    NONE = -1,
    R2_INSTRUCTION1,
    R2_INSTRUCTION2,
    R3_BUNSEN,
    R3_FIRE,
    R5_POTPLANT,
    R5_LIGHTPLANT,
    R6_PLANT,
    R7_NAME,
    R7_GROUP,
}

//
public enum CutSceneTypes
{
    None = 0,
    Level1Intro,
    Level2Intro,
    Level2JournalChecker,
    Level3Intro,
    Level4RatCage,
    Level5PlantGrow,
    Level6Transition,
    Level6Dead,
    Level6Dead2,
    Level7RatSwarm,
    Level7DoorClose,
    Level8ScareRat,
    Level8CheckRat,
    Level8Ending,
    Level9Identity,
    Level9Grow,
    Level9Ending
}
//
public enum RatSpawnerArea
{
    None = 0,
    R3,
    R4_0,
    R4_1,
    R4_Chase,
    R6,
    R7_Swarm,
    R8
}

//General Identification of the enemies type
public enum Enemy_Type
{
    RAT = 0, 
    SPIDER
};

//General Identification of the enemies type
public enum Movement_Angle
{
    DEG_270 = 0,
    DEG_180,
    DEG_90,
    DEG_0

};
