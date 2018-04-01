/***
Author: Carlos Sanchez
Date: 4/1/2018
Version 1.00

Maze.cs - Is used to create a Maze in the form of a 2d array with characters
representing the cells in the maze.  Recursive method to find a path.

Required Classes:
Maze.cs - Used to construct Maze and finding path.

***/
using System.Collections;
using System.Collections.Generic;

public class Maze {

/***************************** CONSTANT VARIABLES *****************************/
    private const char BARRIER = 'X';
    private const char START = 'S';
    private const char FINISH = 'F';
    private const char BACKGROUND =  'N';
    private const char PATH = '+';
    private const char TEMPORARY = '-';
    private const int MINIMUM_WIDTH = 3;
    private const int MINIMUM_HEIGHT = 3;




/****************************** CLASS VARIABLES *******************************/

    private int _width = MINIMUM_WIDTH;
    private int _height = MINIMUM_WIDTH;
    private char[,] _array;

/******************************************************************************/
/******************************** PROPERTIES **********************************/
/******************************************************************************/

    public int Width {
        get {
            return _width;
        }

        set {
            if(value < MINIMUM_WIDTH) {
                value = MINIMUM_WIDTH;
            }
            _width = value;
        }
    }

    public int Height {
        get {
            return _height;
        }

        set {
            if(value  < MINIMUM_HEIGHT) {
                value = MINIMUM_HEIGHT;
            }
            _height = value;
        }
    }

    public char[,] Array {
        get {
            return _array;
        }

        set {
            _array = value;
        }
    }




    /******************************************************************************/
    /******************************* CONSTRUCTORS *********************************/
    /******************************************************************************/

    /// <summary>
    /// CONSTRUCTOR: default constructor
    /// </summary>
    public Maze() {
        setDimension(MINIMUM_WIDTH, MINIMUM_HEIGHT);
        this.Array = generateGrid( Width, Height);
    }

    /// <summary>
    /// CONSTRUCTOR: full constructor
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Maze(int width, int height) {
        setDimension(width, height);
        this.Array = generateGrid (Width, Height);
    }

    /// <summary>
    /// HELPER: sets both width and height
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void setDimension(int width, int height) {
        this.Width = width;
        this.Height = height;
    }



    /******************************************************************************/
    /********************************* OVERRIDES **********************************/
    /******************************************************************************/

    /// <summary>
    /// OVERRIDE: returns the maze in string form with characters reprcenting the 
    /// different cells in maze.
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
        string temp = "";

        for (int h = 0; h < Height; h++) {
            for (int w = 0; w < Width; w++) {
                temp += Array[h, w];
            }
            temp += "\n";
        }

        return temp;
    }


    /******************************************************************************/
    /****************************** CLASS METHODS *********************************/
    /******************************************************************************/


    public void updateMaze() {
        this.Array = new char[Height, Width];
    }


    /// <summary>
    /// Generates a multi _array of chars based on given width and height;
    /// All indexes are filled with a wall.
    /// </summary>
    /// <param name="width"></param> 
    /// <param name="height"></param>
    /// <returns></returns>
    private char[,] generateGrid(int width, int height) {

        char[,] temp = new char[width, height];

        for (int w = 0; w < width; w++) {
            for (int h = 0; h < height; h++) {
                temp[w, h] = symbol(w, h);
            }
        }

        //char[,] temp = new char[height, width];

        //for (int h = 0; h < height; h++) {

        //    for (int w = 0; w < width; w++) {
        //        temp[h, w] = symbol(w, h);
        //    }
        //}
        return temp;
    }

    /// <summary>
    /// WRAPPER: starts off our recursive findMazePath at (0,0);
    /// </summary>
    /// <returns></returns>
    public bool findMazePath() {
        return findMazePath(0, 0);
    }

    /// <summary>
    /// RECURSIVE METHOD: Attemps to find a path thorugh point(x,y).
    /// PRECONDITION: Possible path cells are in BACKGROUND character,
    ///               barrier cells are in ABNORMAL character.
    /// POSTCONDITION: If a path is found, all cells on it are set to 
    ///                the PATH character; all cells that were visited
    ///                but are not on the path are in the TEMPORARY character.
    /// </summary>
    /// <param name="x"></param> The x-coordinate of current cell.
    /// <param name="y"></param> The y-coordinate of current cell.
    /// <returns></returns>
    public bool findMazePath(int x, int y) {

        // Out of bounds
        if (x < 0 || y < 0 || x >= this.Width || y >= this.Height) {
            return false;
        }
        // Cell is on barrier or dead end.
        else if (this.Array[y, x] != BACKGROUND && this.Array[y,x] != START && this.Array[y,x] != FINISH ) {
            return false;
        }
        // Cell is on a path and is maze exit
        else if (x == this.Width - 1 && y == this.Height - 1) {
            this.Array[y, x] = PATH;
            return true;
        }
        // Recursive Case, Attempt to find a path from each neighbor.
        // Tentatively mark cell as o
        else { 
            // Mark path
            this.Array[y, x] = PATH;

            // Check North, East, South, West
            if(findMazePath(x, y + 1) || findMazePath(x + 1, y) 
                || findMazePath(x , y -1) || findMazePath(x-1, y)){
                return true;
            }
            else {
                this.Array[y,x] = TEMPORARY; // Dead end
                return false;
            }
        }
    }

    /// <summary>
    /// HELPER: Used for Initial set up to set cell to either Start,
    ///         Finish, or Background.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private char symbol(int x, int y) {

        if (x == 0 && y == 0) {
             return START;
            //return BACKGROUND;
        }
        else if (x == Width-1 && y == Height-1) {
             return FINISH;
           // return BACKGROUND;
        }
        else return BACKGROUND;

    }

    //public string toString() {
    //    return "Test";
    //}

}






/******************************************************************************/
/********************************* MUTATORS ***********************************/
/******************************************************************************/


/******************************************************************************/
/********************************* ACCESSORS **********************************/
/******************************************************************************/




/******************************************************************************/
/********************************* INTERFACE **********************************/
/******************************************************************************/




/******************************************************************************/
/****************************** STATIC METHODS ********************************/
/******************************************************************************/


/******************************************************************************/
/****************************** HELPER METHODS ********************************/
/******************************************************************************/
