using UnityEditor;
using UnityEngine;
public class LevelEditor : EditorWindow
{
    private const int SEGMENT_COUNT = 24;
    private const float RING_RADIUS = 350;
    private const float BUTTON_SIZE = 80f;
    private const int MIN_WINDOW_WIDTH = 750;
    private const int MIN_WINDOW_HEIGHT = 1150;
    private int selectedLevelIndex = 0;
    private int selectedRingIndex = 0;
    private int selectedSegmentIndex = 0;

    private LevelData selectedLevelData;

   



    [MenuItem("Window/Rings of Ruin/Level Editor")]
    public static void ShowWindow()
    {
        LevelEditor window = GetWindow<LevelEditor>("Level Editor");
        window.minSize = new Vector2(MIN_WINDOW_WIDTH, MIN_WINDOW_HEIGHT);
    }

    private void OnEnable()
    {
        LoadSelectedLevel();
    }


    private void OnGUI()
    {
        Rect paddedRect = new Rect(20, 20, position.width - 40, position.height - 40);
        GUILayout.BeginArea(paddedRect);
        Level();
        if (selectedLevelData != null)
        {
            Runeflare();
            Divider();
            Rings();
            DrawRingLayout();
            SegmentType();
            SpawnItem();
            Bridge();
            EnemyWayPoint();
            PreviewButtons();
        }

        GUILayout.EndArea();
    }



    private void Level()
    {
        EditorGUILayout.BeginHorizontal();


        GUIStyle boldLevelLabel = new GUIStyle(EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Level:", boldLevelLabel, GUILayout.Width(50));

        // Get all existing LevelData assets and create level options
        string[] levelOptions = GetLevelOptions();
        bool hasLevels = levelOptions.Length > 0;


        GUI.enabled = hasLevels;
        int newSelectedIndex = EditorGUILayout.Popup(selectedLevelIndex, levelOptions);

        // Check if selection changed
        if (newSelectedIndex != selectedLevelIndex)
        {
            selectedLevelIndex = newSelectedIndex;

            // Load the selected level data
            if (hasLevels && newSelectedIndex >= 0 && newSelectedIndex < levelOptions.Length)
            {
                string folderPath = "Assets/Levels";
                string[] guids = AssetDatabase.FindAssets("t:LevelData", new[] { folderPath });
                if (newSelectedIndex < guids.Length)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[newSelectedIndex]);
                    selectedLevelData = AssetDatabase.LoadAssetAtPath<LevelData>(path);
                }
            }

            SaveSelectedLevel();
        }

        GUI.enabled = true;
        if (GUILayout.Button("Add Level", GUILayout.Width(100)))
        {
            CreateNewLevel();
            SaveSelectedLevel();
        }

        GUI.enabled = hasLevels;
        if (GUILayout.Button("Delete Level", GUILayout.Width(100)))
        {

        }
        GUI.enabled = true;

        EditorGUILayout.EndHorizontal();

    }

    private string[] GetLevelOptions()
    {
        string folderPath = "Assets/Levels";
        string[] guids = AssetDatabase.FindAssets("t:LevelData", new[] { folderPath });


        string[] levelNames = new string[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
            levelNames[i] = fileName;
        }

        return levelNames;
    }


    private void CreateNewLevel()
    {
        // Ensure the Levels folder exists
        string folderPath = "Assets/Levels";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "Levels");
        }

        // Find the next available level number
        string[] existingLevels = AssetDatabase.FindAssets("t:LevelData", new[] { folderPath });
        int nextLevelNumber = existingLevels.Length;


        // Create the asset name with proper formatting
        string assetName = $"Level {nextLevelNumber + 1}";
        string fullPath = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{assetName}.asset");

        // Create the new LevelData asset
        selectedLevelData = CreateInstance<LevelData>();

        // Create the asset file
        AssetDatabase.CreateAsset(selectedLevelData, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Update the selected index to point to the new level
        selectedLevelIndex = nextLevelNumber; // Arrays are 0-based, so subtract 1

        // Initialize with first ring and select it
        AddRing(0);
        selectedRingIndex = 0;
        selectedSegmentIndex = 0; // Select segment 0 by default
    }



    private void AddRing(int ringIndex)
    {
        selectedLevelData.rings.Add(new Ring(ringIndex));
    }


    private void Runeflare()
    {
        EditorGUILayout.Space(10);
        selectedLevelData.hasRuneFlares = EditorGUILayout.Toggle("Enable Runeflares", selectedLevelData.hasRuneFlares);
        EditorGUILayout.Space(3);

        // Disable the input fields when checkbox is unchecked
        GUI.enabled = selectedLevelData.hasRuneFlares;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Min Spawn Interval:", GUILayout.Width(125));
        selectedLevelData.minRuneFlaresSpawnInterval = EditorGUILayout.FloatField(selectedLevelData.minRuneFlaresSpawnInterval, GUILayout.Width(80));

        EditorGUILayout.Space(20);

        EditorGUILayout.LabelField("Max Spawn Interval:", GUILayout.Width(125));
        selectedLevelData.maxRuneFlaresSpawnInterval = EditorGUILayout.FloatField(selectedLevelData.maxRuneFlaresSpawnInterval, GUILayout.Width(80));

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        // Re-enable GUI elements
        GUI.enabled = true;
    }


    private void Divider()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space(15);
    }


    private void Rings()
    {
        EditorGUILayout.BeginHorizontal();

        GUIStyle boldRingsLabel = new GUIStyle(EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Rings:", boldRingsLabel, GUILayout.Width(50));

        // Get ring options based on the selected level
        string[] ringOptions = GetRingOptions();
        selectedRingIndex = EditorGUILayout.Popup(selectedRingIndex, ringOptions);

        if (GUILayout.Button("Add Ring", GUILayout.Width(100)))
        {
            AddRing(selectedLevelData.rings.Count);
            // Update the selected ring index to the newly added ring
            selectedRingIndex = selectedLevelData.rings.Count - 1;
            selectedSegmentIndex = 0; // Select segment 0 by default
        }

        // Only enable delete button if there are rings and it's not ring 0
        GUI.enabled = selectedRingIndex == selectedLevelData.rings.Count - 1 && selectedRingIndex != 0;
        if (GUILayout.Button("Delete Ring", GUILayout.Width(100)))
        {
            DeleteRing(selectedRingIndex);
        }
        GUI.enabled = true;

        EditorGUILayout.EndHorizontal();
    }

    private void DeleteRing(int ringIndex)
    {
        selectedLevelData.rings.RemoveAt(ringIndex);
        selectedRingIndex = selectedLevelData.rings.Count - 1;
        selectedSegmentIndex = 0;
    }

    private string[] GetRingOptions()
    {
        string[] ringNames = new string[selectedLevelData.rings.Count];
        for (int i = 0; i < selectedLevelData.rings.Count; i++)
        {
            ringNames[i] = $"Ring {i}";
        }
        return ringNames;
    }


    private void DrawRingLayout()
    {
        EditorGUILayout.Space(30);
        float viewWidth = position.width;
        Rect layoutRect = GUILayoutUtility.GetRect(viewWidth, 600f);
        Vector2 ringCenter = new Vector2(layoutRect.x + viewWidth / 2f, layoutRect.y + 375f);

        Handles.BeginGUI();

        for (int i = 0; i < SEGMENT_COUNT; i++)
        {
            DrawSegmentButton(i, ringCenter);
        }

        Handles.EndGUI();
        EditorGUILayout.Space(200);
    }

    private void DrawSegmentButton(int segmentIndex, Vector2 ringCenter)
    {
        float angle = segmentIndex * Mathf.PI * 2f / SEGMENT_COUNT - Mathf.PI / 2f;
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * RING_RADIUS;
        Vector2 buttonCenter = ringCenter + offset;

        Rect buttonRect = new Rect(buttonCenter.x - BUTTON_SIZE / 2, buttonCenter.y - BUTTON_SIZE / 2, BUTTON_SIZE, BUTTON_SIZE);

        // Draw the button with white background
        GUI.color = Color.white;
        if (GUI.Button(buttonRect, ""))
        {
            selectedSegmentIndex = segmentIndex;
        }

        // Draw blue border if this segment is selected
        if (segmentIndex == selectedSegmentIndex)
        {
            GUI.color = new Color(0.227f, 0.474f, 0.733f); // #3a79bb
            float borderThickness = 3f;

            // Draw border lines
            Rect topBorder = new Rect(buttonRect.x, buttonRect.y, buttonRect.width, borderThickness);
            Rect bottomBorder = new Rect(buttonRect.x, buttonRect.y + buttonRect.height - borderThickness, buttonRect.width, borderThickness);
            Rect leftBorder = new Rect(buttonRect.x, buttonRect.y, borderThickness, buttonRect.height);
            Rect rightBorder = new Rect(buttonRect.x + buttonRect.width - borderThickness, buttonRect.y, borderThickness, buttonRect.height);

            GUI.DrawTexture(topBorder, EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(bottomBorder, EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(leftBorder, EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(rightBorder, EditorGUIUtility.whiteTexture);
        }

        // Draw the segment number
        float topPadding = 8f;
        float numberHeight = 15f;
        float numberWidth = 30f;
        Rect numberRect = new Rect(
            buttonRect.x + (buttonRect.width - numberWidth) / 2,
            buttonRect.y + topPadding,
            numberWidth,
            numberHeight
        );

        // Draw the number in white
        GUI.color = Color.white;
        GUI.Label(numberRect, segmentIndex.ToString(), new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Color.white }
        });

        GUI.color = Color.white;
    }



    private void SegmentType()
    {
        EditorGUILayout.BeginHorizontal();

        GUIStyle boldSegmentTypeLabel = new GUIStyle(EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Segment Type:", boldSegmentTypeLabel, GUILayout.Width(90));

        // Get segment type names from the enum
        string[] segmentTypeOptions = System.Enum.GetNames(typeof(SegmentType));

        Segment segment = selectedLevelData.rings[selectedRingIndex].segments[selectedSegmentIndex];
        int currentSegmentTypeIndex = (int)segment.segmentType;

        int newSegmentTypeIndex = EditorGUILayout.Popup(currentSegmentTypeIndex, segmentTypeOptions);

        if (newSegmentTypeIndex != currentSegmentTypeIndex)
        {
            segment.segmentType = (SegmentType)newSegmentTypeIndex;
        }

        EditorGUILayout.EndHorizontal();
    }



    private void SpawnItem()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();

        GUIStyle boldSpawnItemLabel = new GUIStyle(EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Spawn Type:", boldSpawnItemLabel, GUILayout.Width(90));

        string[] spawnItemOptions = System.Enum.GetNames(typeof(SpawnType));
        Segment segment = selectedLevelData.rings[selectedRingIndex].segments[selectedSegmentIndex];

        int currentSpawnTypeIndex = (int)segment.spawnType;

        int newSpawnTypeIndex = EditorGUILayout.Popup(currentSpawnTypeIndex, spawnItemOptions);
        if (newSpawnTypeIndex != currentSpawnTypeIndex)
        {
            segment.spawnType = (SpawnType)newSpawnTypeIndex;
        }

        EditorGUILayout.EndHorizontal();
    }



    



    private void Bridge()
    {
        EditorGUILayout.Space(10);
        Segment segment = selectedLevelData.rings[selectedRingIndex].segments[selectedSegmentIndex];
        segment.hasBridge = EditorGUILayout.Toggle("Bridge", segment.hasBridge);
    }



    private void EnemyWayPoint()
    {
        EditorGUILayout.BeginHorizontal();

        GUIStyle boldEnemyWayPointLabel = new GUIStyle(EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Enemy Way Point:", boldEnemyWayPointLabel, GUILayout.Width(110));

        string[] enemyWayPointOptions = System.Enum.GetNames(typeof(WaypointType));
        Segment segment = selectedLevelData.rings[selectedRingIndex].segments[selectedSegmentIndex];


        int currentEnemyWayPointIndex = (int)segment.waypointType;

        int newEnemyWayPointIndex = EditorGUILayout.Popup(currentEnemyWayPointIndex, enemyWayPointOptions);
        if (newEnemyWayPointIndex != currentEnemyWayPointIndex)
        {
            segment.waypointType = (WaypointType)newEnemyWayPointIndex;
        }

        EditorGUILayout.EndHorizontal();
    }



    private void PreviewButtons()
    {
        EditorGUILayout.Space(50);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Build Preview"))
        {

        }

        if (GUILayout.Button("Clear Preview"))
        {

        }
        EditorGUILayout.EndHorizontal();
    }

    private void SaveSelectedLevel()
    {
        if (selectedLevelData != null)
        {
            string path = AssetDatabase.GetAssetPath(selectedLevelData);
            EditorPrefs.SetString("RingsOfRuin_SelectedLevel", path);
        }
    }

    private void LoadSelectedLevel()
    {
        string savedPath = EditorPrefs.GetString("RingsOfRuin_SelectedLevel", "");
        if (!string.IsNullOrEmpty(savedPath))
        {
            selectedLevelData = AssetDatabase.LoadAssetAtPath<LevelData>(savedPath);
            if (selectedLevelData != null)
            {
                // Find the index of this level in our options
                string[] levelOptions = GetLevelOptions();
                selectedLevelIndex = System.Array.IndexOf(levelOptions, selectedLevelData.name);
            }
        }
    }
}