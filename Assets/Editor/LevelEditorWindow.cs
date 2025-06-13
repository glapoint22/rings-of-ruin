using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom editor window for creating and editing levels in Rings of Ruin.
/// Provides a visual interface for configuring rings, segments, and their properties.
/// </summary>
public class LevelEditorWindow : EditorWindow
{
    // Constants for UI layout and game configuration
    private const int SEGMENT_COUNT = 24;
    private const string LEVEL_DATA_PREF_KEY = "RingsOfRuin_LastLevelDataPath";
    private const float RING_RADIUS = 300f;
    private const float BUTTON_SIZE = 60f;
    private const int MAX_RINGS = 4;
    private const int MIN_WINDOW_WIDTH = 750;
    private const int MIN_WINDOW_HEIGHT = 1150;

    // State variables for the editor
    private List<LevelData> allLevels = new List<LevelData>();
    private LevelPrefabLibrary prefabLibrary;
    private SegmentIconLibrary segmentIconLibrary;
    private LevelData selectedLevelData;
    private int selectedLevelIndex = 0;
    private int selectedRingIndex = 0;
    private int selectedSegmentIndex = -1;



    [MenuItem("Window/Rings of Ruin/Level Editor")]
    public static void ShowWindow()
    {
        LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level Editor");
        window.minSize = new Vector2(MIN_WINDOW_WIDTH, MIN_WINDOW_HEIGHT);
    }



    private void OnEnable()
    {
        LoadAllLevels();
    }



    private void OnGUI()
    {
        LoadPrefabLibrary();
        DrawHeader();
        DrawLevelSelection();
        DrawLevelControls();
        
        if (selectedLevelData == null && allLevels.Count > 0)
        {
            EditorGUILayout.HelpBox("Select or create a LevelData asset to begin.", MessageType.Info);
            return;
        }

        if (selectedLevelData != null)
        {
            DrawAltarSettings();
            DrawGlobalHazardSettings();
            DrawRingControls();
            DrawRingLayout();
            GUILayout.Space(150);
            DrawSegmentDetails();
            DrawPreviewControls();
            
        }
    }

    #region Level Management
    // Handles loading, saving, and managing level data assets
    private void LoadAllLevels()
    {
        allLevels = AssetDatabase.FindAssets("t:LevelData")
            .Select(guid => AssetDatabase.LoadAssetAtPath<LevelData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(ld => ld != null)
            .OrderBy(ld => ld.levelID)
            .ToList();

        RestoreSelectedLevel();
    }

    private void RestoreSelectedLevel()
    {
        if (selectedLevelData == null && allLevels.Count > 0)
        {
            selectedLevelIndex = 0;
            selectedLevelData = allLevels[0];
            SaveSelectedLevelPath();
        }
        else
        {
            string path = EditorPrefs.GetString(LEVEL_DATA_PREF_KEY, string.Empty);
            if (!string.IsNullOrEmpty(path))
            {
                var match = allLevels.FirstOrDefault(ld => AssetDatabase.GetAssetPath(ld) == path);
                if (match != null)
                {
                    selectedLevelData = match;
                    selectedLevelIndex = allLevels.IndexOf(match);
                }
            }
        }
    }

    private void SaveSelectedLevelPath()
    {
        if (selectedLevelData != null)
        {
            string path = AssetDatabase.GetAssetPath(selectedLevelData);
            EditorPrefs.SetString(LEVEL_DATA_PREF_KEY, path);
        }
    }
    #endregion

    #region GUI Drawing
    // Core UI drawing methods for the editor window
    private void DrawHeader()
    {
        GUILayout.Label("Rings of Ruin – Level Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space(20);
    }

    private void DrawLevelSelection()
    {
        GUILayout.Label("Select Level", EditorStyles.boldLabel);

        if (allLevels.Count == 0)
        {
            EditorGUILayout.HelpBox("No LevelData assets found. Click 'Create New Level' to get started.", MessageType.Info);
            return;
        }

        string[] levelNames = allLevels.Select(ld => $"Level {ld.levelID}").ToArray();
        int newIndex = EditorGUILayout.Popup("Select Level", selectedLevelIndex, levelNames);

        if (newIndex != selectedLevelIndex)
        {
            selectedLevelIndex = newIndex;
            selectedLevelData = allLevels[selectedLevelIndex];
            SaveSelectedLevelPath();
        }
    }

    private void DrawLevelControls()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("➕ Create New Level"))
        {
            CreateNewLevel();
        }

        GUI.enabled = selectedLevelData != null;
        if (GUILayout.Button("🗑️ Delete Selected Level"))
        {
            DeleteSelectedLevel();
        }
        GUI.enabled = true;

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }

    private void DrawRingControls()
    {
        GUILayout.BeginHorizontal();

        string[] ringLabels = selectedLevelData.rings.Select((r, i) => $"Ring {i + 1}").ToArray();
        selectedRingIndex = Mathf.Clamp(selectedRingIndex, 0, selectedLevelData.rings.Count - 1);

        int newRingIndex = EditorGUILayout.Popup("Selected Ring", selectedRingIndex, ringLabels);
        if (newRingIndex != selectedRingIndex)
            selectedRingIndex = newRingIndex;

        if (GUILayout.Button("➕ Add Ring", GUILayout.Width(100)))
        {
            AddNewRing();
        }

        GUI.enabled = selectedLevelData.rings.Count > 1;
        if (GUILayout.Button("🗑️ Delete Ring", GUILayout.Width(100)))
        {
            DeleteSelectedRing();
        }
        GUI.enabled = true;

        GUILayout.EndHorizontal();
        EditorGUILayout.Space(10);

        selectedLevelData.rings[selectedRingIndex].rotation =
            (RingRotationDirection)EditorGUILayout.EnumPopup("Ring Rotation", selectedLevelData.rings[selectedRingIndex].rotation);

        EnsureRingSegmentListExists();
    }

    private void DrawPreviewControls()
    {
        EditorGUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("🛠 Build Preview")) BuildPreview();
        if (GUILayout.Button("🧹 Clear Preview")) ClearPreview();
        GUILayout.EndHorizontal();
    }
    #endregion

    #region Level Operations
    // Methods for creating, deleting, and modifying level data
    private void CreateNewLevel()
    {
        string folderPath = "Assets/Levels";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "Levels");
        }

        int nextID = AssetDatabase.FindAssets("t:LevelData", new[] { folderPath }).Length + 1;
        string assetName = $"Level_{nextID:D2}.asset";
        string fullPath = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{assetName}");

        var newLevel = CreateLevelAsset(nextID);
        AssetDatabase.CreateAsset(newLevel, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        selectedLevelData = newLevel;
        selectedRingIndex = 0;
        SaveSelectedLevelPath();

        LoadAllLevels();
        selectedLevelIndex = allLevels.IndexOf(selectedLevelData);
    }




    private LevelData CreateLevelAsset(int levelId)
    {
        var newLevel = ScriptableObject.CreateInstance<LevelData>();
        newLevel.levelID = levelId;
        var newRing = new RingConfiguration { ringIndex = 0 };
        for (int i = 0; i < SEGMENT_COUNT; i++)
        {
            newRing.segments.Add(new SegmentConfiguration { segmentIndex = i });
        }
        newLevel.rings.Add(newRing);
        return newLevel;
    }

    private void DeleteSelectedLevel()
    {
        if (EditorUtility.DisplayDialog("Delete Level", $"Are you sure you want to delete {selectedLevelData.name}?", "Delete", "Cancel"))
        {
            string path = AssetDatabase.GetAssetPath(selectedLevelData);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            selectedLevelData = null;
            EditorPrefs.DeleteKey(LEVEL_DATA_PREF_KEY);
            LoadAllLevels();
        }
    }

    private void AddNewRing()
    {
        if (selectedLevelData.rings.Count >= MAX_RINGS)
        {
            EditorUtility.DisplayDialog("Maximum Rings Reached", 
                $"Cannot add more rings. Maximum limit of {MAX_RINGS} rings has been reached.", 
                "OK");
            return;
        }

        var newRing = new RingConfiguration
        {
            ringIndex = selectedLevelData.rings.Count
        };

        for (int i = 0; i < SEGMENT_COUNT; i++)
        {
            newRing.segments.Add(new SegmentConfiguration { segmentIndex = i });
        }

        selectedLevelData.rings.Add(newRing);
        selectedRingIndex = selectedLevelData.rings.Count - 1;
        EditorUtility.SetDirty(selectedLevelData);
    }

    private void DeleteSelectedRing()
    {
        if (EditorUtility.DisplayDialog("Delete Ring", $"Delete Ring {selectedRingIndex + 1}?", "Delete", "Cancel"))
        {
            selectedLevelData.rings.RemoveAt(selectedRingIndex);
            selectedRingIndex = Mathf.Clamp(selectedRingIndex - 1, 0, selectedLevelData.rings.Count - 1);
            EditorUtility.SetDirty(selectedLevelData);
        }
    }
    #endregion

    #region Ring and Segment Drawing
    // Visual representation of rings and segments in the editor
    private void DrawRingLayout()
    {
        float viewWidth = position.width;
        Rect layoutRect = GUILayoutUtility.GetRect(viewWidth, 600f);
        Vector2 ringCenter = new Vector2(layoutRect.x + viewWidth / 2f, layoutRect.y + 375f);

        Handles.BeginGUI();

        for (int i = 0; i < SEGMENT_COUNT; i++)
        {
            DrawSegmentButton(i, ringCenter);
        }

        Handles.EndGUI();
    }

    private void DrawSegmentButton(int segmentIndex, Vector2 ringCenter)
    {
        float angle = segmentIndex * Mathf.PI * 2f / SEGMENT_COUNT - Mathf.PI / 2f;
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * RING_RADIUS;
        Vector2 buttonCenter = ringCenter + offset;

        Rect buttonRect = new Rect(buttonCenter.x - BUTTON_SIZE / 2, buttonCenter.y - BUTTON_SIZE / 2, BUTTON_SIZE, BUTTON_SIZE);

        SegmentConfiguration segment = selectedLevelData.rings[selectedRingIndex].segments[segmentIndex];
        
        // Draw the button with white background
        GUI.color = Color.white;
        if (GUI.Button(buttonRect, ""))
        {
            selectedSegmentIndex = segmentIndex;
            EditorUtility.SetDirty(selectedLevelData);
        }

        // Draw segment type icon if not normal
        if (segment.segmentType != SegmentType.Normal)
        {
            Sprite typeIcon = segmentIconLibrary.GetSegmentTypeIcon(segment.segmentType);
            if (typeIcon != null)
            {
                // Draw the icon at full button size
                GUI.color = Color.white;
                GUI.DrawTexture(buttonRect, typeIcon.texture, ScaleMode.ScaleToFit);
            }
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

        // Draw the appropriate icon if it's a normal segment
        if (segment.segmentType == SegmentType.Normal)
        {
            Sprite icon = null;
            
            // Get the appropriate icon based on what's on the segment
            if (segment.collectibleType == CollectibleType.Gem)
                icon = segmentIconLibrary.GetCollectibleIcon(CollectibleType.Gem);
            else if (segment.collectibleType == CollectibleType.Coin)
                icon = segmentIconLibrary.GetCollectibleIcon(CollectibleType.Coin);
            else if (segment.collectibleType == CollectibleType.TreasureChest)
                icon = segmentIconLibrary.GetCollectibleIcon(CollectibleType.TreasureChest);
            else if (segment.portalType == PortalType.PortalA)
                icon = segmentIconLibrary.GetPortalIcon(PortalType.PortalA);
            else if (segment.portalType == PortalType.PortalB)
                icon = segmentIconLibrary.GetPortalIcon(PortalType.PortalB);
            else if (segment.enemyType != EnemyType.None)
                icon = segmentIconLibrary.GetEnemyIcon(segment.enemyType);
            else if (segment.pickupType != PickupType.None)
                icon = segmentIconLibrary.GetPickupIcon(segment.pickupType);
            else if (segment.hasCheckpoint)
                icon = segmentIconLibrary.checkpointIcon;
           

            // Draw the icon if we have one
            if (icon != null)
            {
                float iconSize = 20f;
                float spacing = 16f;
                
                GUI.DrawTexture(
                    new Rect(
                        buttonRect.x + (buttonRect.width - iconSize) / 2,
                        buttonRect.y + numberHeight + spacing,
                        iconSize,
                        iconSize
                    ),
                    icon.texture
                );
            }
        }

        // Draw selection outline
        if (segmentIndex == selectedSegmentIndex)
        {
            float outlineThickness = 2f;
            Rect outlineRect = new Rect(
                buttonRect.x - outlineThickness,
                buttonRect.y - outlineThickness,
                buttonRect.width + (outlineThickness * 2),
                buttonRect.height + (outlineThickness * 2)
            );
            
            Handles.color = Color.yellow;
            Handles.DrawLine(new Vector3(outlineRect.x, outlineRect.y), new Vector3(outlineRect.x + outlineRect.width, outlineRect.y));
            Handles.DrawLine(new Vector3(outlineRect.x + outlineRect.width, outlineRect.y), new Vector3(outlineRect.x + outlineRect.width, outlineRect.y + outlineRect.height));
            Handles.DrawLine(new Vector3(outlineRect.x + outlineRect.width, outlineRect.y + outlineRect.height), new Vector3(outlineRect.x, outlineRect.y + outlineRect.height));
            Handles.DrawLine(new Vector3(outlineRect.x, outlineRect.y + outlineRect.height), new Vector3(outlineRect.x, outlineRect.y));
        }

        GUI.color = Color.white;
    }

    private void DrawSegmentDetails()
    {
        if (selectedSegmentIndex < 0 || selectedRingIndex >= selectedLevelData.rings.Count)
        {
            EditorGUILayout.HelpBox("Click a segment to edit its contents.", MessageType.Info);
            return;
        }

        var segment = selectedLevelData.rings[selectedRingIndex].segments[selectedSegmentIndex];

        EditorGUI.BeginChangeCheck();
        DrawSegmentProperties(segment);
        
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(selectedLevelData);
        }
    }

    private void DrawSegmentProperties(SegmentConfiguration segment)
    {
        segment.segmentType = (SegmentType)EditorGUILayout.EnumPopup("Segment Type", segment.segmentType);
        EditorGUILayout.Space(5);

        EditorGUI.BeginDisabledGroup(segment.segmentType != SegmentType.Normal);

        segment.hasCheckpoint = EditorGUILayout.Toggle("Checkpoint", segment.hasCheckpoint);
        segment.collectibleType = (CollectibleType)EditorGUILayout.EnumPopup("Collectable", segment.collectibleType);
        
        // Add the coin count field when treasure chest is selected
        if (segment.collectibleType == CollectibleType.TreasureChest)
        {
            EditorGUI.indentLevel++;
            segment.treasureChestCoinCount = EditorGUILayout.IntField("Coin Count", segment.treasureChestCoinCount);
            EditorGUI.indentLevel--;
        }
        
        segment.pickupType = (PickupType)EditorGUILayout.EnumPopup("Pickup Type", segment.pickupType);
        segment.portalType = (PortalType)EditorGUILayout.EnumPopup("Portal", segment.portalType);
        segment.enemyType = (EnemyType)EditorGUILayout.EnumPopup("Enemy", segment.enemyType);

        EditorGUI.EndDisabledGroup();
    }

    private void DrawAltarSettings()
    {
        selectedLevelData.isAltarLockedByKey = EditorGUILayout.Toggle("Altar Locked", selectedLevelData.isAltarLockedByKey);
        EditorUtility.SetDirty(selectedLevelData);
    }
    #endregion

    #region Preview Management

    private void BuildPreview()
    {
        ClearPreview();

        if (selectedLevelData == null || prefabLibrary == null)
        {
            Debug.LogWarning("Missing level data or prefab library.");
            return;
        }

        Transform previewRoot = GetOrCreatePreviewRoot();

        var tempGO = new GameObject("EditorPreviewBuilder_TEMP");
        var builder = tempGO.AddComponent<LevelBuilder>();

        // Inject prefab library
        var so = new SerializedObject(builder);
        var libProp = so.FindProperty("prefabLibrary");
        libProp.objectReferenceValue = prefabLibrary;
        so.ApplyModifiedProperties();

        builder.BuildLevel(selectedLevelData, previewRoot);

        Object.DestroyImmediate(tempGO);
    }




    private void DrawGlobalHazardSettings()
    {

        selectedLevelData.hasRuneflareHazard = EditorGUILayout.Toggle("Enable Runeflares", selectedLevelData.hasRuneflareHazard);

        if (selectedLevelData.hasRuneflareHazard)
        {
            EditorGUILayout.LabelField("Runeflare Spawn Interval (sec)", EditorStyles.boldLabel);
            selectedLevelData.runeflareIntervalRange = EditorGUILayout.Vector2Field("Min / Max", selectedLevelData.runeflareIntervalRange);

            selectedLevelData.maxConcurrentRuneflares = EditorGUILayout.IntField("Max Concurrent Runeflares", selectedLevelData.maxConcurrentRuneflares);
        }

        EditorUtility.SetDirty(selectedLevelData);

        EditorGUILayout.Space(10);
    }





    private Transform GetOrCreatePreviewRoot()
    {
        var existing = GameObject.Find("_LevelPreview");
        if (existing != null)
            return existing.transform;

        var root = new GameObject("_LevelPreview");
        return root.transform;
    }

    private void ClearPreview()
    {
        var existing = GameObject.Find("_LevelPreview");
        if (existing != null)
            DestroyImmediate(existing);
    }
    #endregion

    #region Utility Methods
    // Helper methods for common operations
    private void LoadPrefabLibrary()
    {
        if (prefabLibrary != null && segmentIconLibrary != null) return;

        string[] guids = AssetDatabase.FindAssets("t:LevelPrefabLibrary");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            prefabLibrary = AssetDatabase.LoadAssetAtPath<LevelPrefabLibrary>(path);
        }

        guids = AssetDatabase.FindAssets("t:SegmentIconLibrary");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            segmentIconLibrary = AssetDatabase.LoadAssetAtPath<SegmentIconLibrary>(path);
        }

        if (prefabLibrary == null)
        {
            Debug.LogWarning("No LevelPrefabLibrary asset found. Please create one via 'Create > Rings of Ruin > Prefab Library'.");
        }

        if (segmentIconLibrary == null)
        {
            Debug.LogWarning("No SegmentIconLibrary asset found. Please create one via 'Create > Rings of Ruin > Segment Icon Library'.");
        }
    }

    private void EnsureRingSegmentListExists()
    {
        if (selectedRingIndex >= selectedLevelData.rings.Count)
            return;

        var ring = selectedLevelData.rings[selectedRingIndex];
        while (ring.segments.Count < SEGMENT_COUNT)
        {
            ring.segments.Add(new SegmentConfiguration
            {
                segmentIndex = ring.segments.Count
            });
        }
    }
    #endregion
}


[InitializeOnLoad]
public static class EditorPreviewCleaner
{
    static EditorPreviewCleaner()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            var previewRoot = GameObject.Find("_LevelPreview");
            if (previewRoot != null)
            {
                Object.DestroyImmediate(previewRoot);
                Debug.Log("Editor preview cleared before entering Play Mode.");
            }
        }
    }
}