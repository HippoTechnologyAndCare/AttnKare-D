using UnityEngine;
using System;
using System.IO;
public class SoftwareTest : MonoBehaviour
{
    public static void CreateTXT(string log, bool set)
    {
        string filepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + log + ".txt";
        //FileStream test = new FileStream(filepath, FileMode.Create);
        if (File.Exists(filepath)) { StreamWriter test = File.AppendText(filepath); 
            if (set) { test.WriteLine("시작시간 " + DateTime.Now.ToString("MM/dd/hh/mm/ss"));  }
            if (!set) { test.WriteLine("종료시간 " + DateTime.Now.ToString("MM/dd/hh/mm/ss")); }
            test.Flush(); test.Close();
        }
        else { 
            StreamWriter test = File.CreateText(filepath);
            if (set) { test.WriteLine("시작시간 " + DateTime.Now.ToString("MM/dd/hh/mm/ss")); }
            if (!set) { test.WriteLine("종료시간 " + DateTime.Now.ToString("MM/dd/hh/mm/ss")); }
            test.Flush(); test.Close();
        }
    }

}