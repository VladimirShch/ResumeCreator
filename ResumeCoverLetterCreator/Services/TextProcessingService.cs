using System.Text.RegularExpressions;

namespace ResumeCoverLetterCreator.Services
{
    public class TextProcessingService : ITextProcessingService
    {
        public string ProcessText(string text, Dictionary<string, string> tags)
        {
            var regExp = new Regex(@"\$[a-zA-Z0-9_]+");
            string result = regExp.Replace(text, match =>
            {
                if(tags.TryGetValue(match.Value.ToLower(), out string? res))
                {
                    if (char.IsUpper(match.Value[1]) && !string.IsNullOrEmpty(res))
                    {
                        res = char.ToUpper(res[0]) + (res.Length > 1 ? res[1..] : string.Empty);
                    }
                    res = ProcessText(res, tags);
                }
                return string.IsNullOrEmpty(res) ? match.Value : res;
            });

            return result;
        }
    }
}
