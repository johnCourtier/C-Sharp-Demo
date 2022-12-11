using Demo.Document.Repository;

namespace Demo.Document.Command
{
    public class DocumentCommandBus : IDocumentCommandBus
    {
        private readonly IDocumentRepository DocumentRepository;

        public DocumentCommandBus(IDocumentRepository documentRepository)
        {
            this.DocumentRepository = documentRepository;
        }

        public DocumentEntity Execute(GetDocumentCommand getDocumentCommand)
        {
            Criteria criteria = getDocumentCommand.Criteria;
            try
            {
                return this.DocumentRepository.Read(criteria);
            }
            catch (RuntimeException exception)
            {
                throw new RuntimeException($"Unable to execute '{getDocumentCommand}' command. Reading from '{this.DocumentRepository}' repository has failed.", exception);
            }
        }

        public void Execute(CreateDocumentCommand createDocumentCommand)
        {
            DocumentEntity document = createDocumentCommand.Document;
            try
            {
                this.DocumentRepository.Create(document);
            }
            catch (RuntimeException exception)
            {
                throw new RuntimeException($"Unable to execute '{createDocumentCommand}' command. Creating by '{this.DocumentRepository}' repository has failed.", exception);
            }
        }

        public void Execute(UpdateDocumentCommand updateDocumentCommand)
        {
            DocumentEntity document = updateDocumentCommand.Document;
            try
            {
                this.DocumentRepository.Update(document);
            }
            catch (RuntimeException exception)
            {
                throw new RuntimeException($"Unable to execute '{updateDocumentCommand}' command. Updating by '{this.DocumentRepository}' repository has failed.", exception);
            }
        }
    }
}