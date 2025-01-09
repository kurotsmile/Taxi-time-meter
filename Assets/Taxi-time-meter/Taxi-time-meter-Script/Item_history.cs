using UnityEngine;
using UnityEngine.UI;

public class Item_history : MonoBehaviour
{
    public Text txt_title;
    public Text txt_tip;
    public Text txt_text_btn_print;
    public Text txt_text_btn_view;
    public Text txt_text_btn_distance;
    public GameObject btn_distance;
    public int index;
    public float p_lat_start, p_lon_start;
    public float p_lat_end, p_lon_end;

    public void click()
    {
        GameObject.Find("App").GetComponent<App>().history.print_item(this.index);
    }

    public void quick_view()
    {
        GameObject.Find("App").GetComponent<App>().history.quick_view_item(this.index);
    }

    public void show_distance()
    {
        Application.OpenURL("https://www.google.com/maps/dir/"+this.p_lat_start.ToString().Replace(",", ".") + ","+this.p_lon_start.ToString().Replace(",", ".") + "/"+this.p_lat_end.ToString().Replace(",", ".") + ","+this.p_lon_end.ToString().Replace(",", ".") + "/");
    }
}
