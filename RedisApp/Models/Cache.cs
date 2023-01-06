using System.ComponentModel.DataAnnotations;

namespace RedisApp.Models
{
    public class Cache
    {
        [Required] public string Key { get; set; }
        [Required] public string Value { get; set; }
    }
}
