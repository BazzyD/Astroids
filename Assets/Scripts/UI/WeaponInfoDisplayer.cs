using UnityEngine;

public class WeaponInfoDisplayer : MonoBehaviour
{
    [SerializeField] private CurrentWeaponData currentWeaponData;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject missile;
    [SerializeField] private GameObject mine;
    [SerializeField] private TMPro.TextMeshProUGUI weaponLevelText;
    private int currentIndex =0;
    private void OnEnable()
    {
        currentWeaponData.OnWeaponChanged += HandelWeaponChange;
    }
    private void OnDisable()
    {
        currentWeaponData.OnWeaponChanged -= HandelWeaponChange;
    }
    private void HandelWeaponChange(int index, int level)
    {
        if(index != currentIndex){
            currentIndex = index;
            HideAll();
            switch (currentIndex)
            {
                case 0:
                projectile.SetActive(true);
                break;
                case 1:
                laser.SetActive(true);
                break;
                case 2:
                missile.SetActive(true);
                break;
                case 3:
                mine.SetActive(true);
                break;
            }
        }
        weaponLevelText.text = $"{level+1}";
    }
    private void HideAll()
    {
        projectile.SetActive(false);
        laser.SetActive(false);
        missile.SetActive(false);
        mine.SetActive(false);
    }
}
