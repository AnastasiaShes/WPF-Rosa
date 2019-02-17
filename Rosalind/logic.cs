using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;

class logic
{
    private string answer;
    private string message = "";

    public string Decision(string id, string variable)
    {
        switch(id)
        {
            case "DNA ": answer = CountingDNA(variable); //Подсчет нуклеотидов ДНК
                break;
            case "RNA ": answer = TranscribindDNA(variable); //Перевод ДНК в РНК
                break;
            case "REVC": answer = ComplementDNA(variable); //Обратное дополнение строки
                break;
            case "PROT": answer = TranscribindProtein(variable); //Перевод РНК в Белок
                break;
            case "GC  ": answer = GCcontent(variable); //% содержание GC
                break;
            case "SUBS": answer = MotifDNA(variable); //Нахождение мотива ДНК
                break;
                
        }
        return answer;
    }

    #region Подсчет нуклеотидов ДНК
    private string CountingDNA(string per)
    {       

        List<char> list = new List<char>(per.ToList());

        int perA = (from a in list where a == 'A' || a == 'a' select a).Count(); //Кол-во эл-ов
        int perC = (from c in list where c == 'C' || c == 'c' select c).Count();
        int perG = (from g in list where g == 'G' || g == 'g' select g).Count();
        int perT = (from t in list where t == 'T' || t == 't' select t).Count();


        message = "Аденин (А): " + perA.ToString() + "\n" + "Цитозин (С): " + perC.ToString() + "\n" + "Гуанин (G): " + perG.ToString() + "\n" + "Тимин (T): " + perT.ToString() + "\n";

        List<char> list2 = new List<char> { 'A', 'a', 'C', 'c', 'G', 'g', 'T', 't'}; 

        var nums = list.Except(list2); //Вычитаем эл-ы из первой коллекции

        string lastPer = "";
        foreach (char n in nums)
        {
            lastPer += n + ", ";
        }
        if (lastPer != "")
        {
            message += "\nЭлементы, которые не учитывались: " + lastPer;
        }

        list.Clear(); list2.Clear();
        return message;
    }
    #endregion

    #region Перевод ДНК в РНК
    private string TranscribindDNA(string per)
    {

        List<char> list = new List<char>(per.ToList());
        List<char> list2 = new List<char>();

        message = "ДНК цепь: ";
        foreach (var u in list)
        {
            if (u == 'A' || u == 'C' || u == 'G' || u == 'T' || u == 'a' || u == 'c' || u == 'g' || u == 't')
            {
                message += u;
                list2.Add(u);
            }
        }

        message += "\nРНК цепь: ";
        foreach (var x in list2)
        {
            if 
                (x == 'T')  message += "U";         
            else if 
                (x == 't') message += 'u';
            else
                message += x;
        }

        var nums = list.Except(list2); //Вычитаем эл-ы из первой коллекции

        string lastPer = "";
        foreach (char n in nums)
        {
            lastPer += n + ", ";
        }
        if (lastPer != "")
        {
            message += "\n\nЭлементы, которые не учитывались: " + lastPer;
        }

        list.Clear(); list2.Clear();  //Очищаем коллекции
        return message;
    }
    #endregion

    #region Обратное дополнение строки
    private string ComplementDNA (string per)
    {

        List<char> list = new List<char>(per.ToList());
        Stack<char> stackDNA = new Stack<char>();

        string DNA = "";
        foreach (var u in list)
        {
            if (u == 'A' || u == 'C' || u == 'G' || u == 'T' || u == 'a' || u == 'c' || u == 'g' || u == 't')
            {
                DNA += u;
                stackDNA.Push(u);
            }
        }

        message += "Цепь ДНК: " + DNA.ToUpper();

        string AND = "";
        foreach (var c in stackDNA)
        {
            if      (c == 'A' || c == 'a') AND += 'T';
            else if (c == 'C' || c == 'c') AND += 'G';
            else if (c == 'G' || c == 'g') AND += 'C';
            else if (c == 'T' || c == 't') AND += 'A';

        }

        message += "\nОбратное дополнение: " + AND;

        var nums = list.Except(stackDNA); //Вычитаем эл-ы из первой коллекции

        string lastPer = "";
        foreach (char n in nums)
        {
            lastPer += n + ", ";
        }
        if (lastPer != "")
        {
            message += "\n\nЭлементы, которые не учитывались: " + lastPer;
        }
        return message;
    }
    #endregion

    #region Перевод РНК в Белок
    private string TranscribindProtein(string per)
    {
        per = per.ToUpper();
        string error = "";
        string variable = "";

        //Все удовлетворающие символы будут храниться в переменной variable
        foreach (var x in per)
        {
            if (x == 'A' || x == 'C' || x == 'G' || x == 'U')
                variable += x;
            else
                error += x + ", ";
        }

        string table = File.ReadAllText("RNAintoProtein.txt"); //Тут хранится таблица

        //Тут мы делем переменную от пользователя на группы по три символа
        var PerUser = (variable.ToUpper()).Select((c, index) => new { c, index })
            .GroupBy(x => x.index / 3)
            .Select(group => group.Select(elem => elem.c))
            .Select(chars => new string(chars.ToArray()));

        //Тут поиск
        foreach (var str in PerUser)
        {
            int index = table.IndexOf(str);
            if (index != -1)               
                message += table[index + 4];
            else
                error += str + ", ";
        }

        if (error != "")
            message += "\nЭлементы, которые не учитывались: " + error;

        return message;
    }
    #endregion

    #region % содержание GC
    private string GCcontent(string per)
    {

        double countPer = (from x in per.ToUpper() where x == 'A' || x == 'C' || x == 'G' || x == 'T' select x).Count(); //Кол-во всех ДНК эл-ов 

        if (countPer != 0)
        {
            double perC = (from c in per.ToUpper() where c == 'C' || c == 'c' select c).Count(); //Кол-во цитозина
            double perG = (from g in per.ToUpper() where g == 'G' || g == 'g' select g).Count(); //Кол-во гуанина

            double Cuntent = ((perC + perG) / countPer) * 100;
            Cuntent = Math.Round(Cuntent, 5); //Округление

            message = "Содержание GC = " + Cuntent.ToString() + " %";

            List<char> list = new List<char> { 'A', 'a', 'C', 'c', 'G', 'g', 'T', 't' };

            var nums = per.Except(list); //Вычитаем эл-ы

            string lastPer = "";
            foreach (var x in nums)
            {
                lastPer += x + ", ";
            }

            if (lastPer != "")
                message += "\n\nЭлементы, которые не учитывались: " + lastPer;
        }
        else
            message = "Введите строку ДНК";
        
        return message;
    }

    #endregion

    #region Нахождение мотива в ДНК
    private string MotifDNA (string per)
    {
        if (per.IndexOf('(') != -1 && per.IndexOf(')') != -1 && per.IndexOf('(') < per.IndexOf(')')) 
        {
            string СlearedStr = "";
            string Garbage = "";
            foreach (var x in per.ToUpper())
            {
                if (x == 'A' || x == 'C' || x == 'G' || x == 'T' || x == '(' || x == ')')
                    СlearedStr += x;
                else
                    Garbage += x;
            }

            int endMotif = СlearedStr.IndexOf(')'); //конец очищенной строки
            СlearedStr = СlearedStr.Remove(endMotif);

            int endDNA = СlearedStr.IndexOf('('); //конец строки ДНК
            int lenStr = СlearedStr.Length; //Длина всей строки
            string DNAstr = СlearedStr.Remove(endDNA); //Строка ДНК
            string MotifStr = СlearedStr.Substring(endDNA + 1, (lenStr - (endDNA + 1))); //Строка мотива

            message += "ДНК: " + DNAstr;
            message += "\nМотив: " + MotifStr;

            if (DNAstr.IndexOf(MotifStr) != -1)
            {

                message += "\n\nСовпадение на: ";

                int index = 0;
                while ((index = DNAstr.IndexOf(MotifStr, index)) != -1)
                {
                    message += (index + 1) + ", ";
                    index += MotifStr.Length;
                }
                message = message.Remove(message.Length - 2);
                message += " позиции ";
            }
            else
                message += "\n\nСовпадений не найдено.";
        }
        else
            message = "Данные не удовлетворяют условию.";

        return message;
    }
    #endregion
}


