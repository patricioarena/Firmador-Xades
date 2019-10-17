using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Http;

namespace Demo.Controllers
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            JObject JSON = new JObject();
            JSON.Add("collection", new JObject(
                new JProperty(new JProperty("key1", "value1")),
                new JProperty(new JProperty("key2", "value2"))
                )
            );
            return Ok(JSON);
        }

        // GET api/values/5
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            JObject JSON = new JObject();
            JSON.Add("GET", new JObject(new JProperty(new JProperty("id", id))));
            return Ok(JSON);
        }

        // POST api/values
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] string value)
        {
            JObject JSON = new JObject();
            JSON.Add("POST", new JObject(new JProperty(new JProperty("value", value))));
            return Ok(JSON);
        }

        // PUT api/values/5
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody] string value)
        {
            JObject JSON = new JObject();
            JSON.Add("PUT", new JObject(new JProperty("id", id.ToString()), new JProperty("value", value)));
            return Ok(JSON);
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            JObject JSON = new JObject();
            JSON.Add("DELETE", new JObject(new JProperty("id", id)));
            return Ok(JSON);
        }
    }
}
