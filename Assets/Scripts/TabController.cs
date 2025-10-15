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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActivateTab(int tabNo)
    {
        for (int i = 0; i < pages.Length; i++)
            tabImages[i].color = Color.red; //Not sure why it wont work, I think this line of code is having issues, will review later
            pages[tabNo].SetActive(true);
    tabImages[tabNo].color = Color.white;
    }
    
}
