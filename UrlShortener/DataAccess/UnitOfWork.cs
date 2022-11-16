using MongoDB.Driver;
using UrlShortener.Application;

namespace UrlShortener.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private IMongoDatabase Database { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MongoClient MongoClient { get; set; }
        private readonly List<Func<Task>> _commands;

        public UnitOfWork(ApplicationSettings applicationSettings)
        {
            MongoClient = new MongoClient(applicationSettings.ConnectionString);
            Database = MongoClient.GetDatabase(applicationSettings.DatabaseName);

            _commands = new List<Func<Task>>();
        }

        // If sharded or has replica
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            using (Session = await MongoClient.StartSessionAsync(cancellationToken: cancellationToken))
            {
                Session.StartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync(cancellationToken);
            }

            return _commands.Count;
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return Database.GetCollection<T>(name);
        }
    }
}