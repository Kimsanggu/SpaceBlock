using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor(typeof(TextUsedImage))]
public class TextEditor : Editor {
    TextUsedImage _Text;
    public override void OnInspectorGUI()
    {
        _Text = target as TextUsedImage;
        _Text.texturePath = (string)EditorGUILayout.TextField("TexturePath", _Text.texturePath);
        _Text._string = (string)EditorGUILayout.TextField("Text", _Text._string);
        _Text.fontSize = (float)EditorGUILayout.FloatField("FontSize", _Text.fontSize);
        _Text.alignWidth = (AlignmentWidth)EditorGUILayout.EnumPopup("Width", _Text.alignWidth);
        _Text.alignHeight = (AlignmentHeight)EditorGUILayout.EnumPopup("Height", _Text.alignHeight);
        _Text.useAlignment = EditorGUILayout.Toggle("UseAlignment", _Text.useAlignment);
        
        
        //GUILayout.BeginHorizontal();
        GUILayout.Label("Spacing");
        _Text.spacingX = (float)EditorGUILayout.FloatField("X", _Text.spacingX);
        //GUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("LoadTexture"))
        {
            _Text.LoadTexture();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Reset"))
        {
            _Text.ContentClear();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("SetText"))
        {
            _Text.SetImage();
        }
        EditorGUILayout.EndVertical();
    }   
}
