using DeploymentTool.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Web;

namespace DeploymentTool.API.Helpers
{
    public static class ResponseHelper
    {
        public static void Response(Object obj, Int32? statusCode = null)
        {
            Response(JsonConvert.SerializeObject(obj), statusCode);
        }

        public static void Response(String json, Int32? statusCode = null)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
            HttpContext.Current.Response.Clear();

            if (statusCode.HasValue)
            {
                HttpContext.Current.Response.StatusCode = statusCode.Value;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress))
                    gZipStream.Write(bytes, 0, bytes.Length);

                byte[] compressed = stream.ToArray();
                HttpContext.Current.Response.OutputStream.Write(compressed, 0, compressed.Length);
            }

            HttpContext.Current.Response.AppendHeader("Content-Encoding", "gzip");
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.End();
        }

        public static T GetInputData<T>(HttpContextBase context)
        {
            string requestData;
            using (var reader = new StreamReader(context.Request.InputStream))
            {
                requestData = reader.ReadToEnd();
            }

            return requestData.ToObject<T>();
        }

        public static async Task<T> GetInputDataAsync<T>(HttpContextBase context)
        {
            string requestData;
            using (var reader = new StreamReader(context.Request.InputStream))
            {
                requestData = await reader.ReadToEndAsync();
            }

            return requestData.ToObject<T>();
        }
    }
}