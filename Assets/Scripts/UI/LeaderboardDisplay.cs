using UnityEngine;
using TMPro;
using System.Linq;

public class LeaderboardDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform contentContainer;
    [SerializeField] private GameObject entryPrefab;

    public void DisplayLeaderboard()
    {
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        SaveManager.Instance.LoadData((data) =>
        {
            // 3. This block runs automatically ONLY after the web data arrives!
            // We still sort locally to perfectly resolve tie-breaker times
            var sortedEntries = data.entries
                .OrderByDescending(e => e.score)
                .ThenBy(e => e.time)
                .ToList();

            for (int i = 0; i < sortedEntries.Count; i++)
            {
                GameObject newEntry = Instantiate(entryPrefab, contentContainer);
                PopulateEntry(newEntry, sortedEntries[i], i + 1);
            }
        });
    }

    private void PopulateEntry(GameObject entryObject, SaveEntry entryData, int rank)
    {
        TMP_Text[] texts = entryObject.GetComponentsInChildren<TMP_Text>();
        
        texts[0].text = rank.ToString() + ".";
        texts[1].text = entryData.playerName;
        texts[2].text = entryData.score.ToString();
        
        // Formats the raw float seconds (e.g., 72.4f) into a readable clock string ("01:12")
        texts[3].text = FormatTime(entryData.time);
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }


}