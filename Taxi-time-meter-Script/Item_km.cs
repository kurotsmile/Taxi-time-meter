using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_km : MonoBehaviour
{
    public Text txt_title;
    public Text txt_tip;
    public int type;
    public int index;
    public Image icon;
    public GameObject btn_delete;
    public GameObject obj_check_point;
    public Text txt_label_del;
    public Text txt_label_edit;

    public void click()
    {
        if (this.type == 0) {
            GameObject.Find("App").GetComponent<Km_list>().show_panel_add_km();
            GameObject.Find("App").GetComponent<App>().carrot.play_sound_click();
        }

        if (this.type == 1) this.edit();
        if (this.type == 2) GameObject.Find("App").GetComponent<Km_list>().show_panel_add_km_price_default();
    }

    public void edit()
    {
        if (this.type == 2) 
            GameObject.Find("App").GetComponent<Km_list>().show_panel_add_km_price_default();
        else
            GameObject.Find("App").GetComponent<Km_list>().show_edit_km(this.index);

        GameObject.Find("App").GetComponent<App>().carrot.play_sound_click();
    }

    public void delete()
    {
        GameObject.Find("App").GetComponent<Km_list>().delete_km(this.index);
        GameObject.Find("App").GetComponent<App>().carrot.play_sound_click();
    }
}
