
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;


namespace test2.Models.ResponseModels
{
    public class ResponseModel
    {
        string Status {get;set;}

        string Message {get;set;}

        int Code {get;set;}

        Dictionary<string, string> Data {get; set;}


    
    }


    
    
}