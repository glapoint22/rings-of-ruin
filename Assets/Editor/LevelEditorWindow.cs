using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    private LevelData selectedLevelData;
    private int selectedRingIndex = 0;
    private int selectedSegmentIndex = -1;

    private const int SegmentCount = 24;
    private float ringRadius = 200f;
    private float buttonSize = 40f;



    [MenuItem("Window/Rings of Ruin/Level Editor")]
    public static void ShowWindow()
    {
        LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level Editor");
        window.minSize = new Vector2(400, 400);
    }

    private void OnGUI()
    {
        GUILayout.Label("Rings of Ruin – Level Editor", EditorStyles.boldLabel);

        selectedLevelData = (LevelData)EditorGUILayout.ObjectField("Level Data", selectedLevelData, typeof(LevelData), false);

        if (selectedLevelData == null) {
            EditorGUILayout.HelpBox("Select a LevelData asset to begin.", MessageType.Info);
            return;
        }

        selectedRingIndex = EditorGUILayout.IntSlider("Ring Index", selectedRingIndex, 0, 3);
        EnsureRingSegmentListExists();

        DrawRingLayout();

        GUILayout.Space(300);
        DrawSegmentDetails();

        if (GUI.changed)
        {
            Repaint();
        }
    }

    private void DrawRingLayout()
    {
        float viewWidth = position.width;
        Vector2 ringCenter = new Vector2(viewWidth / 2f, 200); // place near top

        Handles.BeginGUI(); // allows drawing in GUI space

        for (int i = 0; i < SegmentCount; i++)
        {
            float angle = i * Mathf.PI * 2f / SegmentCount;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * ringRadius;
            Vector2 buttonCenter = ringCenter + offset;

            Rect buttonRect = new Rect(buttonCenter.x - buttonSize / 2, buttonCenter.y - buttonSize / 2, buttonSize, buttonSize);

            SegmentConfiguration segment = selectedLevelData.rings[selectedRingIndex].segments[i];

            Color originalColor = GUI.color;
            GUI.color = segment.isGap ? Color.red : Color.green;

            string label = i.ToString();

            if (segment.gemPrefab)      label += "\n💎";
            if (segment.hazardPrefab)   label += "\n⚠️";
            if (segment.powerUpPrefab)  label += "\n🌀";
            if (segment.portalPrefab)   label += "\n⏩";

            if (GUI.Button(buttonRect, label))
            {
                if (Event.current.alt)
                {
                    segment.isGap = !segment.isGap;
                }

                selectedSegmentIndex = i;
                EditorUtility.SetDirty(selectedLevelData);
            }

            GUI.color = originalColor;
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

        var selectedSegment = selectedLevelData.rings[selectedRingIndex].segments[selectedSegmentIndex];

        
        GUILayout.Label($"Editing Segment {selectedSegment.segmentIndex} (Ring {selectedRingIndex})", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);

        EditorGUI.BeginChangeCheck();

        selectedSegment.isCrumbling = EditorGUILayout.Toggle("Is Crumbling", selectedSegment.isCrumbling);
        selectedSegment.gemPrefab = (GameObject)EditorGUILayout.ObjectField("Gem Prefab", selectedSegment.gemPrefab, typeof(GameObject), false);
        selectedSegment.hazardPrefab = (GameObject)EditorGUILayout.ObjectField("Hazard Prefab", selectedSegment.hazardPrefab, typeof(GameObject), false);
        selectedSegment.powerUpPrefab = (GameObject)EditorGUILayout.ObjectField("Power-up Prefab", selectedSegment.powerUpPrefab, typeof(GameObject), false);
        selectedSegment.portalPrefab = (GameObject)EditorGUILayout.ObjectField("Portal Prefab", selectedSegment.portalPrefab, typeof(GameObject), false);

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(selectedLevelData);
        }
    }



    private void EnsureRingSegmentListExists()
    {
        // Ensure this ring exists in the list
        while (selectedLevelData.rings.Count <= selectedRingIndex)
        {
            selectedLevelData.rings.Add(new RingConfiguration
            {
                ringIndex = selectedLevelData.rings.Count
            });
        }

        // Ensure this ring has 24 segments
        var ring = selectedLevelData.rings[selectedRingIndex];
        while (ring.segments.Count < SegmentCount)
        {
            ring.segments.Add(new SegmentConfiguration
            {
                segmentIndex = ring.segments.Count
            });
        }
    }
}
