namespace ShortURL.Services.Implementation
{
    public class UrlShortenerService
    {

        private const int NumberOfCharsInShortLink = 7;
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private readonly Random _random = new();

        public string GenerateUniqueCode()
        {
            var codeChars = new char[NumberOfCharsInShortLink];

            for (var i = 0; i < NumberOfCharsInShortLink; i++)
            {
                var randomIndex = _random.Next(Alphabet.Length - 1);

                codeChars[i] = Alphabet[randomIndex];

            }
            var code = new string(codeChars);
            return code;
        }
    }
}
