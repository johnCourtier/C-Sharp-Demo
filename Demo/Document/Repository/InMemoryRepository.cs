using Microsoft.Extensions.Caching.Memory;

namespace Demo.Document.Repository
{
    public class InMemoryRepository : IDocumentRepository
    {
        private readonly IMemoryCache MemoryCache;

        public InMemoryRepository(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        public void Create(DocumentEntity document)
        {
            string id = document.Id;
            if (MemoryCache.TryGetValue(id, out DocumentEntity existingDocument))
            {
                throw new RuntimeException($"Unable to create document with '{id}' id. Such document already exists: '{existingDocument}'.");
            }

            MemoryCache.Set(document.Id, document);
        }

        public void Update(DocumentEntity document)
        {
            string id = document.Id;
            if (!MemoryCache.TryGetValue(id, out DocumentEntity existingDocument))
            {
                throw new RuntimeException($"Unable to update document with '{id}' id. Such document does not exist.");
            }

            MemoryCache.Set(document.Id, document);
        }

        public DocumentEntity Read(Criteria criteria)
        {
            if (criteria.Id == null)
            {
                throw new RuntimeException($"Unable to provide document. Repository '{GetType()}' can not process criteria {criteria}");
            }

            string id = criteria.Id;
            if (!MemoryCache.TryGetValue(id, out DocumentEntity document))
            {
                throw new RuntimeException($"Unable to provide document with '{id}' id. No such document exists.");
            }

            return document;
        }
    }
}