using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class AutoLotService : IAutoLotService
{
    public List<InvetoryRecord> GetInvetory()
    {
        List<InvetoryRecord> record = new List<InvetoryRecord>();

        var invetoryrec = new InvetoryRecord {
            
             Color = "Red",
             Make = "BMW",
             PetName = "Milou"

        };

        record.Add(invetoryrec);


        return record;

    }

    public void InsertCar(string make, string color, string petname)
    {
        Console.WriteLine("Inserting Car");
    }

    public void InsertCar(InvetoryRecord car)
    {
        Console.WriteLine("Inserting car in details");
    }
}
