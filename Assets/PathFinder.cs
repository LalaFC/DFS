using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PathFinder : MonoBehaviour
{
    List<MazeNode> nodes;
    public GameObject StartMarker, EndMarker;
    public List<MazeNode> currentPath = new List<MazeNode>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FindPath()
    {
        Instantiate(StartMarker, GameManager.instance.startNode.transform.position, Quaternion.identity);
        Instantiate(EndMarker, GameManager.instance.endNode.transform.position, Quaternion.identity);
        nodes = GameManager.instance.maze.nodesList;
        foreach (var node in nodes)
        {
            node.SetPathState(NodeState.Available);
        }
        GameManager.instance.endNode.GetComponent<MazeNode>().ChangeFloorColor(Color.red);
        StartCoroutine(Search());
    }

    IEnumerator Search()
    {
        Vector2Int size = GameManager.instance.maze.mazeSize;
        List<MazeNode> completedNodes = new List<MazeNode>();

        // Choose starting node
        currentPath.Add(GameManager.instance.startNode.GetComponent<MazeNode>());
        Debug.Log(GameManager.instance.startNode.GetComponent<MazeNode>().gameObject.name + " Added");
        currentPath[0].SetPathState(NodeState.Current);

        while (completedNodes.Count < nodes.Count && nodes[nodes.IndexOf(currentPath[currentPath.Count - 1])].gameObject!=GameManager.instance.endNode)
        {
            // Check nodes next to the current node
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);

            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;
            Debug.Log(currentNodeIndex.ToString() + " " + currentNodeX.ToString() + " " + currentNodeY.ToString());

            if (currentNodeX < size.x - 1 && !nodes[currentNodeIndex].CheckWall(0))
            {
                // Check node to the right of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }
            if (currentNodeX > 0 && !nodes[currentNodeIndex].CheckWall(1))
            {
                // Check node to the left of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            if (currentNodeY < size.y - 1 && !nodes[currentNodeIndex].CheckWall(2))
            {
                // Check node above the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0 && !nodes[currentNodeIndex].CheckWall(3))
            {
                // Check node below the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // Choose next node
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                currentPath.Add(chosenNode);
                chosenNode.SetPathState(NodeState.Current);
            }
            else
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);

                currentPath[currentPath.Count - 1].SetPathState(NodeState.Completed);
                currentPath.RemoveAt(currentPath.Count - 1);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
