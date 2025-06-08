using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private GameObject activePanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private TMP_Text gemCountText;
    [SerializeField] private TMP_Text coinCountText;


    private void Awake()
    {
        ShowPanel(mainMenuPanel);
    }



    private void OnEnable()
    {
        PlayerState.OnGemCollected += OnGemCollected;
        PlayerState.OnCoinCollected += OnCoinCollected;
    }


    public void ShowPanel(GameObject panel)
    {
        // Deactivate the currently active panel if it exists
        if (activePanel != null)
        {
            activePanel.SetActive(false);
        }

        // Activate the new panel
        panel.SetActive(true);

        // Update the active panel reference
        activePanel = panel;
    }




    private void OnGemCollected(int gemCount)
    {
        gemCountText.text = gemCount.ToString();
    }


    private void OnCoinCollected(int coinCount)
    {
        coinCountText.text = coinCount.ToString();
    }


    private void OnDisable()
    {
        PlayerState.OnGemCollected -= OnGemCollected;
        PlayerState.OnCoinCollected -= OnCoinCollected;
    }
}