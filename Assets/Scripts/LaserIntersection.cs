using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class LaserIntersection
{
    public static LaserJunction CyanJunction;
    public static LaserJunction MagentaJunction;
    public static LaserJunction YellowJunction;

    public static int TotalLaserCollisions = 0;

    public static bool CyanJunctionActive = false;
    public static bool MagentaJunctionActive = false;
    public static bool YellowJunctionActive = false;
}