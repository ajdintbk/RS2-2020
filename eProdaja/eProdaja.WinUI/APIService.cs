﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using eProdaja.Model;
using eProdaja.WinUI.Properties;
using Flurl.Http;

namespace eProdaja.WinUI
{
    public class APIService
    {
        private string _resource;
        public string endpoint = $"{Resources.ApiUrl}";
        public APIService(string resource)
        {
            _resource = resource;
        }
        public async Task<T> GetAll<T>(object searchRequest = null)
        {
            var query = await searchRequest?.ToQueryString();

            var list = await $"{endpoint}{_resource}?{query}"
               .GetJsonAsync<T>();

            return list;
        }

        public async Task<T> GetById<T>(object id)
        {
            var url = $"{endpoint}{_resource}/{id}";

            return await url.GetJsonAsync<T>();
        }

        public async Task<T> Insert<T>(object request)
        {
            var url = $"{endpoint}{_resource}";

            try
            {
                return await url.PostJsonAsync(request).ReceiveJson<T>();
            }
            catch (FlurlHttpException ex)
            {
                var errors = await ex.GetResponseJsonAsync<Dictionary<string, string[]>>();

                var stringBuilder = new StringBuilder();
                foreach (var error in errors)
                {
                    stringBuilder.AppendLine($"{error.Key}, ${string.Join(",", error.Value)}");
                }

                MessageBox.Show(stringBuilder.ToString(), "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default(T);
            }

        }

        public async Task<T> Update<T>(int id, object request)
        {
            try
            {
                var url = $"{endpoint}{_resource}/{id}";

                return await url.PutJsonAsync(request).ReceiveJson<T>();
            }
            catch (FlurlHttpException ex)
            {
                var errors = await ex.GetResponseJsonAsync<Dictionary<string, string[]>>();

                var stringBuilder = new StringBuilder();
                foreach (var error in errors)
                {
                    stringBuilder.AppendLine($"{error.Key}, ${string.Join(",", error.Value)}");
                }

                MessageBox.Show(stringBuilder.ToString(), "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default(T);
            }

        }
    }
}
