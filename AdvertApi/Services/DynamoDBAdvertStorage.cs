using AdvertApi.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Services
{
    public class DynamoDBAdvertStorage : IAdvertStorage
    {
        private readonly IMapper _mapper;

        public DynamoDBAdvertStorage(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<string> Add(AdvertModel advert)
        {
            var dbModel = _mapper.Map<AdvertDBModel>(advert);

            using (var client=new AmazonDynamoDBClient())
            {
                using  (var context = new DynamoDBContext(client))
                {
                    await context.SaveAsync(dbModel);

                }

            }

            return dbModel.Id;
        }

        public async Task<bool> CheckDBHealth()
        {
            using (var client = new AmazonDynamoDBClient())
            {
                var tableData = await client.DescribeTableAsync("Adverts");
               
                return string.Compare( tableData.Table.TableStatus , "active",true)==0;
            }
        }

        public async Task Confirm(ConfirmAdvertModel confirm)
        {
            using (var client=new AmazonDynamoDBClient())
            {
                using (var context = new DynamoDBContext(client))
                {
                    var record = await context.LoadAsync<AdvertDBModel>(confirm.Id);
                    if (record == null)
                        throw new KeyNotFoundException($"record with id={confirm.Id} was not found!");

                    if(confirm.Status==AdvertStatus.Active)
                    {
                        record.Status = AdvertStatus.Active;
                        await context.SaveAsync(record);
                    }
                    else
                    {
                        await context.DeleteAsync(record);
                    }
                }
            }
        }

        public async Task<AdvertModel> GetById(string id)
        {
            

            using (var client = new AmazonDynamoDBClient())
            {
                using (var context = new DynamoDBContext(client))
                {
                   var record = await context.LoadAsync<AdvertDBModel>(id);
                    var model = _mapper.Map<AdvertModel>(record);

                    return model;
                }

            }

            
        }
    }
}
