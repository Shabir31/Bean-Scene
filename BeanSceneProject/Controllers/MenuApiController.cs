using BeanSceneProject.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeanSceneProject.Controllers
{
    [Route("api/v1/Items")]
    [ApiController]
    public class MenuApiController : ControllerBase
    {
        private const string CONN_STRING = "mongodb://localhost:27017";

        [HttpGet]
        public async Task<ActionResult> GetMenuItem()
        {
            MongoClient client = new MongoClient(CONN_STRING);
            IMongoCollection<Menu> _collection = client
                .GetDatabase("BeanScene")
                .GetCollection<Menu>("Menu");

            try
            {
            var menuItems = await _collection.Find(_ => true)
                    .Project(menuItem => new
                    {
                        Id = menuItem.Id.ToString(),
                        menuItem.ItemName,
                        menuItem.Description,
                        menuItem.Category,
                        menuItem.Price
                    })
                    .ToListAsync();
            return Ok(menuItems);

            }
            catch(Exception e)
            {

            }

            //return Ok();
            return Ok();
        }


       
    }
}
