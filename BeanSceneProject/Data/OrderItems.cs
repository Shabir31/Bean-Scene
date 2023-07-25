using MongoDB.Bson;

namespace BeanSceneProject.Data
{
    public class OrderItems
    {
      
        public ObjectId Id { get; set; }
        public string ItemName { get; set; }

        public int Quantity { get; set; }
    }
}
