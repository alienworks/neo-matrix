using Microsoft.Extensions.Configuration;
using NeoMatrix.Data.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NeoMatrix.Data
{
    public class RedisService
    {
        private readonly string _redisConfig;
        private ConnectionMultiplexer _redis;
        
        private readonly RedisKey _matrixItemsRedisKey = new RedisKey("l_matrix_items");
        private readonly RedisKey _matrixInfoRedisKey = new RedisKey("h_matrix_info");

        public RedisService(IConfiguration configuration)
        {
            _redisConfig = configuration.GetConnectionString("RedisConnection");
        }

        public void Connect()
        {
            try
            {
                _redis = ConnectionMultiplexer.Connect(_redisConfig);
            }
            catch (RedisConnectionException err)
            {
                Console.WriteLine(err.ToString());
                throw err;
            }
            Console.WriteLine("Connected to Redis");
        }

        public async Task<bool> SetStringPair(string key, string val)
        {
            var db = _redis.GetDatabase();
            return await db.StringSetAsync(key, val);
        }

        public async Task SetMatrixInfo(string now, string time, string nodeCount, string matrixCount)
        {
            var db = _redis.GetDatabase();

            HashEntry[] hashEntries = new HashEntry[] {
                new HashEntry($"elapsed_time:{now}", time),
                new HashEntry($"runing_nodes:{now}", nodeCount),
                new HashEntry($"runing_matrixes:{now}", matrixCount)
            };

            await db.HashSetAsync(_matrixInfoRedisKey, hashEntries);
        }

        public async Task<long> PushMatrixItem(MatrixItemEntity matrixItem)
        {
            var db = _redis.GetDatabase();
            var json = JsonSerializer.Serialize<MatrixItemEntity>(matrixItem);
            return await db.ListRightPushAsync(_matrixItemsRedisKey, json);
        }

        public async Task ClearAllMatrixItem()
        {
            var db = _redis.GetDatabase();
            // start > stop would empty the entire list
            await db.ListTrimAsync(_matrixItemsRedisKey, 1, 0);
        }

        public async Task PushAllMatrixItem(IEnumerable<MatrixItemEntity> matrixItemEntities)
        {
            var requests = matrixItemEntities.Select(async item => await PushMatrixItem(item));
            await Task.WhenAll(requests);
        }

        public async Task GetMatrixItems()
        {
            var db = _redis.GetDatabase();
            RedisValue[] redisValues = await db.ListRangeAsync(_matrixItemsRedisKey, 0, -1);
            IEnumerable<MatrixItemEntity> matrixItemEntities = new List<MatrixItemEntity>();
            
            foreach (var item in redisValues)
            {
                Console.WriteLine("Read {0}", item);
                // Or do orther operation
                // var matrixItem = JsonSerializer.Deserialize<MatrixItemEntity>(item);
                // Console.WriteLine(matrixItem.Available);
            }
            
        }
    }
}
