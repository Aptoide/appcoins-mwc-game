using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CSVWriter : MonoBehaviour
{
    public string[] firstRowData;
    public string filenameNoExtension;

    public void AddNewEntry(string[] values)
    {
        if (firstRowData.Length != values.Length) {
            Debug.LogError("trying to write a row with a different column number than the first row! Should have "
                           + firstRowData.Length + " columns but has " + values.Length);
        }

        StreamWriter outStream = null;
        StreamWriter outStreamBackup = null;

        string backupFilename = filenameNoExtension + "_backup";

        //Check if file exists, if it doesn't create it and add first line
        if (!System.IO.File.Exists(GetPath(filenameNoExtension))) {
            outStream = CreateCSVFile(filenameNoExtension);
        } else {
            //Else open the existing file and add the new entry
            outStream = new StreamWriter(GetPath(filenameNoExtension),true);
        }

        string line = CreateLineEntry(values);

        outStream.WriteLine(line);
        outStream.Close();

        //Check if file exists, if it doesn't create it and add first line
        if (!System.IO.File.Exists(GetPath(backupFilename)))
        {
            outStreamBackup = CreateCSVFile(backupFilename);
        }
        else
        {
            //Else open the existing file and add the new entry
            outStreamBackup = new StreamWriter(GetPath(backupFilename), true);
        }

        line = CreateBackupLineEntry(values);

        outStreamBackup.WriteLine(line);
        outStreamBackup.Close();
    }

    private string CreateLineEntry(string[] entry) {
        string delimiter = ",";
        string line = "";
        for (int index = 0; index < entry.Length; index++)
            line += delimiter + entry[index];

        return line;
    }

    private string CreateBackupLineEntry(string[] entry)
    {
        string line = CreateLineEntry(entry);

        //Prepend the timestamp to have the scores sorted by date
        string date = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
        line = "," + date + line;

        return line;
    }

    private StreamWriter CreateCSVFile(string filename) {
        StreamWriter outStream = System.IO.File.CreateText(GetPath(filename));

        string line = CreateLineEntry(firstRowData);
        outStream.WriteLine(line);

        return outStream;
    }

    // Following method is used to retrive the relative path as device platform
    public string GetPath(string filename)
    {
#if UNITY_EDITOR
        return Application.dataPath+"/" + filename + ".csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath + filename + ".csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+ filename + ".csv";
#else
        return Application.dataPath +"/"+ filename + ".csv";
#endif
    }
}