using Microsoft.Extensions.Caching.Memory;

namespace Demo.Document
{
    public class InMemoryRepository : IDocumentRepository
    {
        private readonly IMemoryCache MemoryCache;

        public InMemoryRepository(IMemoryCache memoryCache)
        {
            this.MemoryCache = memoryCache;
        }

        public void Create(DocumentEntity document)
        {
            String id = document.Id;
            if (this.MemoryCache.TryGetValue<DocumentEntity>(id, out DocumentEntity existingDocument))
            {
                throw new RuntimeException($"Unable to create document with '{id}' id. Such document already exists: '{existingDocument}'.");
            }

            this.MemoryCache.Set<DocumentEntity>(document.Id, document);
        }

        public void Update(DocumentEntity document)
        {
            String id = document.Id;
            if (!this.MemoryCache.TryGetValue<DocumentEntity>(id, out DocumentEntity existingDocument))
            {
                throw new RuntimeException($"Unable to update document with '{id}' id. Such document does not exist.");
            }

            this.MemoryCache.Set<DocumentEntity>(document.Id, document);
        }

        public DocumentEntity Read(Criteria criteria)
        {
            if (criteria.Id == null)
            {
                throw new RuntimeException($"Unable to provide document. Repository {this.GetType()} can no process criteria {criteria}");
            }

            String id = criteria.Id;
            if (!this.MemoryCache.TryGetValue<DocumentEntity>(id, out DocumentEntity document))
            {
                throw new RuntimeException($"Unable to provide document with '{id}' id. No such document exists.");
            }

            return document;
        }
    }
}