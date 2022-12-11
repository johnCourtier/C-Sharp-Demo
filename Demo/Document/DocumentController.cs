using Demo.Document.Command;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Demo.Document
{
    [ApiController]
    [Route("documents")]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> Logger;

        private readonly IDocumentCommandBus DocumentCommandBus;

        public DocumentController(
            IDocumentCommandBus documentCommandBus,
            ILogger<DocumentController> logger
        )
        {
            DocumentCommandBus = documentCommandBus;
            Logger = logger;
        }

        /// <summary>
        /// Reads document with specific id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Document content</returns>
        /// <response code="400">When no document with id exists</response>
        /// <response code="503">When document reading fails</response>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(503)]
        public IActionResult GetDocument(string id)
        {
            GetDocumentCommand getDocumentCommand = new GetDocumentCommand
            {
                Criteria = new Criteria
                {
                    Id = id
                }
            };

            DocumentEntity document;
            try
            {
                document = DocumentCommandBus.Execute(getDocumentCommand);
            }
            catch (RuntimeException exception)
            {
                Logger.LogError(exception, $"Unable to provide document. Execution of '{getDocumentCommand}' command has failed.");
                if (exception is ServiceException)
                {
                    return StatusCode(503);
                }

                return BadRequest(new ProblemDetails()
                {
                    Title = "Document can not be provided.",
                    Status = 400,
                    Detail = $"Document with id '{id}' does not exist."
                });
            }

            return Ok(document);
        }

        /// <summary>
        /// Updates existing document
        /// </summary>
        /// <param name="document"></param>
        /// <response code="400">When document with provided id does not exist</response>
        /// <response code="503">When document persisting fails</response>
        /// <response code="204">Success</response>
        [HttpPut]
        [Consumes("application/json")]
        [Route("")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(503)]
        public IActionResult UpdateDocument([FromBody][Required] DocumentEntity document)
        {
            UpdateDocumentCommand updateDocumentCommand = new UpdateDocumentCommand
            {
                Document = document
            };
            try
            {
                DocumentCommandBus.Execute(updateDocumentCommand);
            }
            catch (RuntimeException exception)
            {
                Logger.LogError(exception, $"Unable to update document. Execution of '{updateDocumentCommand}' command has failed.");
                if (exception is ServiceException)
                {
                    return StatusCode(503);
                }

                return BadRequest(new ProblemDetails()
                {
                    Title = "Document can not be updated.",
                    Status = 400,
                    Detail = $"Document with id '{document.Id}' does not exist."
                });
            }
            return NoContent();
        }

        /// <summary>
        /// Creates new document
        /// </summary>
        /// <param name="document"></param>
        /// <response code="400">When document with provided id already exists</response>
        /// <response code="503">When document persisting fails</response>
        [HttpPost]
        [Consumes("application/json")]
        [Route("")]
        [Produces("application/json")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(503)]
        public IActionResult CreateDocument([FromBody][Required] DocumentEntity document)
        {
            CreateDocumentCommand persistDocumentCommand = new CreateDocumentCommand
            {
                Document = document
            };
            try
            {
                DocumentCommandBus.Execute(persistDocumentCommand);
            }
            catch (RuntimeException exception)
            {
                Logger.LogError(exception, $"Unable to create document. Execution of '{persistDocumentCommand}' command has failed.");
                if (exception is ServiceException)
                {
                    return StatusCode(503);
                }

                return BadRequest(new ProblemDetails()
                {
                    Title = "Document can not be created.",
                    Status = 400,
                    Detail = $"Document with id '{document.Id}' already exists."
                });
            }
            return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
        }
    }
}