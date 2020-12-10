namespace IwbZero.AppServiceBase
{
    public interface IIwbEntityDto<TPrimaryKey>
    where TPrimaryKey:struct
    {
        TPrimaryKey? Id { get; set; }
    }
    public class IwbEntityDto<TPrimaryKey>:IIwbEntityDto<TPrimaryKey>
    where TPrimaryKey:struct
    {
       public TPrimaryKey? Id { get; set; }
    }
}