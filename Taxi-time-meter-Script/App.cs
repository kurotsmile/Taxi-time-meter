using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class App : MonoBehaviour
{
    [Header("Obj App")]
    public GameObject panel_main;
    public History history;
    public GameObject obj_effect_bloom;

    public Sprite[] sp_digit_number;
    public Sprite sp_taxi_play;
    public Sprite sp_taxi_pause;
    public GameObject obj_digit_n;
    public Carrot.Carrot carrot;

    float time_second = 0;
    float speed_time_pending_bill = 0;
    float km = 0f;
    float speed_time_second = 0;
    float bill_price;
    int status_play = 0;
    float p_location_lon_start;
    float p_location_lat_start;

    public Digit_Control[] digit_control;
    public AudioClip sound_click_Clip;

    public GameObject btn_obj_stop;
    public GameObject btn_obj_new_guest;
    public GameObject btn_obj_print;
    public GameObject btn_obj_play;
    public Image img_btn_play;
    public Text txt_btn_play;

    [Header("Setting App")]
    private Carrot.Carrot_Box box_setting;
    public Sprite sp_price_per_km;
    public Sprite sp_price_per_km_list;
    float price_per_km = 0.5f;

    [Header("data test location")]
    public float[] p_lot_test;
    public float[] p_lat_test;
    private List<distance_data> list_distance_test;

    void Start()
    {
        this.carrot.Load_Carrot(this.on_check_exit_app);
        this.carrot.change_sound_click(this.sound_click_Clip);

        this.panel_main.SetActive(true);
        this.show_digit("h", this.digit_control[2].area_digit);
        
        this.price_per_km = PlayerPrefs.GetFloat("price_per_km_val",1f);
        this.show_digit(this.price_per_km.ToString(), this.digit_control[1].area_digit);
        this.btn_obj_stop.SetActive(false);
        this.btn_obj_new_guest.SetActive(false);
        this.btn_obj_print.SetActive(false);
        this.check_statu_play();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        this.history.load();
        this.GetComponent<Km_list>().load();
        this.GetComponent<Currency_list>().on_load();

        if (this.carrot.model_app == Carrot.ModelApp.Develope)
        {
            this.GetComponent<Manager_location>().enabled = false;
            this.list_distance_test = new List<distance_data>();
            for(int i = 0; i < this.p_lat_test.Length-1; i++)
            {
                distance_data d = new distance_data();
                d.lon= this.p_lat_test[i];
                d.lat = this.p_lot_test[i];
                this.list_distance_test.Add(d);
            }
        }
        this.obj_effect_bloom.SetActive(true);
    }

    void Update()
    {
        if (this.status_play==1)
        {
            speed_time_second += 1 * Time.deltaTime;
            if (speed_time_second > 1.4f)
            {
                speed_time_second = 0f;
                time_second++;
                TimeSpan t = TimeSpan.FromSeconds(time_second);
                string s_t = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours,t.Minutes,t.Seconds);
                this.show_digit(s_t, this.digit_control[0].area_digit);
                if (this.carrot.model_app == Carrot.ModelApp.Develope)
                {
                    this.km += 0.2f;
                    this.price_per_km = this.GetComponent<Km_list>().get_price_by_km(this.km);
                    this.show_digit(this.price_per_km.ToString(), this.digit_control[1].area_digit);
                    this.show_digit(this.km.ToString("F2"), this.digit_control[3].area_digit);
                }
            }

            this.speed_time_pending_bill += 1 * Time.deltaTime;
            if (this.speed_time_pending_bill > 8f)
            {
                this.cal_bill();
                this.speed_time_pending_bill = 0f;
            }
        }
    }

    private void on_check_exit_app()
    {
         if (this.history.panel_history.activeInHierarchy)
        {
            this.btn_close_history();
            this.carrot.set_no_check_exit_app();
        }else if (this.GetComponent<Km_list>().panel_km.activeInHierarchy)
        {
            this.GetComponent<Km_list>().btn_close_list_km();
            this.carrot.set_no_check_exit_app();
        }
    }

    private void show_digit(string s_show,Transform tr)
    {
        this.carrot.clear_contain(tr);
        int s_length = s_show.Length;
        string s_n;
        for(int i = 0; i < s_length; i++)
        {
            GameObject obj_n = Instantiate(this.obj_digit_n);
            obj_n.transform.SetParent(tr);
            obj_n.transform.localScale = new Vector3(1f, 1f, 1f);

            s_n = s_show[i].ToString();
            if (s_n == ":")
            {
                obj_n.GetComponent<Image>().sprite = this.sp_digit_number[10];
            }
            else if (s_n == "h")
            {
                obj_n.GetComponent<Image>().sprite = this.sp_digit_number[11];
                obj_n.GetComponent<RectTransform>().sizeDelta = new Vector2(140f, 10f);
            }
            else if (s_n == "."||s_n==",")
            {
                obj_n.GetComponent<Image>().sprite = this.sp_digit_number[12];
            }
            else
            {
                int n = int.Parse(s_show[i].ToString());
                obj_n.GetComponent<Image>().sprite = this.sp_digit_number[n];
            }
        }
    }

    public void btn_play()
    {
        if (this.status_play == 0)
        {
            this.show_digit("0.0", this.digit_control[2].area_digit);
            this.status_play = 1;
            if (this.carrot.model_app == Carrot.ModelApp.Develope)
            {
                distance_data d = this.get_distance_test();
                this.p_location_lat_start = d.lat;
                this.p_location_lon_start = d.lon;
            }
            else
            {
                this.p_location_lat_start = this.GetComponent<Manager_location>().latA;
                this.p_location_lon_start = this.GetComponent<Manager_location>().lonA;
            }

        }
        else if(this.status_play == 1)
        {
           this.status_play = 2;
        }else if (this.status_play == 2)
        {
            this.status_play = 1;
        }

        this.carrot.play_sound_click();
        this.btn_obj_stop.SetActive(true);
        this.check_statu_play();
    }

    private void check_statu_play()
    {
        if (this.status_play == 0)
        {
            this.img_btn_play.sprite = this.sp_taxi_play;
            this.txt_btn_play.text = PlayerPrefs.GetString("begin", "Begin");
            this.digit_control[0].reset_color();
        }

        if (this.status_play==1)
        {
            this.img_btn_play.sprite = this.sp_taxi_pause;
            this.txt_btn_play.text = PlayerPrefs.GetString("pause", "Pause");
            this.digit_control[0].change_color();
            this.carrot.ads.show_ads_Interstitial();
        }
        
        if(this.status_play==2)
        {
            this.img_btn_play.sprite = this.sp_taxi_play;
            this.txt_btn_play.text = PlayerPrefs.GetString("keep_running", "Keep Running");
            this.digit_control[0].change_color();
        } 
    }

    public void btn_show_setting()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.obj_effect_bloom.SetActive(false);
        this.box_setting=this.carrot.Create_Setting();
        box_setting.set_title(PlayerPrefs.GetString("setting","Setting"));

        Carrot.Carrot_Box_Item Item_price_currency = box_setting.create_item_of_top("Item_price_per_km");
        Item_price_currency.set_icon(this.GetComponent<Currency_list>().icon_Currency);
        Item_price_currency.set_title("Currency unit");
        Item_price_currency.set_tip("Please take a moment to rate this app (" + this.GetComponent<Currency_list>().get_cur_symbol_currency() + " - " + this.GetComponent<Currency_list>().get_cur_name_currency() + ")");
        Item_price_currency.set_key_lang_title("currency_unit");
        Item_price_currency.set_key_lang_tip("currency_unit_tip");
        Item_price_currency.set_act(this.GetComponent<Currency_list>().show_list_Currency);
        Item_price_currency.load_lang_data();

        Carrot.Carrot_Box_Btn_Item btn_list_Item_price_currency = Item_price_currency.create_item();
        btn_list_Item_price_currency.set_icon(this.sp_price_per_km_list);
        btn_list_Item_price_currency.GetComponent<Button>().enabled = false;

        Carrot.Carrot_Box_Item Item_price_per_km=box_setting.create_item_of_top("Item_price_per_km");
        Item_price_per_km.set_icon(this.sp_price_per_km);
        Item_price_per_km.set_title("Price per kilometer");
        Item_price_per_km.set_tip("Enter another value to change the price ("+this.price_per_km+")");
        Item_price_per_km.set_key_lang_title("price_per_km");
        Item_price_per_km.set_key_lang_tip("price_per_km_tip");
        Item_price_per_km.set_act(this.GetComponent<Km_list>().btn_show_list_km);
        Item_price_per_km.load_lang_data();

        Carrot.Carrot_Box_Btn_Item btn_list_price_per_km= Item_price_per_km.create_item();
        btn_list_price_per_km.set_icon(this.sp_price_per_km_list);
        btn_list_price_per_km.GetComponent<Button>().enabled = false;

        box_setting.update_color_table_row();
        box_setting.set_act_before_closing(act_close_setting);
    }

    private void act_close_setting()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.obj_effect_bloom.SetActive(true);
    }

    public void btn_show_rate_app()
    {
        this.carrot.show_rate();
    }

    public void btn_stop()
    {
        float lon_end,lat_end;
        this.status_play = 0;
        this.carrot.play_sound_click();
        this.carrot.play_vibrate();
        this.btn_obj_stop.SetActive(false);
        this.check_statu_play();

        if (this.carrot.model_app == Carrot.ModelApp.Develope)
        {
            distance_data d = this.get_distance_test();
            lon_end = d.lon;
            lat_end = d.lat;
        }
        else
        {
            lon_end = this.GetComponent<Manager_location>().lonA;
            lat_end = this.GetComponent<Manager_location>().latA;
        }

        this.history.add(this.time_second, this.km, this.price_per_km, this.GetComponent<Km_list>().get_price_by_km(this.km), 1.2f, DateTime.Now.ToString(), this.p_location_lon_start, this.p_location_lat_start, lon_end, lat_end, this.GetComponent<Currency_list>().get_cur_symbol_currency());


        this.btn_obj_print.SetActive(true);
        this.btn_obj_new_guest.SetActive(true);
        this.btn_obj_play.SetActive(false);
    }

    public void btn_new_guest()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.btn_obj_play.SetActive(true);
        this.btn_obj_print.SetActive(false);
        this.btn_obj_new_guest.SetActive(false);
        this.show_digit("h", this.digit_control[2].area_digit);
        this.show_digit("00:00:00", this.digit_control[0].area_digit);
        this.km = 0f;
        this.time_second = 0f;
        this.bill_price = 0f;
        this.GetComponent<Manager_location>().reset_location();
        this.GetComponent<Km_list>().reset_list_km();
    }

    public void btn_show_history()
    {
        this.history.show_history();
        this.carrot.play_sound_click();
        this.carrot.ads.show_ads_Interstitial();
    }

    public void btn_close_history()
    {
        this.history.close();
        this.panel_main.SetActive(true);
        this.carrot.play_sound_click();
        this.carrot.ads.show_ads_Interstitial();
    }

    private void cal_bill()
    {
        if (this.km < 1.2f)
            bill_price = this.km / this.price_per_km;
        else
            bill_price = this.km * this.price_per_km;

        this.show_digit(bill_price.ToString("F2"), this.digit_control[2].area_digit);
    }

    public void show_digit_price_bill(float f_km)
    {
        this.km = f_km / 1000;
        this.price_per_km = this.GetComponent<Km_list>().get_price_by_km(f_km);
        this.show_digit(this.km.ToString("F2"), this.digit_control[3].area_digit);
    }

    public int get_status_play()
    {
        return this.status_play;
    }

    public void btn_app_share()
    {
        this.carrot.show_share();
    }

    public void btn_app_other()
    {
        this.carrot.show_list_carrot_app();
    }

    public void btn_print_invoice_printing()
    {
        string mobile_num = "+1";
        string message = PlayerPrefs.GetString("invoice_summary", "Invoice Summary")+"\n-------------------\n";
        message = message+ PlayerPrefs.GetString("invoice_date", "Invoice Date") + ":"+ DateTime.Now.ToString("M/d/yyyy")+"\n";
        message = message+ PlayerPrefs.GetString("kilometro", "Kilometro") + ":" + this.km.ToString("F2") + "\n";
        TimeSpan t = TimeSpan.FromSeconds(time_second);
        message = message+ PlayerPrefs.GetString("running_time", "Running time") + ":" + string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds) + "\n";
        if(this.GetComponent<Km_list>().get_length()>0)
        message = message+ PlayerPrefs.GetString("km_promotional_price", "Promotional price of the road") + ":" + this.GetComponent<Km_list>().get_price_by_km(this.km).ToString("F2") + "\n";
        message = message+ PlayerPrefs.GetString("total_amount", "Total amount") + ":" + this.bill_price.ToString("F2") + " "+this.GetComponent<Currency_list>().get_cur_symbol_currency() + "\n";
        string URL = string.Format("sms:{0}?body={1}", mobile_num, message);
        Application.OpenURL(URL);
    }

    public void btn_del_log_Invoice()
    {
        this.history.delete_all_history();
        this.carrot.play_sound_click();
        this.carrot.show_msg("Clear invoice history", "Delete invoice history successfully!",Carrot.Msg_Icon.Success);
    }

    public void btn_show_sel_lang()
    {
        this.carrot.show_list_lang();
    }

    public void set_price_per_km(float price)
    {
        this.price_per_km = price;
        this.show_digit(this.price_per_km.ToString(), this.digit_control[1].area_digit);
    }

    public void btn_show_list_currency()
    {
        this.carrot.play_sound_click();
        this.GetComponent<Currency_list>().show_list_Currency();
    }

    public distance_data get_distance_test()
    {
        int r_distance = UnityEngine.Random.Range(0, this.list_distance_test.Count);
        return this.list_distance_test[r_distance];
    }

    public float get_km()
    {
        return this.km;
    }

    public Carrot.Carrot_Box get_box_setting()
    {
        return this.box_setting;
    }
}

public class distance_data
{
    public float lon;
    public float lat;
}