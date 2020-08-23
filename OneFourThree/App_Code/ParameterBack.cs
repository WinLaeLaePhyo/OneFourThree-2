using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace OneFourThree.App_Code
{
    public class ParameterBack
    {
        string[] Parameter = new string[20];
        public void setParameter(int i,string Value)
        {
            Parameter[i] = Value;
        }
        public string getParameter(int i)
        {
            return Parameter[i].ToString();
        }
    }
}