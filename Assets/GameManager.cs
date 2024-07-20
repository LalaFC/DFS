using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public MazeGenerator maze;
    public GameObject startNode, endNode;
    bool searchEndNode = false;
    [SerializeField] GameObject Camera;
    public GameObject controlPanel;
    public GameObject finishPanel;
    public PathFinder paths;
    public static GameManager instance { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this) { Destroy(this); }
        else { instance = this; }
        controlPanel = GameObject.Find("ControlPanel");
        finishPanel = GameObject.Find("FinishPanel");
        finishPanel.SetActive(false);
        controlPanel.SetActive(false);
        Player = GameObject.FindWithTag("Player");
        paths = GetComponent<PathFinder>();
    }

    // Update is called once per frame
    void Update()
    {
        if(searchEndNode)
        {
            GameObject node;
            if (endNode == null)
            {
                node = maze.nodesList[Random.Range(0, maze.nodesList.Count)].gameObject;
                if (Vector3.Distance(startNode.transform.position, node.transform.position) > 10)
                {
                    endNode = node;
                    endNode.GetComponent<MazeNode>().ChangeFloorColor(Color.red);
                }

            }
            else
            searchEndNode = false;
        }
    }
    public void SetNodes()
    {
        Camera.transform.position = new Vector3(-3.5f, Camera.transform.position.y, Camera.transform.position.z);
        GameObject.Find("BlockerPanel").SetActive(false);
        controlPanel.SetActive(true);
        startNode = maze.nodesList[0].gameObject;
        Player.transform.position = startNode.transform.position;
        startNode.GetComponent<MazeNode>().ChangeFloorColor(Color.green);
        searchEndNode = true;
    }

    public void ResetMaze()
    {
        SceneManager.LoadScene(0);
    }
}
