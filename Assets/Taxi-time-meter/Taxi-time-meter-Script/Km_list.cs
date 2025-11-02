using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class val_km
{
    public int km;
    public float price;
    public bool is_check;
}

public class Km_list : MonoBehaviour
{
    [Header("Main Object")]
    public App app;
    public GameObject panel_km;
    public GameObject panel_km_body_add;
    public GameObject panel_km_body_list;
    public GameObject prefab_km_item;
    public GameObject prefab_km_add_tip;
    public Transform area_km_body_list;
    public Sprite sp_icon_km_default;

    [Header("Obj Add price for Km")]
    public InputField inp_km_price;
    public InputField inp_km_distance;
    public GameObject panel_km_add_distance;

    private int leng_km = 0;
    private int index_edit_km = -1;
    private List<val_km> list_val_km;
    private int index_point_check = -1;

    public void load()
    {
        this.leng_km = PlayerPrefs.GetInt("leng_km", 0);
        this.panel_km.SetActive(false);
        this.panel_km_body_add.SetActive(false);
        this.panel_km_body_list.SetActive(true);

        this.list_val_km = new List<val_km>();
        for (int i = 0; i < this.leng_km; i++)
        {
            if (PlayerPrefs.GetFloat("km_item_price_" + i) != 0)
            {
                val_km val_km = new val_km();
                val_km.km = PlayerPrefs.GetInt("km_item_distance_" + i);
                val_km.price = PlayerPrefs.GetFloat("km_item_price_" + i);
                val_km.is_check = false;
                this.list_val_km.Add(val_km);
            }
        }
    }

    public void btn_show_list_km()
    {
        if (this.GetComponent<App>().get_box_setting() != null) this.GetComponent<App>().get_box_setting().close();

        this.GetComponent<App>().carrot.clear_contain(this.area_km_body_list);
        this.GetComponent<App>().carrot.play_sound_click();

        GameObject item_km_add_tip = Instantiate(this.prefab_km_add_tip);
        item_km_add_tip.transform.SetParent(this.area_km_body_list);
        item_km_add_tip.transform.localPosition = new Vector3(item_km_add_tip.transform.localPosition.x, item_km_add_tip.transform.localPosition.y, item_km_add_tip.transform.localPosition.z);
        item_km_add_tip.transform.localScale = new Vector3(1f, 1f, 1f);
        item_km_add_tip.GetComponent<Item_km>().txt_title.text = app.carrot.L("km_price_add", "Add promotional price corresponding to the distance");
        item_km_add_tip.GetComponent<Item_km>().txt_tip.text =app.carrot.L("km_price_add_tip", "Click here to add promotional prices by route");

        GameObject item_km_default = Instantiate(this.prefab_km_item);
        item_km_default.transform.SetParent(this.area_km_body_list);
        item_km_default.transform.localPosition = new Vector3(item_km_default.transform.localPosition.x, item_km_default.transform.localPosition.y, item_km_default.transform.localPosition.z);
        item_km_default.transform.localScale = new Vector3(1f, 1f, 1f);
        item_km_default.GetComponent<Item_km>().txt_title.text = app.carrot.L("km_price_default", "Default price of each route when not specifically set");
        item_km_default.GetComponent<Item_km>().txt_tip.text = PlayerPrefs.GetFloat("price_per_km_val",1f).ToString() + " " + this.GetComponent<Currency_list>().get_cur_symbol_currency();
        item_km_default.GetComponent<Item_km>().icon.sprite = this.sp_icon_km_default;
        item_km_default.GetComponent<Item_km>().type = 2;
        item_km_default.GetComponent<Item_km>().btn_delete.SetActive(false);
        item_km_default.GetComponent<Item_km>().txt_label_edit.text = app.carrot.L("edit", "Edit");

        this.list_val_km = new List<val_km>();
        for (int i = 0; i < this.leng_km; i++)
        {
            if (PlayerPrefs.GetFloat("km_item_price_" + i) != 0)
            {
                int km_item_distance = PlayerPrefs.GetInt("km_item_distance_" + i);
                GameObject item_km = Instantiate(this.prefab_km_item);
                item_km.transform.SetParent(this.area_km_body_list);
                item_km.transform.localPosition = new Vector3(item_km.transform.localPosition.x, item_km.transform.localPosition.y, item_km.transform.localPosition.z);
                item_km.transform.localScale = new Vector3(1f, 1f, 1f);
                item_km.GetComponent<Item_km>().txt_tip.text = PlayerPrefs.GetFloat("km_item_price_" + i).ToString() + " " + this.GetComponent<Currency_list>().get_cur_symbol_currency();
                item_km.GetComponent<Item_km>().txt_title.text = app.carrot.L("distance", "Distance") + " >= " + km_item_distance + " " + app.carrot.L("kilometro", "kilometro");
                item_km.GetComponent<Item_km>().index = i;
                item_km.GetComponent<Item_km>().txt_label_del.text = app.carrot.L("del", "Delete");
                item_km.GetComponent<Item_km>().txt_label_edit.text = app.carrot.L("edit", "Edit");

                if (this.GetComponent<App>().get_km() >= km_item_distance)
                {
                    item_km.GetComponent<Item_km>().obj_check_point.SetActive(true);
                    item_km.GetComponent<Animator>().enabled = true;
                }
                else
                {
                    item_km.GetComponent<Item_km>().obj_check_point.SetActive(false);
                    item_km.GetComponent<Animator>().enabled = false;
                }

                val_km val_km = new val_km();
                val_km.km = PlayerPrefs.GetInt("km_item_distance_" + i);
                val_km.price = PlayerPrefs.GetFloat("km_item_price_" + i);
                val_km.is_check = false;
                this.list_val_km.Add(val_km);
            }
        }

        this.GetComponent<App>().panel_main.SetActive(false);
        this.panel_km_body_list.SetActive(true);
        this.panel_km_body_add.SetActive(false);
        this.panel_km.SetActive(true);
    }

    public void btn_close_list_km()
    {
        this.GetComponent<App>().panel_main.SetActive(true);
        this.GetComponent<App>().carrot.play_sound_click();
        this.panel_km.SetActive(false);
    }

    public void show_panel_add_km()
    {
        this.index_edit_km = -1;
        this.inp_km_distance.text = "0";
        this.inp_km_price.text = "0";
        this.panel_km_body_list.SetActive(false);
        this.panel_km_body_add.SetActive(true);
        this.panel_km.SetActive(true);
        this.panel_km_add_distance.SetActive(true);
    }

    public void show_panel_add_km_price_default()
    {
        this.index_edit_km = -2;
        this.inp_km_distance.text = "0";
        this.inp_km_price.text = PlayerPrefs.GetFloat("price_per_km_val").ToString();
        this.panel_km_body_list.SetActive(false);
        this.panel_km_body_add.SetActive(true);
        this.panel_km.SetActive(true);
        this.panel_km_add_distance.SetActive(false);
        this.GetComponent<App>().carrot.play_sound_click();
    }

    public void btn_add_km()
    {
        this.GetComponent<App>().carrot.play_sound_click();

        if (this.index_edit_km == -1)
        {
            if (this.validate_inp_price_distance()) this.add_km(float.Parse(this.inp_km_price.text), int.Parse(this.inp_km_distance.text));
        }
        else if (this.index_edit_km == -2)
        {
            PlayerPrefs.SetFloat("price_per_km_val", float.Parse(this.inp_km_price.text));
            this.GetComponent<App>().set_price_per_km(float.Parse(this.inp_km_price.text));
            this.GetComponent<App>().carrot.Show_msg(app.carrot.L("price_per_km", "Price per kilometer"), app.carrot.L("km_price_edit_success", "Promotional price has been successfully updated!"), Carrot.Msg_Icon.Success);
        }
        else
        {
            if (this.validate_inp_price_distance()) this.update_km(float.Parse(this.inp_km_price.text), int.Parse(this.inp_km_distance.text));
        }
    }

    private bool validate_inp_price_distance()
    {
        if (this.inp_km_distance.text.Trim() == "" || this.inp_km_price.text.Trim() == "")
        {
            this.GetComponent<App>().carrot.Show_msg(app.carrot.L("price_per_km", "Price per kilometer"), "Input distance and price must be greater than 0", Carrot.Msg_Icon.Error);
            return false;
        }
        else if (int.Parse(this.inp_km_distance.text) <= 0 || float.Parse(this.inp_km_price.text) <= 0)
        {
            this.GetComponent<App>().carrot.Show_msg(app.carrot.L("price_per_km", "Price per kilometer"), app.carrot.L("km_price_error", "Value input error , Distance and price must be greater than 0 !"), Carrot.Msg_Icon.Error);
            return false;
        }
        else
        {
            return true;
        }
    }


    private void add_km(float price, int km)
    {
        PlayerPrefs.SetFloat("km_item_price_" + this.leng_km, price);
        PlayerPrefs.SetInt("km_item_distance_" + this.leng_km, km);
        this.leng_km++;
        PlayerPrefs.SetInt("leng_km", this.leng_km);
        this.GetComponent<App>().carrot.Show_msg(app.carrot.L("price_per_km", "Price per kilometer"), app.carrot.L("km_price_add_success", "Successfully added the corresponding promotional price!"), Carrot.Msg_Icon.Success);
        this.btn_show_list_km();
    }

    private void update_km(float price, int km)
    {
        PlayerPrefs.SetFloat("km_item_price_" + this.index_edit_km, price);
        PlayerPrefs.SetInt("km_item_distance_" + this.index_edit_km, km);
        this.GetComponent<App>().carrot.Show_msg(app.carrot.L("price_per_km", "Price per kilometer"), app.carrot.L("km_price_edit_success", "Promotional price has been successfully updated!"), Carrot.Msg_Icon.Success);
        this.btn_show_list_km();
    }

    public void delete_km(int index_km)
    {
        PlayerPrefs.DeleteKey("km_item_distance_" + index_km);
        PlayerPrefs.DeleteKey("km_item_price_" + index_km);
        this.btn_show_list_km();
    }

    public void show_edit_km(int index_km)
    {
        this.index_edit_km = index_km;
        this.panel_km_body_add.SetActive(true);
        this.inp_km_distance.text = PlayerPrefs.GetInt("km_item_distance_" + this.index_edit_km).ToString();
        this.inp_km_price.text = PlayerPrefs.GetFloat("km_item_price_" + this.index_edit_km).ToString();
        this.panel_km_add_distance.SetActive(true);
    }

    public void btn_close_km_add()
    {
        this.panel_km_body_add.SetActive(false);
        this.panel_km_body_list.SetActive(true);
        this.GetComponent<App>().carrot.play_sound_click();
    }

    public float get_price_by_km(float km)
    {
        float val_price;
        if (this.index_point_check == -1)
            val_price = PlayerPrefs.GetFloat("price_per_km_val",1f);
        else
            val_price = this.list_val_km[this.index_point_check].price;

        for (int i = 0; i < this.list_val_km.Count; i++)
        {
            if (float.Parse(km.ToString()) >= this.list_val_km[i].km && this.list_val_km[i].is_check == false)
            {
                val_price = this.list_val_km[i].price;
                this.index_point_check = i;
                this.list_val_km[i].is_check = true;
                break;
            }
        }

        return val_price;
    }

    public void reset_list_km()
    {
        this.index_point_check = -1;
    }

    public int get_length()
    {
        return this.leng_km;
    }
}
