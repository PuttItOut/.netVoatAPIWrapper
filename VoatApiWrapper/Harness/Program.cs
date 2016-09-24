﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoatApiWrapper;

namespace Harness {
    class Program {
        static void Main(string[] args) {


            //********************************************************************************************************
            //This is just a simple console application to demonstrate how to use the .netVoatApiWrapper. In your
            //project you will simply compile the VoatApiWrapper (class library) project (or include the source 
            //project in your solution) and consume it with your Tool/App.
            //********************************************************************************************************
            
            //Set your api key and endpoint location
            ApiInfo.ApiPublicKey = ConfigurationManager.AppSettings["voat.api.PublicKey"];
            ApiInfo.ApiPrivateKey = ConfigurationManager.AppSettings["voat.api.PrivateKey"];
            ApiInfo.BaseEndpoint = ConfigurationManager.AppSettings["voat.api.EndPoint"];

            //Authenticate a user using the ApiAuthenticator Object
            var authResult = ApiAuthenticator.Instance.Login(
                ConfigurationManager.AppSettings["voat.UserName"], 
                ConfigurationManager.AppSettings["voat.Password"], 
                true);

            if (!authResult.Success) {
                Console.WriteLine("{0}: {1}", authResult.Error.Type, authResult.Error.Message);
                Console.WriteLine("Press <Enter> to Exit");
                Console.ReadLine();
                return;
            }

            //Create the Proxy Object
            VoatApiProxy api = new VoatApiProxy();

            //All API Responses will be returned in this form (ApiResponse). You can check the .Success property or the .StatusCode to determine if operation succeeded.
            ApiResponse response;

            //Set api defaults example (there isn't a big reason to change these defaults as they provide the safest way to access the api and handle ApiThrottleLimit exception by default).
            api.EnableMultiThreading = false;
            api.RetryOnThrottleLimit = true;
            api.WaitTimeOnThrottleLimit = 1500;
            api.MaxThrottleRetryCount = 1;


            //Submit Discussion
            response = api.SubmitDiscussion("PuttItOutPlease", "This is a post using .NET C# Voat API Wrapper", "Title says it all - Do you like wrappers?");
            if (response.Success) {
                Console.WriteLine(response.Data.ToString());
            } else {
                Console.WriteLine("{0}: {1}", response.Error.Type, response.Error.Message);
            }


            //Retrieve Subverse Submissions 
            response = api.GetSubmissionsBySubverse("news", new { sort = "top", count = 5, index = 0, span = "week" });
            if (response.Success) {
                Console.WriteLine(response.Data[0].ToString());
            } else {
                Console.WriteLine("{0}: {1}", response.Error.Type, response.Error.Message);
            }


            //Test logout 
            ApiAuthenticator.Instance.Logout();

            response = api.GetUserComments("DerpyGuy", null);
            if (response.Success) {
                Console.WriteLine(response.Data[0].ToString());
            } else {
                Console.WriteLine("{0}: {1}", response.Error.Type, response.Error.Message);
            }

            //Keep console window up
            Console.WriteLine("Press <Enter> to Exit");
            Console.ReadLine();

        }
    }
}

