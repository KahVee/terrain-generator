using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI() {
        LevelGenerator lvlGen = (LevelGenerator)target;

        if(DrawDefaultInspector()) {
            if(lvlGen.autoUpdate) {
                lvlGen.GenerateMap();
            }
        }

        if(lvlGen.islandMode) {
            for (int i = 1; i < 9; i++) {
                lvlGen.display.meshRenderer[i].enabled = true;
            }
        } else {
            for (int i = 1; i < 9; i++) {
                lvlGen.display.meshRenderer[i].enabled = false;
            }
        }

        if(GUILayout.Button("Generate")){
            lvlGen.GenerateMap();
        }
    }    
}
