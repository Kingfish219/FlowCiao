using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Flow;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.SqlServer.Repositories
{
    public class ActivityRepository : FlowSqlServerRepository, IActivityRepository
    {
        public ActivityRepository(FlowSettings settings) : base(settings)
        {

        }

        public Task<Guid> Modify(Activity entity)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    Id = entity.Id == default ? Guid.NewGuid() : entity.Id,
                    Executor = entity.ProcessActivityExecutor.ToString()
                };

                using var connection = GetDbConnection();
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_Activity_Modify, toInsert, commandType: CommandType.StoredProcedure);
                entity.Id = toInsert.Id;

                return entity.Id;
            });
        }

        public async Task RegisterActivity(ActivityAssembly activityAssembly)
        {
            var storagePath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                @"FlowCiao\Assembly");
            if (string.IsNullOrWhiteSpace(Path.GetDirectoryName(storagePath)))
            {
                throw new FlowCiaoPersistencyException("Could not get ProgramsData path in order to save the file");
            }

            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            await File.WriteAllBytesAsync(Path.Join(storagePath, activityAssembly.FileName), activityAssembly.FileContent);
        }

        public Task<IProcessActivity> LoadActivity(string activityFileName)
        {
            throw new NotImplementedException();
        }
    }
}
