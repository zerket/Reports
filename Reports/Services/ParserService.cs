using Newtonsoft.Json;

namespace Reports.Services
{
    public static class ParserService
    {
        public static bool Process<T>(this string json, out T result)
        {
            bool success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            result = JsonConvert.DeserializeObject<T>(json, settings);
            return success;
        }
    }
}
