namespace BeanSceneProject.Data
{
    public class SittingType
    {
        public int Id { get; set; }
        public string SittingTypeName { get; set; }
        public List<Sitting> Sittings { get; set; } = new List<Sitting>(); 

        
    }
}
