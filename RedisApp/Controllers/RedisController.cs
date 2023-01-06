using Microsoft.AspNetCore.Mvc;
using RedisApp.Classes;
using RedisApp.Models;

namespace RedisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IRedisAdapter redisAdapter;

        public RedisController(IRedisAdapter redisAdapter)
        {
            this.redisAdapter = redisAdapter;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cache request)
        {
            if (ModelState.IsValid)
            {
                bool result = await redisAdapter.UpdateCacheAsyn(request);
                if (result)
                {
                    return Ok($"Data of key:{request.Key} ,Save Succesfully");
                }
                return BadRequest($"Cannot Save Data of key:{request.Key}");
            }
            return BadRequest(ModelState);
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return BadRequest($"Key is Not Valid");
            }
            string value=await redisAdapter.GetCacheAsync(key);
            if(!string.IsNullOrEmpty(value))
            {
                return Ok($"Value: {value}");
            }
            return BadRequest($"Key:{key} does not exist");
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return BadRequest($"Key is Not Valid");
            }
            bool result = await redisAdapter.DeleteCacheAsyn(key);
            if (result)
            {
                return Ok($"Data of key:{key} ,Delete Succesfully");
            }
            return BadRequest($"Cannot Delete Data,key:{key} Does not exist");
        }
    }
}
