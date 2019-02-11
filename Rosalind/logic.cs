using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

class logic
{
    private string answer;

    public string Decision(string id, string variable)
    {
        switch(id)
        {
            case "DNA ": answer = CountingDNA(variable); //Подсчет нуклеотидов ДНК
                break;
            case "REVC": answer = "Решение для " + id + " с параметрами " + variable;
                break;
            case "RNA ": answer = "Решение для " + id + " с параметрами " + variable;
                break;
        }
        return answer;
    }

    #region Подсчет нуклеотидов ДНК
    private string CountingDNA(string per)
    {
        string count = ""; 

        List<char> list = new List<char>(per.ToList());

        int perA = (from a in list where a == 'A' || a == 'a' select a).Count(); //Кол-во эл-ов
        int perC = (from c in list where c == 'C' || c == 'c' select c).Count();
        int perG = (from g in list where g == 'G' || g == 'g' select g).Count();
        int perT = (from t in list where t == 'T' || t == 't' select t).Count();
        
        
        count = "Аденин (А): " + perA.ToString() + "\n" + "Цитозин (С): " + perC.ToString() + "\n" + "Гуанин (G): " + perG.ToString() + "\n" + "Тимин (T): " + perT.ToString() + "\n";

        List<char> list2 = new List<char> { 'A', 'a', 'C', 'c', 'G', 'g', 'T', 't'}; 

        var nums = list.Except(list2); //Вычитаем эл-ы из первой коллекции

        string lastPer = "";
        foreach (char n in nums)
        {
            lastPer += n + ", ";
        }

        count += "\nЭлементы, которые не учитывались: " + lastPer;

        return count;
    }
    #endregion
}


