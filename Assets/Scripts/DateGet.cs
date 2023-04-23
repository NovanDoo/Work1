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
            if (result[i] == '��')
            {
                if (result[i + 1] == '��')
                {
                    if (result[i + 2] == '��')
                    {
                        //��ʼ��Ѱ��ʱ��Ĵ���
                        i++;
                        while (result[i] != '��' && result[i] != '��')
                        {
                            if (result[i] >= '0' && result[i] <= '9')
                            {
                                back += result[i];
                            }
                        }
                        //�����⵽���»������������ʱresult����洢�ľ�������
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
        int count = 0;//�����������洢8������
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
    public static string restdate()//bΪ�������ڣ�aΪ������
    {
        string back = "";
        //���뱣���ں���������

        int year1 = 0;//�������

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
        //����4�д����������������ں����ڵ�ʱ���
        DateTime date1 = new DateTime(year1, month1, day1);
        DateTime date2 = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
        TimeSpan interval = date2 - date1;
        Debug.Log(date2.ToString() + date1.ToString() + interval.ToString());
        //����Ĵ����ñ����ڼ�ʱ�������ʣ�������
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
        res = baozhi * 30 - res;//�������ʣ������
                                //���һ��
        Debug.Log(res);
        return res.ToString();
    }
}
