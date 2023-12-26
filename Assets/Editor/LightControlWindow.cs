using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public enum LightAppearance
{
    Color,
    FilterAndTemperature
}

public class LightControlWindow : EditorWindow
{
    //GameObject
    static GameObject currentLight;

    //Tranfrom
    static Transform light_Transform;

    //General
    public static LightType light_Type = LightType.Spot;
    public static LightmapBakeType light_Mode = LightmapBakeType.Baked;
    //Shape
    public static float light_InnerSpotAngle = 0;
    public static float light_SpotAngle = 0;
    //Emission
    public static LightAppearance light_Appearance = LightAppearance.Color;
    public static bool light_UseTemperature = false;
    public static float light_ColorTemperature = 0;
    public static Color light_Color = Color.white;
    public static float light_Intensity = 0;
    public static float light_BounceIntensity = 0;
    public static float light_Range = 0;
    //Rendering
    public static LightRenderMode light_RenderMode = LightRenderMode.Auto;
    public static int light_CullingMask = -1;
    //Shadows
    public static LightShadows Light_Shadows = LightShadows.None;
    public static float Light_ShadowRadius = 0;

    bool isTransform = true;
    bool isGeneral = true;
    bool isShape = true;
    bool isEmission = true;
    bool isRendering = true;
    bool isShadows = true;




    bool test1Open;

    string myString1 = "Light";
    string myString = "Light";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    float myFloat2 = 0f;

    

    public static void Init(GameObject light)
    {
        currentLight = light;

        // Get existing open window or if none, make a new one:
        LightControlWindow window = (LightControlWindow)EditorWindow.GetWindow(typeof(LightControlWindow));

        Event currentEvent = Event.current;
        //HandleUtility.GUIPointToScreenPixelCoordinate(currentEvent.mousePosition);
        var mousePosition = EditorGUIUtility.GUIToScreenPoint(currentEvent.mousePosition);
        window.position = new Rect(mousePosition.x + 20, mousePosition.y, 300, 500);
        //window.position = new Rect(960,200, 300, 500);

        //Transform
        light_Transform = currentLight.transform;
        //General
        light_Type = currentLight.GetComponent<Light>().type;
        light_Mode = currentLight.GetComponent<Light>().lightmapBakeType;
        //Spot Shape
        light_InnerSpotAngle = currentLight.GetComponent<Light>().innerSpotAngle;
        light_SpotAngle = currentLight.GetComponent<Light>().spotAngle;
        //Emission
        light_UseTemperature = currentLight.GetComponent<Light>().useColorTemperature;
        if (light_UseTemperature)
        {
            light_Appearance = LightAppearance.FilterAndTemperature;
        }
        else
        {
            light_Appearance = LightAppearance.Color;
        }

        light_ColorTemperature = currentLight.GetComponent<Light>().colorTemperature;
        light_Color = currentLight.GetComponent<Light>().color;
        light_Intensity = currentLight.GetComponent<Light>().intensity;
        light_BounceIntensity = currentLight.GetComponent<Light>().bounceIntensity;
        light_Range = currentLight.GetComponent<Light>().range;
        //Rendering
        light_RenderMode = currentLight.GetComponent<Light>().renderMode;
        light_CullingMask = currentLight.GetComponent<Light>().cullingMask;
        //Shadows
        Light_Shadows = currentLight.GetComponent<Light>().shadows;
        Light_ShadowRadius = currentLight.GetComponent<Light>().shadowRadius;

        window.Show();
    }

    void OnGUI()
    {
        //SerializedObject serializedObject_light = new SerializedObject(currentLight);
        currentLight = EditorGUILayout.ObjectField("Light", currentLight, typeof(GameObject), true) as GameObject;



        isTransform = EditorGUILayout.Foldout(isTransform, "Transform", true);
        if (isTransform)
        {
            Vector3 position = light_Transform.position;
            Vector3 rotation = light_Transform.rotation.eulerAngles;
            Vector3 scale = light_Transform.localScale;
            position = EditorGUILayout.Vector3Field("Position", position);
            rotation = EditorGUILayout.Vector3Field("Rotation", rotation);
            scale = EditorGUILayout.Vector3Field("Scale", scale);

            currentLight.transform.position = position;
            currentLight.transform.rotation = Quaternion.Euler(rotation);
            currentLight.transform.localScale = scale;
        }

        isGeneral = EditorGUILayout.Foldout(isGeneral, "General", true);
        if (isGeneral)
        {
            light_Type = (LightType)EditorGUILayout.EnumPopup("Type", light_Type);
            light_Mode = (LightmapBakeType)EditorGUILayout.EnumPopup("Mode", light_Mode);

            currentLight.GetComponent<Light>().type = light_Type;
            currentLight.GetComponent<Light>().lightmapBakeType = light_Mode;
        }

        isShape = EditorGUILayout.Foldout(isShape, "Shape", true);
        if (isShape)
        {
            light_InnerSpotAngle = EditorGUILayout.FloatField("Inner Spot Angle", light_InnerSpotAngle);
            light_SpotAngle = EditorGUILayout.FloatField("Outer Spot Angle", light_SpotAngle);

            currentLight.GetComponent<Light>().innerSpotAngle = light_InnerSpotAngle;
            currentLight.GetComponent<Light>().spotAngle = light_SpotAngle;
        }

        isEmission = EditorGUILayout.Foldout(isEmission, "Emission", true);
        if (isEmission)
        {
            light_Appearance = (LightAppearance)EditorGUILayout.EnumPopup("Light Appearance", light_Appearance);
            if (light_Appearance == LightAppearance.Color)
            {
                light_UseTemperature = false;
            }
            else
            {
                light_UseTemperature = true;
            }
            if (light_UseTemperature)
            {
                light_Color = EditorGUILayout.ColorField("Filter", light_Color);
                light_ColorTemperature = EditorGUILayout.FloatField("Temperature", light_ColorTemperature);
            }
            else
            {
                light_Color = EditorGUILayout.ColorField("Color", light_Color);
            }
            light_Intensity = EditorGUILayout.FloatField("Intensity", light_Intensity);
            light_BounceIntensity = EditorGUILayout.FloatField("Indirect Multiplier", light_BounceIntensity);
            light_Range = EditorGUILayout.FloatField("Range", light_Range);

            currentLight.GetComponent<Light>().useColorTemperature = light_UseTemperature;
            currentLight.GetComponent<Light>().color = light_Color;
            currentLight.GetComponent<Light>().colorTemperature = light_ColorTemperature;
            currentLight.GetComponent<Light>().intensity = light_Intensity;
            currentLight.GetComponent<Light>().bounceIntensity = light_BounceIntensity;
            currentLight.GetComponent<Light>().range = light_Range;
        }

        isRendering = EditorGUILayout.Foldout(isRendering, "Rendering", true);
        if (isRendering)
        {
            light_RenderMode = (LightRenderMode)EditorGUILayout.EnumPopup("Render Mode", light_RenderMode);
            string[] layerNames = UnityEditorInternal.InternalEditorUtility.layers;
            light_CullingMask = EditorGUILayout.MaskField("Culling Mask", light_CullingMask, layerNames);

            //List<string> layers = new List<string>();
            //List<int> intlayer = new List<int>();
            //for (int i = 0; i < 32; i++)
            //{
            //    string layerToName = LayerMask.LayerToName(i);
            //    if (layerToName != "")
            //    {
            //        layers.Add(layerToName);
            //        intlayer.Add(LayerMask.NameToLayer(layerToName));
            //    }

            //}
            //light_CullingMask = EditorGUILayout.IntPopup("Culling Mask", light_CullingMask, layers.ToArray(), intlayer.ToArray());

            //// Layer 이름 배열을 가져옵니다.
            //string[] layerNames = UnityEditorInternal.InternalEditorUtility.layers;
            //// Layer 이름을 인덱스로 변환하는 배열을 만듭니다.
            //int[] layerIndices = new int[layerNames.Length];
            //for (int i = 0; i < layerNames.Length; i++)
            //{
            //    layerIndices[i] = LayerMask.NameToLayer(layerNames[i]);
            //}
            ////IntPopup을 통해 레이어를 선택할 수 있는 드롭다운을 만듭니다.
            //light_CullingMask = EditorGUILayout.IntPopup("Layer Mask1", light_CullingMask, layerNames, layerIndices);
            //light_CullingMask = EditorGUILayout.MaskField("Culling Mask", light_CullingMask, layerNames);


            currentLight.GetComponent<Light>().renderMode = light_RenderMode;
            currentLight.GetComponent<Light>().cullingMask = light_CullingMask;
        }

        isShadows = EditorGUILayout.Foldout(isShadows, "Shadows", true);
        if (isShadows)
        {
            Light_Shadows = (LightShadows)EditorGUILayout.EnumPopup("Shadows Type", Light_Shadows);
            Light_ShadowRadius = EditorGUILayout.FloatField("Baked Shadow", Light_ShadowRadius);

            currentLight.GetComponent<Light>().shadows = Light_Shadows;
            currentLight.GetComponent<Light>().shadowRadius = Light_ShadowRadius;
        }

        //serializedObject_light.ApplyModifiedProperties();
    }

    //void OnGUI()
    //{
    //    GUIContent guiContent = new GUIContent();
    //    guiContent.text = "Test1";
    //    guiContent.tooltip = "Test2";
    //    //guiContent.image = 

    //    test1Open = EditorGUILayout.Foldout(test1Open, guiContent, true);
    //    if (test1Open)
    //    {
    //        GUILayout.Label("Base Settings1", EditorStyles.boldLabel);
    //        myString1 = EditorGUILayout.TextField("Text Field1", myString);
    //    }
    //    //EditorGUILayout.EndFoldoutHeaderGroup();




    //    GUILayout.Label("Base Settings", EditorStyles.boldLabel);
    //    myString = EditorGUILayout.TextField("Text Field", myString);

    //    groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
    //    myBool = EditorGUILayout.Toggle("Toggle", myBool);
    //    EditorGUILayout.EndToggleGroup();
    //    myFloat2 = EditorGUILayout.Slider("Slider1", myFloat2, 0, 1);
    //    myFloat2 = EditorGUILayout.Slider("Slider1", myFloat2, 0, 1);
    //    myFloat2 = EditorGUILayout.Slider("Slider1", myFloat2, 0, 1);
    //    myFloat2 = EditorGUILayout.Slider("Slider1", myFloat2, 0, 1);
    //    myFloat2 = EditorGUILayout.Slider("Slider1", myFloat2, 0, 1);

    //    EditorGUILayout.BeginFadeGroup(myFloat2);
    //    myFloat = EditorGUILayout.Slider("Slider2", myFloat, -3, 3);
    //    EditorGUILayout.EndFadeGroup();



    //}
}
