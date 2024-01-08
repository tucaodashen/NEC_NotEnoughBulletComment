using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

class BulletComment
{
    private static readonly HttpClient httpClient = new HttpClient();
    private static bool isRunning = true;
    private List<string> currentComment;
    public string json;
    public List<string> currentBulletComment;
    public bool running;

    public string urlAPI = "https://api.live.bilibili.com/xlive/web-room/v1/dM/gethistory?roomid=";
    public string roomID;
    public int Delay = 500;


    public void StartLoop()
    {   
        running=true;
        bool isRunning = true;
        Task.Run(async () =>
        {
            while (isRunning)
            {
                await GetDataFromApi();
                await Task.Delay(TimeSpan.FromMilliseconds(200));
            }
        });
    }

    public void StopLoop()
    {
        running = false;
        isRunning = false;
    }

    async Task GetDataFromApi()
    {
        string apiUrl = urlAPI+roomID;
        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            json = await response.Content.ReadAsStringAsync();

            List<string> temp = CookComment(json);
            if (currentComment!=null)
            {
                currentBulletComment = GetNewComment(currentComment, temp);
            }
            currentComment = temp;
        }
        else
        {
            Console.WriteLine("无法从API获取数据：" + response.StatusCode);
        }
    }

    public static List<string> CookComment(string data)//处理json数据
    {
        List<string> usercomment = new List<string>();

        var jsonObject = JObject.Parse(data);
        var studentsArray = jsonObject["data"];

        foreach (var VARIABLE in studentsArray["admin"])
        {
            string comm = VARIABLE["nickname"] + ":" + VARIABLE["text"];
            usercomment.Add(comm);
        }
        foreach (var VARIABLE in studentsArray["room"])
        {
            string comm = VARIABLE["nickname"] + ":" + VARIABLE["text"];
            usercomment.Add(comm);
        }

        return usercomment;

    }
    public static List<string> GetNewComment(List<string> oldComments, List<string> newComments)
    {

        List<string> differentItems = newComments.Except(oldComments).ToList();
        return differentItems;
    }
}