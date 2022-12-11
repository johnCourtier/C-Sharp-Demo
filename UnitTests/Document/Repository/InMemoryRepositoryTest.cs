using Demo.Document;
using Demo.Document.Repository;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Newtonsoft.Json.Linq;

namespace UnitTests.Document.Repository
{
    [TestFixture]
    public class InMemoryRepositoryTest
    {
        private InMemoryRepository InMemoryRepository;

        private Mock<IMemoryCache> MemoryCache;

        [SetUp]
        public void Setup()
        {
            this.MemoryCache = new Mock<IMemoryCache>();
            this.InMemoryRepository = new InMemoryRepository(this.MemoryCache.Object);
        }

        [Test]
        public void Create_documentExists_RuntimeExceptionThrown()
        {
            DocumentEntity document = new DocumentEntity
            {
                Id = "123",
                Tags = new string[] { "tag1", "tag2" },
                Data = new JArray()
            };

            object existingDocument;
            this.MemoryCache.Setup(me => me.TryGetValue(document.Id, out existingDocument)).Returns(true);

            try
            {
                this.InMemoryRepository.Create(document);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (RuntimeException exception)
            {
                Assert.That(exception.Message, Is.EqualTo($"Unable to create document with '{document.Id}' id. Such document already exists: ''."));
            }

            this.MemoryCache.Verify(me => me.TryGetValue(document.Id, out existingDocument), Times.Once());
        }

        [Test]
        public void Create_success()
        {
            DocumentEntity document = new DocumentEntity
            {
                Id = "123",
                Tags = new string[] { "tag1", "tag2" },
                Data = new JArray()
            };

            object existingDocument;
            this.MemoryCache.Setup(me => me.TryGetValue(document.Id, out existingDocument)).Returns(false);
            this.MemoryCache.Setup(me => me.CreateEntry(document.Id)).Returns(Mock.Of<ICacheEntry>);

            this.InMemoryRepository.Create(document);
            this.MemoryCache.Verify(me => me.TryGetValue(document.Id, out existingDocument), Times.Once());
            this.MemoryCache.Verify(me => me.CreateEntry(document.Id), Times.Once());
        }

        [Test]
        public void Update_documentDoesNotExist_RuntimeExceptionThrown()
        {
            DocumentEntity document = new DocumentEntity
            {
                Id = "123",
                Tags = new string[] { "tag1", "tag2" },
                Data = new JArray()
            };

            object existingDocument;
            this.MemoryCache.Setup(me => me.TryGetValue(document.Id, out existingDocument)).Returns(false);

            try
            {
                this.InMemoryRepository.Update(document);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (RuntimeException exception)
            {
                Assert.That(exception.Message, Is.EqualTo($"Unable to update document with '{document.Id}' id. Such document does not exist."));
            }

            this.MemoryCache.Verify(me => me.TryGetValue(document.Id, out existingDocument), Times.Once());
        }

        [Test]
        public void Update_success()
        {
            DocumentEntity document = new DocumentEntity
            {
                Id = "123",
                Tags = new string[] { "tag1", "tag2" },
                Data = new JArray()
            };

            object existingDocument;
            this.MemoryCache.Setup(me => me.TryGetValue(document.Id, out existingDocument)).Returns(true);
            this.MemoryCache.Setup(me => me.CreateEntry(document.Id)).Returns(Mock.Of<ICacheEntry>);

            this.InMemoryRepository.Update(document);
            this.MemoryCache.Verify(me => me.TryGetValue(document.Id, out existingDocument), Times.Once());
            this.MemoryCache.Verify(me => me.CreateEntry(document.Id), Times.Once());
        }

        [Test]
        public void Read_invalidCriteria_RuntimeExceptionThrown()
        {
            Criteria criteria = new Criteria();

            try
            {
                this.InMemoryRepository.Read(criteria);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (RuntimeException exception)
            {
                Assert.That(exception.Message, Is.EqualTo($"Unable to provide document. Repository '{this.InMemoryRepository.GetType()}' can not process criteria {criteria}"));
            }
        }

        [Test]
        public void Read_documentDoesNotExist_RuntimeExceptionThrown()
        {
            Criteria criteria = new Criteria()
            {
                Id = "123"
            };

            object existingDocument;
            this.MemoryCache.Setup(me => me.TryGetValue(criteria.Id, out existingDocument)).Returns(false);

            try
            {
                this.InMemoryRepository.Read(criteria);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (RuntimeException exception)
            {
                Assert.That(exception.Message, Is.EqualTo($"Unable to provide document with '{criteria.Id}' id. No such document exists."));
            }
        }

        [Test]
        public void Read_success()
        {
            Criteria criteria = new Criteria()
            {
                Id = "123"
            };

            object document = new DocumentEntity
            {
                Id = "123",
                Tags = new string[] { "tag1", "tag2" },
                Data = new JArray()
            };

            this.MemoryCache.Setup(me => me.TryGetValue(criteria.Id, out document)).Returns(true);
            DocumentEntity actualDocument = this.InMemoryRepository.Read(criteria);
            Assert.That(document, Is.EqualTo(actualDocument));
        }
    }
}