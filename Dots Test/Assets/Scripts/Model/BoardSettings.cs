using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardSettings {
    
    //This can change depending on the board
    public static int Rows = 6;
    public static int Columns = 6;

    //Where to store the dots when not in use
    public const float DOT_STORAGE_X = 10000;
    public const float DOT_STORAGE_Y = 10000;

    //This shouldn't be tied to the prefab since some dot images may have different sizes (for different type) but want spacing the same
    public const float DOT_DIAMETER = 30;
    
}
