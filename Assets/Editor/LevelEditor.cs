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
    private bool hasLevels = false;


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
        if (selectedLevelData == null)
        {
            CreateLevel();
        }
        else
        {
            Levels();
            Rings();
            Segments();
            PreviewButtons();
        }
    }


    private void CreateLevel()
    {
        AddLevelButton();
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


    private void Levels()
    {
        DrawSection("Levels");
        EditorGUILayout.BeginHorizontal();
        DrawArea(() =>
        {
            EditorGUILayout.LabelField("Selected Level", EditorStyles.label, GUILayout.Width(110));
            EditorGUILayout.LabelField("Enable Runeflares", EditorStyles.label, GUILayout.Width(110));
            GUI.enabled = selectedLevelData.hasRuneFlares;
            EditorGUILayout.LabelField("Min Spawn Interval", EditorStyles.label, GUILayout.Width(110));
            GUI.enabled = true;
        }, 1, new RectOffset(40, 46, 0, 0));

        DrawArea(() =>
        {
            EditorGUILayout.BeginHorizontal();
            LevelPopup();
            AddLevelButton();
            DeleteLevelButton();
            EditorGUILayout.EndHorizontal();
            selectedLevelData.hasRuneFlares = EditorGUILayout.Toggle(selectedLevelData.hasRuneFlares);
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = selectedLevelData.hasRuneFlares;
            selectedLevelData.minRuneFlaresSpawnInterval = EditorGUILayout.FloatField(selectedLevelData.minRuneFlaresSpawnInterval, GUILayout.Width(144));
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Max Spawn Interval", EditorStyles.label, GUILayout.Width(123));
            selectedLevelData.maxRuneFlaresSpawnInterval = EditorGUILayout.FloatField(selectedLevelData.maxRuneFlaresSpawnInterval, GUILayout.Width(144));
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }, 0, new RectOffset(0, 6, 0, 0));
        EditorGUILayout.EndHorizontal();
    }


    private void Rings()
    {
        DrawSection("Rings");
        EditorGUILayout.BeginHorizontal();
        DrawArea(() =>
        {
            EditorGUILayout.LabelField("Selected Ring", EditorStyles.label, GUILayout.Width(110));
        }, 1, new RectOffset(40, 46, 0, 0));

        DrawArea(() =>
        {
            EditorGUILayout.BeginHorizontal();
            RingPopup();
            AddRingButton();
            DeleteRingButton();
            EditorGUILayout.EndHorizontal();
        }, 0, new RectOffset(0, 6, 0, 0));
        EditorGUILayout.EndHorizontal();
        RingSegments();
    }


    private void Segments()
    {
        DrawSection("Segments");
        EditorGUILayout.BeginHorizontal();
        DrawArea(() =>
        {
            EditorGUILayout.LabelField("Segment Type", EditorStyles.label, GUILayout.Width(110));
            EditorGUILayout.LabelField("Bridge", EditorStyles.label, GUILayout.Width(110));
            EditorGUILayout.LabelField("Spawn Type", EditorStyles.label, GUILayout.Width(110));
            EditorGUILayout.LabelField("Enemy Way Point", EditorStyles.label, GUILayout.Width(110));

        }, 1, new RectOffset(40, 46, 0, 0));

        DrawArea(() =>
        {
            SegmentTypePopup();
            BridgeCheckbox();
            SpawnTypePopup();
            EnemyWaypointPopup();

        }, 0, new RectOffset(0, 6, 0, 0));
        EditorGUILayout.EndHorizontal();
    }


    private void PreviewButtons()
    {
        EditorGUILayout.Space(100);
        EditorGUILayout.BeginHorizontal();
        // GUILayout.Space(30);
        GUILayout.FlexibleSpace();
        BuildPreviewButton();
        ClearPreviewButton();
        GUILayout.Space(10);
        EditorGUILayout.EndHorizontal();
    }


    private void LevelPopup()
    {
        // Get all existing LevelData assets and create level options
        string[] levelOptions = GetLevelOptions();
        hasLevels = levelOptions.Length > 0;

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


    private void AddLevelButton()
    {
        GUI.enabled = true;
        if (GUILayout.Button("Add Level", GUILayout.Width(100)))
        {
            CreateNewLevel();
            SaveSelectedLevel();
        }
    }


    private void DeleteLevelButton()
    {
        GUI.enabled = hasLevels;
        if (GUILayout.Button("Delete Level", GUILayout.Width(100)))
        {

        }
        GUI.enabled = true;
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


    private void SaveSelectedLevel()
    {
        if (selectedLevelData != null)
        {
            string path = AssetDatabase.GetAssetPath(selectedLevelData);
            EditorPrefs.SetString("RingsOfRuin_SelectedLevel", path);
        }
    }


    private void RingPopup()
    {
        string[] ringOptions = new string[selectedLevelData.rings.Count];
        for (int i = 0; i < selectedLevelData.rings.Count; i++)
        {
            ringOptions[i] = $"Ring {i}";
        }
        selectedRingIndex = EditorGUILayout.Popup(selectedRingIndex, ringOptions);
    }


    private void AddRingButton()
    {
        if (GUILayout.Button("Add Ring", GUILayout.Width(100)))
        {
            AddRing(selectedLevelData.rings.Count);
            // Update the selected ring index to the newly added ring
            selectedRingIndex = selectedLevelData.rings.Count - 1;
            selectedSegmentIndex = 0; // Select segment 0 by default
        }
    }


    private void DeleteRingButton()
    {
        GUI.enabled = selectedRingIndex == selectedLevelData.rings.Count - 1 && selectedRingIndex != 0;
        if (GUILayout.Button("Delete Ring", GUILayout.Width(100)))
        {
            DeleteRing(selectedRingIndex);
        }
        GUI.enabled = true;
    }


    private void AddRing(int ringIndex)
    {
        selectedLevelData.rings.Add(new Ring());
        Ring ring = selectedLevelData.rings[selectedLevelData.rings.Count - 1];

        for (int i = 0; i < SEGMENT_COUNT; i++)
        {
            ring.segments.Add(new Segment { ringIndex = ringIndex, segmentIndex = i, ringSegmentType = GetRingSegmentType(0) });
        }
    }


    private void DeleteRing(int ringIndex)
    {
        selectedLevelData.rings.RemoveAt(ringIndex);
        selectedRingIndex = selectedLevelData.rings.Count - 1;
        selectedSegmentIndex = 0;
    }


    private void RingSegments()
    {
        EditorGUILayout.Space(22);
        float viewWidth = position.width;
        Rect layoutRect = GUILayoutUtility.GetRect(viewWidth, 600f);
        Vector2 ringCenter = new Vector2(layoutRect.x + viewWidth / 2f, layoutRect.y + 375f);

        Handles.BeginGUI();
        for (int i = 0; i < SEGMENT_COUNT; i++)
        {
            DrawRingSegmentButton(i, ringCenter);
        }
        Handles.EndGUI();
        EditorGUILayout.Space(172);
    }

    private void DrawRingSegmentButton(int segmentIndex, Vector2 ringCenter)
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
            GUI.color = new Color(0.227f, 0.474f, 0.733f);
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


    private void SegmentTypePopup()
    {
        Segment segment = selectedLevelData.rings[selectedRingIndex].segments[selectedSegmentIndex];
        int currentSegmentTypeIndex = GetSegmentTypeIndex(segment.ringSegmentType);

        int newSegmentTypeIndex = EditorGUILayout.Popup(currentSegmentTypeIndex, new string[] { "Normal", "Gap", "Crumbling", "Spike" });

        if (newSegmentTypeIndex != currentSegmentTypeIndex)
        {
            segment.ringSegmentType = GetRingSegmentType(newSegmentTypeIndex);
        }
    }

    private int GetSegmentTypeIndex(RingSegmentType ringSegmentType)
    {
        // Converts RingSegmentType enum back to small index (0-3) for popup
        return (int)ringSegmentType % 4;
    }

    private RingSegmentType GetRingSegmentType(int segmentTypeIndex)
    {
        // Converts SegmentType enum to RingSegmentType enum for levelPool compatibility
        return (RingSegmentType)(selectedRingIndex * 4 + segmentTypeIndex);
    }


    private void SpawnTypePopup()
    {
        string[] spawnItemOptions = System.Enum.GetNames(typeof(SpawnType));
        Segment segment = selectedLevelData.rings[selectedRingIndex].segments[selectedSegmentIndex];

        int currentSpawnTypeIndex = (int)segment.spawnType;

        int newSpawnTypeIndex = EditorGUILayout.Popup(currentSpawnTypeIndex, spawnItemOptions);
        if (newSpawnTypeIndex != currentSpawnTypeIndex)
        {
            segment.spawnType = (SpawnType)newSpawnTypeIndex;
        }
    }


    private void EnemyWaypointPopup()
    {
        string[] enemyWayPointOptions = System.Enum.GetNames(typeof(WaypointType));
        Segment segment = selectedLevelData.rings[selectedRingIndex].segments[selectedSegmentIndex];

        int currentEnemyWayPointIndex = (int)segment.waypointType;

        int newEnemyWayPointIndex = EditorGUILayout.Popup(currentEnemyWayPointIndex, enemyWayPointOptions);
        if (newEnemyWayPointIndex != currentEnemyWayPointIndex)
        {
            segment.waypointType = (WaypointType)newEnemyWayPointIndex;
        }
    }


    private void BridgeCheckbox()
    {
        Segment segment = selectedLevelData.rings[selectedRingIndex].segments[selectedSegmentIndex];
        segment.hasBridge = EditorGUILayout.Toggle(segment.hasBridge);
    }


    private void BuildPreviewButton()
    {
        if (GUILayout.Button("Build Preview"))
        {

        }
    }


    private void ClearPreviewButton()
    {
        if (GUILayout.Button("Clear Preview"))
        {

        }
    }


    private void DrawSection(string title)
    {
        // Create box style
        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.padding = new RectOffset(3, 3, 3, 3);

        // Set custom background color
        boxStyle.normal.background = EditorGUIUtility.whiteTexture;
        GUI.color = new Color(0.24f, 0.24f, 0.24f); // Dark gray with transparency

        // Begin vertical layout with box
        EditorGUILayout.BeginVertical(boxStyle);
        GUI.color = Color.white;
        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();

        // Draw border after the content
        Rect lastRect = GUILayoutUtility.GetLastRect();
        Handles.BeginGUI();
        Handles.DrawSolidRectangleWithOutline(lastRect, Color.clear, new Color(0.19f, 0.19f, 0.19f)); // Red border
        Handles.EndGUI();
    }



    private void DrawArea(System.Action content, float width = 0, RectOffset padding = null)
    {
        GUIStyle style = new GUIStyle();
        style.padding = padding ?? new RectOffset(15, 15, 15, 15);

        if (width > 0)
        {
            EditorGUILayout.BeginVertical(style, GUILayout.Width(width));
        }
        else
        {
            EditorGUILayout.BeginVertical(style);
        }
        content?.Invoke();
        GUILayout.Space(10);
        EditorGUILayout.EndVertical();
    }
}