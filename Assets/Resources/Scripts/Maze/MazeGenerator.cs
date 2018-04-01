/***
Author: Carlos Sanchez
Date: 4/1/2018
Version 1.00

MazeGenerator.cs - Generates a 3D representation of a Maze.cs class for visualization.
The use is able to edit the generated Maze by adding and removing barriers. The user is
then able to find a path which is then visualied on screen.

Required Classes:
Maze.cs - Used to construct Maze and finding path.

***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MazeGenerator : MonoBehaviour {
/***************************** CONSTANT VARIABLES *****************************/
    private const char START = 'S';
    private const char FINISH = 'F';
    private const char BACKGROUND = 'N';
    private const char PATH = '+';
    private const char BARRIER = 'X';



    /****************************** CLASS VARIABLES *******************************/



    Material blue;
    GameObject root;
    private Maze maze;

    GameObject[,] cellArray;

    List<GameObject> walls;
    List<GameObject> cells;

    [Range(2, 100)]
    public int width = 3;

    [Range(2,100)]
    public int height = 3;

/******************************************************************************/
/****************************** MONO BEHAVIOUR ********************************/
/******************************************************************************/

    // Use this for initialization
    void Start () {

        // Root gameObject to hold maze parts
        root = new GameObject();
        root.name = "Maze";

        // Instantiate Array List
        walls = new List<GameObject>();
        cells = new List<GameObject>();

        // Create a new maze
        maze = new Maze(width, height);
        cellArray = new GameObject[width, height];
       // Debug.Log(maze);

        // BuildMaze
        buildMaze(maze);

    }

	
	// Update is called once per frame
	void Update () {

        // User wants to add a Barrier
		if(Input.GetMouseButton(0)) {
            updateCell(true);
        }

        // User wants to remove a Barrier
        if(Input.GetMouseButton(1)) {
            updateCell(false);
        }
	}

/******************************************************************************/
/****************************** CLASS METHODS *********************************/
/******************************************************************************/

    /// <summary>
    /// METHOD: builds the 3D representation of our Maze class.
    /// </summary>
    /// <param name="maze"></param>
    public void buildMaze(Maze maze) { 
        generatorBorder(maze.Width, maze.Height);
        generateCells(maze);
    }

    /// <summary>
    /// METHOD: Resets the maze by detroying all game objects related to maze 
    /// then rebuilding it.
    /// </summary>
    public void resetMaze() {

        foreach (GameObject wall in walls) {
            Destroy(wall);
        }

        foreach (GameObject cell in cells) {
            Destroy(cell);
        }

        walls.Clear();
        cells.Clear();

        maze = new Maze(width, height);
        buildMaze(maze);
    }

    /// <summary>
    /// METHOD: Draws the path from Start to finish  in Unity enviroment.
    /// </summary>
    public void findPath() {
        Debug.Log("Is there a path: " + maze.findMazePath());

        for (int x = 0; x < maze.Array.GetLength(0); x++) {
            for (int y = 0; y < maze.Array.GetLength(1); y++) {
                if (maze.Array[x, y] == PATH && (x != 0 || y != 0)
                    && (x != maze.Width - 1 || y != maze.Height - 1)) {
                    cellArray[x, y].GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
        Debug.Log(maze);
    }


    /// <summary>
    /// METHOD: builds the borders of our maze which ly on our Maze's index out of bounds.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void generatorBorder(int width, int height) {

        // Local variables
        GameObject northWall, eastWall, southWall, westWall;
        string[] names = { "North_Wall", "East_Wall", "South_Wall", "West_Wall" };

        
        // Create walls and ad them to list
        walls.Add(northWall = GameObject.CreatePrimitive(PrimitiveType.Cube));
        walls.Add(eastWall = GameObject.CreatePrimitive(PrimitiveType.Cube));
        walls.Add(southWall = GameObject.CreatePrimitive(PrimitiveType.Cube));
        walls.Add(westWall = GameObject.CreatePrimitive(PrimitiveType.Cube));


        // Scale walls to correct size
        northWall.transform.localScale = new Vector3(width , 1, 1);
        southWall.transform.localScale = new Vector3(width, 1, 1);
        westWall.transform.localScale = new Vector3(1, 1, height);
        eastWall.transform.localScale = new Vector3(1, 1, height);

        //place walls in correct position
        northWall.transform.position = new Vector3(width / 2f, .5f, .5f);
        southWall.transform.position = new Vector3(width / 2f, .5f, -height - .5f);
        westWall.transform.position = new Vector3(-.5f, .5f, -height / 2f);
        eastWall.transform.position = new Vector3(width + .5f, .5f, -height / 2f);


        // Change Color, rename and set root as parent
        for (int i = 0; i < names.Length; i++) {
            walls[i].GetComponent<Renderer>().material.color = Color.black;
            walls[i].name = names[i];
            walls[i].transform.parent = root.transform;
        }


    }

    /// <summary>
    /// METHOD: generates default cells that make up our Maze, which include a Starting Cell, Finish Cell,
    /// and Background cells for everything else.
    /// </summary>
    /// <param name="maze"></param>
    private void generateCells(Maze maze) {
        Vector3 currentPosition = Vector3.zero;
        Vector3 offset = new Vector3(.5f, -.5f, -.5f);
        GameObject floor;

        for (int h = 0; h < maze.Array.GetLength(0); h++) {
            for (int w = 0; w < maze.Array.GetLength(1); w++) {

                // Instantiate Tile move to correct position and add to root
                cells.Add(floor = GameObject.CreatePrimitive(PrimitiveType.Cube));
                cellArray[h, w] = floor;
                floor.name = string.Format("{0},{1}", w, h);
                floor.transform.position = currentPosition + offset;
                floor.transform.parent = root.transform;    
                  
                currentPosition.x += 1;

                if (maze.Array[h,w] == START) {
                    floor.GetComponent<Renderer>().material.color = Color.green;
                }

                else if(maze.Array[h,w] == FINISH) {
                    floor.GetComponent<Renderer>().material.color = Color.red;
                }
                else {
                    floor.GetComponent<Renderer>().material.color = Color.grey;
                }
            }
            currentPosition.x = 0;
            currentPosition.z -= 1;
        }
    }

    /// <summary>
    /// Method: detects cell position then passes gameObject to be updateMaze to update
    /// the selected cell.
    /// </summary>
    /// <param name="toBarrier"></param>
    private void updateCell(bool toBarrier) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            updateCell(hit.collider.gameObject, toBarrier);
        }
    }

    /// <summary>
    /// METHOD: Updates the selected cell to either be a Barrier or Background.
    /// </summary>
    /// <param name="cell"></param> The cell to update.
    /// <param name="toBarrier"></param> updates cell to Barrier if toBarrier is true, else updates to background.
    /// <returns></returns>
    private bool updateCell(GameObject cell, bool toBarrier) {

        string[] temp = cell.name.Split(',');
        int x, y;

        x = y = -1;

        if (temp.Length == 2) {
            try {
                x = Int32.Parse(temp[0]);
                y = Int32.Parse(temp[1]);
            }
            catch (Exception e) {
                Debug.LogError(e);
            }

            // move cell up
            if (toBarrier && maze.Array[y, x] == BACKGROUND) {
                maze.Array[y, x] = BARRIER;
                Vector3 pos = cell.transform.position;
                pos.y = .5f;
                cell.transform.position = pos;
                cell.GetComponent<Renderer>().material.color = Color.black;

                // Debug.Log(maze);
            }
            // move cell down
            else if (!toBarrier && maze.Array[y, x] == BARRIER) {
                maze.Array[y, x] = BACKGROUND;
                Vector3 pos = cell.transform.position;
                pos.y = -.5f;
                cell.transform.position = pos;
                cell.GetComponent<Renderer>().material.color = Color.grey;
            }

            return true;
        }
        else {
            return false;
        }
    }



    public void logMaze() {
      //  char c = (char)126;
      //  Debug.Log(c);
        Debug.Log(maze);
    }
}

/**

    
       // blue = Resources.Load("Blue", typeof(Material)) as Material;

        //private void changeCell(int x, int y, char c) {
    //    maze.Array[y,x] = c;
    //}

 **/
