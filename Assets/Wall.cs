using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public enum WallType {Left, Right, Ground};
    public WallType wallType;
}
