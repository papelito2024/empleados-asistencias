
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;


namespace test2.Models.ResponseModels
{
    public class ResponseModel
    {
       public  string Status {get;set;}

        public  string Message {get;set;}

        public  int StatusCode {get;set;}

         public object Data {get; set;}


    
    }


    
    
}