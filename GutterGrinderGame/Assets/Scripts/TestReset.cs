using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Test helper: press R in Play mode to wipe save and reload. Delete before release.</summary>
public class TestReset : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            WipeSaveAndMemory();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Save wiped - reloaded fresh (damage should be 1)");
        }
    }

    void WipeSaveAndMemory()
    {
        // disk
        PlayerPrefs.DeleteAll();
        // memory (DontDestroyOnLoad singletons survive reload, so reset them too)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.coins = 0;
            GameManager.Instance.totalKills = 0;
            GameManager.Instance.distance = 0;
            GameManager.Instance.bestDistance = 0;
        }
        if (WeaponSystem.Instance != null)
            WeaponSystem.Instance.coreLevel = 0;
        // persist the cleared values so the 5s autosave doesn't restore old numbers
        if (GameManager.Instance != null)
            GameManager.Instance.Save();
        else
            PlayerPrefs.Save();
    }

    [ContextMenu("Wipe Save Now")]
    public void WipeNow()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Save wiped in Editor. Stop Play and Play again for fresh stats.");
    }
}
