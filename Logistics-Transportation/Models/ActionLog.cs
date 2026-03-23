namespace Logistics_Transportation.Models
{
    public class ActionLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Action {  get; set; }
        public string EntityName { get; set; }
        public int? EntityId { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
