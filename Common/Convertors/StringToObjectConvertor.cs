using System.ComponentModel;

namespace Common.Convertors
{
    public static class StringToObjectConvertor
    {
        public static bool TryParseToObject<T>(this string str, out T? result)
        {
            try
            {
                var convert = TypeDescriptor.GetConverter(typeof(T));
                if (convert.ConvertFromString(str) is T res)
                {
                    result = res;
                    return true;
                }
            }
            catch{}
            
            result = default;
            return false;
        }
    }
}
