using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeanSceneProject.Data
{
    public class Orders
    {
        
        
        public ObjectId Id { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime DateTime { get; set; }
        public string SelectedTable { get; set; }
        public string Comment { get; set; }
        
        public List<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
        


        
        


    }
}
