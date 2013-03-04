using Newtonsoft.Json;
namespace ES.Lab.Infrastructure
{
    public static class Extensions
    {
         public static string ToJson(this object self)
         {
             return JsonConvert.SerializeObject(self);
         }
    }
}