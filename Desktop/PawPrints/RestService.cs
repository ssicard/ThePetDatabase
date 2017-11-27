﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing;

public class RestService
{


    public static string GetCall(string url)
    {

        try
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            HttpClient client;


            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;

            string html = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response2 = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response2.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            Console.WriteLine(html);
            return html;
        }

        catch (Exception ex)
        {
            if (ex.InnerException.Message.Equals("An error occurred while sending the request."))
            {
                return "-2";
            }
            return ex.Message.Trim('\n');

        }
    }

    public static Image getImageFromUrl(string url)
    {
        WebClient wc = new WebClient();
        byte[] bytes = wc.DownloadData(url);
        System.Drawing.Image img = System.Drawing.Image.FromStream(new MemoryStream(bytes)); ;
        return img;
    }

    public static string sendImageToUrl(string actionUrl, string paramString, byte[] paramFileBytes)
    {
        HttpContent stringContent = new StringContent(paramString);
        HttpContent bytesContent = new ByteArrayContent(paramFileBytes);
        using (var client = new HttpClient())
        using (var formData = new MultipartFormDataContent())
        {
            formData.Add(stringContent, "param1", "param1");
            formData.Add(bytesContent, "picture", "picture");
            var response = client.PostAsync(actionUrl, formData).Result;
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
           return  response.Content.ReadAsStringAsync().Result.Trim();

        }

    }


    public static string PostCall(string body, string url)
    {
        try
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            HttpClient client;
            var authData = string.Format("{0}:{1}", "test", "pswd");
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = client.PostAsync(url, content).Result;

            var responseString =  response.Content.ReadAsStringAsync().Result;

            Debug.WriteLine(responseString);
            return responseString;

        }
        catch (Exception ex)
        {
            if (ex.InnerException.Message.Equals("An error occurred while sending the request."))
            {
                return "-2";
            }
            return ex.InnerException.Message.Trim('\n');
        }

    }

}

