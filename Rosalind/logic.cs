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
            case "HAMM": answer = CountMutation(variable); //Подсчет точечных мутаций
                break;
            case "PRTM":  answer = ProteinMass(variable); //Расчет массы белка
                break;
            case "CONS": answer = ConsensusProfile(variable); //Консенсус и профиль
                break;
        }
        return answer;
    }

    #region Очистка ДНК от мусора
    private string ClearedDNA (string str)
    {
        string chainDNA = "";

        List<char> strDNA = new List<char> { 'A', 'a', 'C', 'c', 'G', 'g', 'T', 't' };

        foreach (var x in str)
        {
            if (strDNA.Contains(x)) chainDNA += x;
        }

        return chainDNA;
    }
    private string ClearedDNA(string str, char separator)
    {
        string chainDNA = "";

        List<char> strDNA = new List<char> { 'A', 'a', 'C', 'c', 'G', 'g', 'T', 't', separator };

        foreach (var x in str)
        {
            if (strDNA.Contains(x)) chainDNA += x;
        }

        return chainDNA;
    }
    #endregion    

    #region Подсчет нуклеотидов ДНК
    private string CountingDNA(string per)
    {

        List<char> strDNA = new List<char>(ClearedDNA(per).ToList()); //ClearedDNA(per) это очищенная строка ДНК

        int perA = (from a in strDNA where a == 'A' || a == 'a' select a).Count(); //Кол-во эл-ов
        int perC = (from c in strDNA where c == 'C' || c == 'c' select c).Count();
        int perG = (from g in strDNA where g == 'G' || g == 'g' select g).Count();
        int perT = (from t in strDNA where t == 'T' || t == 't' select t).Count();
        
        message += "Аденин (А): " + perA.ToString() + "\nЦитозин (С): " + perC.ToString() + "\nГуанин (G): " + perG.ToString() + "\nТимин (T): " + perT.ToString() + "\n";

        string notDNA = "";

        var nums = per.Except(strDNA); //Вычитаем эл-ы из первой коллекции
        foreach (var x in nums) notDNA += x + ", ";

        if (notDNA != "")
        {
            message += "\nЭлементы, которые не учитывались: " + notDNA;
            message = message.Remove(message.Length - 2); //Удаляем последнюю запятую
        }

        strDNA.Clear();

        return message;
    }
    #endregion    

    #region Перевод ДНК в РНК
    private string TranscribindDNA(string per)
    {
        List<char> strDNA = new List<char>(ClearedDNA(per).ToList()); //ClearedDNA(per) это очищенная строка ДНК

        message = "ДНК цепь: " + ClearedDNA(per);

        message += "\nРНК цепь: ";

        foreach (var x in strDNA)
        {
            if 
                (x == 'T')  message += "U";         
            else if 
                (x == 't') message += 'u';
            else
                message += x;
        }

        string notDNA = "";

        var nums = per.Except(strDNA); //Вычитаем эл-ы из первой коллекции
        foreach (var x in nums) notDNA += x + ", ";

        if (notDNA != "")
        {
            message += "\n\nЭлементы, которые не учитывались: " + notDNA;
            message = message.Remove(message.Length - 2); //Удаляем последнюю запятую
        }

        strDNA.Clear();

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

        //Тут мы делим переменную от пользователя на группы по три символа
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

    #region Подсчет точечных мутаций
    private string CountMutation(string per)
    {
        string DNA = ClearedDNA(per, '/');

        string[] strDNA = DNA.Split(new char[] { '/' });

        try
        {
            message = "Строка 1: "+strDNA[0] + "\nСтрока 2: " + strDNA[1];

            if (DNA.IndexOf('/') == DNA.LastIndexOf('/'))
            {
                message += "\n\nМутации на ";

                int countMut = 0;

                if (strDNA[0].Length == strDNA[1].Length)
                {
                    char[] oneDNA = strDNA[0].ToUpper().ToCharArray();
                    char[] twoDNA = strDNA[1].ToUpper().ToCharArray();

                    for (int i = 0; i < strDNA[0].Length; i++)
                    {
                        if (oneDNA[i] != twoDNA[i])
                        {
                            message += i + 1 + ", ";
                            countMut++;
                        }
                    }
                    if (countMut > 0)
                    {
                        message = message.Remove(message.Length - 2);
                        message += " позиции";
                        message += "\nКолличество мутаций: " + countMut.ToString();
                    }
                    else
                    {
                        message = message.Remove(message.Length - 3);
                        message += "отсутствуют";
                    }
                }
                else
                    message = "Строки ДНК должны быть равной длины.";
            }
            else
                message = "Данные не удовлетворяют условию. \nВ строке должен быть только один разделитель";
        }
        catch(IndexOutOfRangeException)
        {
            message = "Данные не удовлетворяют условию. \nВозможно Вы не поставили разделитель '/' ";
        }
        return message;
    }
    #endregion

    #region Расчет массы белка
    private string ProteinMass(string per)
    {
        string PerUser = ""; 
        string error = ""; //Для мусора
        //Оставляем в строке только буквы
        foreach (var x in per)
        {
            if (Char.IsLetter(x)) PerUser += x;
            else error += x;
        }

        PerUser = PerUser.ToUpper(); //Переводим все символы в верхний регистр
        double mass = 0; //Масса белковой строки
        string table = File.ReadAllText("MonoisotopicMass.txt"); //Тут хранится таблица

        message += "Строка: ";
        //Тут поиск
        foreach (var str in PerUser)
        {
            int index = table.IndexOf(str);
            if (index != -1)
            {
                message += str;
                mass += Convert.ToDouble(table.Substring((index + 4), 9));
            }
            else
                error += str;
        }

        message += "\nМасса белка: " + mass.ToString() + " Da";

        if (error != "")
        {
            message += "\n\nЭлементы, которые не учитывались: ";

            error = new string(error.ToCharArray().Distinct().ToArray());//Удаляем повторяющиеся символы
            foreach (var x in error)
            {
                message += x + ", ";
            }

            message = message.Substring(0, message.Length - 2); //Удаляем последнюю запятую
        }
        return message;
    }
    #endregion

    #region Консенсус и профиль
    private string ConsensusProfile( string per)
    {
        string userPer = ClearedDNA(per, '/'); //Убираем эл-ты не из цепи ДНК

        string[] str = userPer.ToLower().Split('/');//Перевод строки в нижний регистр + разбиваем строку на подстроки

        List<int> mass = new List<int>(); //Тут будет строка с кол-вом эл на каждой позиции
        List<string> сonsensus = new List<string>(); //Тут будет строка с Consensus

        int countSep = (from a in userPer where a == '/' select a).Count(); //Кол-во разделителей
        int countLen = str[0].Length;//кол-во букв 

        //Ниже мы выводим строки
        for (int i = 0; i <= countSep; i++)
        {
            message += "Строка " + (i + 1) + ": " + str[i].ToUpper() + "\n";
        }

        //Ниже мы определяем длину каждой строки и если хоть одна не совпадает, то останавливаем решение
        bool stop = false;
        for (int g = 1; g < str.Count(); g++)
        {
            if (str[0].Length != str[g].Length)
            {
                message += "\nДанные не удовлетворяют условию.\nКоличество букв в строках должно совпадать.\nОбратите внимание на " + (g + 1) + " строку";
                stop = true;
            }
        }

        if (stop == false)
        {
            int perA, perC, perG, perT;

            for (int x = 0; x < countLen; x++) //буквы
            {
                perA = perC = perG = perT = 0;

                for (int y = 0; y <= countSep; y++) //строки
                {
                         if (str[y][x] == 'a') { perA++; }
                    else if (str[y][x] == 'c') { perC++; }
                    else if (str[y][x] == 'g') { perG++; }
                    else if (str[y][x] == 't') { perT++; }
                }

                mass.Add(perA); mass.Add(perC); mass.Add(perG); mass.Add(perT);

                     if (perA > perC && perA > perG && perA > perT) сonsensus.Add("A");
                else if (perC > perA && perC > perG && perC > perT) сonsensus.Add("C");
                else if (perG > perA && perG > perC && perG > perT) сonsensus.Add("G");
                else сonsensus.Add("T");

            }

            string strA = "", strC = "", strG = "", strT = "";

            for (int x = 0; x < mass.Count(); x = x + 4)
            {
                strA += mass[x] + " ";
                strC += mass[x + 1] + " ";
                strG += mass[x + 2] + " ";
                strT += mass[x + 3] + " ";
            }

            message += "\nA: " + strA + "\nC: " + strC + "\nG: " + strG + "\nT: " + strT;

            message += "\n\nКонсенсус: ";
            foreach(var x in сonsensus)
            {
                message += x;
            }
        }
        return message;
    }
    #endregion
}


