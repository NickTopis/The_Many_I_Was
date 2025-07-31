using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSequence : MonoBehaviour
{
    [SerializeField] StartingSequence start;
    void Start()
    {
        start.StartSequence();
    }


}
