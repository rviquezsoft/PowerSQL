// See https://aka.ms/new-console-template for more information
using PowerSQL.DBMotors;
using PowerSQL.DBMotors.SQLServer;
using PowerSQL.Exceptions;
using PowerSQL.Interfaces;
using System.Data.SqlClient;

ISqlServerSelectService<dynamic> sqlServer = 
    new SqlServerSelectService<dynamic>(
        "Server=localhost;Initial Catalog=Tareas;User Id=sa;" +
        "Password=sa;Persist Security Info=True;" +
        "MultipleActiveResultSets=True;",
        new SerializeService(new PrintExceptions()),
        new DeserealizeService<dynamic>(new PrintExceptions()),
        new SQLParametersService(new PrintExceptions()),
        new GetPropertiesService<prueba>(), 
        new PrintExceptions());


//var result = await sqlServer
//    .select("select * from eidemo04.CLIENTE where nombre=@nombre");

List<dynamic> result = await sqlServer.selectAll();
foreach (var item in result)
{
    Console.WriteLine(item.ID+""+item.DESCRIPCION);
}
Console.ReadKey();



class prueba
{
    public string? ID { get; set; }
    public string? DESCRIPCION { get; set; }
}