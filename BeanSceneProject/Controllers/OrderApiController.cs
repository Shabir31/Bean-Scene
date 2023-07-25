using BeanSceneProject.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeanSceneProject.Controllers
{
   

    [Route("api/v1/orders")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private const string CONN_STRING = "mongodb://localhost:27017";

        [HttpPost]
        public async Task<IActionResult> CreateOrder(SaveOrderModel model)
        {
            MongoClient client = new MongoClient(CONN_STRING);
            IMongoCollection<Orders> _collection = client
                .GetDatabase("BeanScene")
                .GetCollection<Orders>("orders");

            IMongoCollection<Menu> menuCollection = client
                .GetDatabase("BeanScene")
                .GetCollection<Menu>("Menu");

            var itemsToOrder = model.OrderItems.Select(i => new ObjectId(i.ItemId)).ToList();

            var menuItems = menuCollection.Find(item => itemsToOrder.Contains(item.Id)).ToList();

            try
            {
                Orders order = new Orders();
                order.OrderItems = model.OrderItems.Select((menuItem) => new OrderItems()
                {
                    Id = new ObjectId(menuItem.ItemId),
                    Quantity = menuItem.Quantity,
                    ItemName = menuItems.Where(i => i.Id == new ObjectId(menuItem.ItemId))
                        .Select(i => i.ItemName)
                        .First()
                    // need to map data to OrderItems
                }).ToList();
                order.SelectedTable = model.SelectedTableName;
                order.DateTime = DateTime.Now;
                order.Comment = model.Comment;
                order.OrderNumber = model.OrderNumber;
                
                await _collection.InsertOneAsync(order);
                

                

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
    public class SaveOrderModel
    {
        public SaveOrderModel()
        {
            OrderItems = new List<SaveOrderItemModel>();
        }

        public string SelectedTableName { get; set; }
        public string Comment { get; set; }
        public string OrderNumber { get; set; }
        public List<SaveOrderItemModel> OrderItems { get; set; }
    }

    public class SaveOrderItemModel
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }   
        public int Quantity { get; set; }
    }
}
