using PowerSQL.DBMotors;
using PowerSQL.DBMotors.SQLServer;
using PowerSQL.Exceptions;
using PowerSQL.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;



IDBActionQuery<dynamic> query = new Select<dynamic>(
    "Server=localhost;Initial Catalog=Tareas;User Id=sa;" +
        "Password=sa;Persist Security Info=True;" +
        "MultipleActiveResultSets=True;", new PrintExceptions(),
    new GetPropertiesService<dynamic>(), new SQLParametersService(new PrintExceptions()),
    new SerializeService(new PrintExceptions()),
    new DeserealizeService<dynamic>(new PrintExceptions()));

IDBActionNonQuery insert = new Insert<prueba>(
    "Server=localhost;Initial Catalog=Tareas;User Id=sa;" +
        "Password=sa;Persist Security Info=True;" +
        "MultipleActiveResultSets=True;", new PrintExceptions(),
    new GetPropertiesService<prueba>(),
    new SQLParametersService(new PrintExceptions()));

List<prueba> objlist = new List<prueba>();

Stopwatch stopWatch = new Stopwatch();
for (int i = 0; i < 100; i++)
{

    stopWatch.Start();
    objlist.Add(new prueba { ID = i.ToString(), DESCRIPCION = "desc" + i.ToString() });
    //var inserted = await insert.execute("insert into prueba(ID,DESCRIPCION)VALUES(@ID,@DESC)",
    //("@ID", Guid.NewGuid().ToString()), ("@DESC", "registro # " + Guid.NewGuid().ToString()));
    Console.WriteLine("inserted");
}

//var result = await query.execute("SELECT ID,DESCRIPCION FROM prueba");

//foreach (var item in result)
//{
//    stopWatch.Start();
//    Console.WriteLine(item.ID + "" + item.DESCRIPCION);
//}
stopWatch.Stop();

TimeSpan ts = stopWatch.Elapsed;

// Format and display the TimeSpan value.
string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds / 10);
Console.WriteLine("RunTime " + elapsedTime);


Console.ReadKey();




class prueba
{
    public string? ID { get; set; }
    public string? DESCRIPCION { get; set; }
   
}



