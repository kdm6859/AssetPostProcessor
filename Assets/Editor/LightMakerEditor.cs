using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

[CustomEditor(typeof(LightMaker))]
public class LightMakerEditor : Editor, IPointerClickHandler
{
    private void OnSceneGUI()
    {
        var component = target as LightMaker;
        var transform = component.transform;

        Event currentEvent = Event.current;

        //Tools.current = Tool.Move;

        

        //Light그룹 추가(A키)
        if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.A)
        {
            component.LightGroups.Add(new LightGroup());
            component.LightGroups[component.LightGroups.Count - 1].Group = new GameObject("Group" + (component.LightGroups.Count - 1));
            component.LightGroups[component.LightGroups.Count - 1].Group.transform.parent = transform;
            Debug.Log("LightGroups 추가\nLightGroups : " + component.LightGroups.Count);

            //Undo.RegisterCompleteObjectUndo(component.gameObject, "undo");
            Undo.RegisterCompleteObjectUndo(component, "undo");
        }
        //Light그룹 선택 변경(C키)
        else if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.C)
        {
            component.LightGroupIndex++;
            if (component.LightGroupIndex >= component.LightGroups.Count)
            {
                component.LightGroupIndex = 0;
            }
            Debug.Log("Light그룹 인덱스 변경\n인덱스 : " + component.LightGroupIndex);

            Undo.RegisterCompleteObjectUndo(component, "undo");
            //currentEvent.Use();
        }
        ////Light타입 변경(V키)
        //else if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.V)
        //{
        //    component.Light_Type_Num++;
        //    if (component.Light_Type_Num >= 3)
        //    {
        //        component.Light_Type_Num = 0;
        //    }
        //    Debug.Log("Light타입 인덱스 변경\n인덱스 : " + component.Light_Type_Num);

        //    Undo.RegisterCompleteObjectUndo(component.gameObject, "undo");
        //    //currentEvent.Use();
        //}
        //Light그룹 Light 추가(ctrl + left click)
        else if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && currentEvent.control)//&&currentEvent.clickCount > 0 && currentEvent.isMouse)
        {
            //에디터에서 마우스 포지션을 구하는 법
            //
            //var mousePosition = currentEvent.mousePosition * EditorGUIUtility.pixelsPerPoint;
            //mousePosition.y = Camera.current.pixelHeight - mousePosition.y;
            //Ray ray = Camera.current.ScreenPointToRay(mousePosition);
            // 또는 
            Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
            //Physics.Raycast(ray, out hit, Mathf.Infinity, mask);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("클릭 : " + hit.point);
                //GameObject point = Instantiate(component.pointPrefab, hit.point, Quaternion.identity);
                //point.transform.parent = transform;

                //component.points.Add(point);
                Debug.Log("그룹 갯수" + component.LightGroups.Count);
                if (component.LightGroups.Count == 0)
                {
                    Debug.Log("첫 생성");
                    component.LightGroups.Add(new LightGroup());
                    component.LightGroups[component.LightGroups.Count - 1].Group = new GameObject("Group" + (component.LightGroups.Count - 1));
                    component.LightGroups[component.LightGroups.Count - 1].Group.transform.parent = transform;
                    component.LightGroupIndex = 0;
                }
                Undo.RegisterCompleteObjectUndo(component, "undo");

                GameObject newLightObj = Instantiate(component.LightPrefab, hit.point, Quaternion.identity);
                component.LightGroups[component.LightGroupIndex].Lights.Add(newLightObj);
                newLightObj.transform.parent = component.LightGroups[component.LightGroupIndex].Group.transform;

                Light newLight = newLightObj.GetComponent<Light>();
                newLight.type = component.Light_Type;
                newLight.lightmapBakeType = component.Light_Mode;
                newLight.innerSpotAngle = component.Light_InnerSpotAngle;
                newLight.spotAngle = component.Light_SpotAngle;
                newLight.color = component.Light_Color;
                newLight.intensity = component.Light_Intensity;
                newLight.bounceIntensity = component.Light_BounceIntensity;
                newLight.range = component.Light_Range;
                newLight.shadows = component.Light_Shadows;
                newLight.shadowRadius = component.Light_ShadowRadius;

                //Undo 등록
                Selection.activeObject = null; //Undo 등록할 때 선택된 오브젝트가 있으면 문제가 발생할 수 있음
                //Undo.DestroyObjectImmediate(newLightObj);
                
                EditorUtility.SetDirty(component);
                Undo.RegisterCreatedObjectUndo(newLightObj, "undo");
                EditorUtility.SetDirty(newLightObj);
            }
        }
        //Light그룹 삭제(Shift + del)
        else if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.Delete && currentEvent.shift)
        {
            if (component.LightGroups.Count > 0)
            {
                DestroyImmediate(component.LightGroups[component.LightGroupIndex].Group);
                //Destroy(component.LightGroups[component.LightGroupIndex].Group);
                component.LightGroups.RemoveAt(component.LightGroupIndex);
                if (component.LightGroupIndex == component.LightGroups.Count && component.LightGroups.Count != 0)
                {
                    Debug.Log(component.LightGroupIndex + " " + component.LightGroups.Count);
                    component.LightGroupIndex--;
                }
            }

            Undo.RegisterCompleteObjectUndo(component, "undo");
            //Undo.RegisterCompleteObjectUndo(component, "undo");
        }
        //Light 삭제(del)
        else if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.Delete)
        {
            if (component.LightGroups[component.LightGroupIndex].Lights.Count > 0)
            {
                DestroyImmediate(component.LightGroups[component.LightGroupIndex].Lights[component.LightGroups[component.LightGroupIndex].Lights.Count - 1]);
                //Destroy(component.LightGroups[component.LightGroupIndex].Group);
                component.LightGroups[component.LightGroupIndex].Lights.RemoveAt(component.LightGroups[component.LightGroupIndex].Lights.Count - 1);
            }

            Undo.RegisterCompleteObjectUndo(component, "undo");
            //Undo.RegisterCompleteObjectUndo(component.LightGroups[component.LightGroupIndex].Lights[component.LightGroups[component.LightGroupIndex].Lights.Count - 1], "undo");
        }
        //오브젝트 여러개 선택 테스트
        else if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.E)
        {
            Debug.Log("E");
            Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("클릭 : " + hit.transform);
                component.targets.Add(hit.transform.gameObject);
            }
        }
        //component.tf = false;
        if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.R)
        {
            Debug.Log("R");
            if (component.tf)
                component.tf = false;
            else
            {
                Debug.Log("R2");
                component.tr2 = true;
                component.tf = true;
            }
        }

        //씬 뷰에서 다른 오브젝트
        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
        {

            // 스크린 좌표를 월드 좌표로 변환
            //Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);

            // 마우스 포인터 아래의 오브젝트 찾기
            GameObject selectedObject = HandleUtility.PickGameObject(currentEvent.mousePosition, true);

            if (selectedObject != null)
            {
                Debug.Log("Object Selected: " + selectedObject.name);
                Selection.activeGameObject = selectedObject;

                //라이트 오브젝트일 경우 윈도우창 띄우기
                if (selectedObject.GetComponent<Light>() != null)
                {
                    LightControlWindow.Init(selectedObject);

                    //selectedObject.GetComponent<Light>().
                    ////Light window = (Light)EditorWindow.GetWindow(typeof(Light));
                    //SerializedObject serializedObject = new SerializedObject(selectedObject.GetComponent<Light>());
                    //serializedObject.ApplyModifiedProperties();
                    ////EditorUtility.DisplayInspector(serializedObject);

                    //// 선택한 오브젝트에 대한 SerializedObject 생성
                    ////SerializedObject objSerialized = new SerializedObject(obj);

                    //// Properties 창 띄우기
                    //Editor editor = Editor.CreateEditor(selectedObject.GetComponent<Light>());
                    //EditorWindow window = EditorWindow.
                    ////editor.OnInspectorGUI();
                    //editor.OnPreviewGUI();


                    //CustomInspector.OpenWindow(selectedObject);
                }
            }
        }

        

        if (component.tf)
            Selection.objects = component.targets.ToArray();
        else
            Selection.activeObject = component;



        //Selection.objects = component.targets.ToArray();

        //Delete키를 눌렀을 때 오브젝트가 삭제되지 않게 한다.
        if (currentEvent.keyCode == KeyCode.Delete)
            currentEvent.Use();

        //클릭 시 다른 오브젝트가 선택되는 것을 막는다.
        //Selection.activeObject = component;

        // 에디터 활성화 시에 여러 오브젝트를 선택
        //Object[] targets = serializedObject.targetObjects;
        //Selection.objects = targets;


        if(Tools.current == Tool.Move)
        {
            //현재 선택된 enemyMoveArea의 포인트들의 핸들을 표시
            if (component.LightGroups.Count > 0)
            {
                for (int i = 0; i < component.LightGroups[component.LightGroupIndex].Lights.Count; i++)
                {
                    //component.pointPositions[i] = Handles.PositionHandle(component.pointPositions[i], Quaternion.identity);
                    component.LightGroups[component.LightGroupIndex].Lights[i].transform.position = PositionHandle(component.LightGroups[component.LightGroupIndex].Lights[i].transform.position);
                }
            }
        }
        else if(Tools.current == Tool.Rotate)
        {
            //현재 선택된 enemyMoveArea의 포인트들의 핸들을 표시
            if (component.LightGroups.Count > 0)
            {
                for (int i = 0; i < component.LightGroups[component.LightGroupIndex].Lights.Count; i++)
                {
                    component.LightGroups[component.LightGroupIndex].Lights[i].transform.rotation = 
                        Handles.RotationHandle(component.LightGroups[component.LightGroupIndex].Lights[i].transform.rotation,
                        component.LightGroups[component.LightGroupIndex].Lights[i].transform.position);
                }
            }
        }
        

        if (currentEvent.type == EventType.KeyDown)
        {
            //Debug.Log("SetDirty");
            EditorUtility.SetDirty(component);
        }


        //Scene view에 메뉴얼 표시
        Handles.BeginGUI();
        //var oldbgcolor = GUI.backgroundColor;
        GUI.backgroundColor = Color.cyan;     // 배경 색 지정
        GUI.color = Color.cyan;
        //GUI.color = Color.HSVToRGB(0.83f, 1f, 1f);
        GUIStyle guiBoxStyle = new GUIStyle(GUI.skin.box);
        guiBoxStyle.fontSize = component.manualFontSize;
        guiBoxStyle.alignment = TextAnchor.UpperLeft;
        float guiBoxWidth = 19.1536f;
        float guiBoxHeight = 9.4227f; //텍스트 1줄 : 1.3461
        GUI.Box(new Rect(43, 0, guiBoxWidth * guiBoxStyle.fontSize, guiBoxHeight * guiBoxStyle.fontSize),
            "<Manual>\nLightGroup Add : A\nLightGroup Change : C\nLightGroup Remove : shift + del\nLight Add : ctrl + left click\nLightGroup Count : "
            + component.LightGroups.Count + "\nCurrent Area Index : " + component.LightGroupIndex, guiBoxStyle);
        //GUI.backgroundColor = oldbgcolor;
        //Handles.EndGUI();


        //EditorGUI.EndChangeCheck();

    }


    Vector3 snap;
    void OnEnable()
    {
        EditorGUI.BeginChangeCheck();

        var component = target as LightMaker;
        var transform = component.transform;

        Event currentEvent = Event.current;

        //SnapSettings 의  수치를 얻어오기
        var snapX = EditorPrefs.GetFloat("MoveSnapX", 1f);
        var snapY = EditorPrefs.GetFloat("MoveSnapY", 1f);
        var snapZ = EditorPrefs.GetFloat("MoveSnapZ", 1f);
        snap = new Vector3(snapX, snapY, snapZ);

        Debug.Log("되니?");


        if (component.tr2)
        {
            component.tr2 = false;
        }
        //하이어라키창에서 본인오브젝트 클릭시 Selection.activeObject 대상 본인오브젝트로 바꿈
        else
        {
            component.tf = false;
            Selection.activeObject = component;
        }
    }

    private void OnDisable()
    {
        EditorGUI.EndChangeCheck();
    }

    Vector3 PositionHandle(Vector3 position)
    {
        var component = target as LightMaker;
        var transform = component.transform;

        //var position = transform.position;
        //var size = 10;
        var size = HandleUtility.GetHandleSize(position) * 0.4f;

        //X축
        Handles.color = Handles.xAxisColor;
        position =
            Handles.Slider(position, transform.right, size, Handles.ArrowHandleCap, snap.x);
        //Handles.CircleHandleCap(0,)

        //Y축
        Handles.color = Handles.yAxisColor;
        position =
            Handles.Slider(position, transform.up, size, Handles.ArrowHandleCap, snap.y);

        //Z축
        Handles.color = Handles.zAxisColor;
        position =
            Handles.Slider(position, transform.forward, size, Handles.ArrowHandleCap, snap.z);



        return position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");

        var component = target as LightMaker;
        component.target = eventData.pointerClick;
    }
}

//public class CustomInspector : EditorWindow
//{
//    static GameObject selectedObject;

//    [MenuItem("Window/Open Custom Inspector")]
//    public static void OpenWindow(GameObject obj)
//    {
//        selectedObject = obj;

//        CustomInspector window = EditorWindow.GetWindow<CustomInspector>();
//        window.Show();
//    }

//    void OnGUI()
//    {
//        // 선택된 게임 오브젝트 가져오기
//        //GameObject selectedObject = Selection.activeGameObject;

//        GUILayout.Label("Light Test", EditorStyles.boldLabel);

//        if (selectedObject != null)
//        {
//            // 선택된 게임 오브젝트에 대한 SerializedObject 생성
//            SerializedObject objSerialized = new SerializedObject(selectedObject);


//            // Inspector 창 표시
//            objSerialized.Update();
//            //Editor.CreateEditor(selectedObject).OnInspectorGUI();
//            objSerialized.ApplyModifiedProperties();
//        }
//        else
//        {
//            EditorGUILayout.LabelField("No object selected.");
//        }
//    }
//}