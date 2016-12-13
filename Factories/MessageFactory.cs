using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using wall.Models;
using Microsoft.Extensions.Options;


namespace wall.Factory
{
    public class MessageFactory : IFactory<Message>
    {
       private readonly IOptions<MySqlOptions> mysqlConfig;
    
        public MessageFactory(IOptions<MySqlOptions> conf) {
            mysqlConfig = conf;
    }

        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(mysqlConfig.Value.ConnectionString);
            }
        }
        public void Add(Message item, long user)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  $"INSERT INTO messages (user_id, message, created_at, updated_at) VALUES ({user}, @message, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, item);
            }
        }
        public IEnumerable<Message> Getmessages()
        {
            using (IDbConnection dbConnection = Connection)
            {
                var query = $"SELECT * FROM messages JOIN users ON messages.user_id WHERE users.Id = messages.user_id";
                dbConnection.Open();
                var combined = dbConnection.Query<Message, User, Message>(query, (message, user) => { message.user = user; return message; });
                return combined;
            }
        }
         public void AddComment(Comment item, long user, long message)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  $"INSERT INTO comments (user_id, comment, message_id, created_at, updated_at) VALUES ({user}, @comment, {message}, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, item);
            }
        }
        public IEnumerable<Comment> Getcomments()
        {
            using (IDbConnection dbConnection = Connection)
            {
                var query = $"SELECT * FROM comments JOIN users ON comments.user_id WHERE users.Id = comments.user_id";
                dbConnection.Open();
                var combined = dbConnection.Query<Comment, User, Comment>(query, (comment, user) => { comment.user = user; return comment; });
                return combined;
            }
        }
    }
}