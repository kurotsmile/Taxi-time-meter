using UnityEngine;
using UnityEngine.UI;

public class Currency_list : MonoBehaviour
{
    [Header("Obj main")]
    public App app;

    [Header("Obj Currency")]
    public Sprite icon_Currency;
    public Sprite icon_money;
    public Sprite icon_Currency_select;
    public Text txt_icon_fare_currency_symbol;
    public Text txt_icon_fare_currency_name;

    [Header("Data Currency")]
    public string[] name_currency;
    public string[] code_currency;
    public string[] symbol_currency;

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
        this.app.obj_effect_bloom.SetActive(false);
        this.box_Currency = this.app.carrot.Create_Box(PlayerPrefs.GetString("currency_unit", "Currency unit"), this.icon_Currency);

        for (int i = 0; i < this.code_currency.Length; i++)
        {
            var index_currency = i;
            string s_tip = this.code_currency[i] + " - " + this.symbol_currency[i];
            Carrot.Carrot_Box_Item item_Currency_obj = box_Currency.create_item("currency_" + i);
            item_Currency_obj.set_icon(this.icon_money);
            item_Currency_obj.set_title(this.name_currency[i]);
            item_Currency_obj.set_tip(s_tip);
            item_Currency_obj.set_act(() => this.select_Currency(this.code_currency[index_currency], this.symbol_currency[index_currency]));

            if (this.get_cur_symbol_currency() != this.symbol_currency[i])
            {
                Carrot.Carrot_Box_Btn_Item btn_check = item_Currency_obj.create_item();
                btn_check.set_icon(this.icon_Currency_select);
                btn_check.set_color(this.app.carrot.color_highlight);
                Destroy(btn_check.GetComponent<Button>());
            }
        }

        this.box_Currency.update_color_table_row();
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
        this.app.carrot.play_sound_click();
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
