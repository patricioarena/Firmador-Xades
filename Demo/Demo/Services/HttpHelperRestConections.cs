using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace AngularApi.Controllers
{
    public class HttpHelperRestConections
    {

        WebClient client;
        public enum backendUrl { coreApiUrl, adServiceUrl }
        public string urlBackend;

        public HttpHelperRestConections(string backendUrl)
        {
            client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            urlBackend = ConfigurationManager.AppSettings[backendUrl].ToString();
        }

        public JObject restCallGet(string uri, ApiController api)
        {
            JObject jsonHeades = new JObject();
            bool tieneAuthorizationBasic = false;
            try
            {
                // armo un json con los headers
                foreach (var oneHeader in api.Request.Headers)
                {
                    var header = oneHeader.Key;
                    var value = oneHeader.Value.FirstOrDefault();
                    jsonHeades.Add(header, value);
                }

                if (api.Request.Headers.Contains("Authorization"))
                {
                    foreach (var value in api.Request.Headers.GetValues("Authorization"))
                    {
                        if (value.Contains("Basic"))
                        {
                            client.Headers["Authorization"] = value;
                            tieneAuthorizationBasic = true;
                        }

                    }
                }
                if (!tieneAuthorizationBasic)
                {
                    client.UseDefaultCredentials = true;
                }
                var text = client.DownloadString(urlBackend + uri);
                JObject jobject = JObject.Parse(text);
                jobject.Add("request headers", jsonHeades);
                return jobject;
            }
            catch (Exception e)
            {
                var errorObject = new JObject();
                errorObject.Add("error", e.Message);
                errorObject.Add("url", urlBackend + uri);
                errorObject.Add("headers", jsonHeades);
                return errorObject;
            }
        }


        public JObject restCallPost(string uri, object body, ApiController api)
        {
            JObject jsonHeades = new JObject();
            bool tieneAuthorizationBasic = false;
            try
            {
                // armo un json con los headers
                foreach (var oneHeader in api.Request.Headers)
                {
                    var header = oneHeader.Key;
                    var value = oneHeader.Value.FirstOrDefault();
                    jsonHeades.Add(header, value);
                }

                if (api.Request.Headers.Contains("Authorization"))
                {
                    foreach (var value in api.Request.Headers.GetValues("Authorization"))
                    {
                        if (value.Contains("Basic"))
                        {
                            client.Headers["Authorization"] = value;
                            tieneAuthorizationBasic = true;
                        }

                    }
                }
                if (!tieneAuthorizationBasic)
                {
                    client.UseDefaultCredentials = true;
                }

                var bodyRest = JObject.FromObject(body).ToString();
                var response = client.UploadString(urlBackend + uri, bodyRest);
                JObject jobject = JObject.Parse(response);

                jobject.Add("request headers", jsonHeades);
                return jobject;
            }
            catch (Exception e)
            {
                var errorObject = new JObject();
                errorObject.Add("error", e.Message);
                errorObject.Add("url", urlBackend + uri);
                errorObject.Add("headers", jsonHeades);
                return errorObject;
            }
        }


        public JObject restCallPut(string uri, object body, ApiController api)
        {
            JObject jsonHeades = new JObject();
            bool tieneAuthorizationBasic = false;
            try
            {
                // armo un json con los headers
                foreach (var oneHeader in api.Request.Headers)
                {
                    var header = oneHeader.Key;
                    var value = oneHeader.Value.FirstOrDefault();
                    jsonHeades.Add(header, value);
                }

                if (api.Request.Headers.Contains("Authorization"))
                {
                    foreach (var value in api.Request.Headers.GetValues("Authorization"))
                    {
                        if (value.Contains("Basic"))
                        {
                            client.Headers["Authorization"] = value;
                            tieneAuthorizationBasic = true;
                        }

                    }
                }
                if (!tieneAuthorizationBasic)
                {
                    client.UseDefaultCredentials = true;
                }

                var bodyRest = JObject.FromObject(body).ToString();
                var response = client.UploadString(urlBackend + uri + "/update", bodyRest);
                JObject jobject = JObject.Parse(response);

                jobject.Add("request headers", jsonHeades);
                return jobject;
            }
            catch (Exception e)
            {
                var errorObject = new JObject();
                errorObject.Add("error", e.Message);
                errorObject.Add("url", urlBackend + uri);
                errorObject.Add("headers", jsonHeades);
                return errorObject;
            }
        }


        public JObject restCallDelete(string uri, ApiController api)
        {
            JObject jsonHeades = new JObject();
            bool tieneAuthorizationBasic = false;
            try
            {
                // armo un json con los headers
                foreach (var oneHeader in api.Request.Headers)
                {
                    var header = oneHeader.Key;
                    var value = oneHeader.Value.FirstOrDefault();
                    jsonHeades.Add(header, value);
                }

                if (api.Request.Headers.Contains("Authorization"))
                {
                    foreach (var value in api.Request.Headers.GetValues("Authorization"))
                    {
                        if (value.Contains("Basic"))
                        {
                            client.Headers["Authorization"] = value;
                            tieneAuthorizationBasic = true;
                        }

                    }
                }
                if (!tieneAuthorizationBasic)
                {
                    client.UseDefaultCredentials = true;
                }

                var response = client.DownloadString(urlBackend + uri + "/delete");
                JObject jobject = JObject.Parse(response);

                jobject.Add("request headers", jsonHeades);
                return jobject;
            }
            catch (Exception e)
            {
                var errorObject = new JObject();
                errorObject.Add("error", e.Message);
                errorObject.Add("url", urlBackend + uri);
                errorObject.Add("headers", jsonHeades);
                return errorObject;
            }
        }
    }
}