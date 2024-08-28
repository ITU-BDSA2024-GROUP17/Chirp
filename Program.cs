using System.Collections;
using System.Text.RegularExpressions;


try
{

    Dictionary<string, ArrayList> map = new Dictionary<string, ArrayList>();

    using (StreamReader reader = new StreamReader("chirp_cli_db.csv"))
    {
        string line; 

        string headers = reader.ReadLine();
        
        foreach (var item in headers.Split(','))
        {
            map.Add(item, new ArrayList());
        }

        while ((line = reader.ReadLine()) != null)
        {            
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            //Separating columns to array
            string[] X = CSVParser.Split(line);
            

        }
    }

}
catch (IOException e)
{
    Console.WriteLine("The file could not be read:");
    Console.WriteLine(e.Message);
}