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

        LeaderboardData data = SaveManager.LoadData();

        // Sort the data by score (Highest first)
        // If scores are equal, we sort by time (Lowest time is better)
        var sortedEntries = data.entries
            .OrderByDescending(e => e.score)
            .ThenBy(e => e.time)
            .ToList();

        for (int i = 0; i < sortedEntries.Count; i++)
        {
            GameObject newEntry = Instantiate(entryPrefab, contentContainer);
            PopulateEntry(newEntry, sortedEntries[i], i + 1);
        }
    }

    private void PopulateEntry(GameObject entryObject, SaveEntry entryData, int rank)
    {
        // Pro-tip: Use a small script on the prefab to hold these references instead of GetComponent
        TMP_Text[] texts = entryObject.GetComponentsInChildren<TMP_Text>();
        
        texts[0].text = rank.ToString();
        texts[1].text = entryData.score.ToString();
        texts[2].text = FormatTime(entryData.time);
    }

    private string FormatTime(float seconds)
    {
        // Formats seconds into 00:00 style
        int minutes = (int)seconds / 60;
        int secs = (int)seconds % 60;
        return string.Format("{0:00}:{1:00}", minutes, secs);
    }
}