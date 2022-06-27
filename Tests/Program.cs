using PowerSQL.DBMotors;
using PowerSQL.DBMotors.SQLServer;
using PowerSQL.Exceptions;
using PowerSQL.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


 string cnx = "Server=localhost;Initial Catalog=RICHARD;User Id=sa;" +
        "Password=Rivipe19866.;Persist Security Info=True;" +
        "MultipleActiveResultSets=True;";
PrintExceptions exceptions = new PrintExceptions();
GetPropertiesService<TEST> propertiesService = new GetPropertiesService<TEST>();
SQLParametersService sqlParametersService = new SQLParametersService(exceptions);
SerializeService serializeService = new SerializeService(exceptions);
DeserealizeService<TEST> deserealizeService = new DeserealizeService<TEST>(exceptions);

IDBActionQuery<TEST> query = new Select<TEST>(cnx, exceptions,
    propertiesService, sqlParametersService,serializeService,deserealizeService);

IDBActionNonQuery insert = new Insert<TEST>(cnx, exceptions,
    propertiesService,sqlParametersService);

IDBActionNonQuery update = new Update<TEST>(cnx, exceptions,
    propertiesService, sqlParametersService);

IDBActionNonQuery delete = new Delete<TEST>(cnx, exceptions, sqlParametersService);

IBulkInsert<TEST> bulkInsert = new BulkInsert<TEST>(cnx,exceptions);

List<TEST> list = new List<TEST>();

Stopwatch stopWatch = new Stopwatch();
//for (int i = 0; i < 1000000; i++)
//{

//    stopWatch.Start();
//    list.Add(new TEST { ID = i.ToString(), NOMBRE = "desc" + i.ToString() });
//var inserted = await insert.buildAndExecute(("@ID","1002"),("@NOMBRE","VICTOR"));
//var updated = await update.execute("update TEST set ID=@ID WHERE ID=@ID2",("@ID", "1001"), ("@ID2", "1002"));
//var deleted = await delete.buildAndExecute();
//    Console.WriteLine("inserted");
//}
//var res = await bulkInsert.BulkData(list);
//Console.WriteLine(res);
var result = await query.execute("SELECT * FROM TEST");

foreach (var item in result)
{
    stopWatch.Start();
    Console.WriteLine(item.ID + "---" + item.NOMBRE);
}
stopWatch.Stop();

TimeSpan ts = stopWatch.Elapsed;

// Format and display the TimeSpan value.
string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds / 10);
Console.WriteLine("RunTime " + elapsedTime);


Console.ReadKey();




class TEST
{
    public string? ID { get; set; }
    public string? NOMBRE { get; set; }
   
}



