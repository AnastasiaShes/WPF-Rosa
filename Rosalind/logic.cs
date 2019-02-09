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
    
}

public class Information
{
    private readonly string name;
    private readonly string index;
    private readonly string condition;
    private readonly string text;

    public Information(string name, string index, string condition, string text)
    {
        this.name = name;
        this.index = index;
        this.condition = condition;
        this.text = text;
    }

    public string Name
    {
        get { return name; }
        set { }
    }

    public string Index
    {
        get { return index; }
        set { }
    }

    public string Condition
    {
        get { return condition; }
        set { }
    }

    public string Text
    {
        get { return text; }
        set { }
    }
}
