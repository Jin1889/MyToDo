﻿using MyToDo.Shared.Contact;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyTodo.Service
{
    public class HttpRestClient
    {
        private readonly string apiUrl;
        private readonly RestClient client;

        public HttpRestClient(string apiUrl)
        {
            this.apiUrl = apiUrl;
            client = new RestClient();
        }

        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            var request = new RestRequest(apiUrl + baseRequest.Route, baseRequest.Method);
            //request.AddHeader("Content-Type", baseRequest.ContentType);
            //if (baseRequest.Parameter != null)
            //    request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            //client.BuildUri(request);
            if (baseRequest.Parameter != null)
                request.AddJsonBody(JsonConvert.SerializeObject(baseRequest.Parameter));
            var response = await client.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
        }

        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            var request = new RestRequest(apiUrl + baseRequest.Route, baseRequest.Method);
            //request.AddHeader("Content-Type", baseRequest.ContentType);
            //if (baseRequest.Parameter != null)
            //    request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            ////client.BuildUri(request);
            if (baseRequest.Parameter != null)
                request.AddJsonBody(JsonConvert.SerializeObject(baseRequest.Parameter));
            var response = await client.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
        }
    }
}
