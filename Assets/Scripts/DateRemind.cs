using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateRemind : MonoBehaviour
{
    //����������������Ҫ����update����ʵʱ���£�����ȡ�����������ںͱ����ڴ��룬Ȼ������ʱ���
    public static string restdate(string a, string b)//bΪ�������ڣ�aΪ������
    {
        string back = "";
        //���뱣���ں���������

        int year1 = 0;//�������

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
        for (int i = 0; i < a.Length; i++)
        {
            baozhi *= 10;
            baozhi += a[i] - '0';
        }
        res = baozhi * 30 - res;//�������ʣ������
                                //���һ��
        Debug.Log(res);
        return res.ToString();
    }
}
