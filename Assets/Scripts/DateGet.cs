using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateGet : MonoBehaviour
{
    public static string ManufactureDate;
    public static string QualityGuaranteePeriod;
    public string Datecut(string result)
    {
        string back = "";
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i] == '保')
            {
                if (result[i + 1] == '质')
                {
                    if (result[i + 2] == '期')
                    {
                        //开始做寻找时间的处理
                        i++;
                        while (result[i] != '月' && result[i] != '年')
                        {
                            if (result[i] >= '0' && result[i] <= '9')
                            {
                                back += result[i];
                            }
                        }
                        //如果检测到了月或年就跳出，此时result里面存储的就是数字
                        break;
                    }
                }
            }
        }
        return back;
    }
    public static string Datecut2(string result)
    {
        string back = "";
        int count = 0;//生产日期最多存储8个数字
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i] >= '0' && result[i] <= '9' && count <= 8)
            {
                count++;
                back += result[i];
            }
        }
        return back;
    }
    public static string restdate()//b为生产日期，a为保质期
    {
        string back = "";
        //传入保质期和生产日期

        int year1 = 0;//计算出年

        for (int i = 0; i < 4; i++)
        {
            year1 *= 10;
            year1 += ManufactureDate[i] - '0';
        }
        int month1 = 0;
        for (int i = 4; i < 6; i++)
        {
            month1 *= 10;
            month1 += ManufactureDate[i] - '0';
        }
        int day1 = 0;
        for (int i = 6; i < 8; i++)
        {
            day1 *= 10;
            day1 += ManufactureDate[i] - '0';
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
        for (int i = 0; i < QualityGuaranteePeriod.Length; i++)
        {
            baozhi *= 10;
            baozhi += QualityGuaranteePeriod[i] - '0';
        }
        res = baozhi * 30 - res;//计算出还剩多少天
                                //输出一下
        Debug.Log(res);
        return res.ToString();
    }
}
