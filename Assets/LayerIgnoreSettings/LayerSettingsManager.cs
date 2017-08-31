using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LayerSettingsManager : MonoBehaviour
{




    public static bool HasData;
    private static LayerIgnoreData data;
    [SerializeField]
    private static TextAsset pawsConfigText;
    [SerializeField]
    private static TextAsset shelter2ConfigText;

    private static string pawsConfig = "PawsLayerConfig";
    private static string shelterConfig = "Shelter2LayerConfig";

    private static bool[] allLayerIgnores = new bool[496];
    private static bool pawsChange;

    public static Dictionary<string, bool> dictionary = new Dictionary<string, bool>();

    public static void SetLayersMatrix(bool isShelter)
    {
        string dataStr = "";
        if (isShelter)
        {
            var ta = Resources.Load(shelterConfig) as TextAsset;
            dataStr = ta.text;
        }
        else
        {
            var ta = Resources.Load(pawsConfig) as TextAsset;
            dataStr = ta.text;
        }

        data = JsonUtility.FromJson<LayerIgnoreData>(dataStr);
        BuildDictionary();

        for (int i = 0; i < 32; i++)
        {
            for (int j = 31; j >= i; j--)
            {
                if (isShelter)
                {
                    //进入shelter2时，根据配置忽略指定层
                    if (pawsChange)
                    {
                        //Physics.IgnoreLayerCollision(i, j, allLayerIgnores[count]);
                        //count++;

                        if (GetLayerIgnore(i, j) == 1)
                        {
                            Physics.IgnoreLayerCollision(i, j, true);
                        }
                        else if (GetLayerIgnore(i, j) == 0)
                        {
                            Physics.IgnoreLayerCollision(i, j, false);
                        }
                    }
                }
                else
                {
                    //进入爪子时，所有层不忽略
                    if (GetLayerIgnore(i, j) == 1)
                    {
                        Physics.IgnoreLayerCollision(i, j, true);
                    }
                    else if (GetLayerIgnore(i, j) == 0)
                    {
                        Physics.IgnoreLayerCollision(i, j, false);
                    }
                }
            }
        }
        if (isShelter)
        {
            pawsChange = false;
        }
        else
        {
            pawsChange = true;
        }

    }

    public static void GetAllLayerIgnore()
    {
        int counter = 0;
        for (int i = 0; i < 32; i++)
        {
            for (int j = 31; j >= i; j--)
            {
                allLayerIgnores[counter] = Physics.GetIgnoreLayerCollision(i, j);
                //DataPrefs.data.shelterLayersIgnoreSettings[counter] =  Physics.GetIgnoreLayerCollision(i, j);
                counter++;
            }
        }
        HasData = true;
    }

    void OnGUI()
    {
        if (GUILayout.Button("Shelter"))
        {
            SetLayersMatrix(true);
        }
        if (GUILayout.Button("Paws"))
        {
            SetLayersMatrix(false);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Export Layer Config")]
    public void Create()
    {
        dictionary = new Dictionary<string, bool>();
        data = new LayerIgnoreData();
        for (int i = 0; i < 32; i++)
        {
            for (int j = 31; j >= i; j--)
            {
                AddLayerIgnore(i, j, Physics.GetIgnoreLayerCollision(i, j));
                //DataPrefs.data.shelterLayersIgnoreSettings[counter] =  Physics.GetIgnoreLayerCollision(i, j);
            }
        }
        SeparateDictionary();

        // 自定义资源保存路径
        string path = Application.dataPath + "/Config";

        // 如果项目总不包含该路径，创建一个
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        //将类名 Bullet 转换为字符串
        //拼接保存自定义资源（.asset） 路径
        //path = string.Format("Assets/Config/{0}.asset", (typeof(LayerIgnoreConfig).ToString()));
        path = string.Format("{0}/Config/{1}.txt", Application.dataPath, "Shelter2LayerConfig");
        Debug.Log(path);
        var jsonStr = JsonUtility.ToJson(data);
        Debug.Log(jsonStr);
        FileInfo file = new FileInfo(path);
        StreamWriter sw = file.CreateText();
        sw.Write(jsonStr);
        sw.Close();
        // 生成自定义资源到指定路径
        //AssetDatabase.CreateAsset(config, path);
    }
#endif

    #region Tools
    public void AddLayerIgnore(int layer1, int layer2, bool ignore)
    {
        if (LayerMask.LayerToName(layer1) == "" || LayerMask.LayerToName(layer2) == "")
        {
            return;
        }
        if (ContainsKey(layer1, layer2))
        {
            Debug.LogError("Layer config already has this key");
            return;
        }
        else
        {
            var sb = new StringBuilder();
            sb.Append(LayerMask.LayerToName(layer1));
            sb.Append(LayerMask.LayerToName(layer2));
            dictionary.Add(sb.ToString(), ignore);
        }
    }
    public void SeparateDictionary()
    {

        data.keys = new List<string>(dictionary.Keys);
        data.values = new List<bool>(dictionary.Values);
    }

    public bool ContainsKey(int layer1, int layer2)
    {
        var s1 = LayerMask.LayerToName(layer1);
        var s2 = LayerMask.LayerToName(layer2);
        if (dictionary.ContainsKey(s1 + s2) || dictionary.ContainsKey(s2 + s1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private static void BuildDictionary()
    {
        if (dictionary != null)
        {
            dictionary.Clear();
        }
        else
        {
            dictionary = new Dictionary<string, bool>();
        }
        var count = Mathf.Min(data.keys.Count, data.values.Count);
        for (int i = 0; i < count; i++)
        {
            dictionary.Add(data.keys[i], data.values[i]);
        }


    }

    public static int GetLayerIgnore(int layer1, int layer2)
    {
        if (LayerMask.LayerToName(layer1) == "" || LayerMask.LayerToName(layer2) == "")
        {
            return -1;
        }
        var k = GetKey(layer1, layer2);
        if (k != string.Empty)
        {
            if (dictionary[k])
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            //TODO 如果没有这一对key，需要设置失败
            //Debug.LogError("Layer config has no key");
            return -1;
        }
    }

    public static string GetKey(int layer1, int layer2)
    {
        var s1 = LayerMask.LayerToName(layer1);
        var s2 = LayerMask.LayerToName(layer2);
        if (dictionary.ContainsKey(s1 + s2))
        {
            return s1 + s2;

        }
        else if (dictionary.ContainsKey(s2 + s1))
        {
            return s2 + s1;
        }
        else
        {
            //Debug.LogError("Layer config has no key");
            return string.Empty;
        }
    }
    #endregion
}

public class LayerIgnoreData
{
    public List<string> keys = new List<string>();
    public List<bool> values = new List<bool>();

    public LayerIgnoreData()
    {
        keys = new List<string>();
        values = new List<bool>();
    }
}
