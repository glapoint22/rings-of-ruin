using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class LevelEditor : EditorWindow
{
    private const int SEGMENT_COUNT = 24;
    private const float RING_RADIUS = 350;
    private const float BUTTON_SIZE = 80f;
    private const int MIN_WINDOW_WIDTH = 750;
    private const int MIN_WINDOW_HEIGHT = 1150;
    private const float SEGMENT_BUTTON_TOP_PADDING = 5f;
     private const float SEGMENT_BUTTON_NUMBER_WIDTH = 30f;
    private const float SEGMENT_BUTTON_NUMBER_HEIGHT = 15f;
    private int selectedLevelIndex = 0;
    private int selectedRingIndex = 0;
    private int selectedSegmentIndex = 0;
    // private Segment selectedSegment;
    private LevelData selectedLevelData;
    private bool hasLevels = false;
    private LevelPool levelPool;
    private SegmentIconLibrary segmentIconLibrary;
    private Segment selectedSegment;


    [MenuItem("Window/Rings of Ruin/Level Editor")]
    public static void ShowWindow()
    {
        LevelEditor window = GetWindow<LevelEditor>("Level Editor");
        window.minSize = new Vector2(MIN_WINDOW_WIDTH, MIN_WINDOW_HEIGHT);
    }

    private void OnEnable()
    {
        LoadSelectedLevel();
        LoadLevelPool();
        LoadSegmentIconLibrary();
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
            Divider();
            PreviewButtons();
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


    private void LoadLevelPool()
    {
        string[] guids = AssetDatabase.FindAssets("t:LevelPool");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            levelPool = AssetDatabase.LoadAssetAtPath<LevelPool>(path);
        }
    }

    private void LoadSegmentIconLibrary()
    {
        string[] guids = AssetDatabase.FindAssets("t:SegmentIconLibrary");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            segmentIconLibrary = AssetDatabase.LoadAssetAtPath<SegmentIconLibrary>(path);
        }
    }

    private void CreateLevel()
    {
        // Create a centered vertical layout
        EditorGUILayout.BeginVertical();

        // Add flexible space to push content to center
        GUILayout.FlexibleSpace();

        // Center the content horizontally
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        // Create the centered content with a fixed width for better alignment
        EditorGUILayout.BeginVertical(GUILayout.Width(350));

        // Display the message
        GUIStyle messageStyle = new GUIStyle(EditorStyles.label);
        messageStyle.alignment = TextAnchor.MiddleCenter;
        messageStyle.fontSize = 16;
        messageStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
        messageStyle.wordWrap = true; // Enable word wrapping

        EditorGUILayout.LabelField("No levels have been created yet. Click the button below to add a level.", messageStyle);
        EditorGUILayout.Space(20);

        // Center the button horizontally within its container
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        AddLevelButton();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        // Add flexible space to push content to center
        GUILayout.FlexibleSpace();

        EditorGUILayout.EndVertical();
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
            EditorGUI.BeginChangeCheck();
            selectedLevelData.hasRuneFlares = EditorGUILayout.Toggle(selectedLevelData.hasRuneFlares);
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = selectedLevelData.hasRuneFlares;
            selectedLevelData.minRuneFlaresSpawnInterval = EditorGUILayout.FloatField(selectedLevelData.minRuneFlaresSpawnInterval, GUILayout.Width(144));
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Max Spawn Interval", EditorStyles.label, GUILayout.Width(123));
            selectedLevelData.maxRuneFlaresSpawnInterval = EditorGUILayout.FloatField(selectedLevelData.maxRuneFlaresSpawnInterval, GUILayout.Width(144));
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(selectedLevelData);
            }
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
            EditorGUILayout.LabelField("Enemy Waypoint", EditorStyles.label, GUILayout.Width(110));

        }, 1, new RectOffset(40, 46, 0, 0));

        DrawArea(() =>
        {
            SegmentTypePopup();
            BridgeCheckbox();
            SpawnTypePopup();
            EnemyWaypointPopup();
        }, 0, new RectOffset(0, 6, 0, 0));
    }


    private void Divider()
    {
        EditorGUILayout.EndHorizontal();
        GUI.color = new Color(0.48f, 0.48f, 0.48f);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUI.color = Color.white;
    }


    private void PreviewButtons()
    {
        EditorGUILayout.Space(90);
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
            // Defer the deletion to the next frame to avoid GUI layout issues
            EditorApplication.delayCall += DeleteSelectedLevel;
        }
        GUI.enabled = true;
    }


    private void DeleteSelectedLevel()
    {
        if (selectedLevelData == null) return;

        // Get the path of the asset to delete
        string assetPath = AssetDatabase.GetAssetPath(selectedLevelData);

        // Delete the asset file
        AssetDatabase.DeleteAsset(assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Clear the selected level data
        selectedLevelData = null;
        selectedLevelIndex = 0;
        selectedRingIndex = 0;
        selectedSegmentIndex = 0;

        // Clear the saved selection
        EditorPrefs.DeleteKey("RingsOfRuin_SelectedLevel");

        // Check if there are any remaining levels
        string[] remainingLevels = GetLevelOptions();
        if (remainingLevels.Length > 0)
        {
            // Select the first available level (or adjust index if needed)
            string folderPath = "Assets/Levels";
            string[] guids = AssetDatabase.FindAssets("t:LevelData", new[] { folderPath });
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                selectedLevelData = AssetDatabase.LoadAssetAtPath<LevelData>(path);
                selectedLevelIndex = 0;
                SaveSelectedLevel();
            }
        }
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
        EditorUtility.SetDirty(selectedLevelData);
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
        var newSelectedRingIndex = EditorGUILayout.Popup(selectedRingIndex, ringOptions);
        if (newSelectedRingIndex != selectedRingIndex)
        {
            selectedRingIndex = newSelectedRingIndex;
            selectedSegmentIndex = 0;
            EditorUtility.SetDirty(selectedLevelData);
        }
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
        selectedRingIndex = selectedLevelData.rings.Count - 1;

        for (int i = 0; i < SEGMENT_COUNT; i++)
        {
            ring.segments.Add(new Segment { ringIndex = ringIndex, segmentIndex = i, ringSegmentType = GetRingSegmentType(0) });
        }
        EditorUtility.SetDirty(selectedLevelData);
    }


    private void DeleteRing(int ringIndex)
    {
        selectedLevelData.rings.RemoveAt(ringIndex);
        selectedRingIndex = selectedLevelData.rings.Count - 1;
        selectedSegmentIndex = 0;
        EditorUtility.SetDirty(selectedLevelData);
    }


    private void RingSegments()
    {
        Rect layoutRect = GUILayoutUtility.GetRect(position.width, 800f);
        Handles.BeginGUI();
        for (int i = 0; i < SEGMENT_COUNT; i++)
        {
            SegmentButton(i, layoutRect);
        }
        Handles.EndGUI();
    }


    private void SegmentButton(int segmentIndex, Rect layoutRect)
    {
        Rect buttonRect = GetSegmentButtonRect(segmentIndex, layoutRect);
        if (GUI.Button(buttonRect, ""))
        {
            selectedSegmentIndex = segmentIndex;
            selectedSegment = selectedLevelData.rings[selectedRingIndex].segments[selectedSegmentIndex];
        }
        SetSegmentButtonContent(segmentIndex, buttonRect);
        SetSelectedSegmentButton(segmentIndex, buttonRect);
    }


    private Rect GetSegmentButtonRect(int segmentIndex, Rect layoutRect)
    {
        Vector2 ringCenter = new Vector2(layoutRect.x + position.width / 2f, layoutRect.y + layoutRect.height / 2f);
        float angle = segmentIndex * Mathf.PI * 2f / SEGMENT_COUNT - Mathf.PI / 2f;
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * RING_RADIUS;
        Vector2 buttonCenter = ringCenter + offset;
        return new Rect(buttonCenter.x - BUTTON_SIZE / 2, buttonCenter.y - BUTTON_SIZE / 2, BUTTON_SIZE, BUTTON_SIZE);
    }


    private void SetSelectedSegmentButton(int segmentIndex, Rect buttonRect)
    {
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
            GUI.color = Color.white;
        }
    }


    private void SetSegmentButtonContent(int segmentIndex, Rect buttonRect)
    {
        SetSegmentButtonIcon(buttonRect, segmentIndex);
        SetSegmentButtonNumber(segmentIndex, buttonRect);
    }


    private void SetSegmentButtonNumber(int segmentIndex, Rect buttonRect)
    {
        Rect numberRect = new Rect(buttonRect.x + (buttonRect.width - SEGMENT_BUTTON_NUMBER_WIDTH) / 2, buttonRect.y + SEGMENT_BUTTON_TOP_PADDING, SEGMENT_BUTTON_NUMBER_WIDTH, SEGMENT_BUTTON_NUMBER_HEIGHT);

        GUI.Label(numberRect, segmentIndex.ToString(), new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Color.white }
        });
        GUI.color = Color.white;
    }


    private void SetSegmentButtonIcon(Rect buttonRect, int segmentIndex)
    {
        SetSegmentTypeIcon(buttonRect, segmentIndex);
        SetSpawnTypeIcon(buttonRect, segmentIndex);
    }


    private void SetSegmentTypeIcon(Rect buttonRect, int segmentIndex)
    {
        Segment segment = selectedLevelData.rings[selectedRingIndex].segments[segmentIndex];
        Sprite segmentTypeIcon = segmentIconLibrary.GetSegmentTypeIcon(GetSegmentTypeIndex(segment.ringSegmentType));
        if (segmentTypeIcon != null)
        {
            GUI.DrawTexture(buttonRect, segmentTypeIcon.texture, ScaleMode.ScaleToFit);
        }
    }


    private void SetSpawnTypeIcon(Rect buttonRect, int segmentIndex)
    {
        List<Sprite> icons = GetSegmentIconList(segmentIndex);
        if (icons.Count > 0) DrawMultipleIcons(buttonRect, icons);
    }


    private List<Sprite> GetSegmentIconList(int segmentIndex)
    {
        var icons = new List<Sprite>();
        Segment segment = selectedLevelData.rings[selectedRingIndex].segments[segmentIndex];

        if (segment.spawnType != SpawnType.None)
        {
            var icon = segmentIconLibrary.GetSpawnTypeIcon(segment.spawnType);
            if (icon != null) icons.Add(icon);
        }

        if (segment.waypointType != WaypointType.None)
        {
            var icon = segmentIconLibrary.GetWaypointTypeIcon(segment.waypointType);
            if (icon != null) icons.Add(icon);
        }

        if (segment.hasBridge)
        {
            var icon = segmentIconLibrary.bridgeIcon;
            if (icon != null) icons.Add(icon);
        }
        return icons;
    }


    private void DrawMultipleIcons(Rect buttonRect, List<Sprite> icons)
    {
        float iconSize = 20f;
        float spacing = 8f;
        float extraSpacing = 9f; // Additional spacing for 1 and 2 icons
        float triangleAdjustment = -5f; // Move triangle formation up by 5px
        float yOffset = SEGMENT_BUTTON_NUMBER_HEIGHT + SEGMENT_BUTTON_TOP_PADDING;

        switch (icons.Count)
        {
            case 1:
                // Single icon centered, moved down 9px
                GUI.DrawTexture(
                    new Rect(
                        buttonRect.x + (buttonRect.width - iconSize) / 2,
                        buttonRect.y + yOffset + spacing + extraSpacing,
                        iconSize,
                        iconSize
                    ),
                    icons[0].texture
                );
                break;

            case 2:
                // Two icons side by side, moved down 9px
                float twoIconWidth = iconSize * 2 + spacing;
                float startX = buttonRect.x + (buttonRect.width - twoIconWidth) / 2;

                GUI.DrawTexture(
                    new Rect(startX, buttonRect.y + yOffset + spacing + extraSpacing, iconSize, iconSize),
                    icons[0].texture
                );
                GUI.DrawTexture(
                    new Rect(startX + iconSize + spacing, buttonRect.y + yOffset + spacing + extraSpacing, iconSize, iconSize),
                    icons[1].texture
                );
                break;

            case 3:
                // Triangle formation: 1 on top, 2 below, moved up 5px
                float topIconX = buttonRect.x + (buttonRect.width - iconSize) / 2;
                float bottomStartX = buttonRect.x + (buttonRect.width - (iconSize * 2 + spacing)) / 2;

                // Top icon - moved down 2px
                GUI.DrawTexture(
                    new Rect(topIconX, buttonRect.y + yOffset + spacing + triangleAdjustment + 2f, iconSize, iconSize),
                    icons[0].texture
                );

                // Bottom two icons - moved up 2px
                GUI.DrawTexture(
                    new Rect(bottomStartX, buttonRect.y + yOffset + spacing * 2 + iconSize + triangleAdjustment - 2f, iconSize, iconSize),
                    icons[1].texture
                );
                GUI.DrawTexture(
                    new Rect(bottomStartX + iconSize + spacing, buttonRect.y + yOffset + spacing * 2 + iconSize + triangleAdjustment - 2f, iconSize, iconSize),
                    icons[2].texture
                );
                break;
        }
    }


    private void SegmentTypePopup()
    {
        EditorGUI.BeginChangeCheck();
        int currentSegmentTypeIndex = GetSegmentTypeIndex(selectedSegment.ringSegmentType);
        int newSegmentTypeIndex = EditorGUILayout.Popup(currentSegmentTypeIndex, new string[] { "Normal", "Gap", "Crumbling", "Spike" });

        if (newSegmentTypeIndex != currentSegmentTypeIndex)
        {
            selectedSegment.ringSegmentType = GetRingSegmentType(newSegmentTypeIndex);
            EditorUtility.SetDirty(selectedLevelData);
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
        int currentSpawnTypeIndex = (int)selectedSegment.spawnType;
        int newSpawnTypeIndex = EditorGUILayout.Popup(currentSpawnTypeIndex, spawnItemOptions);

        if (newSpawnTypeIndex != currentSpawnTypeIndex)
        {
            selectedSegment.spawnType = (SpawnType)newSpawnTypeIndex;
            EditorUtility.SetDirty(selectedLevelData);
        }
    }


    private void EnemyWaypointPopup()
    {
        string[] enemyWayPointOptions = System.Enum.GetNames(typeof(WaypointType));
        int currentEnemyWayPointIndex = (int)selectedSegment.waypointType;
        int newEnemyWayPointIndex = EditorGUILayout.Popup(currentEnemyWayPointIndex, enemyWayPointOptions);

        if (newEnemyWayPointIndex != currentEnemyWayPointIndex)
        {
            selectedSegment.waypointType = (WaypointType)newEnemyWayPointIndex;
            EditorUtility.SetDirty(selectedLevelData);
        }
    }


    private void BridgeCheckbox()
    {
        EditorGUI.BeginChangeCheck();
        selectedSegment.hasBridge = EditorGUILayout.Toggle(selectedSegment.hasBridge);
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(selectedLevelData);
        }
    }


    private void BuildPreviewButton()
    {
        if (GUILayout.Button("Build Preview", GUILayout.Width(100)))
        {

        }
    }


    private void ClearPreviewButton()
    {
        if (GUILayout.Button("Clear Preview", GUILayout.Width(100)))
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
        GUI.color = new Color(0.27f, 0.27f, 0.27f); // Dark gray with transparency

        // Begin vertical layout with box
        EditorGUILayout.BeginVertical(boxStyle);
        GUI.color = Color.white;
        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();

        // Draw border after the content
        Rect lastRect = GUILayoutUtility.GetLastRect();
        Handles.BeginGUI();
        Handles.DrawSolidRectangleWithOutline(lastRect, Color.clear, new Color(0.2f, 0.2f, 0.2f)); // Red border
        Handles.EndGUI();
        EditorGUILayout.Space(7);
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
        GUILayout.Space(10);
    }
}