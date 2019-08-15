using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
[ServiceContract]
public interface IAutoLotService
{

    [OperationContract]
    void InsertCar(string make, string color , string petname);

    [OperationContract(Name = "InsertCarWithDetails")]
    void InsertCar(InvetoryRecord car);

    [OperationContract]
    List<InvetoryRecord> GetInvetory();


}

