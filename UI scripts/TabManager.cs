using UdonSharp;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//this script manages all the individual tabs in a given tab-based UI system
//code inspired by https://www.youtube.com/watch?v=211t6r12XPQ

public class TabManager : UdonSharpBehaviour
{
    public TabButton[] tabButtons;//all tab buttons, see TabButton for more info
    public Sprite tabIdle;
    public Sprite tabSelected;
    private int curTab = -1;

	public void OnTabSelected(TabButton button)//called when an indivudual button has been interacted with
    {
        ResetTabs();//make tabs have idle sprite
        button.image.sprite = tabSelected;//since this button is selected, set it to active sprite
        if (curTab == button.tabIndex)//if an already active button is selected, it shall be turned off, and its page deactivated
        {
            curTab = -1;
            button.image.sprite = tabIdle;
        }
        else
        {
            curTab = button.tabIndex;//individual buttons have no idea where they are in the manager's tabButton array, hence why it must be user supplied
        }
        for (int i = 0; i < tabButtons.Length; i++)
        {
            if (i == curTab)
            {
                tabButtons[i].page.SetActive(true);//active the tab's page
            }
            else
            {
                tabButtons[i].page.SetActive(false);//deactivate the tab's page
            }
        }
    }

    //not sure why this Resize method is included, but the code is preserved just in case

    //public TabButton[] Resize<TabButton>(TabButton[] array, int newSize)
    //{
    //    TabButton[] sourceArray = array;
    //    if (sourceArray == null)
    //    {
    //        array = new TabButton[newSize];
    //    }
    //    else if (sourceArray.Length != newSize)
    //    {
    //        TabButton[] destinationArray = new TabButton[newSize];
    //        Array.Copy(sourceArray, 0, destinationArray, 0, (sourceArray.Length > newSize) ? newSize : sourceArray.Length);
    //        array = destinationArray;
    //    }
    //    return array;
    //}

    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            button.image.sprite = tabIdle;//sets all tab sprites to idle sprite
        }
    }
}
