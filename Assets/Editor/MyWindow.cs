using UnityEngine;
using UnityEditor;

public class MyWindow : EditorWindow {
	string myString = "Hello World 안뇽";
	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;
	float myFloat2 = 0f;
	
	// Add menu named "My Window" to the Window menu
	[MenuItem ("Window/My Window")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		MyWindow window = (MyWindow)EditorWindow.GetWindow (typeof (MyWindow));
		window.Show();
	}
	
	void OnGUI () {
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
		myString = EditorGUILayout.TextField ("Text Field", myString);
		
		groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
		myBool = EditorGUILayout.Toggle ("Toggle", myBool);
        EditorGUILayout.EndToggleGroup();

        myFloat2 = EditorGUILayout.Slider("Slider1", myFloat2, 0, 1);

        EditorGUILayout.BeginFadeGroup(myFloat2);
        myFloat = EditorGUILayout.Slider ("Slider2", myFloat, -3, 3);
        EditorGUILayout.EndFadeGroup();

        

    }
}