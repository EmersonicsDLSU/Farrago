using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// A class the represents the color of the mainPlayer
public class ColorMixer
{
    // Fields List
    public Color color;
    public ColorCode color_code;
    // Methods List
    // public constructor
    public ColorMixer(Color color, ColorCode color_code)
    {
        // initializes fields
        this.color = color;
        this.color_code = color_code;
    }

    // operator overload '+', e.g. ColorMixer1 + ColorMixer2 = ColorMixer3;
    public static ColorMixer operator +(ColorMixer color1, ColorMixer color2)
    {
        // If no combination is found, we just return the latest absorbed color
        ColorMixer color3 = color2;
        // Pre-defined Color pair and their resulting color 
        var colorPairs = new List<List<ColorCode>>()  
        {   
            // 1st and 2nd value are the color combinations. 3rd parameter is the resulting color   
            new List<ColorCode>() {ColorCode.RED, ColorCode.YELLOW, ColorCode.ORANGE},  
            new List<ColorCode>() {ColorCode.RED, ColorCode.BLUE, ColorCode.VIOLET},
            new List<ColorCode>() {ColorCode.BLUE, ColorCode.YELLOW, ColorCode.GREEN}
        };   
        // get all the related color combination in the 1st column
        var relatedColorList1 = colorPairs.FindAll(x => 
            (x[0] == color1.color_code || x[0] == color2.color_code));
        // get all the related color combination in the 2nd column
        var relatedColorList2 = relatedColorList1.FindAll(x => 
            (x[1] == color1.color_code || x[1] == color2.color_code));
        // allocate memory and assigns values to the new ColorMixer obj
        if (relatedColorList2.Count != 0)
            color3 = new ColorMixer(PotionAbsorption.color_Code_To_UColor[relatedColorList2[0][2]],
                relatedColorList2[0][2]);
        return color3; // returns the new ColorMixer obj
    }
}
