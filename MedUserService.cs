using System;
using System.Collections.Generic;
using System.IO;

class MedUserService
{
    private string path = "MedUser.txt";

    public List<User> LoadDoctors()
    {
        List<User> doctors = new List<User>();

        if (!File.Exists(path))
            return doctors;

        var lines = File.ReadAllLines(path);

        foreach (var line in lines)
        {
            doctors.Add(User.FromFileString(line));
        }

        return doctors;
    }

    public void SaveDoctor(User doctor)
    {
        File.AppendAllText(path, doctor.ToFileString() + "\n");
    }
    public void SaveAllDoctors(List<User> doctors)
    {
        List<string> lines = new List<string>();

        foreach (var d in doctors)
        {
            lines.Add(d.ToFileString());
        }

          File.WriteAllLines("MedUser.txt", lines);

    }
}
    
