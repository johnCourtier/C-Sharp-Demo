namespace Demo.Document
{
    public interface IDocumentRepository
    {
        /// <exception cref="RuntimeException">Thrown when document obtaining fails</exception>
        public DocumentEntity Read(Criteria criteria);

        /// <exception cref="RuntimeException">Thrown when document creation fails</exception>
        public void Create(DocumentEntity document);

        /// <exception cref="RuntimeException">Thrown when document updating fails</exception>
        public void Update(DocumentEntity document);
    }
}