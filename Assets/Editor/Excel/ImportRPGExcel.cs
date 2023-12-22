using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// 유니티에서 지원하는 AssetPostprocessor를
/// 이용해서 엑셀 파일을 ScriptableObject로 변경하는
/// 스크립트
/// </summary>
public class ImportRPGExcel : AssetPostprocessor
{
    static readonly string filePath = "Assets/Editor/Data/RPGData.xlsx";
    static readonly string playerExportPath = "Assets/Resources/Data/PlayerLevelData.asset";
    static readonly string enemyExportPath = "Assets/Resources/Data/MonsterLevelData.asset";
    static readonly string storeExportPath = "Assets/Resources/Data/StoreData.asset";

    // Player json 데이터 경로.
    private static string playerDataPath = "Assets/Resources/Data/PlayerLevelData.json";

    [MenuItem("DataImport/ExcelImport #&g")]
    static void ExcelImport()
    {
        Debug.Log("Excel data covert start.");

        //엑셀파일 오픈
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

        //Open된 엑셀 파일 메모리에 생성
        IWorkbook book = new XSSFWorkbook(stream);

        //스크립터블오브젝트 생성
        //MakePlayerData(ref book);
        MakeEnemyData(book);
        MakeStoreData(book);

        //Json 생성
        CreatePlayerJsonData(book);

        //엑셀파일 클로즈
        stream.Close();

        Debug.Log("Excel data covert complete.");
    }

    /// <summary>
    /// 에셋이 유니티 엔진에 추가되면 실행되는 엔진 함수
    /// </summary>
    /// <param name="importedAssets"></param>
    /// <param name="deletedAssets"></param>
    /// <param name="movedAssets"></param>
    /// <param name="movedFromAssetPaths"></param>
    static void OnPostprocessAllAssets(
        string[] importedAssets, string[] deletedAssets,
        string[] movedAssets, string[] movedFromAssetPaths)
    {

        //임포트 된 모픈 파일을 검색함
        foreach (string s in importedAssets)
        {
            //우리가 원하는 파일 일때만 수행
            if (s == filePath)
            {
                Debug.Log("Excel data covert start.");

                //엑셀파일 오픈
                FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

                //Open된 엑셀 파일 메모리에 생성
                IWorkbook book = new XSSFWorkbook(stream);

                //EditorCoroutineUtility.StartCoroutine(MakePlayerData(book), true);

                //스크립터블오브젝트 생성
                MakePlayerData(book);
                MakeEnemyData(book);
                MakeStoreData(book);

                //Json 생성
                CreatePlayerJsonData(book);

                //엑셀파일 클로즈
                stream.Close();

                Debug.Log("Excel data covert complete.");
            }
        }
    }

    /// <summary>
    /// 주인공 정보를 ScriptableObject만듬
    /// </summary>
    static void MakePlayerData()
    {
        //SciprtableObject를 생성
        PlayerLevelData data = ScriptableObject.CreateInstance<PlayerLevelData>();

        // 이미 파일이 존재하는지 확인
        if (File.Exists(playerExportPath))
        {
            // 파일이 이미 존재한다면 삭제하거나 다른 경로로 지정
            File.Delete(playerExportPath);
            // 또는 다른 경로로 지정
            // playerExportPath = "Assets/Resources/Data/NewPlayerLevelData.asset";
        }
        //ScriptableObject를 파일로 만듬
        AssetDatabase.CreateAsset((ScriptableObject)data, playerExportPath);
        //수정 불가능하게 설정(에디터에서 수정 하게 하려면 주석처리)
        data.hideFlags = HideFlags.NotEditable;

        //자료를 삭제(초기화)
        data.list.Clear();

        //엑셀 파일을 Open
        using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            //Open된 엑셀 파일 메모리에 생성
            IWorkbook book = new XSSFWorkbook(stream);

            //두번째(주인공 정보) Sheet를 열기
            ISheet sheet = book.GetSheetAt(1);
            //3번쨰 줄(row)부터 읽기
            for (int i = 2; i <= sheet.LastRowNum; i++)
            {
                //줄(row)읽기
                IRow row = sheet.GetRow(i);
                //시리얼라이즈를 위한 임시 객체 생성
                PlayerLevelData.Attribute a = new PlayerLevelData.Attribute();
                //레벨 
                a.level = (int)row.GetCell(0).NumericCellValue;
                //최대 체력
                a.maxHP = (int)row.GetCell(1).NumericCellValue;
                //기본 공격력
                a.baseAttack = (int)row.GetCell(2).NumericCellValue;
                //필요 경험치
                a.reqExp = (int)row.GetCell(3).NumericCellValue;
                //이동 속도
                a.moveSpeed = (int)row.GetCell(4).NumericCellValue;
                //회전 속도
                a.turnSpeed = (int)row.GetCell(5).NumericCellValue;
                //공격 범위
                a.attackRange = (float)row.GetCell(6).NumericCellValue;
                //리스트에 추가하기
                data.list.Add(a);
            }

            //stream.Close();
        }

        //위에서 생성된 SciprtableObject의 파일을 찾음
        ScriptableObject obj =
            AssetDatabase.LoadAssetAtPath(playerExportPath, typeof(ScriptableObject)) as ScriptableObject;
        //디스크에 쓰기
        EditorUtility.SetDirty(obj);
    }

    /// <summary>
    /// 주인공 정보를 ScriptableObject만듬
    /// </summary>
    static void MakePlayerData(IWorkbook book)
    {
        //SciprtableObject를 생성
        PlayerLevelData data = ScriptableObject.CreateInstance<PlayerLevelData>();

        // 이미 파일이 존재하는지 확인
        if (File.Exists(playerExportPath))
        {
            // 파일이 이미 존재한다면 삭제하거나 다른 경로로 지정
            File.Delete(playerExportPath);

            //bool isReceived = true;

            //GetValueAsync().ContinueWith(LoadFunc);

            //while (isReceived)
            //{
            //    print("데이터 수신 여부를 확인하고있습니다.");

            //    yield return new WaitForSeconds(1f);
            //}

            //void LoadFunc(Task<DataSnapshot> task)
            //{
            //    if (task.IsFaulted)
            //    {
            //        print("DB 받기 실패");
            //    }
            //    else if (task.IsCanceled)
            //    {
            //        print("DB 받기 취소");
            //    }
            //    else if (task.IsCompleted)
            //    {
            //        DataSnapshot snapshot = task.Result;

            //        foreach (var data in snapshot.Children)
            //        {
            //            string json = data.GetRawJsonValue();
            //            print(json);
            //        }

            //        print("데이터를 성공적으로 받았습니다.");

            //        isReceived = false;
            //    }
            //}



            //while (File.Exists(playerExportPath))
            //{
            //    Debug.Log("삭제중...");
            //    yield return null;
            //}



            // 또는 다른 경로로 지정
            // playerExportPath = "Assets/Resources/Data/NewPlayerLevelData.asset";
            
        }

        //수정 불가능하게 설정(에디터에서 수정 하게 하려면 주석처리)
        data.hideFlags = HideFlags.NotEditable;

        //자료를 삭제(초기화)
        data.list.Clear();

        ////엑셀 파일을 Open
        //using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        //{

        //}



        //두번째(주인공 정보) Sheet를 열기
        ISheet sheet = book.GetSheetAt(1);
        //3번쨰 줄(row)부터 읽기
        for (int i = 2; i <= sheet.LastRowNum; i++)
        {
            //줄(row)읽기
            IRow row = sheet.GetRow(i);
            //시리얼라이즈를 위한 임시 객체 생성
            PlayerLevelData.Attribute a = new PlayerLevelData.Attribute();
            //레벨 
            a.level = (int)row.GetCell(0).NumericCellValue;
            //최대 체력
            a.maxHP = (int)row.GetCell(1).NumericCellValue;
            //기본 공격력
            a.baseAttack = (int)row.GetCell(2).NumericCellValue;
            //필요 경험치
            a.reqExp = (int)row.GetCell(3).NumericCellValue;
            //이동 속도
            a.moveSpeed = (int)row.GetCell(4).NumericCellValue;
            //회전 속도
            a.turnSpeed = (int)row.GetCell(5).NumericCellValue;
            //공격 범위
            a.attackRange = (float)row.GetCell(6).NumericCellValue;
            //리스트에 추가하기
            data.list.Add(a);
        }

        //ScriptableObject를 파일로 만듬
        AssetDatabase.CreateAsset((ScriptableObject)data, playerExportPath);

        //stream.Close();
        
        //위에서 생성된 SciprtableObject의 파일을 찾음
        ScriptableObject obj =
            AssetDatabase.LoadAssetAtPath(playerExportPath, typeof(ScriptableObject)) as ScriptableObject;
        //디스크에 쓰기
        EditorUtility.SetDirty(obj);
    }


    /// <summary>
    /// 슬라임 정보를 ScriptableObject만듬
    /// </summary>
    static void MakeEnemyData(IWorkbook book)
    {
        //SciprtableObject를 생성
        MonsterLevelData data = ScriptableObject.CreateInstance<MonsterLevelData>();
        // 이미 파일이 존재하는지 확인
        if (File.Exists(enemyExportPath))
        {
            // 파일이 이미 존재한다면 삭제하거나 다른 경로로 지정
            File.Delete(enemyExportPath);

            // 또는 다른 경로로 지정
            // playerExportPath = "Assets/Resources/Data/NewPlayerLevelData.asset";
        }


        

        //수정 불가능하게 설정(에디터에서 수정 하게 하려면 주석처리)
        data.hideFlags = HideFlags.NotEditable;

        //자료를 삭제(초기화)
        data.infos.Clear();

        ////엑셀 파일을 Open
        //using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        //{

        //}




        for (int j = 2; j < 5; j++)
        {
            ISheet sheet = book.GetSheetAt(j);
            MonsterLevelData.Race info = new MonsterLevelData.Race();
            //몬스터 이름 가져오기
            info.name = sheet.SheetName.ToString();

            //4번쨰 줄(row)부터 읽기
            for (int i = 2; i <= sheet.LastRowNum; i++)
            {
                //줄(row)읽기
                IRow row = sheet.GetRow(i);
                //시리얼라이즈를 위한 임시 객체 생성
                MonsterLevelData.Attribute a = new MonsterLevelData.Attribute();
                //레벨 
                a.level = (int)row.GetCell(0).NumericCellValue;
                //최대 체력
                a.maxHP = (int)row.GetCell(1).NumericCellValue;
                //기본 공격력
                a.attack = (int)row.GetCell(2).NumericCellValue;
                //방어력
                a.defence = (int)row.GetCell(3).NumericCellValue;
                //얻는 경험치
                a.gainExp = (int)row.GetCell(4).NumericCellValue;
                a.walkSpeed = (float)row.GetCell(5).NumericCellValue;
                a.runSpeed = (float)row.GetCell(6).NumericCellValue;
                //회전 속도
                a.turnSpeed = (int)row.GetCell(7).NumericCellValue;
                //공격 범위
                a.attackRange = (float)row.GetCell(8).NumericCellValue;
                //얻는 금화
                a.gainGold = (int)row.GetCell(9).NumericCellValue;
                //리스트에 추가하기
                info.list.Add(a);
            }

            data.infos.Add(info);
        }

        //ScriptableObject를 파일로 만듬
        AssetDatabase.CreateAsset((ScriptableObject)data, enemyExportPath);

        //stream.Close();

        //위에서 생성된 SciprtableObject의 파일을 찾음
        ScriptableObject obj =
            AssetDatabase.LoadAssetAtPath(enemyExportPath, typeof(ScriptableObject)) as ScriptableObject;
        //디스크에 쓰기
        EditorUtility.SetDirty(obj);
    }

    static void MakeStoreData(IWorkbook book)
    {
        //SciprtableObject를 생성
        StoreData data = ScriptableObject.CreateInstance<StoreData>();
        // 이미 파일이 존재하는지 확인
        if (File.Exists(storeExportPath))
        {
            // 파일이 이미 존재한다면 삭제하거나 다른 경로로 지정
            File.Delete(storeExportPath);
            // 또는 다른 경로로 지정
            // playerExportPath = "Assets/Resources/Data/NewPlayerLevelData.asset";
        }
        
        //수정 불가능하게 설정(에디터에서 수정 하게 하려면 주석처리)
        data.hideFlags = HideFlags.NotEditable;

        //자료를 삭제(초기화)
        data.list.Clear();

        ////엑셀 파일을 Open
        //using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        //{

        //}


        ISheet sheet = book.GetSheetAt(5);

        //3번쨰 줄(row)부터 읽기
        for (int i = 2; i <= sheet.LastRowNum; i++)
        {
            //줄(row)읽기
            IRow row = sheet.GetRow(i);
            //시리얼라이즈를 위한 임시 객체 생성
            StoreData.Attribute a = new StoreData.Attribute();
            //등급 
            a.grade = (int)row.GetCell(0).NumericCellValue;
            //무기 공격력
            a.weaponAtk = (int)row.GetCell(1).NumericCellValue;
            //방어구 방어력
            a.ArmorDef = (int)row.GetCell(2).NumericCellValue;
            //무기 가격
            a.WeaponPrice = (int)row.GetCell(3).NumericCellValue;
            //방어구 가격
            a.ArmorPrice = (int)row.GetCell(4).NumericCellValue;

            //리스트에 추가하기
            data.list.Add(a);
        }
        //ScriptableObject를 파일로 만듬
        AssetDatabase.CreateAsset((ScriptableObject)data, storeExportPath);

        //stream.Close();

        //위에서 생성된 SciprtableObject의 파일을 찾음
        ScriptableObject obj =
            AssetDatabase.LoadAssetAtPath(enemyExportPath, typeof(ScriptableObject)) as ScriptableObject;
        //디스크에 쓰기
        EditorUtility.SetDirty(obj);
    }




    // 플레이어 json 데이터 생성 ( json 파일 생성 )
    static void CreatePlayerJsonData(IWorkbook book)
    {
        //List<Attribute_> list = new List<Attribute_>();
        PlayerLevelData_json data = new PlayerLevelData_json();

        //using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        //{

        //}

        ISheet sheet = book.GetSheetAt(1);

        for (int ix = 2; ix < sheet.LastRowNum + 1; ++ix)
        {
            //줄(row)읽기
            IRow row = sheet.GetRow(ix);
            //시리얼라이즈를 위한 임시 객체 생성
            PlayerLevelData_json.Attribute a = new PlayerLevelData_json.Attribute();
            //레벨 
            a.level = (int)row.GetCell(0).NumericCellValue;
            //최대 체력
            a.maxHP = (int)row.GetCell(1).NumericCellValue;
            //기본 공격력
            a.baseAttack = (int)row.GetCell(2).NumericCellValue;
            //필요 경험치
            a.reqExp = (int)row.GetCell(3).NumericCellValue;
            //이동 속도
            a.moveSpeed = (int)row.GetCell(4).NumericCellValue;
            //회전 속도
            a.turnSpeed = (int)row.GetCell(5).NumericCellValue;
            //공격 범위
            a.attackRange = (float)row.GetCell(6).NumericCellValue;
            //리스트에 추가하기
            data.list.Add(a);
        }

        // json 데이터를 파일에 쓰기.
        //string levelJsonData = SimpleJson.SimpleJson.SerializeObject(list);
        string levelJsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(playerDataPath, levelJsonData, System.Text.Encoding.UTF8);
    }
}



//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;
//using System.IO;
//using NPOI.XSSF.UserModel;
//using NPOI.SS.UserModel;

//public class ImportRPGExcel : AssetPostprocessor
//{
//    static readonly string filePath = "Assets/Editor/Data/RPGData.xlsx";
//    static readonly string playerExportPath = "Assets/Resources/Data/PlayerLevelData.asset";
//    static readonly string enemyExportPath = "Assets/Resources/Data/MonsterLevelData.asset";
//    static readonly string storeExportPath = "Assets/Resources/Data/StoreData.asset";
//    private static string playerDataPath = "Assets/Resources/Data/PlayerLevelData.json";

//    [MenuItem("DataImport/ExcelImport #&g")]
//    static void ExcelImport()
//    {
//        Debug.Log("Excel data covert start.");

//        // 엑셀 파일 오픈
//        using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
//        {
//            // Open된 엑셀 파일 메모리에 생성
//            IWorkbook book = new XSSFWorkbook(stream);

//            // 스크립터블 오브젝트 생성 및 데이터 채우기
//            MakePlayerData(book);
//            MakeEnemyData(book);
//            MakeStoreData(book);

//            // Json 생성
//            CreatePlayerJsonData(book);

//            // 엑셀 파일 클로즈
//            stream.Close();
//        }

//        Debug.Log("Excel data covert complete.");
//    }

//    /// <summary>
//    /// 에셋이 유니티 엔진에 추가되면 실행되는 엔진 함수
//    /// </summary>
//    /// <param name="importedAssets"></param>
//    /// <param name="deletedAssets"></param>
//    /// <param name="movedAssets"></param>
//    /// <param name="movedFromAssetPaths"></param>
//    static void OnPostprocessAllAssets(
//        string[] importedAssets, string[] deletedAssets,
//        string[] movedAssets, string[] movedFromAssetPaths)
//    {

//        //임포트 된 모픈 파일을 검색함
//        foreach (string s in importedAssets)
//        {
//            //우리가 원하는 파일 일때만 수행
//            if (s == filePath)
//            {
//                Debug.Log("Excel data covert start.");

//                // 엑셀 파일 오픈
//                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
//                {
//                    // Open된 엑셀 파일 메모리에 생성
//                    IWorkbook book = new XSSFWorkbook(stream);

//                    // 스크립터블 오브젝트 생성 및 데이터 채우기
//                    MakePlayerData(book);
//                    MakeEnemyData(book);
//                    MakeStoreData(book);

//                    // Json 생성
//                    CreatePlayerJsonData(book);

//                    // 엑셀 파일 클로즈
//                    stream.Close();
//                }

//                Debug.Log("Excel data covert complete.");
//            }
//        }
//    }

//    static void MakePlayerData(IWorkbook book)
//    {
//        PlayerLevelData data = ScriptableObject.CreateInstance<PlayerLevelData>();
//        AssetDatabase.CreateAsset(data, playerExportPath);
//        data.hideFlags = HideFlags.NotEditable;

//        data.list.Clear();
//        ISheet sheet = book.GetSheetAt(1);

//        for (int i = 2; i <= sheet.LastRowNum; i++)
//        {
//            IRow row = sheet.GetRow(i);
//            PlayerLevelData.Attribute a = new PlayerLevelData.Attribute();
//            a.level = (int)row.GetCell(0).NumericCellValue;
//            a.maxHP = (int)row.GetCell(1).NumericCellValue;
//            a.baseAttack = (int)row.GetCell(2).NumericCellValue;
//            a.reqExp = (int)row.GetCell(3).NumericCellValue;
//            a.moveSpeed = (int)row.GetCell(4).NumericCellValue;
//            a.turnSpeed = (int)row.GetCell(5).NumericCellValue;
//            a.attackRange = (float)row.GetCell(6).NumericCellValue;
//            data.list.Add(a);
//        }

//        EditorUtility.SetDirty(data);
//    }

//    static void MakeEnemyData(IWorkbook book)
//    {
//        MonsterLevelData data = ScriptableObject.CreateInstance<MonsterLevelData>();
//        AssetDatabase.CreateAsset(data, enemyExportPath);
//        data.hideFlags = HideFlags.NotEditable;
//        data.infos.Clear();

//        for (int j = 2; j < 5; j++)
//        {
//            ISheet sheet = book.GetSheetAt(j);
//            MonsterLevelData.Race info = new MonsterLevelData.Race();
//            info.name = sheet.SheetName.ToString();

//            for (int i = 2; i <= sheet.LastRowNum; i++)
//            {
//                IRow row = sheet.GetRow(i);
//                MonsterLevelData.Attribute a = new MonsterLevelData.Attribute();
//                a.level = (int)row.GetCell(0).NumericCellValue;
//                a.maxHP = (int)row.GetCell(1).NumericCellValue;
//                a.attack = (int)row.GetCell(2).NumericCellValue;
//                a.defence = (int)row.GetCell(3).NumericCellValue;
//                a.gainExp = (int)row.GetCell(4).NumericCellValue;
//                a.walkSpeed = (float)row.GetCell(5).NumericCellValue;
//                a.runSpeed = (float)row.GetCell(6).NumericCellValue;
//                a.turnSpeed = (int)row.GetCell(7).NumericCellValue;
//                a.attackRange = (float)row.GetCell(8).NumericCellValue;
//                a.gainGold = (int)row.GetCell(9).NumericCellValue;
//                info.list.Add(a);
//            }

//            data.infos.Add(info);
//        }

//        EditorUtility.SetDirty(data);
//    }

//    static void MakeStoreData(IWorkbook book)
//    {
//        StoreData data = ScriptableObject.CreateInstance<StoreData>();
//        AssetDatabase.CreateAsset(data, storeExportPath);
//        data.hideFlags = HideFlags.NotEditable;
//        data.list.Clear();

//        ISheet sheet = book.GetSheetAt(5);

//        for (int i = 2; i <= sheet.LastRowNum; i++)
//        {
//            IRow row = sheet.GetRow(i);
//            StoreData.Attribute a = new StoreData.Attribute();
//            a.grade = (int)row.GetCell(0).NumericCellValue;
//            a.weaponAtk = (int)row.GetCell(1).NumericCellValue;
//            a.ArmorDef = (int)row.GetCell(2).NumericCellValue;
//            a.WeaponPrice = (int)row.GetCell(3).NumericCellValue;
//            a.ArmorPrice = (int)row.GetCell(4).NumericCellValue;
//            data.list.Add(a);
//        }

//        EditorUtility.SetDirty(data);
//    }

//    static void CreatePlayerJsonData(IWorkbook book)
//    {
//        PlayerLevelData_json data = new PlayerLevelData_json();
//        ISheet sheet = book.GetSheetAt(1);

//        for (int ix = 2; ix < sheet.LastRowNum + 1; ++ix)
//        {
//            IRow row = sheet.GetRow(ix);
//            PlayerLevelData_json.Attribute a = new PlayerLevelData_json.Attribute();
//            a.level = (int)row.GetCell(0).NumericCellValue;
//            a.maxHP = (int)row.GetCell(1).NumericCellValue;
//            a.baseAttack = (int)row.GetCell(2).NumericCellValue;
//            a.reqExp = (int)row.GetCell(3).NumericCellValue;
//            a.moveSpeed = (int)row.GetCell(4).NumericCellValue;
//            a.turnSpeed = (int)row.GetCell(5).NumericCellValue;
//            a.attackRange = (float)row.GetCell(6).NumericCellValue;
//            data.list.Add(a);
//        }

//        string levelJsonData = JsonUtility.ToJson(data, true);
//        File.WriteAllText(playerDataPath, levelJsonData, System.Text.Encoding.UTF8);
//    }
//}