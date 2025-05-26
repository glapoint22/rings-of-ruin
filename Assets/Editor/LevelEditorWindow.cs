using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    private List<LevelData> allLevels = new List<LevelData>();
    private int selectedLevelIndex = 0;

    private LevelPrefabLibrary prefabLibrary;

    private float ringRadius = 400f;
    private float buttonSize = 80f;
    private LevelData selectedLevelData;
    private int selectedRingIndex = 0;
    private int selectedSegmentIndex = -1;
    private const int SegmentCount = 24;
    private const string LevelDataPrefKey = "RingsOfRuin_LastLevelDataPath";

    [MenuItem("Window/Rings of Ruin/Level Editor")]
    public static void ShowWindow()
    {
        LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level Editor");
        window.minSize = new Vector2(1000, 1000);
    }


    private void OnEnable()
    {
        LoadAllLevels();
    }


    private void OnGUI()
    {
        LoadPrefabLibrary();

        GUILayout.Label("Rings of Ruin – Level Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space(20);

        // 🔄 Refresh the level list each time OnGUI runs
        RefreshLevelList();

        GUILayout.Label("Select Level", EditorStyles.boldLabel);

        if (allLevels.Count == 0)
        {
            EditorGUILayout.HelpBox("No LevelData assets found. Click 'Create New Level' to get started.", MessageType.Info);
        }
        else
        {
            string[] levelNames = allLevels.Select(ld => $"Level {ld.levelID}").ToArray();
            int newIndex = EditorGUILayout.Popup("Select Level", selectedLevelIndex, levelNames);

            if (newIndex != selectedLevelIndex)
            {
                selectedLevelIndex = newIndex;
                selectedLevelData = allLevels[selectedLevelIndex];
                string path = AssetDatabase.GetAssetPath(selectedLevelData);
                EditorPrefs.SetString(LevelDataPrefKey, path);
            }
        }

        EditorGUILayout.Space(10);

        // ➕ / 🗑️ Level controls
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("➕ Create New Level"))
        {
            string folderPath = "Assets/Levels";

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets", "Levels");
            }

            int nextID = AssetDatabase.FindAssets("t:LevelData", new[] { folderPath }).Length + 1;
            string assetName = $"Level_{nextID:D2}.asset";
            string fullPath = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{assetName}");

            var newLevel = ScriptableObject.CreateInstance<LevelData>();
            newLevel.levelID = nextID;
            var newRing = new RingConfiguration { ringIndex = 0 };
            for (int i = 0; i < 24; i++)
            {
                newRing.segments.Add(new SegmentConfiguration { segmentIndex = i });
            }
            newLevel.rings.Add(newRing);

            AssetDatabase.CreateAsset(newLevel, fullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            selectedLevelData = newLevel;
            selectedRingIndex = 0;
            EditorPrefs.SetString(LevelDataPrefKey, fullPath);

            RefreshLevelList();
            selectedLevelIndex = allLevels.IndexOf(selectedLevelData);
        }

        GUI.enabled = selectedLevelData != null;
        if (GUILayout.Button("🗑️ Delete Selected Level"))
        {
            if (EditorUtility.DisplayDialog("Delete Level", $"Are you sure you want to delete {selectedLevelData.name}?", "Delete", "Cancel"))
            {
                string path = AssetDatabase.GetAssetPath(selectedLevelData);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
                selectedLevelData = null;
                EditorPrefs.DeleteKey(LevelDataPrefKey);
                RefreshLevelList();
            }
        }
        GUI.enabled = true;

        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        

        if (selectedLevelData == null)
        {
            EditorGUILayout.HelpBox("Select or create a LevelData asset to begin.", MessageType.Info);
            return;
        }

        DrawAltarSettings();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

        string[] ringLabels = selectedLevelData.rings.Select((r, i) => $"Ring {i + 1}").ToArray();
        selectedRingIndex = Mathf.Clamp(selectedRingIndex, 0, selectedLevelData.rings.Count - 1);

        int newRingIndex = EditorGUILayout.Popup("Selected Ring", selectedRingIndex, ringLabels);
        if (newRingIndex != selectedRingIndex)
            selectedRingIndex = newRingIndex;

        if (GUILayout.Button("➕ Add Ring", GUILayout.Width(100)))
        {
            var newRing = new RingConfiguration
            {
                ringIndex = selectedLevelData.rings.Count
            };

            for (int i = 0; i < SegmentCount; i++)
            {
                newRing.segments.Add(new SegmentConfiguration { segmentIndex = i });
            }

            selectedLevelData.rings.Add(newRing);
            selectedRingIndex = selectedLevelData.rings.Count - 1;
            EditorUtility.SetDirty(selectedLevelData);
        }

        GUI.enabled = selectedLevelData.rings.Count > 1;

        if (GUILayout.Button("🗑️ Delete Ring", GUILayout.Width(100)))
        {
            if (EditorUtility.DisplayDialog("Delete Ring", $"Delete Ring {selectedRingIndex + 1}?", "Delete", "Cancel"))
            {
                selectedLevelData.rings.RemoveAt(selectedRingIndex);
                selectedRingIndex = Mathf.Clamp(selectedRingIndex - 1, 0, selectedLevelData.rings.Count - 1);
                EditorUtility.SetDirty(selectedLevelData);
            }
        }

        GUI.enabled = true;
        GUILayout.EndHorizontal();

        EditorGUILayout.Space(10);

        selectedLevelData.rings[selectedRingIndex].rotation =
          (RingRotationDirection)EditorGUILayout.EnumPopup("Ring Rotation", selectedLevelData.rings[selectedRingIndex].rotation);

        EnsureRingSegmentListExists();

        EditorGUILayout.Space(10);
        DrawRingLayout();
        GUILayout.Space(900);
        DrawSegmentDetails();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("🛠 Build Preview")) BuildPreview();
        if (GUILayout.Button("🧹 Clear Preview")) ClearPreview();
        GUILayout.EndHorizontal();
    }



    private void RefreshLevelList()
    {
        allLevels = AssetDatabase.FindAssets("t:LevelData")
            .Select(guid => AssetDatabase.LoadAssetAtPath<LevelData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(ld => ld != null)
            .OrderBy(ld => ld.levelID)
            .ToList();
    }



    private void DrawRingLayout()
    {
        float viewWidth = position.width;
        Vector2 ringCenter = new Vector2(viewWidth / 2f, 650f);

        Handles.BeginGUI();

        for (int i = 0; i < SegmentCount; i++)
        {
            float angle = i * Mathf.PI * 2f / SegmentCount - Mathf.PI / 2f;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * ringRadius;
            Vector2 buttonCenter = ringCenter + offset;

            Rect buttonRect = new Rect(buttonCenter.x - buttonSize / 2, buttonCenter.y - buttonSize / 2, buttonSize, buttonSize);

            SegmentConfiguration segment = selectedLevelData.rings[selectedRingIndex].segments[i]; // <- this must be here

            // Color logic
            switch (segment.segmentType)
            {
                case SegmentType.Gap:
                    GUI.color = Color.red;
                    break;
                case SegmentType.Crumbling:
                    GUI.color = new Color(1f, 0.6f, 0.1f);
                    break;
                default:
                    GUI.color = Color.green;
                    break;
            }

            // Build label with emoji indicators
            string label = i.ToString();
            if (segment.segmentType == SegmentType.Normal)
            {
                if (segment.collectibleType == CollectibleType.Gem)
                    label += "\n💎";
                else if (segment.collectibleType == CollectibleType.Coin)
                    label += "\n🪙";
                if (segment.hazardType != HazardType.None) label += "\n⚠️";
                if (segment.hasPortal) label += "\n⏩";
                if (segment.enemyType != EnemyType.None) label += "\n👾";
                if (segment.pickupType != PickupType.None) label += "\n✨";
                if (segment.hasCheckpoint) label += "\n🚩";
                if (segment.isLocked) label += "\n🔒";
            }

            if (GUI.Button(buttonRect, label))
            {
                selectedSegmentIndex = i;
                EditorUtility.SetDirty(selectedLevelData);
            }

            GUI.color = Color.white;
        }





        Handles.EndGUI();
    }

    private void DrawSegmentDetails()
    {
        if (selectedSegmentIndex < 0 || selectedRingIndex >= selectedLevelData.rings.Count)
        {
            EditorGUILayout.HelpBox("Click a segment to edit its contents.", MessageType.Info);
            return;
        }

        var segment = selectedLevelData.rings[selectedRingIndex].segments[selectedSegmentIndex];

        GUILayout.Label($"Editing Segment {segment.segmentIndex} (Ring {selectedRingIndex})", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();

        segment.segmentType = (SegmentType)EditorGUILayout.EnumPopup("Segment Type", segment.segmentType);

        EditorGUI.BeginDisabledGroup(segment.segmentType != SegmentType.Normal);

        segment.collectibleType = (CollectibleType)EditorGUILayout.EnumPopup("Collectable", segment.collectibleType);


        segment.hazardType = (HazardType)EditorGUILayout.EnumPopup("Hazard Type", segment.hazardType);
        segment.pickupType = (PickupType)EditorGUILayout.EnumPopup("Pickup Type", segment.pickupType);

        segment.hasPortal = EditorGUILayout.Toggle("Portal", segment.hasPortal);
        segment.hasCheckpoint = EditorGUILayout.Toggle("Checkpoint", segment.hasCheckpoint);

        segment.enemyType = (EnemyType)EditorGUILayout.EnumPopup("Enemy", segment.enemyType);

        segment.isLocked = EditorGUILayout.Toggle("Locked Gate", segment.isLocked);


        EditorGUI.EndDisabledGroup();

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(selectedLevelData);
        }
    }


    private void EnsureRingSegmentListExists()
    {
        if (selectedRingIndex >= selectedLevelData.rings.Count)
            return;

        var ring = selectedLevelData.rings[selectedRingIndex];
        while (ring.segments.Count < SegmentCount)
        {
            ring.segments.Add(new SegmentConfiguration
            {
                segmentIndex = ring.segments.Count
            });
        }
    }



    private void DrawAltarSettings()
    {
        selectedLevelData.isAltarLockedByKey = EditorGUILayout.Toggle("Altar Locked", selectedLevelData.isAltarLockedByKey);

    }



    private Transform GetOrCreatePreviewRoot()
    {
        var existing = GameObject.Find("_LevelPreview");
        if (existing != null)
            return existing.transform;

        var root = new GameObject("_LevelPreview");
        return root.transform;
    }



    private void BuildPreview()
    {
        ClearPreview();

        if (selectedLevelData == null)
        {
            Debug.LogWarning("No LevelData selected.");
            return;
        }

        Transform previewRoot = GetOrCreatePreviewRoot();

        foreach (var ring in selectedLevelData.rings)
        {
            float radius = 5f + ring.ringIndex * 2.5f;

            for (int i = 0; i < ring.segments.Count; i++)
            {
                var segment = ring.segments[i];
                float angle = -i * Mathf.PI * 2f / SegmentCount + Mathf.PI / 2f;
                Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                Quaternion rotation = Quaternion.Euler(0, -angle * Mathf.Rad2Deg, 0);




                int ringIndex = ring.ringIndex;

                GameObject prefab = segment.segmentType == SegmentType.Gap
                    ? prefabLibrary.gapSegmentPrefabs[ringIndex]
                    : prefabLibrary.normalSegmentPrefabs[ringIndex];


                if (prefab == null) continue;

                GameObject segmentGO = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                segmentGO.transform.position = position;
                segmentGO.transform.rotation = rotation;
                segmentGO.transform.SetParent(previewRoot);
                segmentGO.name = $"Ring{ring.ringIndex}_Seg{i}";


                RingSegment ringSegment = segmentGO.GetComponent<RingSegment>();
                if (ringSegment == null) continue;


                // --- 1. Collectible (Gem/Coin)
                if (segment.collectibleType != CollectibleType.None && ringSegment.SlotCollectible != null)
                {
                    prefab = prefabLibrary.GetCollectiblePrefab(segment.collectibleType);
                    if (prefab != null)
                    {
                        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                        go.transform.SetParent(ringSegment.SlotCollectible, false);
                        go.name = $"Collectible_{segment.collectibleType}";
                    }
                }



                // --- 2. Pickup (Shield, Cloak, etc.)
                if (segment.pickupType != PickupType.None && ringSegment.SlotPickup != null)
                {
                    prefab = prefabLibrary.GetPickupPrefab(segment.pickupType);
                    if (prefab != null)
                    {
                        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                        go.transform.SetParent(ringSegment.SlotPickup, false);
                        go.name = $"Pickup_{segment.pickupType}";
                    }
                }


                // --- 3. Hazard (Crusher, Crumbling)
                if (segment.hazardType != HazardType.None && ringSegment.SlotHazard != null)
                {
                    prefab = prefabLibrary.GetHazardPrefab(segment.hazardType);
                    if (prefab != null)
                    {
                        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                        go.transform.SetParent(ringSegment.SlotHazard, false);
                        go.name = $"Hazard_{segment.hazardType}";
                    }
                }

                // --- 4. Portal
                if (segment.hasPortal && ringSegment.SlotPortal != null)
                {
                    prefab = prefabLibrary.portalPrefab;
                    if (prefab != null)
                    {
                        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                        go.transform.SetParent(ringSegment.SlotPortal, false);
                        go.name = $"Portal";
                    }
                }

                // --- 5. Enemy
                if (segment.enemyType != EnemyType.None && ringSegment.SlotEnemy != null)
                {
                    prefab = prefabLibrary.GetEnemyPrefab(segment.enemyType);
                    if (prefab != null)
                    {
                        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                        go.transform.SetParent(ringSegment.SlotEnemy, false);
                        go.name = $"Enemy_{segment.enemyType}";
                    }
                }

                // --- 6. Checkpoint Flag
                if (segment.hasCheckpoint && ringSegment.SlotCheckpoint != null)
                {
                    prefab = prefabLibrary.checkpointPrefab;
                    if (prefab != null)
                    {
                        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                        go.transform.SetParent(ringSegment.SlotCheckpoint, false);
                        go.name = $"Checkpoint";
                    }
                }
            }



        }
    }



    private void ClearPreview()
    {
        var existing = GameObject.Find("_LevelPreview");
        if (existing != null)
            DestroyImmediate(existing);
    }



    private void LoadPrefabLibrary()
    {
        if (prefabLibrary != null) return;

        string[] guids = AssetDatabase.FindAssets("t:LevelPrefabLibrary");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            prefabLibrary = AssetDatabase.LoadAssetAtPath<LevelPrefabLibrary>(path);
        }

        if (prefabLibrary == null)
        {
            Debug.LogWarning("No LevelPrefabLibrary asset found. Please create one via 'Create > Rings of Ruin > Prefab Library'.");
        }
    }


    private void LoadAllLevels()
    {
        allLevels = AssetDatabase.FindAssets("t:LevelData")
            .Select(guid => AssetDatabase.LoadAssetAtPath<LevelData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(ld => ld != null)
            .OrderBy(ld => ld.levelID)
            .ToList();

        // Select the first level if nothing is selected
        if (selectedLevelData == null && allLevels.Count > 0)
        {
            selectedLevelIndex = 0;
            selectedLevelData = allLevels[0];

            string path = AssetDatabase.GetAssetPath(selectedLevelData);
            EditorPrefs.SetString(LevelDataPrefKey, path);
        }
        else
        {
            // Try to restore selectedLevelData from EditorPrefs if available
            string path = EditorPrefs.GetString(LevelDataPrefKey, string.Empty);
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


}