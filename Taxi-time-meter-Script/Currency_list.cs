using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Currency_list : MonoBehaviour
{
    public Sprite icon_Currency;
    public Sprite icon_Currency_select;
    public Text txt_icon_fare_currency_symbol;
    public Text txt_icon_fare_currency_name;

    private string sel_name_currency;
    private string sel_symbol_currency;
    private Carrot.Carrot_Box box_Currency = null;

    public void on_load()
    {
        this.sel_name_currency = PlayerPrefs.GetString("name_currency", "USD");
        this.sel_symbol_currency = PlayerPrefs.GetString("symbol_currency","$");
        this.txt_icon_fare_currency_name.text = this.sel_name_currency;
        this.txt_icon_fare_currency_symbol.text = this.sel_symbol_currency;
    }

    public void show_list_Currency()
    {
        WWWForm frm_list_currency=this.GetComponent<App>().carrot.frm_act("get_list_currency");
        //this.GetComponent<App>().carrot.send(frm_list_currency,after_show_list_Currency);
    }

    private void after_show_list_Currency(string s_data)
    {
        this.GetComponent<App>().obj_effect_bloom.SetActive(false);
        box_Currency = this.GetComponent<App>().carrot.Create_Box(PlayerPrefs.GetString("currency_unit", "Currency unit"), this.icon_Currency);
        IList list_c = (IList)Carrot.Json.Deserialize(s_data);

        for (int i = 0; i < list_c.Count; i++)
        {
            IDictionary data_c = (IDictionary)list_c[i];
            Carrot.Carrot_Box_Item item_Currency_obj = box_Currency.create_item("currency_" + i);
            item_Currency_obj.set_icon(this.icon_Currency);
            item_Currency_obj.set_title(data_c["code"].ToString());
            item_Currency_obj.set_tip(data_c["symbol"].ToString());
            item_Currency_obj.set_act(() => this.select_Currency(data_c["code"].ToString(), data_c["symbol"].ToString()));

            if(this.get_cur_symbol_currency()== data_c["symbol"].ToString())
            {
                Carrot.Carrot_Box_Btn_Item btn_check=item_Currency_obj.create_item();
                btn_check.set_icon(this.icon_Currency_select);
            }
        }
        box_Currency.set_act_before_closing(act_close_list_currency);
    }

    private void act_close_list_currency()
    {
        this.GetComponent<App>().obj_effect_bloom.SetActive(true);
    }

    public void select_Currency(string s_name_Currency,string symbol_currency)
    {
        this.sel_name_currency = s_name_Currency;
        this.sel_symbol_currency = symbol_currency;
        PlayerPrefs.SetString("name_currency", this.sel_name_currency);
        PlayerPrefs.SetString("symbol_currency", this.sel_symbol_currency);
        this.txt_icon_fare_currency_name.text = this.sel_name_currency;
        this.txt_icon_fare_currency_symbol.text = this.sel_symbol_currency;
        this.GetComponent<App>().carrot.play_sound_click();
        if (this.box_Currency != null) this.box_Currency.close();
        
    }

    public string get_cur_name_currency()
    {
        return this.sel_name_currency;
    }

    public string get_cur_symbol_currency()
    {
        return this.sel_symbol_currency;
    }
}
