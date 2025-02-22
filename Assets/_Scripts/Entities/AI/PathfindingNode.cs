using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode
{
    // public static int instancesCreated = 0;
    // public int id;
    public int fCost;
    public int gCost;
    public int hCost;
    public Vector3Int pos;

    public PathfindingNode prev;

    public PathfindingNode(int fCost, int gCost, int hCost, Vector3Int pos, PathfindingNode prev)
    {
        this.fCost = fCost;
        this.gCost = gCost;
        this.hCost = hCost;
        this.pos = pos;
        this.prev = prev;
        // id = instancesCreated++; 
        // Debug.Log(id); 
    }

    public class NodeComparer : IComparer<PathfindingNode>
    {
        public int Compare(PathfindingNode x, PathfindingNode y)
        {
            int retVal = x.fCost.CompareTo(y.fCost);
            if (retVal == 0)
            {
                retVal = x.hCost.CompareTo(y.hCost);
                if (retVal == 0)
                {

                    // retVal = x.id.CompareTo(y.id);
                    // retVal = x.GetHashCode().CompareTo(y.GetHashCode());
                    retVal = x.pos.x.CompareTo(y.pos.x);
                    if (retVal == 0)
                    {
                        retVal = x.pos.y.CompareTo(y.pos.y);
                        // if they have the same x and y and cost and stuff they might as well be the same thing 
                    }
                }
            }
            // if (retVal == 0)
            // { 
            //     Debug.Log("Pain");
            // }

            return retVal;
        }
    }
}
