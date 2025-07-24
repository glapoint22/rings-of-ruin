using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    // Constants for UI layout and game configuration
    private const int SEGMENT_COUNT = 24;
    private const string LEVEL_DATA_PREF_KEY = "RingsOfRuin_LastLevelDataPath";
    private const float RING_RADIUS = 350;
    private const float BUTTON_SIZE = 80f;
    private const int MAX_RINGS = 4;
    private const int MIN_WINDOW_WIDTH = 750;
    private const int MIN_WINDOW_HEIGHT = 1150;

    // State variables for the editor
    private List<LevelDataOld> allLevels = new List<LevelDataOld>();
    private LevelPool levelPool;
    private SegmentIconLibrary segmentIconLibrary;
    private LevelDataOld selectedLevelData;
    private int selectedLevelIndex = 0;
    private int selectedRingIndex = 0;
    private int selectedSegmentIndex = -1;



    [MenuItem("Window/Rings of Ruin/Level Editor Old")]
    public static void ShowWindow()
    {
        LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level Editor Old");
        window.minSize = new Vector2(MIN_WINDOW_WIDTH, MIN_WINDOW_HEIGHT);
    }



    private void OnEnable()
    {
        LoadAllLevels();
    }



    private void OnGUI()
    {
        LoadLevelPool();
        DrawHeader();
        
        // Add padding around the entire top section
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20); // Left padding
        
        EditorGUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        DrawLevelSelection();
        DrawLevelControls();
        GUILayout.EndHorizontal();
        
        if (selectedLevelData == null && allLevels.Count > 0)
        {
            EditorGUILayout.HelpBox("Select or create a LevelData asset to begin.", MessageType.Info);
            EditorGUILayout.EndVertical();
            GUILayout.Space(20); // Right padding
            EditorGUILayout.EndHorizontal();
            return;
        }

        if (selectedLevelData != null)
        {
            DrawGlobalHazardSettings();
            DrawRingControls();
            EditorGUILayout.EndVertical();
            
            GUILayout.Space(20); // Right padding
            EditorGUILayout.EndHorizontal();
            
            DrawRingLayout();
            GUILayout.Space(190);
            DrawSegmentDetails();
            DrawPreviewControls();
        }
    }

    #region Level Management
    // Handles loading, saving, and managing level data assets
    private void LoadAllLevels()
    {
        allLevels = AssetDatabase.FindAssets("t:LevelData")
            .Select(guid => AssetDatabase.LoadAssetAtPath<LevelDataOld>(AssetDatabase.GUIDToAssetPath(guid)))
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
        // GUILayout.Label("Rings of Ruin – Level Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);
    }

    private void DrawLevelSelection()
    {
        if (allLevels.Count == 0)
        {
            EditorGUILayout.HelpBox("No LevelData assets found. Click 'Create New Level' to get started.", MessageType.Info);
            return;
        }

        string[] levelNames = allLevels.Select(ld => $"Level {ld.levelID}").ToArray();
        int newIndex = EditorGUILayout.Popup("Selected Level", selectedLevelIndex, levelNames);

        if (newIndex != selectedLevelIndex)
        {
            selectedLevelIndex = newIndex;
            selectedLevelData = allLevels[selectedLevelIndex];
            SaveSelectedLevelPath();
        }
    }

    private void DrawLevelControls()
    {
        if (GUILayout.Button("➕ Add Level", GUILayout.Width(100)))
        {
            CreateNewLevel();
        }

        GUI.enabled = selectedLevelData != null;
        if (GUILayout.Button("🗑️ Delete Level", GUILayout.Width(100)))
        {
            DeleteSelectedLevel();
        }
        GUI.enabled = true;
    }

    private void DrawRingControls()
    {
        GUILayout.BeginHorizontal();

        string[] ringLabels = selectedLevelData.rings.Select((r, i) => $"Ring {i}").ToArray();
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
        EditorGUILayout.Space(30);

        EnsureRingSegmentListExists();
    }

    private void DrawPreviewControls()
    {
        EditorGUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20); // Left padding
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("🛠 Build Preview")) BuildPreview();
        if (GUILayout.Button("🧹 Clear Preview")) ClearPreview();
        GUILayout.EndHorizontal();
        
        GUILayout.Space(20); // Right padding
        EditorGUILayout.EndHorizontal();
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




    private LevelDataOld CreateLevelAsset(int levelId)
    {
        var newLevel = ScriptableObject.CreateInstance<LevelDataOld>();
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
        if (EditorUtility.DisplayDialog("Delete Ring", $"Delete Ring {selectedRingIndex}?", "Delete", "Cancel"))
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
        // if (segment.segmentType != SegmentType.Normal)
        // {
        //     Sprite typeIcon = segmentIconLibrary.GetSegmentTypeIcon(segment.segmentType);
        //     if (typeIcon != null)
        //     {
        //         // Draw the icon at full button size
        //         GUI.color = Color.white;
        //         GUI.DrawTexture(buttonRect, typeIcon.texture, ScaleMode.ScaleToFit);
        //     }
        // }

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

        // Get all icons for this segment
        var icons = GetSegmentIcons(segment);

        // Draw icons based on count
        if (icons.Count > 0)
        {
            DrawMultipleIcons(buttonRect, icons, numberHeight + topPadding);
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
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20); // Left padding
        
        EditorGUILayout.BeginVertical();
        
        segment.segmentType = (SegmentType)EditorGUILayout.EnumPopup("Segment Type", segment.segmentType);
        EditorGUILayout.Space(10);

        EditorGUI.BeginDisabledGroup(segment.segmentType != SegmentType.Normal);

        

        EditorGUI.indentLevel += 2;
        // Find if any other segment in the level is set as player start
        bool otherSegmentHasPlayerStart = selectedLevelData.rings
            .SelectMany((r, ri) => r.segments.Select((s, si) => new { Segment = s, RingIdx = ri, SegIdx = si }))
            .Any(x => x.Segment.isPlayerStart && (x.RingIdx != selectedRingIndex || x.SegIdx != segment.segmentIndex));

        // Only enable the Player checkbox if either none are set, or this is the one set
        bool shouldDisablePlayer = (otherSegmentHasPlayerStart && !segment.isPlayerStart) || HasAnyOtherOccupyingOption(segment, excludePlayer: segment.isPlayerStart);
        EditorGUI.BeginDisabledGroup(shouldDisablePlayer);
        bool newIsPlayerStart = EditorGUILayout.Toggle("Player", segment.isPlayerStart);
        EditorGUI.EndDisabledGroup();

        // If the value changed and is now checked, uncheck all others
        if (newIsPlayerStart != segment.isPlayerStart)
        {
            if (newIsPlayerStart)
            {
                // Uncheck all others
                foreach (var ring in selectedLevelData.rings)
                {
                    foreach (var seg in ring.segments)
                    {
                        seg.isPlayerStart = false;
                    }
                }
                segment.isPlayerStart = true;
            }
            else
            {
                segment.isPlayerStart = false;
            }
        }

        // Disable key checkbox if ANY other field is selected
        bool anyOtherFieldSelectedForKey = HasAnyOtherOccupyingOption(segment, excludeKey: segment.hasKey);
        EditorGUI.BeginDisabledGroup(anyOtherFieldSelectedForKey);
        EditorGUILayout.Space(1);
        segment.hasKey = EditorGUILayout.Toggle("Key", segment.hasKey);
        EditorGUI.EndDisabledGroup();

        // Disable health checkbox if ANY other field is selected
        bool anyOtherFieldSelectedForHealth = HasAnyOtherOccupyingOption(segment, excludeHealth: segment.hasHealth);
        EditorGUI.BeginDisabledGroup(anyOtherFieldSelectedForHealth);
        segment.hasHealth = EditorGUILayout.Toggle("Health", segment.hasHealth);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space(1);

        // Disable collectible if ANY other field is selected
        bool anyOtherFieldSelected = HasAnyOtherOccupyingOption(segment, excludeCollectible: segment.collectibleType);
        EditorGUI.BeginDisabledGroup(anyOtherFieldSelected);
        segment.collectibleType = (CollectibleType)EditorGUILayout.EnumPopup("Collectible", segment.collectibleType);

        // Add the coin count field when treasure chest is selected
        if (segment.collectibleType == CollectibleType.TreasureChest)
        {
            EditorGUI.indentLevel++;
            segment.treasureChestCoinCount = EditorGUILayout.IntField("Coin Count", segment.treasureChestCoinCount);
            EditorGUI.indentLevel--;
        }
        EditorGUI.EndDisabledGroup();

        // Disable spell type if ANY other field is selected
        bool anyOtherFieldSelectedForSpell = HasAnyOtherOccupyingOption(segment, excludeSpell: segment.spellType);
        EditorGUI.BeginDisabledGroup(anyOtherFieldSelectedForSpell);
        segment.spellType = (SpellType)EditorGUILayout.EnumPopup("Spell", segment.spellType);
        EditorGUI.EndDisabledGroup();


        EditorGUI.indentLevel -= 2;
        EditorGUILayout.Space(30);

        

        EditorGUI.indentLevel += 2;
        // Bridge checkbox - always enabled since it's non-occupying
        segment.hasBridge = EditorGUILayout.Toggle("Bridge", segment.hasBridge);
        EditorGUILayout.Space(1);

        // Enemy waypoint dropdown - always enabled since it's non-occupying
        segment.enemyType = (EnemySpawnType)EditorGUILayout.EnumPopup("Enemy Waypoint", segment.enemyType);
        EditorGUI.indentLevel -= 2;

        EditorGUI.EndDisabledGroup(); // End the segment type disabled group
        
        EditorGUILayout.EndVertical();
        
        GUILayout.Space(20); // Right padding
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(20);
    }

    #endregion

    #region Preview Management

    private void BuildPreview()
    {
        ClearPreview();

        if (selectedLevelData == null || levelPool == null)
        {
            Debug.LogWarning("Missing level data or multi prefab pool.");
            return;
        }

        Transform previewRoot = GetOrCreatePreviewRoot();

        // Ensure all four ring roots exist under the preview root
        for (int i = 0; i < 4; i++)
        {
            string ringName = $"Ring_{i}";
            Transform ringRoot = previewRoot.Find(ringName);
            if (ringRoot == null)
            {
                GameObject ringGO = new GameObject(ringName);
                ringGO.transform.SetParent(previewRoot);
            }
        }

        var tempGO = new GameObject("EditorPreviewBuilder_TEMP");
        var builder = tempGO.AddComponent<LevelBuilderOld>();

        // Inject multi prefab pool
        var so = new SerializedObject(builder);
        var poolProp = so.FindProperty("levelPool");
        poolProp.objectReferenceValue = levelPool;

        // Set the level root
        var levelRootProp = so.FindProperty("levelRoot");
        levelRootProp.objectReferenceValue = previewRoot;

        so.ApplyModifiedProperties();

        // Initialize the pool manually (since Start() won't be called in editor)
        levelPool.Initialize(previewRoot);

        builder.BuildLevel(selectedLevelData);

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
    private void LoadLevelPool()
    {
        if (levelPool != null && segmentIconLibrary != null) return;

        string[] guids = AssetDatabase.FindAssets("t:LevelPool");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            levelPool = AssetDatabase.LoadAssetAtPath<LevelPool>(path);
        }

        guids = AssetDatabase.FindAssets("t:SegmentIconLibrary");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            segmentIconLibrary = AssetDatabase.LoadAssetAtPath<SegmentIconLibrary>(path);
        }

        if (levelPool == null)
        {
            Debug.LogWarning("No LevelPool asset found. Please create one via 'Create > Rings of Ruin > Multi Prefab Pool'.");
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

    private bool HasAnyOtherOccupyingOption(SegmentConfiguration segment, CollectibleType excludeCollectible = CollectibleType.None, SpellType excludeSpell = SpellType.None, bool excludeKey = false, bool excludeHealth = false, bool excludePlayer = false)
    {
        return (segment.collectibleType != CollectibleType.None && segment.collectibleType != excludeCollectible) ||
               (segment.spellType != SpellType.None && segment.spellType != excludeSpell) ||
               (segment.hasKey && !excludeKey) ||
               (segment.hasHealth && !excludeHealth) ||
               (segment.isPlayerStart && !excludePlayer);
    }

    private List<Sprite> GetSegmentIcons(SegmentConfiguration segment)
    {
        var icons = new List<Sprite>();

        // If it's not a normal segment, only show the segment type icon
        if (segment.segmentType != SegmentType.Normal)
        {
            // var typeIcon = segmentIconLibrary.GetSegmentTypeIcon(segment.segmentType);
            // if (typeIcon != null) icons.Add(typeIcon);
            return icons;
        }

        // For normal segments, collect all option icons
        // Collectibles
        if (segment.collectibleType != CollectibleType.None)
        {
            // var icon = segmentIconLibrary.GetCollectibleIcon(segment.collectibleType);
            // if (icon != null) icons.Add(icon);
        }

        // Spells
        if (segment.spellType != SpellType.None)
        {
            // var icon = segmentIconLibrary.GetSpellIcon(segment.spellType);
            // if (icon != null) icons.Add(icon);
        }

        

        // Enemies
        if (segment.enemyType != EnemySpawnType.None)
        {
            var icon = segmentIconLibrary.GetEnemySpawnTypeIcon(segment.enemyType);
            if (icon != null) icons.Add(icon);
        }

        // Utility Items
        if (segment.hasKey)
        {
            // var icon = segmentIconLibrary.GetUtilityItemIcon(UtilityItem.Key);
            // if (icon != null) icons.Add(icon);
        }
        if (segment.hasHealth)
        {
            // var icon = segmentIconLibrary.GetUtilityItemIcon(UtilityItem.Health);
            // if (icon != null) icons.Add(icon);
        }
        if (segment.isPlayerStart)
        {
            // var icon = segmentIconLibrary.GetUtilityItemIcon(UtilityItem.Player);
            // if (icon != null) icons.Add(icon);
        }
        if (segment.hasBridge)
        {
            // var icon = segmentIconLibrary.GetUtilityItemIcon(UtilityItem.Bridge);
            // if (icon != null) icons.Add(icon);
        }

        return icons;
    }

    private void DrawMultipleIcons(Rect buttonRect, List<Sprite> icons, float yOffset)
    {
        float iconSize = 20f;
        float spacing = 8f;
        float extraSpacing = 9f; // Additional spacing for 1 and 2 icons
        float triangleAdjustment = -5f; // Move triangle formation up by 5px
        
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