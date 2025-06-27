using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HexNodeDesigner : MonoBehaviour
{
    public HexNodeAStar HexNode;

    public bool isWalkable = true;
    public bool isRamp;
    public bool isCliff;
    public bool isFlyOnly;
    public int heightLevel = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            HexNode.node.isWalkable = isWalkable;
            HexNode.node.isRamp = isRamp;
            HexNode.node.isCliff = isCliff;
            HexNode.node.isFlyOnly = isFlyOnly;
            HexNode.node.heightLevel = heightLevel;

            Debug.Log(HexNode.node.isWalkable);
            Debug.Log("Update");
        }
    }
}
