using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LightGroup
{
    [SerializeField] GameObject group;
    public GameObject Group { get { return group; } set { group = value; } }

    [SerializeField] List<GameObject> lights = new List<GameObject>();
    public List<GameObject> Lights { get { return lights; } set { lights = value; } }
}

public class LightMaker : MonoBehaviour
{
    public List<LightGroup> lightGroups = new List<LightGroup>();
    public List<LightGroup> LightGroups { get { return lightGroups; } set { lightGroups = value; } }
    int lightGroupIndex = 0;
    public int LightGroupIndex { get { return lightGroupIndex; } set { lightGroupIndex = value; } }

    public int manualFontSize = 13;

    [SerializeField] GameObject lightPrefab;
    public GameObject LightPrefab { get { return lightPrefab; } set { lightPrefab = value; } }


    int light_Type_Num = 0;
    public int Light_Type_Num { get { return light_Type_Num; } set { light_Type_Num = value; } }
    public LightType Light_Type = LightType.Spot;
    public LightmapBakeType Light_Mode = LightmapBakeType.Baked;
    public float Light_InnerSpotAngle = 0;
    public float Light_SpotAngle = 0;
    public Color Light_Color = Color.white;
    public float Light_Intensity = 0;
    public float Light_BounceIntensity = 0;
    public float Light_Range = 0;
    public LightShadows Light_Shadows = LightShadows.None;
    public float Light_ShadowRadius = 0;

    [NonSerialized] int current_light_Type_Num = 0;
    public int Current_Light_Type_Num { get { return current_light_Type_Num; } set { current_light_Type_Num = value; } }
    [NonSerialized] public LightType Current_Light_Type = LightType.Spot;
    [NonSerialized] public LightmapBakeType Current_Light_Mode = LightmapBakeType.Baked;
    [NonSerialized] public float Current_Light_InnerSpotAngle = 0;
    [NonSerialized] public float Current_Light_SpotAngle = 0;
    [NonSerialized] public Color Current_Light_Color = Color.white;
    [NonSerialized] public float Current_Light_Intensity = 0;
    [NonSerialized] public float Current_Light_BounceIntensity = 0;
    [NonSerialized] public float Current_Light_Range = 0;
    [NonSerialized] public LightShadows Current_Light_Shadows = LightShadows.None;
    [NonSerialized] public float Current_Light_ShadowRadius = 0;


    public List<UnityEngine.Object> targets = new List<UnityEngine.Object>();
    public GameObject target;
    public bool tf = false;
    public bool tr2 = false;

    public GameObject testLight;

    // Start is called before the first frame update
    void Start()
    {
        //General
        testLight.GetComponent<Light>().type = LightType.Spot;
        testLight.GetComponent<Light>().lightmapBakeType = LightmapBakeType.Baked;
        //Spot Shape
        testLight.GetComponent<Light>().innerSpotAngle = 10;
        testLight.GetComponent<Light>().spotAngle = 30;
        //Emission
        testLight.GetComponent<Light>().useColorTemperature = true;
        testLight.GetComponent<Light>().colorTemperature = 0;
        testLight.GetComponent<Light>().color = Color.HSVToRGB(0.5f, 0.5f, 0.5f);
        testLight.GetComponent<Light>().intensity = 100;
        testLight.GetComponent<Light>().bounceIntensity = 100;
        testLight.GetComponent<Light>().range = 100;
        //Rendering
        testLight.GetComponent<Light>().renderMode = LightRenderMode.Auto;
        List<string> layers = new List<string>();
        layers.Add("UI");
        layers.Add("Water");
        testLight.GetComponent<Light>().cullingMask = LayerMask.GetMask(layers.ToArray());

        string[] layers1 = new string[32];
        List<int> intlayer = new List<int>();
        for (int i = 0; i < 32; i++)
        {
            layers1[i] = LayerMask.LayerToName(i);
            if (layers1[i] != "")
                intlayer.Add(i);
            Debug.Log(layers1[i]);
        }


        Debug.Log(LayerMask.LayerToName(0));
        Debug.Log("Ȯ�� : " + ~(LayerMask.GetMask("Default")));
        Debug.Log(LayerMask.GetMask(layers.ToArray()));
        Debug.Log(LayerMask.NameToLayer("UI"));

        //Shadows
        testLight.GetComponent<Light>().shadows = LightShadows.Soft;
        testLight.GetComponent<Light>().shadowRadius = 10;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
