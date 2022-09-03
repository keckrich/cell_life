using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour
{

    public Settings settingsValues;
    
    void Awake()
    {
        settingsValues.forces = new float[4][]{
            new float[4]{settingsValues.yellow_yellow, settingsValues.yellow_red, settingsValues.yellow_green, settingsValues.yellow_blue},
            new float[4]{settingsValues.red_yellow, settingsValues.red_red, settingsValues.red_green, settingsValues.red_blue},
            new float[4]{settingsValues.green_yellow, settingsValues.green_red, settingsValues.green_green, settingsValues.green_blue},
            new float[4]{settingsValues.blue_yellow, settingsValues.blue_red, settingsValues.blue_green, settingsValues.blue_blue}
        };

        settingsValues.ranges = new float[4][]{
            new float[4]{settingsValues.yellow_yellow_range, settingsValues.yellow_red_range, settingsValues.yellow_green_range, settingsValues.yellow_blue_range},
            new float[4]{settingsValues.red_yellow_range, settingsValues.red_red_range, settingsValues.red_green_range, settingsValues.red_blue_range},
            new float[4]{settingsValues.green_yellow_range, settingsValues.green_red_range, settingsValues.green_green_range, settingsValues.green_blue_range},
            new float[4]{settingsValues.blue_yellow_range, settingsValues.blue_red_range, settingsValues.blue_green_range, settingsValues.blue_blue_range}
        };
    }


}
