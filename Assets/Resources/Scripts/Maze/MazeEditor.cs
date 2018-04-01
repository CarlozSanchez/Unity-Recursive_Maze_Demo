using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MazeGenerator))]
public class MazeEditor : Editor {


    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        DrawDefaultInspector();

        MazeGenerator myScript = (MazeGenerator)target;

        if (GUILayout.Button("Reset Maze")) {
            myScript.resetMaze();
        }

        if (GUILayout.Button("Log Maze")) {
            myScript.logMaze();
        }
        if(GUILayout.Button("Find Path")) {
            myScript.findPath();
        }
    }

}
