using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AirBnB_NY_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AirBnB_NY_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        List<Client> ClientList = new List<Client>();

        public ClientController()
        {
            SetData();
        }

        // GET: api/Client
        [HttpGet]
        public string Get()
        {
            return JsonConvert.SerializeObject(ClientList);
        }

        // GET: api/Client/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(string id)
        {
            return JsonConvert.SerializeObject(ClientList.Find(c => c.Id.Equals(id)));
        }

        public void SetData()
        {
            using (var reader = new StreamReader(@"Models/AB_NYC_2019.csv"))
            {
                ClientList = new List<Client>();
                while (!reader.EndOfStream)
                {
                    Client c = new Client();
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (values[0] == "id")
                    {
                        continue;
                    }
                    //Because of possibility of unclean data we curate the lines
                    else if(values.Length > 13)
                    {
                        c.Id = values[0];
                        c.Name = values[1];
                        c.Host_Id = values[2];
                        c.Host_name = values[3];
                        c.neighbourhood_group = values[4];
                        c.neighbourhood = values[5];
                        c.Ltd = values[6];
                        c.Lngtd = values[7];
                        c.Room_type = values[8];
                        c.price = values[9];
                        c.minimum_nights = values[10];
                        c.last_review = values[11];
                        c.reviews_per_month = values[12];
                        c.calculated_host_listings_count = values[13];
                        c.availability_365 = values[14];
                    }
                    ClientList.Add(c);
                }
            }
        }
    }
}
