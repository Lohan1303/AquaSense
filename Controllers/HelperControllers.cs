using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AquaSense
{
    public static class HelperControllers
    {
        //public static Boolean VerificaUserLogado(ISession session)
        //{
        //   string logado = session.GetString("Logado");
        //    if (logado == null)
        //       return false;
        //   else
        //        return true;
        //}

        public static void SetObject(this ISession session, string key, object value)
        {
            var json = JsonConvert.SerializeObject(value);
            session.SetString(key, json);
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }
    }

}

