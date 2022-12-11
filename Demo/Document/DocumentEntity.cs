using Newtonsoft.Json.Linq;

namespace Demo.Document
{
    public class DocumentEntity
    {
        public String Id { get; init; }

        public String[] Tags { get; init; }

        public JContainer Data { get; init; }
    }
}