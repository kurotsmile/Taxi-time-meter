using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class History : MonoBehaviour
{
    public GameObject panel_history;
    public GameObject obj_item_history_prefab;
    public GameObject obj_view_history_prefab;
    public Transform area_body;
    private int length_history = 0;
    public Color32 color_nomal;
    public Color32 color_hightlight;
    public Sprite sp_icon_history;
    private Carrot.Carrot_Box box_history = null;

    public void load()
    {
        this.length_history = PlayerPrefs.GetInt("length_history", 0);
    }

    public void show_history()
    {
        if (this.length_history == 0)
        {
            this.GetComponent<App>().carrot.show_msg(PlayerPrefs.GetString("driving_history","Invoice history"),PlayerPrefs.GetString("driving_history_none","Empty invoice history!"),Carrot.Msg_Icon.Alert);
        }
        else
        {
            this.panel_history.SetActive(true);
            this.GetComponent<App>().carrot.clear_contain(this.area_body);
            for (int i = this.length_history - 1; i >= 0; i--)
            {
                GameObject obj_history_item = Instantiate(this.obj_item_history_prefab);
                obj_history_item.transform.SetParent(this.area_body);
                obj_history_item.transform.localScale = new Vector3(1f, 1f, 1f);
                TimeSpan t = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("h_time_" + i));
                obj_history_item.GetComponent<Item_history>().txt_title.text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
                obj_history_item.GetComponent<Item_history>().txt_tip.text = PlayerPrefs.GetString("h_date_time_" + i);
                obj_history_item.GetComponent<Item_history>().txt_text_btn_print.text = PlayerPrefs.GetString("invoice_printing", "Invoice printing");
                obj_history_item.GetComponent<Item_history>().txt_text_btn_view.text = PlayerPrefs.GetString("quick_view", "Quick view");
                if (PlayerPrefs.GetFloat("h_lon_end_" + i, 0f) > 0f)
                {
                    obj_history_item.GetComponent<Item_history>().txt_text_btn_distance.text = PlayerPrefs.GetString("distance", "Distance");
                    obj_history_item.GetComponent<Item_history>().btn_distance.SetActive(true);
                    obj_history_item.GetComponent<Item_history>().p_lat_start = PlayerPrefs.GetFloat("h_lat_start_"+i);
                    obj_history_item.GetComponent<Item_history>().p_lon_start = PlayerPrefs.GetFloat("h_lon_start_"+i);
                    obj_history_item.GetComponent<Item_history>().p_lat_end = PlayerPrefs.GetFloat("h_lat_end_" + i);
                    obj_history_item.GetComponent<Item_history>().p_lon_end = PlayerPrefs.GetFloat("h_lon_end_"+i);
                }
                else
                {
                    obj_history_item.GetComponent<Item_history>().btn_distance.SetActive(false);
                }
                
                obj_history_item.GetComponent<Item_history>().index = i;
                if (i % 2 == 0)
                    obj_history_item.GetComponent<Image>().color = this.color_nomal;
                else
                    obj_history_item.GetComponent<Image>().color = this.color_hightlight;
            }
            this.GetComponent<App>().panel_main.SetActive(false);
        }
    }

    public void close()
    {
        this.panel_history.SetActive(false);
    }

    public void add(float f_time,float f_km,float f_price_km,float f_price_km_Promotional, float f_bill,string s_date_time,float p_lon_start,float p_lat_start, float p_lon_end, float p_lat_end,string s_currency)
    {
        PlayerPrefs.SetFloat("h_time_"+this.length_history,f_time);
        PlayerPrefs.SetFloat("h_km_"+this.length_history, f_km);
        PlayerPrefs.SetFloat("h_price_km_" + this.length_history, f_price_km);
        PlayerPrefs.SetFloat("h_price_km_Promotional_" + this.length_history, f_price_km_Promotional);
        PlayerPrefs.SetFloat("h_bill_" + this.length_history, f_bill);
        PlayerPrefs.SetString("h_date_time_" + this.length_history, s_date_time);
        PlayerPrefs.SetFloat("h_lon_start_" + this.length_history, p_lon_start);
        PlayerPrefs.SetFloat("h_lat_start_" + this.length_history, p_lat_start);
        PlayerPrefs.SetFloat("h_lon_end_" + this.length_history, p_lon_end);
        PlayerPrefs.SetFloat("h_lat_end_" + this.length_history, p_lat_end);
        PlayerPrefs.SetString("h_currency_" + this.length_history, s_currency);
        this.length_history++;
        PlayerPrefs.SetInt("length_history", this.length_history);
    }

    public void delete_all_history()
    {
        for(int i = 0; i < this.length_history; i++)
        {
            PlayerPrefs.DeleteKey("h_time_" + i);
            PlayerPrefs.DeleteKey("h_km_" + i);
            PlayerPrefs.DeleteKey("h_price_km_" + i);
            PlayerPrefs.DeleteKey("h_bill_" + i);
            PlayerPrefs.DeleteKey("h_date_time_" + i);
        }

        PlayerPrefs.DeleteKey("length_history");
        this.length_history = 0;
    }

    public void print_item(int index_history)
    {
        float h_price_km = PlayerPrefs.GetFloat("h_price_km_"+ index_history);
        float h_km = PlayerPrefs.GetFloat("h_km_" + index_history);
        float bill_price;
        string mobile_num = "+1";
        string message = PlayerPrefs.GetString("invoice_summary", "Invoice Summary")+"\n-------------------\n";
        message = message + PlayerPrefs.GetString("invoice_date", "Invoice Date") +":"+PlayerPrefs.GetString("h_date_time_"+index_history) + "\n";
        message = message + PlayerPrefs.GetString("kilometro", "Kilometro") +":"+ h_price_km.ToString("F2") + "\n";
        TimeSpan t = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("h_time_" + index_history));
        message = message + PlayerPrefs.GetString("running_time", "Running time") + ":" + string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds) + "\n";
        message = message + PlayerPrefs.GetString("km_promotional_price", "Promotional price of the road") + ":" + PlayerPrefs.GetFloat("h_price_km_Promotional_" + index_history) +" "+PlayerPrefs.GetString("h_currency_"+index_history,"$") + "\n";

        if (h_km < 1.2f)
            bill_price = h_km / h_price_km;
        else
            bill_price = h_km * h_price_km;

        message = message + "-------------------\n";
        message = message + PlayerPrefs.GetString("total_amount", "Total amount") +":" + bill_price.ToString("F2") + " "+ PlayerPrefs.GetString("h_currency_" + index_history, "$") + "\n";
        string URL = string.Format("sms:{0}?body={1}", mobile_num, message);
        Application.OpenURL(URL);
    }

    public void quick_view_item(Index index_history)
    {
        this.box_history=this.GetComponent<App>().carrot.Create_Box(PlayerPrefs.GetString("quick_view", "Quick View"),this.sp_icon_history);
        float h_price_km = PlayerPrefs.GetFloat("h_price_km_" + index_history);
        float h_km = PlayerPrefs.GetFloat("h_km_" + index_history);
        float bill_price;
        string message = PlayerPrefs.GetString("invoice_summary", "Invoice Summary") + "\n-------------------\n";
        message = message + PlayerPrefs.GetString("invoice_date", "Invoice Date") + ":" + PlayerPrefs.GetString("h_date_time_" + index_history) + "\n";
        message = message + PlayerPrefs.GetString("kilometro", "Kilometro") + ":" + h_price_km.ToString("F2") + "\n";
        TimeSpan t = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("h_time_" + index_history));
        message = message + PlayerPrefs.GetString("running_time", "Running time") + ":" + string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds) + "\n";
        message = message + PlayerPrefs.GetString("km_promotional_price", "Promotional price of the road") + ":" + PlayerPrefs.GetFloat("h_price_km_Promotional_" + index_history) + " " + PlayerPrefs.GetString("h_currency_" + index_history, "$") + "\n";

        if (h_km < 1.2f)
            bill_price = h_km / h_price_km;
        else
            bill_price = h_km * h_price_km;

        message = message + "-------------------\n";
        message = message + PlayerPrefs.GetString("total_amount", "Total amount") + ":" + bill_price.ToString("F2") + " " + PlayerPrefs.GetString("h_currency_" + index_history, "$") + "\n";

        GameObject item_text_view_history = Instantiate(this.obj_view_history_prefab);
        item_text_view_history.transform.SetParent(this.box_history.area_all_item);
        item_text_view_history.transform.localPosition = new Vector3(item_text_view_history.transform.localPosition.x, item_text_view_history.transform.localPosition.y, item_text_view_history.transform.localPosition.z);
        item_text_view_history.transform.localScale = new Vector3(1f,1f,1f);
        item_text_view_history.GetComponent<Text>().text = message;
        this.GetComponent<App>().carrot.play_sound_click();
    }
}
