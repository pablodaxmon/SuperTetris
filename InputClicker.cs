using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputClicker : MonoBehaviour
{
    [SerializeField]
    [Range(1, 7)]
    int posInYAxis = 1;
    private void OnMouseDown()
    {
        BoardManager.instance.moveCurrentGroupYAxis(posInYAxis);
    }
}
