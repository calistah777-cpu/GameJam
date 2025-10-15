using UnityEngine;
using UnityEngine.UI;
public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActivateTab(0); 
    }

    public void ActivateTab(int tabNo)
    {
        for (int i = 0; i < pages.Length; i++) {
            pages[i].SetActive(false);
            tabImages[i].color = Color.red;
        }
            //tabImages[i].color = Color.red; //Not sure why it wont work, I think this line of code is having issues, will review later
            pages[tabNo].SetActive(true);
            tabImages[tabNo].color = Color.white;
    }
    
}
