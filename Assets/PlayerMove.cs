using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] List<MazeNode> Paths;
    bool canMove = false;
    GameObject player;
    public GameObject boom;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
    }
    private void Update()
    {
        if (GameManager.instance.paths.currentPath!=null)
        Paths = GameManager.instance.paths.currentPath;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove && Paths.Count > 0)
        {
            player.transform.position = Vector3.Lerp(transform.position, Paths[0].transform.position, 3*Time.fixedDeltaTime);
            transform.LookAt(Paths[0].transform.position);
            if (Paths.Count == 1)
            {
                GameManager.instance.startNode = Paths[0].gameObject;
            }
            if (Vector3.Distance(transform.position, Paths[0].transform.position) <0.2f)
            {
                Paths[0].ChangeFloorColor(Color.black);
                Paths.Remove(Paths[0]);
            }

        }
        else if (canMove && Paths.Count == 0)
        {
            canMove = false;
            GameManager.instance.paths.TurnOffMarker();
            GameManager.instance.endNode = null;
            GameManager.instance.FindEndNode();
        }
    }
    public void Move()
    {
        canMove= true;
        transform.LookAt(GameManager.instance.endNode.transform.position);
    }
}
