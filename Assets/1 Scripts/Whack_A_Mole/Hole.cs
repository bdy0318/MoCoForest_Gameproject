using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    //두더지 홀이 배치되어 있는 순번(왼쪽 상단부터 0)
    [field: SerializeField]
    public int HoleIndex { private set; get; }
}
