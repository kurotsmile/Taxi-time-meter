using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Digit_Control : MonoBehaviour
{
    public Transform area_digit;
    public Color32 color_change;

    public void change_color()
    {
        foreach(Transform obj_digit in this.area_digit)
        {
            obj_digit.gameObject.GetComponent<Image>().color = this.color_change;
        }
    }

    public void reset_color()
    {
        foreach (Transform obj_digit in this.area_digit)
        {
            obj_digit.gameObject.GetComponent<Image>().color = Color.white;
        }
    }
}
