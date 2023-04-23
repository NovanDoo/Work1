using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateRemind : MonoBehaviour
{
    //下面这个程序可能需要放在update里面实时更新，将获取到的生产日期和保质期传入，然后计算出时间差
    public static string restdate(string a, string b)//b为生产日期，a为保质期
    {
        string back = "";
        //传入保质期和生产日期

        int year1 = 0;//计算出年

        for (int i = 0; i < 4; i++)
        {

            year1 *= 10;
            year1 += b[i] - '0';

        }
        int month1 = 0;
        for (int i = 4; i < 6; i++)
        {
            month1 *= 10;
            month1 += b[i] - '0';

        }
        int day1 = 0;
        for (int i = 6; i < 8; i++)
        {
            day1 *= 10;
            day1 += b[i] - '0';

        }
        //下面4行代码计算出了生产日期和现在的时间差
        DateTime date1 = new DateTime(year1, month1, day1);
        DateTime date2 = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
        TimeSpan interval = date2 - date1;
        Debug.Log(date2.ToString() + date1.ToString() + interval.ToString());
        //下面的代码用保质期减时间差计算出剩余的天数
        back = interval.ToString();
        int res = 0;
        int j = 0;
        while (back[j] != '.')
        {
            res *= 10;
            res += back[j] - '0';
            j++;
        }
        int baozhi = 0;
        for (int i = 0; i < a.Length; i++)
        {
            baozhi *= 10;
            baozhi += a[i] - '0';
        }
        res = baozhi * 30 - res;//计算出还剩多少天
                                //输出一下
        Debug.Log(res);
        return res.ToString();
    }
}
