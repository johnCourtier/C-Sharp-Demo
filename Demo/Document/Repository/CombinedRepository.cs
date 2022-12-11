namespace Demo.Document.Repository
{
    public class CombinedRepository : IDocumentRepository
    {
        private readonly IDocumentRepository PrimaryRepository;

        private readonly IDocumentRepository SecondaryRepository;

        private readonly ILogger<CombinedRepository> Logger;

        public CombinedRepository(
            ILogger<CombinedRepository> logger,
            IDocumentRepository primaryRepository,
            IDocumentRepository secondaryRepository
        )
        {
            this.Logger = logger;
            this.PrimaryRepository = primaryRepository;
            this.SecondaryRepository = secondaryRepository;
        }

        public void Create(DocumentEntity document)
        {
            try
            {
                this.PrimaryRepository.Create(document);
            }
            catch (RuntimeException exception)
            {
                throw new RuntimeException($"Unable to create document with '{document.Id}' id. Repository '{this.PrimaryRepository}' has failed.", exception);
            }

            try
            {
                this.SecondaryRepository.Create(document);
            }
            catch (RuntimeException exception)
            {
                throw new RuntimeException($"Unable to create document with '{document.Id}' id. Repository '{this.SecondaryRepository}' has failed.", exception);
            }
        }

        public DocumentEntity Read(Criteria criteria)
        {
            try
            {
                return this.PrimaryRepository.Read(criteria);
            }
            catch (RuntimeException exception)
            {
                this.Logger.LogError(exception, $"Unable to provide document with '{criteria.Id}' id. Repository '{this.PrimaryRepository}' has failed.");
            }

            DocumentEntity? document;
            try
            {
                document = this.SecondaryRepository.Read(criteria);
            }
            catch (RuntimeException exception)
            {
                this.Logger.LogError(exception, $"Unable to provide document with '{criteria.Id}' id. Repository '{this.SecondaryRepository}' has failed.");
                document = null;
            }

            if (document == null)
            {
                throw new RuntimeException($"Unable to provide document with '{criteria.Id}' id. Repositories '{this.PrimaryRepository}' and '{this.SecondaryRepository}' has failed.");
            }

            try
            {
                this.PrimaryRepository.Create(document);
            }
            catch (RuntimeException exception)
            {
                this.Logger.LogError(exception, $"Unable to create document with '{document.Id}' id. Repository '{this.PrimaryRepository}' has failed.");
            }

            return document;
        }

        public void Update(DocumentEntity document)
        {
            try
            {
                this.PrimaryRepository.Update(document);
            }
            catch (RuntimeException exception)
            {
                throw new RuntimeException($"Unable to update document with '{document.Id}' id. Repository '{this.PrimaryRepository}' has failed.", exception);
            }

            try
            {
                this.SecondaryRepository.Update(document);
            }
            catch (RuntimeException exception)
            {
                throw new RuntimeException($"Unable to update document with '{document.Id}' id. Repository '{this.SecondaryRepository}' has failed.", exception);
            }
        }
    }
}